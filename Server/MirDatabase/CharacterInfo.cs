using System.Drawing;
﻿using Server.MirEnvir;
using Server.MirNetwork;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class CharacterInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// ID
        /// </summary>
        public int Index;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 等级
        /// </summary>
        public ushort Level;
        /// <summary>
        /// 职业
        /// </summary>
        public MirClass Class;
        /// <summary>
        /// 性别
        /// </summary>
        public MirGender Gender;

        /// <summary>
        /// 发型
        /// </summary>
        public byte Hair;

        /// <summary>
        /// 公会ID，未加入为-1，加入则是公会编号
        /// </summary>
        public int GuildIndex = -1;

        /// <summary>
        /// 创建IP
        /// </summary>
        public string CreationIP;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate;
        /// <summary>
        /// 是否被引用
        /// </summary>
        public bool Banned;
        /// <summary>
        /// 禁用原因
        /// </summary>
        public string BanReason = string.Empty;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiryDate;

        /// <summary>
        /// 是否进制聊天
        /// </summary>
        public bool ChatBanned;

        /// <summary>
        /// 进制聊天过期时间
        /// </summary>
        public DateTime ChatBanExpiryDate;

        /// <summary>
        /// 最后登录的IP
        /// </summary>
        public string LastIP = string.Empty;
        /// <summary>
        /// 最后退出登录的日期
        /// </summary>
        public DateTime LastLogoutDate;

        /// <summary>
        /// 最后登录的日期
        /// </summary>
        public DateTime LastLoginDate;

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool Deleted;

        /// <summary>
        /// 删除日期
        /// </summary>
        public DateTime DeleteDate;

        //Marriage

        /// <summary>
        /// 婚姻状况，如果结婚 Married 指向对方的ID，没有结婚则等于0
        /// </summary>
        public int Married = 0;
        /// <summary>
        /// 结婚时间
        /// </summary>
        public DateTime MarriedDate;

        //Mentor 师徒系统
        public int Mentor = 0;
        /// <summary>
        /// 关系建立时间
        /// </summary>
        public DateTime MentorDate;

        /// <summary>
        /// 是否是师傅
        /// </summary>
        public bool IsMentor;
        /// <summary>
        /// 师徒值
        /// </summary>
        public long MentorExp = 0;

        //Location

        /// <summary>
        /// 所在地图的索引
        /// </summary>
        public int CurrentMapIndex;

        /// <summary>
        /// 所在的坐标
        /// </summary>
        public Point CurrentLocation;
        /// <summary>
        /// 所在的方向
        /// </summary>
        public MirDirection Direction;

        /// <summary>
        /// 复活点的地图索引
        /// </summary>
        public int BindMapIndex;

        /// <summary>
        /// 复活点的坐标
        /// </summary>
        public Point BindLocation;

        /// <summary>
        /// 生命值，魔法值
        /// </summary>
        public int HP, MP;

        /// <summary>
        /// 经验值
        /// </summary>
        public long Experience;

        /// <summary>
        /// 攻击模式
        /// </summary>
        public AttackMode AMode;

        /// <summary>
        /// 宠物模式
        /// </summary>
        public PetMode PMode;
        /// <summary>
        /// 是否允许组队
        /// </summary>
        public bool AllowGroup;

        /// <summary>
        /// 是否允许交易
        /// </summary>
        public bool AllowTrade;

        /// <summary>
        /// 是否允许观察
        /// </summary>
        public bool AllowObserve;
        /// <summary>
        /// PK值
        /// </summary>
        public int PKPoints;

        /// <summary>
        /// 是否是新的一天
        /// </summary>
        public bool NewDay;
        /// <summary>
        /// Thrusting 刺杀
        /// HalfMoon 半月
        /// CrossHalfMoon 十字半月
        /// </summary>
        public bool Thrusting, HalfMoon, CrossHalfMoon;
        /// <summary>
        /// 双击
        /// </summary>
        public bool DoubleSlash;

        /// <summary>
        /// 精神状态
        /// 1: 技巧射击
        /// 2: 群体攻击
        /// </summary>
        public byte MentalState;

        /// <summary>
        /// 精神状态(影响精神状态的效果强度)
        /// </summary>
        public byte MentalStateLvl;

        /// <summary>
        /// Inventory: 背包
        /// Equipment: 装备
        /// Trade: 交易
        /// QuestInventory: 任务物品栏
        /// Refine: 强化/镶嵌道具栏
        /// </summary>
        public UserItem[] Inventory = new UserItem[46], Equipment = new UserItem[14], Trade = new UserItem[10], QuestInventory = new UserItem[40], Refine = new UserItem[16];
        /// <summary>
        /// 租借道具
        /// </summary>
        public List<ItemRentalInformation> RentedItems = new List<ItemRentalInformation>();
        /// <summary>
        /// 待删除的租借道具
        /// </summary>
        public List<ItemRentalInformation> RentedItemsToRemove = new List<ItemRentalInformation>();
        /// <summary>
        /// 是否有租借道具
        /// </summary>
        public bool HasRentedItem;
        /// <summary>
        /// 当前强化的道具
        /// </summary>
        public UserItem CurrentRefine = null;

        /// <summary>
        /// 收集/强化计时
        /// </summary>
        public long CollectTime = 0, RefineTimeRemaining = 0;

        /// <summary>
        /// 已学技能列表
        /// </summary>
        public List<UserMagic> Magics = new List<UserMagic>();

        /// <summary>
        /// 宠物列表
        /// </summary>
        public List<PetInfo> Pets = new List<PetInfo>();

        /// <summary>
        /// 当前Buff
        /// </summary>
        public List<Buff> Buffs = new List<Buff>();
        /// <summary>
        /// 中毒状态或负面状态
        /// </summary>
        public List<Poison> Poisons = new List<Poison>();
        /// <summary>
        /// 邮件列表
        /// </summary>
        public List<MailInfo> Mail = new List<MailInfo>();
        /// <summary>
        /// 好有列表
        /// </summary>
        public List<FriendInfo> Friends = new List<FriendInfo>();
        /// <summary>
        /// 智能生物(物品道具召唤的)
        /// </summary>
        public List<UserIntelligentCreature> IntelligentCreatures = new List<UserIntelligentCreature>();
        /// <summary>
        /// 货币
        /// </summary>
        public int PearlCount;
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<QuestProgressInfo> CurrentQuests = new List<QuestProgressInfo>();
        /// <summary>
        /// 已完成任务列表
        /// </summary>
        public List<int> CompletedQuests = new List<int>();
        /// <summary>
        /// 任务是否满足条件的标记
        /// </summary>
        public bool[] Flags = new bool[Globals.FlagIndexCount];
        /// <summary>
        /// 账户信息
        /// </summary>
        public AccountInfo AccountInfo;

        /// <summary>
        /// 玩家实例
        /// </summary>
        public PlayerObject Player;

        /// <summary>
        /// 坐骑
        /// </summary>
        public MountInfo Mount;

        /// <summary>
        /// 商城购买记录
        /// </summary>
        public Dictionary<int, int> GSpurchases = new Dictionary<int, int>();

        /// <summary>
        /// 排行信息（不存数据库，不发送客户端）
        /// </summary>
        public int[] Rank = new int[2];//dont save this in db!(and dont send it to clients :p)
        /// <summary>
        /// 英雄上线数量
        /// </summary>
        public int MaximumHeroCount = 1;
        /// <summary>
        /// 英雄数组
        /// </summary>
        public HeroInfo[] Heroes;
        /// <summary>
        /// 当前激活的英雄索引
        /// </summary>
        public int CurrentHeroIndex;

        /// <summary>
        /// 英雄是否已召唤
        /// </summary>
        public bool HeroSpawned;
        /// <summary>
        /// 英雄行为模式（跟随、攻击、休息等）
        /// </summary>
        public HeroBehaviour HeroBehaviour;

        public CharacterInfo() { }
        /// <summary>
        /// 创建新角色
        /// </summary>
        /// <param name="p"> Packet </param>
        /// <param name="c"> MirConnection </param>
        public CharacterInfo(ClientPackets.NewCharacter p, MirConnection c)
        {
            Name = p.Name;
            Class = p.Class;
            Gender = p.Gender;
            Heroes = new HeroInfo[MaximumHeroCount];

            CreationIP = c.IPAddress;
            CreationDate = Envir.Now;
        }
        /// <summary>
        /// 加载角色
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        /// <param name="version">版本</param>
        /// <param name="customVersion">自定义版本</param>
        public CharacterInfo(BinaryReader reader, int version, int customVersion)
        {
            // 加载
            Load(reader, version, customVersion);
        }
        /// <summary>
        /// 生成角色实例
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        /// <param name="version">版本</param>
        /// <param name="customVersion">自定义版本</param>
        public virtual void Load(BinaryReader reader, int version, int customVersion)
        {
            // ID
            Index = reader.ReadInt32();
            // 角色名字
            Name = reader.ReadString();

            if (version < 62)
            {
                // 等级
                Level = (ushort)reader.ReadByte();
            }
            else
            {
                // 等级
                Level = reader.ReadUInt16();
            }
            // 角色
            Class = (MirClass)reader.ReadByte();
            // 性别
            Gender = (MirGender)reader.ReadByte();
            Hair = reader.ReadByte();

            CreationIP = reader.ReadString();
            CreationDate = DateTime.FromBinary(reader.ReadInt64());

            Banned = reader.ReadBoolean();
            BanReason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());

            LastIP = reader.ReadString();
            LastLogoutDate = DateTime.FromBinary(reader.ReadInt64());

            if (version > 81)
            {
                LastLoginDate = DateTime.FromBinary(reader.ReadInt64());
            }

            Deleted = reader.ReadBoolean();
            DeleteDate = DateTime.FromBinary(reader.ReadInt64());

            CurrentMapIndex = reader.ReadInt32();
            CurrentLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            BindMapIndex = reader.ReadInt32();
            BindLocation = new Point(reader.ReadInt32(), reader.ReadInt32());

            if (version <= 84)
            {
                HP = reader.ReadUInt16();
                MP = reader.ReadUInt16();
            }
            else
            {
                HP = reader.ReadInt32();
                MP = reader.ReadInt32();
            }

            Experience = reader.ReadInt64();

            AMode = (AttackMode)reader.ReadByte();
            PMode = (PetMode)reader.ReadByte();

            if (version > 34)
            {
                PKPoints = reader.ReadInt32();
            }
            #region 背包初始化
            int count = reader.ReadInt32();
            // 重置背包
            Array.Resize(ref Inventory, count);

            for (int i = 0; i < count; i++)
            {
                /*
                 * 数据格式：
                 * true 100 fasle false true 101
                 */
                if (!reader.ReadBoolean()) continue;
                // 初始化物品
                UserItem item = new UserItem(reader, version, customVersion);

                if (Envir.BindItem(item) && i < Inventory.Length)
                {
                    // 把物品放入背包
                    Inventory[i] = item;
                }
            }
            #endregion
            #region 装备初始化
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                // 初始化装备
                UserItem item = new UserItem(reader, version, customVersion);
                if (Envir.BindItem(item) && i < Equipment.Length)
                {
                    Equipment[i] = item;
                }
            }
            #endregion
            #region 初始化任务物品
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, version, customVersion);
                if (Envir.BindItem(item) && i < QuestInventory.Length)
                {
                    QuestInventory[i] = item;
                }
            }
            #endregion
            #region 初始化技能
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UserMagic magic = new UserMagic(reader, version, customVersion);
                if (magic.Info == null) continue;

                magic.CastTime = int.MinValue;
                Magics.Add(magic);
            }
            #endregion
            #region 战士技能开关
            Thrusting = reader.ReadBoolean();
            HalfMoon = reader.ReadBoolean();
            CrossHalfMoon = reader.ReadBoolean();
            DoubleSlash = reader.ReadBoolean();
            #endregion
            #region 弓箭手
            MentalState = reader.ReadByte();
            #endregion

            #region 宝宝初始化
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                // TODO: 重点看下 PetInfo
                Pets.Add(new PetInfo(reader, version, customVersion));
            }
            #endregion

            AllowGroup = reader.ReadBoolean();
            // 初始化任务标记
            for (int i = 0; i < Globals.FlagIndexCount; i++)
            {
                Flags[i] = reader.ReadBoolean();
            }

            GuildIndex = reader.ReadInt32();

            AllowTrade = reader.ReadBoolean();
            if (version > 104)
                AllowObserve = reader.ReadBoolean();

            #region 初始化任务进度

            count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                // TODO: 重点看 QuestProgressInfo
                QuestProgressInfo quest = new QuestProgressInfo(reader, version, customVersion);

                if (quest == null || quest.Info == null || quest.IsOrphan)
                {
                    Console.WriteLine($"[Load] Skipped orphan QuestProgress (Index={quest?.Index}) for character: {Name}");
                    continue;
                }
                if (Envir.BindQuest(quest))
                {
                    CurrentQuests.Add(quest);
                }
            }
            #endregion
            #region 添加buff
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Buff buff = new Buff(reader, version, customVersion);

                Buffs.Add(buff);
            }
            #endregion

            #region 添加邮件
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Mail.Add(new MailInfo(reader, version, customVersion));
            }
            #endregion

            #region 添加智能宠物
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UserIntelligentCreature creature = new UserIntelligentCreature(reader, version, customVersion);
                if (creature.Info == null) continue;
                IntelligentCreatures.Add(creature);
            }
            #endregion
            #region 无效代码
            if (version == 45)
            {
                var old1 = (IntelligentCreatureType)reader.ReadByte();
                var old2 = reader.ReadBoolean();
            }
            #endregion


            PearlCount = reader.ReadInt32();
            #region 完成的任务
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                CompletedQuests.Add(reader.ReadInt32());
            }
            #endregion

            #region 强化道具，TODO: 后面细看
            if (reader.ReadBoolean())
            {
                CurrentRefine = new UserItem(reader, version, customVersion);
            }

            if (CurrentRefine != null)
            {
                Envir.BindItem(CurrentRefine);
            }

            RefineTimeRemaining = reader.ReadInt64();

            CollectTime = Envir.Time + RefineTimeRemaining;
            #endregion

            #region 初始化好友列表
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Friends.Add(new FriendInfo(reader, version, customVersion));
            }
            #endregion

            // version 大于 75 的话有可能有租赁道具
            if (version > 75)
            {
                count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    RentedItems.Add(new ItemRentalInformation(reader, version, customVersion));
                }

                HasRentedItem = reader.ReadBoolean();
            }
            #region 婚姻状况
            Married = reader.ReadInt32();
            MarriedDate = DateTime.FromBinary(reader.ReadInt64());
            Mentor = reader.ReadInt32();
            MentorDate = DateTime.FromBinary(reader.ReadInt64());
            #endregion
            #region 师徒状况
            IsMentor = reader.ReadBoolean();
            MentorExp = reader.ReadInt64();
            #endregion
            #region 购买记录
            if (version >= 63)
            {
                int logCount = reader.ReadInt32();

                for (int i = 0; i < logCount; i++)
                {
                    GSpurchases.Add(reader.ReadInt32(), reader.ReadInt32());
                }
            }
            #endregion
            #region 初始化英雄
            if (version > 98)
            {
                MaximumHeroCount = reader.ReadInt32();
                Heroes = new HeroInfo[MaximumHeroCount];
                if (version > 102)
                {
                    for (int i = 0; i < MaximumHeroCount; i++)
                    {
                        int heroIndex = reader.ReadInt32();
                        if (heroIndex > 0)
                            Heroes[i] = Envir.GetHeroInfo(heroIndex);
                    }
                }
                else
                {
                    for (int i = 0; i < MaximumHeroCount; i++)
                        Heroes[i] = new HeroInfo(reader, version, customVersion);
                }

                if (version < 104) reader.ReadInt32();
                CurrentHeroIndex = reader.ReadInt32();
                HeroSpawned = reader.ReadBoolean();
            }
            else Heroes = new HeroInfo[MaximumHeroCount];
            #endregion
            // 英雄模式
            if (version > 100)
                HeroBehaviour = (HeroBehaviour)reader.ReadByte();

        }

        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write((byte) Class);
            writer.Write((byte) Gender);
            writer.Write(Hair);

            writer.Write(CreationIP);
            writer.Write(CreationDate.ToBinary());

            writer.Write(Banned);
            writer.Write(BanReason);
            writer.Write(ExpiryDate.ToBinary());

            writer.Write(LastIP);
            writer.Write(LastLogoutDate.ToBinary());
            writer.Write(LastLoginDate.ToBinary());

            writer.Write(Deleted);
            writer.Write(DeleteDate.ToBinary());

            writer.Write(CurrentMapIndex);
            writer.Write(CurrentLocation.X);
            writer.Write(CurrentLocation.Y);
            writer.Write((byte)Direction);
            writer.Write(BindMapIndex);
            writer.Write(BindLocation.X);
            writer.Write(BindLocation.Y);

            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Experience);

            writer.Write((byte) AMode);
            writer.Write((byte) PMode);

            writer.Write(PKPoints);

            writer.Write(Inventory.Length);
            for (int i = 0; i < Inventory.Length; i++)
            {
                writer.Write(Inventory[i] != null);
                if (Inventory[i] == null) continue;

                Inventory[i].Save(writer);
            }

            writer.Write(Equipment.Length);
            for (int i = 0; i < Equipment.Length; i++)
            {
                writer.Write(Equipment[i] != null);
                if (Equipment[i] == null) continue;

                Equipment[i].Save(writer);
            }

            writer.Write(QuestInventory.Length);
            for (int i = 0; i < QuestInventory.Length; i++)
            {
                writer.Write(QuestInventory[i] != null);
                if (QuestInventory[i] == null) continue;

                QuestInventory[i].Save(writer);
            }

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].Save(writer);
            }

            writer.Write(Thrusting);
            writer.Write(HalfMoon);
            writer.Write(CrossHalfMoon);
            writer.Write(DoubleSlash);
            writer.Write(MentalState);

            writer.Write(Pets.Count);
            for (int i = 0; i < Pets.Count; i++)
            {
                Pets[i].Save(writer);
            }

            writer.Write(AllowGroup);

            for (int i = 0; i < Flags.Length; i++)
            {
                writer.Write(Flags[i]);
            }

            writer.Write(GuildIndex);

            writer.Write(AllowTrade);
            writer.Write(AllowObserve);

            writer.Write(CurrentQuests.Count);
            for (int i = 0; i < CurrentQuests.Count; i++)
            {
                CurrentQuests[i].Save(writer);
            }

            writer.Write(Buffs.Count);
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].Save(writer);
            }

            writer.Write(Mail.Count);
            for (int i = 0; i < Mail.Count; i++)
            {
                Mail[i].Save(writer);
            }

            writer.Write(IntelligentCreatures.Count);
            for (int i = 0; i < IntelligentCreatures.Count; i++)
            {
                IntelligentCreatures[i].Save(writer);
            }

            writer.Write(PearlCount);

            writer.Write(CompletedQuests.Count);
            for (int i = 0; i < CompletedQuests.Count; i++)
            {
                writer.Write(CompletedQuests[i]);
            }


            writer.Write(CurrentRefine != null);
            if (CurrentRefine != null)
            {
                CurrentRefine.Save(writer);
            }

            RefineTimeRemaining = CollectTime - Envir.Time;

            if (RefineTimeRemaining < 0)
                RefineTimeRemaining = 0;

            writer.Write(RefineTimeRemaining);

            writer.Write(Friends.Count);
            for (int i = 0; i < Friends.Count; i++)
            {
                if (Friends[i].Info == null) continue;
                Friends[i].Save(writer);
            }

            writer.Write(RentedItems.Count);
            foreach (var rentedItemInformation in RentedItems)
            {
                rentedItemInformation.Save(writer);
            }

            writer.Write(HasRentedItem);

            writer.Write(Married);
            writer.Write(MarriedDate.ToBinary());
            writer.Write(Mentor);
            writer.Write(MentorDate.ToBinary());
            writer.Write(IsMentor);
            writer.Write(MentorExp);

            writer.Write(GSpurchases.Count);

            foreach (var item in GSpurchases)
            {
                writer.Write(item.Key);
                writer.Write(item.Value);
            }

            writer.Write(MaximumHeroCount);
            for (int i = 0; i < Heroes.Length; i++)
                writer.Write(Heroes[i] != null ? Heroes[i].Index : 0);
            writer.Write(CurrentHeroIndex);
            writer.Write(HeroSpawned);
            writer.Write((byte)HeroBehaviour);
        }

        public SelectInfo ToSelectInfo()
        {
            return new SelectInfo
                {
                    Index = Index,
                    Name = Name,
                    Level = Level,
                    Class = Class,
                    Gender = Gender,
                    LastAccess = LastLogoutDate
                };
        }

        /// <summary>
        /// 是否有智能宠物
        /// </summary>
        /// <param name="petType"></param>
        /// <returns></returns>
        public bool CheckHasIntelligentCreature(IntelligentCreatureType petType)
        {
            for (int i = 0; i < IntelligentCreatures.Count; i++)
            {
                if (IntelligentCreatures[i].PetType == petType) return true;
            }

            return false;
        }
        /// <summary>
        /// 重置背包
        /// </summary>
        /// <returns>背包空间数量</returns>
        public virtual int ResizeInventory()
        {
            if (Inventory.Length >= 86) return Inventory.Length;

            if (Inventory.Length == 46)
            {
                Array.Resize(ref Inventory, Inventory.Length + 8);
            }
            else
            {
                Array.Resize(ref Inventory, Inventory.Length + 4);
            }

            return Inventory.Length;
        }
    }

    public class PetInfo
    {
        public int MonsterIndex;
        public int HP;
        public uint Experience;
        public byte Level, MaxPetLevel;

        public long TameTime;

        public PetInfo(MonsterObject ob)
        {
            MonsterIndex = ob.Info.Index;
            HP = ob.HP;
            Experience = ob.PetExperience;
            Level = ob.PetLevel;
            MaxPetLevel = ob.MaxPetLevel;
        }

        public PetInfo(BinaryReader reader, int version, int customVersion)
        {
            MonsterIndex = reader.ReadInt32();
            if (MonsterIndex == 271) MonsterIndex = 275;

            if (version <= 84)
            {
                HP = (int)reader.ReadUInt32();
            }
            else
            {
                HP = reader.ReadInt32();
            }

            Experience = reader.ReadUInt32();
            Level = reader.ReadByte();
            MaxPetLevel = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterIndex);
            writer.Write(HP);
            writer.Write(Experience);
            writer.Write(Level);
            writer.Write(MaxPetLevel);
        }
    }
    /// <summary>
    /// MountInfo 用于封装玩家的坐骑信息，判断玩家是否可以骑乘、攻击等操作
    /// </summary>
    public class MountInfo
    {

        private bool PlayerHasMap
        {
            get { return Player != null && Player.CurrentMap != null; }
        }

        /// <summary>
        /// 持有的玩家对象
        /// </summary>
        public HumanObject Player;
        /// <summary>
        /// 坐骑类型，-1 表示未设置或没有坐骑
        /// </summary>
        public short MountType = -1;
        /// <summary>
        /// 是否可以骑乘坐骑（需要有坐骑且鞍具已装备）
        /// </summary>
        public bool CanRide
        {
            get { return HasMount && Slots[(int)MountSlot.Saddle] != null; }
        }
        /// <summary>
        /// 是否可以在当前地图骑乘坐骑（地图允许骑乘且玩家有坐骑）
        /// </summary>
        public bool CanMapRide
        {
            get { return HasMount && PlayerHasMap && !Player.CurrentMap.Info.NoMount; }
        }
        /// <summary>
        /// 是否可以在副本或特殊地图骑乘坐骑
        /// 条件：
        /// 1. 玩家有坐骑
        /// 2. 当前地图允许骑乘
        /// 3. 如果地图需要马勒（Bridle），则必须装备马勒
        /// </summary>
        public bool CanDungeonRide
        {
            get { return HasMount && CanMapRide && (!Player.CurrentMap.Info.NeedBridle || Slots[(int)MountSlot.Reins] != null); }
        }
        /// <summary>
        /// 是否可以骑乘状态下进行攻击
        /// 条件：
        /// 1. 玩家有坐骑且装备了铃铛（Bells）
        /// 2. 或者玩家没有骑乘坐骑
        /// </summary>
        public bool CanAttack
        {
            get { return HasMount && Slots[(int)MountSlot.Bells] != null || !RidingMount; }
        }
        /// <summary>
        /// 是否能够降低坐骑的忠诚度消耗（装备缎带 Ribbon）
        /// </summary>
        public bool SlowLoyalty
        {
            get { return HasMount && Slots[(int)MountSlot.Ribbon] != null; }
        }
        /// <summary>
        /// 玩家是否拥有坐骑（玩家装备栏中 Mount 插槽非空）
        /// </summary>
        public bool HasMount
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount] != null; }
        }
        /// <summary>
        /// 玩家是否正在骑乘坐骑
        /// </summary>
        private bool RidingMount
        {
            get { return Player.RidingMount; }
            set { Player.RidingMount = value; }
        }
        /// <summary>
        /// 坐骑的各个装备槽位（鞍、缰绳、铃铛、缎带等）
        /// </summary>
        public UserItem[] Slots
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount].Slots; }
        }

        /// <summary>
        /// 构造函数，绑定玩家对象
        /// </summary>
        /// <param name="ob">HumanObject 玩家对象</param>
        public MountInfo(HumanObject ob)
        {
            Player = ob;
        }
    }

    public class FriendInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public int Index;

        private CharacterInfo _Info;
        public CharacterInfo Info
        {
            get
            {
                if (_Info == null)
                {
                    _Info = Envir.GetCharacterInfo(Index);
                }

                return _Info;
            }
        }

        public bool Blocked;
        public string Memo;

        public FriendInfo(CharacterInfo info, bool blocked)
        {
            Index = info.Index;
            Blocked = blocked;
            Memo = "";
        }

        public FriendInfo(BinaryReader reader, int version, int customVersion)
        {
            Index = reader.ReadInt32();
            Blocked = reader.ReadBoolean();
            Memo = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Blocked);
            writer.Write(Memo);
        }

        public ClientFriend CreateClientFriend()
        {
            return new ClientFriend()
            {
                Index = Index,
                Name = Info.Name,
                Blocked = Blocked,
                Memo = Memo,
                Online = Info.Player != null && Info.Player.Node != null
            };
        }
    }
}
