using System.Collections.Concurrent;
using System.Net.Sockets;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using C = ClientPackets;
using S = ServerPackets;
using System.Text.RegularExpressions;
using Server.Utils;

namespace Server.MirNetwork
{
    public enum GameStage { None, Login, Select, Game, Observer, Disconnected }

    public class MirConnection
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }

        public readonly int SessionID;
        public readonly string IPAddress;
        /// <summary>
        /// 游戏当前的状态， Stage 构造函数里没有初始化，C# 默认是 0，也就是 None
        /// </summary>
        public GameStage Stage;

        private TcpClient _client;
        private ConcurrentQueue<Packet> _receiveList;
        private ConcurrentQueue<Packet> _sendList;
        private Queue<Packet> _retryList;

        private bool _disconnecting;
        public bool Connected;

        /// <summary>
        /// 用于标记断开连接，会保持 500ms的时间，过期后会在过期的那次tick中清理掉连接
        /// </summary>
        public bool Disconnecting
        {
            get { return _disconnecting; }
            set
            {
                if (_disconnecting == value) return;
                _disconnecting = value;
                TimeOutTime = Envir.Time + 500;
            }
        }
        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        byte[] _rawData = new byte[0];
        byte[] _rawBytes = new byte[8 * 1024];

        /// <summary>
        /// 账户信息
        /// </summary>
        public AccountInfo Account;
        public PlayerObject Player;
        /// <summary>
        /// 观战系统集合
        /// 存放所有正在观战本玩家的连接
        /// 举例：PlayerA 观察 PlayerB
        /// 则 PlayerB.Observers 包含 PlayerA
        /// 一对多关系：一个玩家可以被多人观战
        /// </summary>
        public List<MirConnection> Observers = new List<MirConnection>();

        /// <summary>
        /// 我正在观察谁
        /// 举例：PlayerA.Observing = PlayerB
        /// Observing 表示“正在观察的对象”
        /// 一对一关系：一个玩家同时只能观战一个目标
        /// </summary>
        public MirConnection Observing;

        public List<ItemInfo> SentItemInfo = new List<ItemInfo>();
        public List<MonsterInfo> SentMonsterInfo = new List<MonsterInfo>();
        public List<NPCInfo> SentNPCInfo = new List<NPCInfo>();
        public List<QuestInfo> SentQuestInfo = new List<QuestInfo>();
        public List<RecipeInfo> SentRecipeInfo = new List<RecipeInfo>();
        public List<UserItem> SentChatItem = new List<UserItem>(); //TODO - Add Expiry time
        public List<MapInfo> SentMapInfo = new List<MapInfo>();
        public List<ulong> SentHeroInfo = new List<ulong>();
        public bool WorldMapSetupSent;
        public bool StorageSent;
        public bool HeroStorageSent;
        public Dictionary<long, DateTime> SentRankings = new Dictionary<long, DateTime>();

        private DateTime _dataCounterReset;
        private int _dataCounter;
        private FixedSizedQueue<Packet> _lastPackets;

        public MirConnection(int sessionID, TcpClient client)
        {
            SessionID = sessionID;
            IPAddress = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            Envir.UpdateIPBlock(IPAddress, TimeSpan.FromSeconds(Settings.IPBlockSeconds));

            MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.IPAddressConnected), IPAddress));

            _client = client;
            _client.NoDelay = true;

            TimeConnected = Envir.Time;
            TimeOutTime = TimeConnected + Settings.TimeOut;

            _lastPackets = new FixedSizedQueue<Packet>(10);

            _receiveList = new ConcurrentQueue<Packet>();
            _sendList = new ConcurrentQueue<Packet>();
            _sendList.Enqueue(new S.Connected());
            _retryList = new Queue<Packet>();

            Connected = true;
            BeginReceive();
        }

        public void AddObserver(MirConnection c)
        {
            Observers.Add(c);

            if (c.Observing != null)
                c.Observing.Observers.Remove(c);
            c.Observing = this;

            c.Stage = GameStage.Observer;
        }

        private void BeginReceive()
        {
            if (!Connected) return;

            try
            {
                // _rawBytes 默认 8K 缓冲区，用于接受客户端发送的数据，如果一次接受>8K 会分多次接收，如果大于次数上限会锁定IP，不在接收
                _client.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
            }
            catch
            {
                Disconnecting = true;
            }
        }

        private void ReceiveData(IAsyncResult result)
        {
            // 连接断开时，直接返回
            if (!Connected) return;

            int dataRead; // 存储客户端发送的数据

            try
            {
                dataRead = _client.Client.EndReceive(result); // 读取数据
            }
            catch
            {
                Disconnecting = true; // 读取失败，标记为待断开
                return;
            }

            if (dataRead == 0) // 未读取到数据
            {
                Disconnecting = true; // 标记为待断开
                return;
            }

            if (_dataCounterReset < Envir.Now) // 判断是否需要重置数据计数器（Envir.now 是服务器运行时间）
            {
                _dataCounterReset = Envir.Now.AddSeconds(5);// 5秒内未收到数据，重置计数器
                _dataCounter = 0; // 计数器清零
            }

            _dataCounter++;

            try
            {
                byte[] rawBytes = result.AsyncState as byte[];

                byte[] temp = _rawData;
                _rawData = new byte[dataRead + temp.Length];
                Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
                Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

                Packet p;
                // 取出数据放到队列中等待下次tick中解析
                // ReceivePacket 会处理黏包问题
                // out: 允许吧内部变量赋值给外部变量，达到内部修改外部也会跟着变的效果，out是成对出现的，两个out变量是双向绑定的
                while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
                    _receiveList.Enqueue(p);
            }
            catch
            {
                // 解析错误后封ip24小时
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));

                MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.IPAddressDisconnectedInvalidPacket), IPAddress));
                // 标记为待断开
                Disconnecting = true;
                return;
            }
            // 如果包太大(因为tcp会自动分成多个小包，会导致频繁回调自身)或者频繁调用服务端接口，封ip24小时
            if (_dataCounter > Settings.MaxPacket)
            {
                // 封24小时
                Envir.UpdateIPBlock(IPAddress, TimeSpan.FromHours(24));
                // 创建包裹List
                List<string> packetList = new List<string>();
                // 取出最近收到的10个包
                while (_lastPackets.Count > 0)
                {
                    // 尝试取出包转成 Packet 类型 并赋值给 pkt（C#7.0 支持不声明类型直接通过 out Packet pkt 声明一个局部变量）
                    _lastPackets.TryDequeue(out Packet pkt);
                    // 尝试转成ClientPacketIds枚举类型，转不成功是0.toString()，转成功就是 Index.toString() 复制给cPacket
                    Enum.TryParse<ClientPacketIds>((pkt?.Index ?? 0).ToString(), out ClientPacketIds cPacket);
                    // 添加到 packetList 中
                    packetList.Add(cPacket.ToString());
                }

                MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.IPAddressDisconnectedLargePackets), IPAddress, String.Join(",", packetList.Distinct())));
                // 标记为关闭状态
                Disconnecting = true;
                return;
            }
            // 继续接收下次tcp信息
            BeginReceive();
        }
        private void BeginSend(List<byte> data)
        {
            if (!Connected || data.Count == 0) return;

            //Interlocked.Add(ref Network.Sent, data.Count);

            try
            {
                // 发送数据，SendData 作为回调函数
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, Disconnecting);
            }
            catch
            {
                // 发送失败,Disconnecting = true,等待释放
                Disconnecting = true;
            }
        }
        // 发送完成
        private void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result); // 清理异步上下文
            }
            catch
            { }

        }
        /// <summary>
        /// 加入消息队列
        /// </summary>
        /// <param name="p"></param>
        public void Enqueue(Packet p)
        {
            if (p == null) return;
            // 加入消息队列
            if (_sendList != null && p != null)
                // _sendList 会在 Process 中处理
                _sendList.Enqueue(p);
            // 如果不是观察者返回
            if (!p.Observable) return;
            // 遍历 Observers对象
            foreach (MirConnection c in Observers)
                c.Enqueue(p);
        }
        // 在 Envir.cs 中调用，约 20ms调用一次
        public void Process()
        {
            if (_client == null || !_client.Connected)
            {
                // 20: 表示当客户端连接对象为空或未连接时使用（网络连接已断开）
                Disconnect(20);
                return;
            }
            // 如果不为空解析数据包
            // 尝试取出数据放到_lastPackets 队列中
            // _receiveList 出队， _lastPackets 入队
            while (!_receiveList.IsEmpty && !Disconnecting)
            {
                Packet p;
                if (!_receiveList.TryDequeue(out p)) continue;
                // 记录最近收到的数据包，大小10 用于封IP时记录封禁原因
                _lastPackets.Enqueue(p);
                // 更新超时时间
                TimeOutTime = Envir.Time + Settings.TimeOut;
                // 解析用户发送过来的信息
                ProcessPacket(p);

                if (_receiveList == null)
                    return;
            }
            // 把重试的数据放到_receiveList中
            // 重试数据包含角色冷却中发到服务端的指令
            while (_retryList.Count > 0)
                _receiveList.Enqueue(_retryList.Dequeue());
            // 超时时断开连接
            if (Envir.Time > TimeOutTime)
            {
                // 超时
                Disconnect(21);
                return;
            }
            #region "检查有没有要发送的数据，如果有放入data中"
            if (_sendList == null || _sendList.Count <= 0) return;

            List<byte> data = new List<byte>();

            while (!_sendList.IsEmpty)
            {
                Packet p;
                if (!_sendList.TryDequeue(out p) || p == null) continue;
                data.AddRange(p.GetPacketBytes());
            }
            #endregion
            // 发送数据data
            BeginSend(data);
        }
        private void ProcessPacket(Packet p)
        {
            if (p == null || Disconnecting) return;

            switch (p.Index)
            {
                // 检查版本
                case (short)ClientPacketIds.ClientVersion:
                	//检查客户端版本是否与服务端版本匹配
                    ClientVersion((C.ClientVersion) p);
                    break;
                case (short)ClientPacketIds.Disconnect:
                    Disconnect(22);
                    break;
                case (short)ClientPacketIds.KeepAlive: // Keep Alive
                    ClientKeepAlive((C.KeepAlive)p);
                    break;
                case (short)ClientPacketIds.NewAccount:
                    NewAccount((C.NewAccount) p);
                    break;
                case (short)ClientPacketIds.ChangePassword:
                    ChangePassword((C.ChangePassword) p);
                    break;
                case (short)ClientPacketIds.Login:
                    // 登录
                    Login((C.Login) p);
                    break;
                case (short)ClientPacketIds.NewCharacter:
                    NewCharacter((C.NewCharacter) p);
                    break;
                case (short)ClientPacketIds.DeleteCharacter:
                    DeleteCharacter((C.DeleteCharacter) p);
                    break;
                case (short)ClientPacketIds.StartGame:
                    StartGame((C.StartGame) p);
                    break;
                case (short)ClientPacketIds.LogOut:
                    LogOut();
                    break;
                case (short)ClientPacketIds.Turn:
                    Turn((C.Turn) p);
                    break;
                case (short)ClientPacketIds.Walk:
                    Walk((C.Walk) p);
                    break;
                case (short)ClientPacketIds.Run:
                    Run((C.Run) p);
                    break;
                case (short)ClientPacketIds.Chat:
                    Chat((C.Chat) p);
                    break;
                case (short)ClientPacketIds.MoveItem:
                    MoveItem((C.MoveItem) p);
                    break;
                case (short)ClientPacketIds.StoreItem:
                    StoreItem((C.StoreItem) p);
                    break;
                case (short)ClientPacketIds.DepositRefineItem:
                    DepositRefineItem((C.DepositRefineItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRefineItem:
                    RetrieveRefineItem((C.RetrieveRefineItem)p);
                    break;
                case (short)ClientPacketIds.RefineCancel:
                    RefineCancel((C.RefineCancel)p);
                    break;
                case (short)ClientPacketIds.RefineItem:
                    RefineItem((C.RefineItem)p);
                    break;
                case (short)ClientPacketIds.CheckRefine:
                    CheckRefine((C.CheckRefine)p);
                    break;
                case (short)ClientPacketIds.ReplaceWedRing:
                    ReplaceWedRing((C.ReplaceWedRing)p);
                    break;
                case (short)ClientPacketIds.DepositTradeItem:
                    DepositTradeItem((C.DepositTradeItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveTradeItem:
                    RetrieveTradeItem((C.RetrieveTradeItem)p);
                    break;
                case (short)ClientPacketIds.TakeBackItem:
                    TakeBackItem((C.TakeBackItem) p);
                    break;
                case (short)ClientPacketIds.MergeItem:
                    MergeItem((C.MergeItem) p);
                    break;
                case (short)ClientPacketIds.EquipItem:
                    EquipItem((C.EquipItem) p);
                    break;
                case (short)ClientPacketIds.RemoveItem:
                    RemoveItem((C.RemoveItem) p);
                    break;
                case (short)ClientPacketIds.RemoveSlotItem:
                    RemoveSlotItem((C.RemoveSlotItem)p);
                    break;
                case (short)ClientPacketIds.SplitItem:
                    SplitItem((C.SplitItem) p);
                    break;
                case (short)ClientPacketIds.UseItem:
                    UseItem((C.UseItem) p);
                    break;
                case (short)ClientPacketIds.DropItem:
                    DropItem((C.DropItem) p);
                    break;
                case (short)ClientPacketIds.TakeBackHeroItem:
                    TakeBackHeroItem((C.TakeBackHeroItem)p);
                    break;
                case (short)ClientPacketIds.TransferHeroItem:
                    TransferHeroItem((C.TransferHeroItem)p);
                    break;
                case (short)ClientPacketIds.DropGold:
                    DropGold((C.DropGold) p);
                    break;
                case (short)ClientPacketIds.PickUp:
                    PickUp();
                    break;
                case (short)ClientPacketIds.RequestMapInfo:
                    RequestMapInfo((C.RequestMapInfo)p);
                    break;
                case (short)ClientPacketIds.RequestMonsterInfo:
                    RequestMonsterInfo((C.RequestMonsterInfo)p);
                    break;
                case (short)ClientPacketIds.RequestNPCInfo:
                    RequestNPCInfo((C.RequestNPCInfo)p);
                    break;
                case (short)ClientPacketIds.RequestItemInfo:
                    RequestItemInfo((C.RequestItemInfo)p);
                    break;
                case (short)ClientPacketIds.TeleportToNPC:
                    TeleportToNPC((C.TeleportToNPC)p);
                    break;
                case (short)ClientPacketIds.SearchMap:
                    SearchMap((C.SearchMap)p);
                    break;
                case (short)ClientPacketIds.Inspect:
                    Inspect((C.Inspect)p);
                    break;
                case (short)ClientPacketIds.Observe:
                    Observe((C.Observe)p);
                    break;
                case (short)ClientPacketIds.ChangeAMode:
                    ChangeAMode((C.ChangeAMode)p);
                    break;
                case (short)ClientPacketIds.ChangePMode:
                    ChangePMode((C.ChangePMode)p);
                    break;
                case (short)ClientPacketIds.ChangeTrade:
                    ChangeTrade((C.ChangeTrade)p);
                    break;
                case (short)ClientPacketIds.Attack:
                    Attack((C.Attack)p);
                    break;
                case (short)ClientPacketIds.RangeAttack:
                    RangeAttack((C.RangeAttack)p);
                    break;
                case (short)ClientPacketIds.Harvest:
                    Harvest((C.Harvest)p);
                    break;
                case (short)ClientPacketIds.CallNPC:
                    CallNPC((C.CallNPC)p);
                    break;
                case (short)ClientPacketIds.BuyItem:
                    BuyItem((C.BuyItem)p);
                    break;
                case (short)ClientPacketIds.CraftItem:
                    CraftItem((C.CraftItem)p);
                    break;
                case (short)ClientPacketIds.SellItem:
                    SellItem((C.SellItem)p);
                    break;
                case (short)ClientPacketIds.RepairItem:
                    RepairItem((C.RepairItem)p);
                    break;
                case (short)ClientPacketIds.BuyItemBack:
                    BuyItemBack((C.BuyItemBack)p);
                    break;
                case (short)ClientPacketIds.SRepairItem:
                    SRepairItem((C.SRepairItem)p);
                    break;
                case (short)ClientPacketIds.MagicKey:
                    MagicKey((C.MagicKey)p);
                    break;
                case (short)ClientPacketIds.Magic:
                    Magic((C.Magic)p);
                    break;
                case (short)ClientPacketIds.SwitchGroup:
                    SwitchGroup((C.SwitchGroup)p);
                    return;
                case (short)ClientPacketIds.AddMember:
                    AddMember((C.AddMember)p);
                    return;
                case (short)ClientPacketIds.DellMember:
                    DelMember((C.DelMember)p);
                    return;
                case (short)ClientPacketIds.GroupInvite:
                    GroupInvite((C.GroupInvite)p);
                    return;
                case (short)ClientPacketIds.NewHero:
                    NewHero((C.NewHero)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotValue:
                    SetAutoPotValue((C.SetAutoPotValue)p);
                    break;
                case (short)ClientPacketIds.SetAutoPotItem:
                    SetAutoPotItem((C.SetAutoPotItem)p);
                    break;
                case (short)ClientPacketIds.SetHeroBehaviour:
                    SetHeroBehaviour((C.SetHeroBehaviour)p);
                    break;
                case (short)ClientPacketIds.ChangeHero:
                    ChangeHero((C.ChangeHero)p);
                    break;
                case (short)ClientPacketIds.TownRevive:
                    TownRevive();
                    return;
                case (short)ClientPacketIds.SpellToggle:
                    SpellToggle((C.SpellToggle)p);
                    return;
                case (short)ClientPacketIds.ConsignItem:
                    ConsignItem((C.ConsignItem)p);
                    return;
                case (short)ClientPacketIds.MarketSearch:
                    MarketSearch((C.MarketSearch)p);
                    return;
                case (short)ClientPacketIds.MarketRefresh:
                    MarketRefresh();
                    return;
                case (short)ClientPacketIds.MarketPage:
                    MarketPage((C.MarketPage) p);
                    return;
                case (short)ClientPacketIds.MarketBuy:
                    MarketBuy((C.MarketBuy)p);
                    return;
                case (short)ClientPacketIds.MarketGetBack:
                    MarketGetBack((C.MarketGetBack)p);
                    return;
                case (short)ClientPacketIds.MarketSellNow:
                    MarketSellNow((C.MarketSellNow)p);
                    return;
                case (short)ClientPacketIds.RequestUserName:
                    RequestUserName((C.RequestUserName)p);
                    return;
                case (short)ClientPacketIds.RequestChatItem:
                    RequestChatItem((C.RequestChatItem)p);
                    return;
                case (short)ClientPacketIds.EditGuildMember:
                    EditGuildMember((C.EditGuildMember)p);
                    return;
                case (short)ClientPacketIds.EditGuildNotice:
                    EditGuildNotice((C.EditGuildNotice)p);
                    return;
                case (short)ClientPacketIds.GuildInvite:
                    GuildInvite((C.GuildInvite)p);
                    return;
                case (short)ClientPacketIds.RequestGuildInfo:
                    RequestGuildInfo((C.RequestGuildInfo)p);
                    return;
                case (short)ClientPacketIds.GuildNameReturn:
                    GuildNameReturn((C.GuildNameReturn)p);
                    return;
                case (short)ClientPacketIds.GuildStorageGoldChange:
                    GuildStorageGoldChange((C.GuildStorageGoldChange)p);
                    return;
                case (short)ClientPacketIds.GuildStorageItemChange:
                    GuildStorageItemChange((C.GuildStorageItemChange)p);
                    return;
                case (short)ClientPacketIds.GuildWarReturn:
                    GuildWarReturn((C.GuildWarReturn)p);
                    return;
                case (short)ClientPacketIds.MarriageRequest:
                    MarriageRequest((C.MarriageRequest)p);
                    return;
                case (short)ClientPacketIds.MarriageReply:
                    MarriageReply((C.MarriageReply)p);
                    return;
                case (short)ClientPacketIds.ChangeMarriage:
                    ChangeMarriage((C.ChangeMarriage)p);
                    return;
                case (short)ClientPacketIds.DivorceRequest:
                    DivorceRequest((C.DivorceRequest)p);
                    return;
                case (short)ClientPacketIds.DivorceReply:
                    DivorceReply((C.DivorceReply)p);
                    return;
                case (short)ClientPacketIds.AddMentor:
                    AddMentor((C.AddMentor)p);
                    return;
                case (short)ClientPacketIds.MentorReply:
                    MentorReply((C.MentorReply)p);
                    return;
                case (short)ClientPacketIds.AllowMentor:
                    AllowMentor((C.AllowMentor)p);
                    return;
                case (short)ClientPacketIds.CancelMentor:
                    CancelMentor((C.CancelMentor)p);
                    return;
                case (short)ClientPacketIds.TradeRequest:
                    TradeRequest((C.TradeRequest)p);
                    return;
                case (short)ClientPacketIds.TradeGold:
                    TradeGold((C.TradeGold)p);
                    return;
                case (short)ClientPacketIds.TradeReply:
                    TradeReply((C.TradeReply)p);
                    return;
                case (short)ClientPacketIds.TradeConfirm:
                    TradeConfirm((C.TradeConfirm)p);
                    return;
                case (short)ClientPacketIds.TradeCancel:
                    TradeCancel((C.TradeCancel)p);
                    return;
                case (short)ClientPacketIds.EquipSlotItem:
                    EquipSlotItem((C.EquipSlotItem)p);
                    break;
                case (short)ClientPacketIds.FishingCast:
                    FishingCast((C.FishingCast)p);
                    break;
                case (short)ClientPacketIds.FishingChangeAutocast:
                    FishingChangeAutocast((C.FishingChangeAutocast)p);
                    break;
                case (short)ClientPacketIds.AcceptQuest:
                    AcceptQuest((C.AcceptQuest)p);
                    break;
                case (short)ClientPacketIds.FinishQuest:
                    FinishQuest((C.FinishQuest)p);
                    break;
                case (short)ClientPacketIds.AbandonQuest:
                    AbandonQuest((C.AbandonQuest)p);
                    break;
                case (short)ClientPacketIds.ShareQuest:
                    ShareQuest((C.ShareQuest)p);
                    break;
                case (short)ClientPacketIds.AcceptReincarnation:
                    AcceptReincarnation();
                    break;
                case (short)ClientPacketIds.CancelReincarnation:
                     CancelReincarnation();
                    break;
                case (short)ClientPacketIds.CombineItem:
                    CombineItem((C.CombineItem)p);
                    break;
                case (short)ClientPacketIds.AwakeningNeedMaterials:
                    AwakeningNeedMaterials((C.AwakeningNeedMaterials)p);
                    break;
                case (short)ClientPacketIds.AwakeningLockedItem:
                    Enqueue(new S.AwakeningLockedItem { UniqueID = ((C.AwakeningLockedItem)p).UniqueID, Locked = ((C.AwakeningLockedItem)p).Locked });
                    break;
                case (short)ClientPacketIds.Awakening:
                    Awakening((C.Awakening)p);
                    break;
                case (short)ClientPacketIds.DisassembleItem:
                    DisassembleItem((C.DisassembleItem)p);
                    break;
                case (short)ClientPacketIds.DowngradeAwakening:
                    DowngradeAwakening((C.DowngradeAwakening)p);
                    break;
                case (short)ClientPacketIds.ResetAddedItem:
                    ResetAddedItem((C.ResetAddedItem)p);
                    break;
                case (short)ClientPacketIds.SendMail:
                    SendMail((C.SendMail)p);
                    break;
                case (short)ClientPacketIds.ReadMail:
                    ReadMail((C.ReadMail)p);
                    break;
                case (short)ClientPacketIds.CollectParcel:
                    CollectParcel((C.CollectParcel)p);
                    break;
                case (short)ClientPacketIds.DeleteMail:
                    DeleteMail((C.DeleteMail)p);
                    break;
                case (short)ClientPacketIds.LockMail:
                    LockMail((C.LockMail)p);
                    break;
                case (short)ClientPacketIds.MailLockedItem:
                    Enqueue(new S.MailLockedItem { UniqueID = ((C.MailLockedItem)p).UniqueID, Locked = ((C.MailLockedItem)p).Locked });
                    break;
                case (short)ClientPacketIds.MailCost:
                    MailCost((C.MailCost)p);
                    break;
                case (short)ClientPacketIds.RequestIntelligentCreatureUpdates:
                    RequestIntelligentCreatureUpdates((C.RequestIntelligentCreatureUpdates)p);
                    break;
                case (short)ClientPacketIds.UpdateIntelligentCreature:
                    UpdateIntelligentCreature((C.UpdateIntelligentCreature)p);
                    break;
                case (short)ClientPacketIds.IntelligentCreaturePickup:
                    IntelligentCreaturePickup((C.IntelligentCreaturePickup)p);
                    break;
                case (short)ClientPacketIds.AddFriend:
                    AddFriend((C.AddFriend)p);
                    break;
                case (short)ClientPacketIds.RemoveFriend:
                    RemoveFriend((C.RemoveFriend)p);
                    break;
                case (short)ClientPacketIds.RefreshFriends:
                    {
                        if (Stage != GameStage.Game) return;
                        Player.GetFriends();
                        break;
                    }
                case (short)ClientPacketIds.AddMemo:
                    AddMemo((C.AddMemo)p);
                    break;
                case (short)ClientPacketIds.GuildBuffUpdate:
                    GuildBuffUpdate((C.GuildBuffUpdate)p);
                    break;
                case (short)ClientPacketIds.GameshopBuy:
                    GameshopBuy((C.GameshopBuy)p);
                    return;
                case (short)ClientPacketIds.NPCConfirmInput:
                    NPCConfirmInput((C.NPCConfirmInput)p);
                    break;
                case (short)ClientPacketIds.ReportIssue:
                    ReportIssue((C.ReportIssue)p);
                    break;
                case (short)ClientPacketIds.GetRanking:
                    GetRanking((C.GetRanking)p);
                    break;
                case (short)ClientPacketIds.Opendoor:
                    Opendoor((C.Opendoor)p);
                    break;
                case (short)ClientPacketIds.GetRentedItems:
                    GetRentedItems();
                    break;
                case (short)ClientPacketIds.ItemRentalRequest:
                    ItemRentalRequest();
                    break;
                case (short)ClientPacketIds.ItemRentalFee:
                    ItemRentalFee((C.ItemRentalFee)p);
                    break;
                case (short)ClientPacketIds.ItemRentalPeriod:
                    ItemRentalPeriod((C.ItemRentalPeriod)p);
                    break;
                case (short)ClientPacketIds.DepositRentalItem:
                    DepositRentalItem((C.DepositRentalItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRentalItem:
                    RetrieveRentalItem((C.RetrieveRentalItem)p);
                    break;
                case (short)ClientPacketIds.CancelItemRental:
                    CancelItemRental();
                    break;
                case (short)ClientPacketIds.ItemRentalLockFee:
                    ItemRentalLockFee();
                    break;
                case (short)ClientPacketIds.ItemRentalLockItem:
                    ItemRentalLockItem();
                    break;
                case (short)ClientPacketIds.ConfirmItemRental:
                    ConfirmItemRental();
                    break;
                case (short)ClientPacketIds.GuildTerritoryPage:
                    GuildTerritoryPage((C.GuildTerritoryPage)p);
                    return;
                case (short)ClientPacketIds.PurchaseGuildTerritory:
                    PurchaseGuildTerritory((C.PurchaseGuildTerritory)p);
                    return;
                case (short)ClientPacketIds.DeleteItem:
                    DeleteItem((C.DeleteItem)p);
                    break;
                default:
                    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.InvalidPacketReceived), p.Index));
                    break;
            }
        }
        // 软断开，不关闭tcp连接，是否真实断开由客户端说了算
        public void SoftDisconnect(byte reason)
        {
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame(reason);

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;
            }

            Account = null;
        }
        public void Disconnect(byte reason)
        {
            if (!Connected) return;

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.Connections)
                Envir.Connections.Remove(this);

            lock (Envir.AccountLock)
            {
                if (Player != null)
                    Player.StopGame(reason);

                if (Account != null && Account.Connection == this)
                    Account.Connection = null;
            }

            if (Observing != null)
                Observing.Observers.Remove(this);

            Account = null;

            _sendList = null;
            _receiveList = null;
            _retryList = null;
            _rawData = null;

            if (_client != null) _client.Client.Dispose();
            _client = null;
        }
        public void SendDisconnect(byte reason)
        {
            if (!Connected)
            {
                Disconnecting = true;
                SoftDisconnect(reason);
                return;
            }

            Disconnecting = true;

            List<byte> data = new List<byte>();

            data.AddRange(new S.Disconnect { Reason = reason }.GetPacketBytes());

            BeginSend(data);
            SoftDisconnect(reason);
        }
        public void CleanObservers()
        {
            foreach (MirConnection c in Observers)
            {
                c.Stage = GameStage.Login;
                c.Enqueue(new S.ReturnToLogin());
            }
        }
        // 检查客户端
        private void ClientVersion(C.ClientVersion p)
        {
            // 如果不是默认状态，说明已经处理过了，直接返回
            if (Stage != GameStage.None) return;
            // 是否配置了客户端检测
            if (Settings.CheckVersion)
            {
                // 检测版本是否一致
                bool match = false;

                foreach (var hash in Settings.VersionHashes)
                {
                    if (Functions.CompareBytes(hash, p.VersionHash))
                    {
                        match = true;
                        break;
                    }
                }
                // 没匹配上，断开连接
                if (!match)
                {
                    Disconnecting = true;

                    List<byte> data = new List<byte>();
                    // 创建客户端连接 Packet 实例(S.ClientVersion)，包含断开连接的原因
                    data.AddRange(new S.ClientVersion { Result = 0 }.GetPacketBytes());
                    // 发送给客户端断开连接的原因
                    BeginSend(data);
                    // 逻辑断开不关闭tcp连接
                    SoftDisconnect(10);
                    // 写入消息队列
                    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.PlayerDisconnectedWrongClientVersion), SessionID));
                    return;
                }
            }
            // 加入消息队列
            MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.ClientVersionMatched), SessionID, IPAddress));
            // 创建客户端连接 Packet 实例(S.ClientVersion)，包含成功原因
            Enqueue(new S.ClientVersion { Result = 1 });

            Stage = GameStage.Login;
        }
        private void ClientKeepAlive(C.KeepAlive p)
        {
            Enqueue(new S.KeepAlive
            {
                Time = p.Time
            });
        }
        private void NewAccount(C.NewAccount p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.NewAccountBeingCreated), SessionID, IPAddress));
            Envir.NewAccount(p, this);
        }
        private void ChangePassword(C.ChangePassword p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.PasswordBeingChanged), SessionID, IPAddress));
            Envir.ChangePassword(p, this);
        }

        // 登录
        private void Login(C.Login p)
        {
            if (Stage != GameStage.Login) return;

            MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization((ServerTextKeys.UserLoggingIn), SessionID, IPAddress));
            Envir.Login(p, this);
        }
        private void NewCharacter(C.NewCharacter p)
        {
            if (Stage != GameStage.Select) return;

            Envir.NewCharacter(p, this, Account.AdminAccount);
        }
        private void DeleteCharacter(C.DeleteCharacter p)
        {
            if (Stage != GameStage.Select) return;

            if (!Settings.AllowDeleteCharacter)
            {
                Enqueue(new S.DeleteCharacter { Result = 0 });
                return;
            }

            CharacterInfo temp = null;


            for (int i = 0; i < Account.Characters.Count; i++)
			{
			    if (Account.Characters[i].Index != p.CharacterIndex) continue;

			    temp = Account.Characters[i];
			    break;
			}

            if (temp == null)
            {
                Enqueue(new S.DeleteCharacter { Result = 1 });
                return;
            }

            temp.Deleted = true;
            temp.DeleteDate = Envir.Now;
            Envir.RemoveRank(temp);
            Enqueue(new S.DeleteCharacterSuccess { CharacterIndex = temp.Index });
        }

        /// <summary>
		/// 开始游戏
		/// </summary>
		/// <param name="p"></param>
        private void StartGame(C.StartGame p)
        {
            // Stage 必须是选择角色界面
            if (Stage != GameStage.Select) return;
            // 检查是否允许开始游戏 并且 账号不为null 账号不是管理员账号
            if (!Settings.AllowStartGame && (Account == null || (Account != null && !Account.AdminAccount)))
            {
                Enqueue(new S.StartGame { Result = 0 });
                return;
            }
            // 账号不为null
            if (Account == null)
            {
                Enqueue(new S.StartGame { Result = 1 });
                return;
            }

            // 初始化角色信息实例 默认值为null
            CharacterInfo info = null;
            // 遍历账号角色列表 检查角色索引是否匹配
            for (int i = 0; i < Account.Characters.Count; i++)
            {
                if (Account.Characters[i].Index != p.CharacterIndex) continue;

                info = Account.Characters[i];
                break;
            }
            // 角色不存在
            if (info == null)
            {
                Enqueue(new S.StartGame { Result = 2 });
                return;
            }
            // 角色已被禁用
            if (info.Banned)
            {
                if (info.ExpiryDate > Envir.Now)
                {
                    Enqueue(new S.StartGameBanned { Reason = info.BanReason, ExpiryDate = info.ExpiryDate });
                    return;
                }
                info.Banned = false;
            }
            // 角色已被禁用的原因
            info.BanReason = string.Empty;
            // 设置过期时间
            info.ExpiryDate = DateTime.MinValue;
            // 未使用的变量
            long delay = (long) (Envir.Now - info.LastLogoutDate).TotalMilliseconds;


            //if (delay < Settings.RelogDelay)
            //{
            //    Enqueue(new S.StartGameDelay { Milliseconds = Settings.RelogDelay - delay });
            //    return;
            //}
            // 初始化 PlayerObject 实例
            Player = new PlayerObject(info, this);
            // 开始游戏
            Player.StartGame();
        }

        public void LogOut()
        {
            if (Stage == GameStage.Game)
            {
                if (Envir.Time < Player.LogTime)
                {
                    Enqueue(new S.LogOutFailed());
                    return;
                }

                Player.StopGame(23);

                Stage = GameStage.Select;
                Player = null;

                Enqueue(new S.LogOutSuccess { Characters = Account.GetSelectInfo() });
            }
            else if (Stage == GameStage.Observer)
            {
                if (Observing != null)
                    Observing.Observers.Remove(this);

                Observing = null;
                Stage = GameStage.Select;

                Enqueue(new S.LogOutSuccess { Characters = Account.GetSelectInfo() });
            }
        }

        private void Turn(C.Turn p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Turn(p.Direction);
        }
        private void Walk(C.Walk p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Walk(p.Direction);
        }
        private void Run(C.Run p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Run(p.Direction);
        }

        private void Chat(C.Chat p)
        {
            if (p.Message.Length > Globals.MaxChatLength)
            {
                SendDisconnect(2);
                return;
            }

            if (Stage == GameStage.Game)
            {
                Player.Chat(p.Message, p.LinkedItems);
            }
            else if (Stage == GameStage.Observer)
            {
                if (!p.Message.StartsWith("@")) return;

                string message = p.Message.Remove(0, 1);
                string[] parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return;

                if (string.Equals(parts[0], "OBSERVE", StringComparison.OrdinalIgnoreCase))
                {
                    if (parts.Length < 2) return;

                    PlayerObject player = Envir.GetPlayer(parts[1]);
                    if (player == null) return;
                    if ((!player.AllowObserve || !Settings.AllowObserve) &&
                        (Account == null || !Account.AdminAccount)) return;

                    player.AddObserver(this);
                }
            }
        }

        private void MoveItem(C.MoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MoveItem(p.Grid, p.From, p.To);
        }
        private void StoreItem(C.StoreItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.StoreItem(p.From, p.To);
        }

        private void DepositRefineItem(C.DepositRefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DepositRefineItem(p.From, p.To);
        }

        private void RetrieveRefineItem(C.RetrieveRefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RetrieveRefineItem(p.From, p.To);
        }

        private void RefineCancel(C.RefineCancel p)
        {
            if (Stage != GameStage.Game) return;

            Player.RefineCancel();
        }

        private void RefineItem(C.RefineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RefineItem(p.UniqueID);
        }

        private void CheckRefine(C.CheckRefine p)
        {
            if (Stage != GameStage.Game) return;

            Player.CheckRefine(p.UniqueID);
        }

        private void ReplaceWedRing(C.ReplaceWedRing p)
        {
            if (Stage != GameStage.Game) return;

            Player.ReplaceWeddingRing(p.UniqueID);
        }

        private void DepositTradeItem(C.DepositTradeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DepositTradeItem(p.From, p.To);
        }

        private void RetrieveTradeItem(C.RetrieveTradeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RetrieveTradeItem(p.From, p.To);
        }
        private void TakeBackItem(C.TakeBackItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TakeBackItem(p.From, p.To);
        }
        private void MergeItem(C.MergeItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.MergeItem(p.GridFrom, p.GridTo, p.IDFrom, p.IDTo);
        }
        private void EquipItem(C.EquipItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.EquipItem(p.Grid, p.UniqueID, p.To);
        }
        private void RemoveItem(C.RemoveItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveItem(p.Grid, p.UniqueID, p.To);
        }
        private void RemoveSlotItem(C.RemoveSlotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.FromUniqueID);
        }
        private void SplitItem(C.SplitItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SplitItem(p.Grid, p.UniqueID, p.Count);
        }
        private void UseItem(C.UseItem p)
        {
            if (Stage != GameStage.Game) return;

            switch (p.Grid)
            {
                case MirGridType.Inventory:
                    Player.UseItem(p.UniqueID);
                    break;
                case MirGridType.HeroInventory:
                    Player.HeroUseItem(p.UniqueID);
                    break;
            }
        }
        private void DropItem(C.DropItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropItem(p.UniqueID, p.Count, p.HeroInventory);
        }

        private void TakeBackHeroItem(C.TakeBackHeroItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TakeBackHeroItem(p.From, p.To);
        }

        private void TransferHeroItem(C.TransferHeroItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.TransferHeroItem(p.From, p.To);
        }
        private void DropGold(C.DropGold p)
        {
            if (Stage != GameStage.Game) return;

            Player.DropGold(p.Amount);
        }
        private void PickUp()
        {
            if (Stage != GameStage.Game) return;

            Player.PickUp();
        }

        private void RequestMapInfo(C.RequestMapInfo p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestMapInfo(p.MapIndex);
        }

        private void RequestMonsterInfo(C.RequestMonsterInfo p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestMonsterInfo(p.MonsterIndex);
        }

        private void RequestNPCInfo(C.RequestNPCInfo p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestNPCInfo(p.NPCIndex);
        }

        private void RequestItemInfo(C.RequestItemInfo p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestItemInfo(p.ItemIndex);
        }

        private void TeleportToNPC(C.TeleportToNPC p)
        {
            if (Stage != GameStage.Game) return;

            Player.TeleportToNPC(p.ObjectID);
        }

        private void SearchMap(C.SearchMap p)
        {
            if (Stage != GameStage.Game) return;

            Player.SearchMap(p.Text);
        }
        private void Inspect(C.Inspect p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;

            if (p.Ranking)
            {
                Envir.Inspect(this, (int)p.ObjectID);
            }
            else if (p.Hero)
            {
                Envir.InspectHero(this, (int)p.ObjectID);
            }
            else
            {
                Envir.Inspect(this, p.ObjectID);
            }
        }
        private void Observe(C.Observe p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;

            Envir.Observe(this, p.Name);
        }
        private void ChangeAMode(C.ChangeAMode p)
        {
            if (Stage != GameStage.Game) return;

            Player.AMode = p.Mode;

            Enqueue(new S.ChangeAMode {Mode = Player.AMode});
        }
        private void ChangePMode(C.ChangePMode p)
        {
            if (Stage != GameStage.Game) return;

            Player.PMode = p.Mode;

            Enqueue(new S.ChangePMode { Mode = Player.PMode });
        }
        private void ChangeTrade(C.ChangeTrade p)
        {
            if (Stage != GameStage.Game) return;

            Player.AllowTrade = p.AllowTrade;
        }
        private void Attack(C.Attack p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.Attack(p.Direction, p.Spell);
        }
        private void RangeAttack(C.RangeAttack p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && (Player.ActionTime > Envir.Time || Player.AttackTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                Player.RangeAttack(p.Direction, p.TargetLocation, p.TargetID);
        }
        private void Harvest(C.Harvest p)
        {
            if (Stage != GameStage.Game) return;

            if (!Player.Dead && Player.ActionTime > Envir.Time)
                _retryList.Enqueue(p);
            else
                Player.Harvest(p.Direction);
        }

        private void CallNPC(C.CallNPC p)
        {
            if (Stage != GameStage.Game) return;

            if (p.Key.Length > 30) //No NPC Key should be that long.
            {
                SendDisconnect(2);
                return;
            }

            if (p.ObjectID == Envir.DefaultNPC.LoadedObjectID && Player.NPCObjectID == Envir.DefaultNPC.LoadedObjectID)
            {
                Player.CallDefaultNPC(p.Key);
                return;
            }

            if (p.ObjectID == uint.MaxValue)
            {
                Player.CallDefaultNPC(DefaultNPCType.Client, null);
                return;
            }

            Player.CallNPC(p.ObjectID, p.Key);
        }

        private void BuyItem(C.BuyItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.BuyItem(p.ItemIndex, p.Count, p.Type);
        }
        private void CraftItem(C.CraftItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.CraftItem(p.UniqueID, p.Count, p.Slots);
        }
        private void SellItem(C.SellItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SellItem(p.UniqueID, p.Count);
        }
        private void RepairItem(C.RepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID);
        }
        private void BuyItemBack(C.BuyItemBack p)
        {
            if (Stage != GameStage.Game) return;

           // Player.BuyItemBack(p.UniqueID, p.Count);
        }
        private void SRepairItem(C.SRepairItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RepairItem(p.UniqueID, true);
        }
        private void MagicKey(C.MagicKey p)
        {
            if (Stage != GameStage.Game) return;

            HumanObject actor = Player;
            if (p.Key > 16 || p.OldKey > 16)
            {
                if (!Player.HeroSpawned || Player.Hero.Dead) return;
                actor = Player.Hero;
            }

            for (int i = 0; i < actor.Info.Magics.Count; i++)
            {
                UserMagic magic = actor.Info.Magics[i];
                if (magic.Spell != p.Spell)
                {
                    if (magic.Key == p.Key)
                        magic.Key = 0;
                    continue;
                }

                magic.Key = p.Key;
            }
        }
        private void Magic(C.Magic p)
        {
            if (Stage != GameStage.Game) return;

            HumanObject actor = Player;
            if (Player.HeroSpawned && p.ObjectID == Player.Hero.ObjectID)
                actor = Player.Hero;

            if (actor.Dead) return;

            if (!actor.Dead && (actor.ActionTime > Envir.Time || actor.SpellTime > Envir.Time))
                _retryList.Enqueue(p);
            else
                actor.BeginMagic(p.Spell, p.Direction, p.TargetID, p.Location, p.SpellTargetLock);
        }

        private void SwitchGroup(C.SwitchGroup p)
        {
            if (Stage != GameStage.Game) return;

            Player.SwitchGroup(p.AllowGroup);
        }
        private void AddMember(C.AddMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMember(p.Name);
        }
        private void DelMember(C.DelMember p)
        {
            if (Stage != GameStage.Game) return;

            Player.DelMember(p.Name);
        }
        private void GroupInvite(C.GroupInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GroupInvite(p.AcceptInvite);
        }

        private void NewHero(C.NewHero p)
        {
            if (Stage != GameStage.Game) return;

            Player.NewHero(p);
        }

        private void SetAutoPotValue(C.SetAutoPotValue p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetAutoPotValue(p.Stat, p.Value);
        }

        private void SetAutoPotItem(C.SetAutoPotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetAutoPotItem(p.Grid, p.ItemIndex);
        }

        private void SetHeroBehaviour(C.SetHeroBehaviour p)
        {
            if (Stage != GameStage.Game) return;

            Player.SetHeroBehaviour(p.Behaviour);
        }

        private void ChangeHero(C.ChangeHero p)
        {
            if (Stage != GameStage.Game) return;

            Player.ChangeHero(p.ListIndex);
        }

        private void TownRevive()
        {
            if (Stage != GameStage.Game) return;

            Player.TownRevive();
        }

        private void SpellToggle(C.SpellToggle p)
        {
            if (Stage != GameStage.Game) return;

            if (p.canUse > SpellToggleState.None)
            {
                Player.SpellToggle(p.Spell, p.canUse);
                return;
            }
            if (Player.HeroSpawned)
                Player.Hero.SpellToggle(p.Spell, p.canUse);
        }
        private void ConsignItem(C.ConsignItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.ConsignItem(p.UniqueID, p.Price, p.Type);
        }
        private void GuildTerritoryPage(C.GuildTerritoryPage p)
        {
            if (Stage != GameStage.Game) return;

            Player.GetGuildTerritories(p.Page);
        }

        private void PurchaseGuildTerritory(C.PurchaseGuildTerritory p)
        {
            if (Stage != GameStage.Game) return;

            Player.PurchaseGuildTerritory(p.Owner);
        }
        private void MarketSearch(C.MarketSearch p)
        {
            if (Stage != GameStage.Game) return;

            Player.UserMatch = p.Usermode;
            Player.MinShapes = p.MinShape;
            Player.MaxShapes = p.MaxShape;
            Player.MarketPanelType = p.MarketType;

            Player.MarketSearch(p.Match, p.Type);
        }
        private void MarketRefresh()
        {
            if (Stage != GameStage.Game) return;

            Player.MarketSearch(string.Empty, Player.MatchType);
        }

        private void MarketPage(C.MarketPage p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketPage(p.Page);
        }
        private void MarketBuy(C.MarketBuy p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketBuy(p.AuctionID, p.BidPrice);
        }
        private void MarketSellNow(C.MarketSellNow p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketSellNow(p.AuctionID);
        }

        private void MarketGetBack(C.MarketGetBack p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarketGetBack(p.Mode, p.AuctionID);
        }
        private void RequestUserName(C.RequestUserName p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestUserName(p.UserID);
        }
        private void RequestChatItem(C.RequestChatItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.RequestChatItem(p.ChatItemID);
        }
        private void EditGuildMember(C.EditGuildMember p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildMember(p.Name,p.RankName,p.RankIndex,p.ChangeType);
        }
        private void EditGuildNotice(C.EditGuildNotice p)
        {
            if (Stage != GameStage.Game) return;
            Player.EditGuildNotice(p.notice);
        }
        private void GuildInvite(C.GuildInvite p)
        {
            if (Stage != GameStage.Game) return;

            Player.GuildInvite(p.AcceptInvite);
        }
        private void RequestGuildInfo(C.RequestGuildInfo p)
        {
            if (Stage != GameStage.Game) return;
            Player.RequestGuildInfo(p.Type);
        }
        private void GuildNameReturn(C.GuildNameReturn p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildNameReturn(p.Name);
        }
        private void GuildStorageGoldChange(C.GuildStorageGoldChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageGoldChange(p.Type, p.Amount);
        }
        private void GuildStorageItemChange(C.GuildStorageItemChange p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildStorageItemChange(p.Type, p.From, p.To);
        }
        private void GuildWarReturn(C.GuildWarReturn p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildWarReturn(p.Name);
        }


        private void MarriageRequest(C.MarriageRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarriageRequest();
        }

        private void MarriageReply(C.MarriageReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.MarriageReply(p.AcceptInvite);
        }

        private void ChangeMarriage(C.ChangeMarriage p)
        {
            if (Stage != GameStage.Game) return;

            if (Player.Info.Married == 0)
            {
                Player.AllowMarriage = !Player.AllowMarriage;
                if (Player.AllowMarriage)
                    Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.YouAllowMarriageRequests), ChatType.Hint);
                else
                    Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.YouBlockMarriageRequests), ChatType.Hint);
            }
            else
            {
                Player.AllowLoverRecall = !Player.AllowLoverRecall;
                if (Player.AllowLoverRecall)
                    Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.YouAllowRecallFromLover), ChatType.Hint);
                else
                    Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.YouBlockRecallFromLover), ChatType.Hint);
            }
        }

        private void DivorceRequest(C.DivorceRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.DivorceRequest();
        }

        private void DivorceReply(C.DivorceReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.DivorceReply(p.AcceptInvite);
        }

        private void AddMentor(C.AddMentor p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMentor(p.Name);
        }

        private void MentorReply(C.MentorReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.MentorReply(p.AcceptInvite);
        }

        private void AllowMentor(C.AllowMentor p)
        {
            if (Stage != GameStage.Game) return;

                Player.AllowMentor = !Player.AllowMentor;
                if (Player.AllowMentor)
                Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.AllowingMentorRequests), ChatType.Hint);
                else
                    Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.BlockingMentorRequests), ChatType.Hint);
        }

        private void CancelMentor(C.CancelMentor p)
        {
            if (Stage != GameStage.Game) return;

            Player.MentorBreak(true);
        }

        private void TradeRequest(C.TradeRequest p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeRequest();
        }
        private void TradeGold(C.TradeGold p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeGold(p.Amount);
        }
        private void TradeReply(C.TradeReply p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeReply(p.AcceptInvite);
        }
        private void TradeConfirm(C.TradeConfirm p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeConfirm(p.Locked);
        }
        private void TradeCancel(C.TradeCancel p)
        {
            if (Stage != GameStage.Game) return;

            Player.TradeCancel();
        }
        private void EquipSlotItem(C.EquipSlotItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.EquipSlotItem(p.Grid, p.UniqueID, p.To, p.GridTo, p.ToUniqueID);
        }

        private void FishingCast(C.FishingCast p)
        {
            if (Stage != GameStage.Game) return;

            Player.FishingCast(p.CastOut, true);
        }

        private void FishingChangeAutocast(C.FishingChangeAutocast p)
        {
            if (Stage != GameStage.Game) return;

            Player.FishingChangeAutocast(p.AutoCast);
        }

        private void AcceptQuest(C.AcceptQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.AcceptQuest(p.QuestIndex); //p.NPCIndex,
        }

        private void FinishQuest(C.FinishQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.FinishQuest(p.QuestIndex, p.SelectedItemIndex);
        }

        private void AbandonQuest(C.AbandonQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.AbandonQuest(p.QuestIndex);
        }

        private void ShareQuest(C.ShareQuest p)
        {
            if (Stage != GameStage.Game) return;

            Player.ShareQuest(p.QuestIndex);
        }

        private void AcceptReincarnation()
        {
            if (Stage != GameStage.Game) return;

            if (Player.ReincarnationHost != null && Player.ReincarnationHost.ReincarnationReady)
            {
                Player.Revive(Player.Stats[Stat.HP] / 2, true);
                Player.ReincarnationHost = null;
                return;
            }

            Player.ReceiveChat(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.ReincarnationFailed), ChatType.System);
        }

        private void CancelReincarnation()
        {
            if (Stage != GameStage.Game) return;
            Player.ReincarnationExpireTime = Envir.Time;

        }

        private void CombineItem(C.CombineItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.CombineItem(p.Grid, p.IDFrom, p.IDTo);
        }

        private void Awakening(C.Awakening p)
        {
            if (Stage != GameStage.Game) return;

            Player.Awakening(p.UniqueID, p.Type);
        }

        private void AwakeningNeedMaterials(C.AwakeningNeedMaterials p)
        {
            if (Stage != GameStage.Game) return;

            Player.AwakeningNeedMaterials(p.UniqueID, p.Type);
        }

        private void DisassembleItem(C.DisassembleItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DisassembleItem(p.UniqueID);
        }

        private void DowngradeAwakening(C.DowngradeAwakening p)
        {
            if (Stage != GameStage.Game) return;

            Player.DowngradeAwakening(p.UniqueID);
        }

        private void ResetAddedItem(C.ResetAddedItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.ResetAddedItem(p.UniqueID);
        }

        public void SendMail(C.SendMail p)
        {
            if (Stage != GameStage.Game) return;

            if (p.Gold > 0 || p.ItemsIdx.Length > 0)
            {
                Player.SendMail(p.Name, p.Message, p.Gold, p.ItemsIdx, p.Stamped);
            }
            else
            {
                Player.SendMail(p.Name, p.Message);
            }
        }

        public void ReadMail(C.ReadMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.ReadMail(p.MailID);
        }

        public void CollectParcel(C.CollectParcel p)
        {
            if (Stage != GameStage.Game) return;

            Player.CollectMail(p.MailID);
        }

        public void DeleteMail(C.DeleteMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.DeleteMail(p.MailID);
        }

        public void LockMail(C.LockMail p)
        {
            if (Stage != GameStage.Game) return;

            Player.LockMail(p.MailID, p.Lock);
        }

        public void MailCost(C.MailCost p)
        {
            if (Stage != GameStage.Game) return;

            uint cost = Player.GetMailCost(p.ItemsIdx, p.Gold, p.Stamped);

            Enqueue(new S.MailCost { Cost = cost });
        }

        private void RequestIntelligentCreatureUpdates(C.RequestIntelligentCreatureUpdates p)
        {
            if (Stage != GameStage.Game) return;

            Player.SendIntelligentCreatureUpdates = p.Update;
        }

        private void UpdateIntelligentCreature(C.UpdateIntelligentCreature p)
        {
            if (Stage != GameStage.Game) return;

            ClientIntelligentCreature petUpdate = p.Creature;
            if (petUpdate == null) return;

            if (p.ReleaseMe)
            {
                Player.ReleaseIntelligentCreature(petUpdate.PetType);
                return;
            }
            else if (p.SummonMe)
            {
                Player.SummonIntelligentCreature(petUpdate.PetType);
                return;
            }
            else if (p.UnSummonMe)
            {
                Player.UnSummonIntelligentCreature(petUpdate.PetType);
                return;
            }
            else
            {
                //Update the creature info
                for (int i = 0; i < Player.Info.IntelligentCreatures.Count; i++)
                {
                    if (Player.Info.IntelligentCreatures[i].PetType == petUpdate.PetType)
                    {
                        var reg = new Regex(@"^[A-Za-z0-9]{" + Globals.MinCharacterNameLength + "," + Globals.MaxCharacterNameLength + "}$");

                        if (reg.IsMatch(petUpdate.CustomName))
                        {
                            Player.Info.IntelligentCreatures[i].CustomName = petUpdate.CustomName;
                        }

                        Player.Info.IntelligentCreatures[i].SlotIndex = petUpdate.SlotIndex;
                        Player.Info.IntelligentCreatures[i].Filter = petUpdate.Filter;
                        Player.Info.IntelligentCreatures[i].petMode = petUpdate.petMode;
                    }
                    else continue;
                }

                if (Player.CreatureSummoned)
                {
                    if (Player.SummonedCreatureType == petUpdate.PetType)
                        Player.UpdateSummonedCreature(petUpdate.PetType);
                }
            }
        }

        private void IntelligentCreaturePickup(C.IntelligentCreaturePickup p)
        {
            if (Stage != GameStage.Game) return;

            Player.IntelligentCreaturePickup(p.MouseMode, p.Location);
        }

        private void AddFriend(C.AddFriend p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddFriend(p.Name, p.Blocked);
        }

        private void RemoveFriend(C.RemoveFriend p)
        {
            if (Stage != GameStage.Game) return;

            Player.RemoveFriend(p.CharacterIndex);
        }

        private void AddMemo(C.AddMemo p)
        {
            if (Stage != GameStage.Game) return;

            Player.AddMemo(p.CharacterIndex, p.Memo);
        }
        private void GuildBuffUpdate(C.GuildBuffUpdate p)
        {
            if (Stage != GameStage.Game) return;
            Player.GuildBuffUpdate(p.Action,p.Id);
        }
        private void GameshopBuy(C.GameshopBuy p)
        {
            if (Stage != GameStage.Game) return;
            Player.GameshopBuy(p.GIndex, p.Quantity, p.PType);
        }

        private void NPCConfirmInput(C.NPCConfirmInput p)
        {
            if (Stage != GameStage.Game) return;

            Player.NPCData["NPCInputStr"] = p.Value;

            if (p.NPCID == Envir.DefaultNPC.LoadedObjectID && Player.NPCObjectID == Envir.DefaultNPC.LoadedObjectID)
            {
                Player.CallDefaultNPC(p.PageName);
                return;
            }

            Player.CallNPC(Player.NPCObjectID, p.PageName);
        }

        public List<byte[]> Image = new List<byte[]>();

        private void ReportIssue(C.ReportIssue p)
        {
            if (Stage != GameStage.Game) return;

            return;

            // Image.Add(p.Image);

            // if (p.ImageChunk >= p.ImageSize)
            // {
            //     System.Drawing.Image image = Functions.ByteArrayToImage(Functions.CombineArray(Image));
            //     image.Save("Reported-" + Player.Name + "-" + DateTime.Now.ToString("yyMMddHHmmss") + ".jpg");
            //     Image.Clear();
            // }
        }
        private void GetRanking(C.GetRanking p)
        {
            if (Stage != GameStage.Game && Stage != GameStage.Observer) return;
            Envir.GetRanking(this, p.RankType, p.RankIndex, p.OnlineOnly);
        }

        private void Opendoor(C.Opendoor p)
        {
            if (Stage != GameStage.Game) return;
            Player.Opendoor(p.DoorIndex);
        }

        private void GetRentedItems()
        {
            if (Stage != GameStage.Game)
                return;

            Player.GetRentedItems();
        }

        private void ItemRentalRequest()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalRequest();
        }

        private void ItemRentalFee(C.ItemRentalFee p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.SetItemRentalFee(p.Amount);
        }

        private void ItemRentalPeriod(C.ItemRentalPeriod p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.SetItemRentalPeriodLength(p.Days);
        }

        private void DepositRentalItem(C.DepositRentalItem p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.DepositRentalItem(p.From, p.To);
        }

        private void RetrieveRentalItem(C.RetrieveRentalItem p)
        {
            if (Stage != GameStage.Game)
                return;

            Player.RetrieveRentalItem(p.From, p.To);
        }

        private void CancelItemRental()
        {
            if (Stage != GameStage.Game)
                return;

            Player.CancelItemRental();
        }

        private void ItemRentalLockFee()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalLockFee();
        }

        private void ItemRentalLockItem()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ItemRentalLockItem();
        }

        private void ConfirmItemRental()
        {
            if (Stage != GameStage.Game)
                return;

            Player.ConfirmItemRental();
        }

        public void CheckItemInfo(ItemInfo info, bool dontLoop = false)
        {
            if ((dontLoop == false) && (info.ClassBased | info.LevelBased)) //send all potential data so client can display it
            {
                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                {
                    if ((Envir.ItemInfoList[i] != info) && (Envir.ItemInfoList[i].Name.StartsWith(info.Name)))
                        CheckItemInfo(Envir.ItemInfoList[i], true);
                }
            }

            foreach (MirConnection observer in Observers)
                observer.CheckItemInfo(info, dontLoop);

            if (SentItemInfo.Contains(info)) return;
            Enqueue(new S.NewItemInfo { Info = info });
            SentItemInfo.Add(info);
        }

        public void CheckMonsterInfo(int monsterIndex)
        {
            CheckMonsterInfo(Envir.GetMonsterInfo(monsterIndex));
        }

        public void CheckMonsterInfo(MonsterInfo info)
        {
            if (info == null) return;

            foreach (MirConnection observer in Observers)
                observer.CheckMonsterInfo(info);

            if (SentMonsterInfo.Contains(info)) return;

            Enqueue(new S.NewMonsterInfo { Info = info.ClientInformation });
            SentMonsterInfo.Add(info);
        }

        public void CheckNPCInfo(int npcIndex)
        {
            CheckNPCInfo(Envir.GetNPCInfo(npcIndex));
        }

        public void CheckNPCInfo(NPCInfo info)
        {
            if (info == null) return;

            foreach (MirConnection observer in Observers)
                observer.CheckNPCInfo(info);

            if (SentNPCInfo.Contains(info)) return;

            Enqueue(new S.NewNPCInfo { Info = info.ClientInformation });
            SentNPCInfo.Add(info);
        }
        public void CheckItem(UserItem item)
        {
            CheckItemInfo(item.Info);

            for (int i = 0; i < item.Slots.Length; i++)
            {
                if (item.Slots[i] == null) continue;

                CheckItemInfo(item.Slots[i].Info);
            }

            CheckHeroInfo(item);
        }
        private void CheckHeroInfo(UserItem item)
        {
            if (item.AddedStats[Stat.Hero] == 0) return;
            if (SentHeroInfo.Contains(item.UniqueID)) return;

            HeroInfo heroInfo = Envir.GetHeroInfo(item.AddedStats[Stat.Hero]);
            if (heroInfo == null) return;

            Enqueue(new S.NewHeroInfo { Info = heroInfo.ClientInformation });
            SentHeroInfo.Add(item.UniqueID);
        }

        private void DeleteItem(C.DeleteItem p)
        {
            if (Stage != GameStage.Game) return;

            Player.DeleteItem(p.UniqueID, p.Count);
        }
    }

    public class MirConnectionLog {
        public string IPAddress = "";
        public List<long> AccountsMade = new List<long>();
        public List<long> CharactersMade = new List<long>();
    }
}
