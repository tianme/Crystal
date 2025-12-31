using Server.MirNetwork;
using Server.MirEnvir;
using Server.Utils;
using C = ClientPackets;

namespace Server.MirDatabase
{
    public class AccountInfo
    {

        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 消息队列，用于记录服务器日志
        /// </summary>
        protected static MessageQueue MessageQueue => MessageQueue.Instance;
        // 索引
        public int Index;
        /// <summary>
        /// 账号ID默认空字符串
        /// </summary>
        public string AccountID = string.Empty;
        /// <summary>
        /// 密码字段默认空字符串
        /// </summary>
        private string password = string.Empty;
        /// <summary>
        /// 密码属性，set自动hash自动加盐
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                Salt = Crypto.GenerateSalt();
                password = Crypto.HashPassword(value, Salt);

            }
        }
        /// <summary>
        /// 盐（用于记录当前密码所用的盐）
        /// </summary>
        public byte[] Salt = new byte[24];
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName = string.Empty;
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDate;
        /// <summary>
        /// 密保问题
        /// </summary>
        public string SecretQuestion = string.Empty;
        /// <summary>
        /// 密保答案
        /// </summary>
        public string SecretAnswer = string.Empty;

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string EMailAddress = string.Empty;
        /// <summary>
        /// 创建IP
        /// </summary>
        public string CreationIP = string.Empty;

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Banned;

        /// <summary>
        /// 需要修改密码
        /// </summary>
        public bool RequirePasswordChange;

        /// <summary>
        /// 禁用的原因
        /// </summary>
        public string BanReason = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiryDate;

        /// <summary>
        /// 密码错误计数
        /// </summary>
        public int WrongPasswordCount;

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastIP = string.Empty;

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastDate;

        /// <summary>
        /// 角色集合
        /// </summary>
        public List<CharacterInfo> Characters = new List<CharacterInfo>();

        /// <summary>
        /// 仓库
        /// </summary>
        public UserItem[] Storage = new UserItem[80];

        /// <summary>
        /// 是否有扩展仓库
        /// </summary>
        public bool HasExpandedStorage;
        /// <summary>
        /// 扩展仓库的过期时间
        /// </summary>
        public DateTime ExpandedStorageExpiryDate;

        /// <summary>
        /// 金币
        /// </summary>
        public uint Gold;

        /// <summary>
        /// 点券 / 代币 / 游戏币
        /// </summary>
        public uint Credit;

        /// <summary>
        /// TCP 连接对象 + 玩家绑定 + 网络收发逻辑
        /// </summary>
        public MirConnection Connection;

        /// <summary>
        /// 拍卖行
        /// </summary>
        public LinkedList<AuctionInfo> Auctions = new LinkedList<AuctionInfo>();

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool AdminAccount;

        public AccountInfo()
        {

        }
        // 创建账户
        public AccountInfo(C.NewAccount p)
        {
            AccountID = p.AccountID;

            Password = p.Password;
            UserName = p.UserName;
            SecretQuestion = p.SecretQuestion;
            SecretAnswer = p.SecretAnswer;
            EMailAddress = p.EMailAddress;

            BirthDate = p.BirthDate;
            CreationDate = Envir.Now;
        }

        /// <summary>
        /// 初始化账户信息
        /// </summary>
        /// <param name="reader"></param>
        public AccountInfo(BinaryReader reader)
        {

            Index = reader.ReadInt32(); // 读取索引

            AccountID = reader.ReadString();
            if (Envir.LoadVersion < 94)
                Password = reader.ReadString();// 明文密码
            else
                password = reader.ReadString();// 读取密码（加盐后的）

            if (Envir.LoadVersion > 93)
                Salt = reader.ReadBytes(reader.ReadInt32()); // 读取盐

            if (Envir.LoadVersion > 97)
                RequirePasswordChange = reader.ReadBoolean(); // 是否需要修改密码
            // 用户名
            UserName = reader.ReadString();
            // 生日
            BirthDate = DateTime.FromBinary(reader.ReadInt64());
            // 密保问题
            SecretQuestion = reader.ReadString();
            // 密保答案
            SecretAnswer = reader.ReadString();
            // 邮件地址
            EMailAddress = reader.ReadString();
            // 创建IP
            CreationIP = reader.ReadString();
            // 创建日期
            CreationDate = DateTime.FromBinary(reader.ReadInt64());
            // 是否被禁用
            Banned = reader.ReadBoolean();
            // 被禁原因
            BanReason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());
            // 最后登录IP
            LastIP = reader.ReadString();
            // 最后登录日期
            LastDate = DateTime.FromBinary(reader.ReadInt64());
            // 一共有多少个角色
            int count = reader.ReadInt32();
            // 初始化角色信息
            for (int i = 0; i < count; i++)
            {
                // 创建角色实例
                var info = new CharacterInfo(reader, Envir.LoadVersion, Envir.LoadCustomVersion) { AccountInfo = this };

                if (info.Deleted && info.DeleteDate.AddMonths(Settings.ArchiveDeletedCharacterAfterMonths) <= Envir.Now)
                {
                    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.PlayerArchivedAfterDeletionMonths), info.Name, Settings.ArchiveDeletedCharacterAfterMonths));
                    Envir.SaveArchivedCharacter(info);
                    continue;
                }

                if (info.LastLoginDate == DateTime.MinValue && info.CreationDate.AddMonths(Settings.ArchiveInactiveCharacterAfterMonths) <= Envir.Now)
                {
                    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.PlayerArchivedAfterNoLoginMonths), info.Name, Settings.ArchiveInactiveCharacterAfterMonths));
                    Envir.SaveArchivedCharacter(info);
                    continue;
                }

                if (info.LastLoginDate > DateTime.MinValue && info.LastLoginDate.AddMonths(Settings.ArchiveInactiveCharacterAfterMonths) <= Envir.Now)
                {
                    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.PlayerArchivedAfterInactivityMonths), info.Name, Settings.ArchiveInactiveCharacterAfterMonths));
                    Envir.SaveArchivedCharacter(info);
                    continue;
                }

                Characters.Add(info);
            }

            if (Envir.LoadVersion > 75)
            {
                HasExpandedStorage = reader.ReadBoolean();
                ExpandedStorageExpiryDate = DateTime.FromBinary(reader.ReadInt64());
            }

            Gold = reader.ReadUInt32();
            if (Envir.LoadVersion >= 63) Credit = reader.ReadUInt32();

            count = reader.ReadInt32();

            Array.Resize(ref Storage, count);

            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                if (Envir.BindItem(item) && i < Storage.Length)
                    Storage[i] = item;
            }

            if (Envir.LoadVersion >= 10) AdminAccount = reader.ReadBoolean();
            if (!AdminAccount)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i] == null) continue;
                    if (Characters[i].Deleted) continue;
                    if ((Envir.Now - Characters[i].LastLogoutDate).TotalDays > 13) continue;
                    Envir.CheckRankUpdate(Characters[i]);
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(AccountID);
            writer.Write(Password);
            writer.Write(Salt.Length);
            writer.Write(Salt);
            writer.Write(RequirePasswordChange);

            writer.Write(UserName);
            writer.Write(BirthDate.ToBinary());
            writer.Write(SecretQuestion);
            writer.Write(SecretAnswer);
            writer.Write(EMailAddress);

            writer.Write(CreationIP);
            writer.Write(CreationDate.ToBinary());

            writer.Write(Banned);
            writer.Write(BanReason);
            writer.Write(ExpiryDate.ToBinary());

            writer.Write(LastIP);
            writer.Write(LastDate.ToBinary());

            writer.Write(Characters.Count);
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].Save(writer);
            }

            writer.Write(HasExpandedStorage);
            writer.Write(ExpandedStorageExpiryDate.ToBinary());
            writer.Write(Gold);
            writer.Write(Credit);
            writer.Write(Storage.Length);
            for (int i = 0; i < Storage.Length; i++)
            {
                writer.Write(Storage[i] != null);
                if (Storage[i] == null) continue;

                Storage[i].Save(writer);
            }
            writer.Write(AdminAccount);
        }

        public List<SelectInfo> GetSelectInfo()
        {
            List<SelectInfo> list = new List<SelectInfo>();

            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].Deleted) continue;
                list.Add(Characters[i].ToSelectInfo());
                if (list.Count >= Globals.MaxCharacterCount) break;
            }

            return list;
        }

        public int ExpandStorage()
        {
            if (!HasExpandedStorage)
            {
                if (Storage.Length == Globals.StorageGridSize)
                    Array.Resize(ref Storage, Storage.Length + Globals.StorageGridSize);
            }

            return Storage.Length;
        }

        public bool IsValidStorageIndex(int index)
        {
            if (index >= Globals.StorageGridSize)
            {
                var level = index / Globals.StorageGridSize;
                if (level > (HasExpandedStorage ? 1 : 0))
                    return false;
            }
            return true;
        }
    }
}
