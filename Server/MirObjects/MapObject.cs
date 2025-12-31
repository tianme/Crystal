using System.Drawing;
﻿using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects.Monsters;
using S = ServerPackets;

namespace Server.MirObjects
{
    public abstract class MapObject
    {
        /// <summary>
        /// 全局消息队列
        /// </summary>
        protected static MessageQueue MessageQueue
        {
            get { return MessageQueue.Instance; }
        }
        /// <summary>
        /// 环境对象
        /// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 对象ID
        /// </summary>
        public readonly uint ObjectID = Envir.ObjectID;
        /// <summary>
        /// 种族
        /// </summary>
        public abstract ObjectType Race { get; }
        /// <summary>
        /// 地图名字
        /// </summary>
        public abstract string Name { get; set; }

        public long ExplosionInflictedTime;
        public int ExplosionInflictedStage;
        /// <summary>
        /// 生成线程
        /// </summary>
        private int SpawnThread;

        private Map _currentMap;
        /// <summary>
        /// 当前地图。
        /// <para>设置 CurrentMap 时：如果 value 为 null，则 CurrentMapIndex 设置为 0；否则 CurrentMapIndex 设置为 _currentMap.Info.Index。</para>
        /// <para>也就是说，设置 CurrentMap 的同时，会同步更新子类实现的 CurrentMapIndex。</para>
        /// <para>注意：CurrentMapIndex 是抽象属性，子类必须重写。</para>
        /// </summary>
        public Map CurrentMap
        {
            set
            {
                _currentMap = value;
                CurrentMapIndex = _currentMap != null ? _currentMap.Info.Index : 0;
            }
            get { return _currentMap; }
        }
        /// <summary>
        /// 当前地图的索引
        /// <para>注意：CurrentMapIndex 是抽象属性，子类必须重写。</para>
        /// </summary>
        public abstract int CurrentMapIndex { get; set; }
        /// <summary>
        /// 当前的坐标
        /// <para>注意：CurrentLocation 是抽象属性，子类必须重写。</para>
        /// </summary>
        public abstract Point CurrentLocation { get; set; }
        /// <summary>
        /// 方向
        /// <para> 抽象属性 </para>
        /// </summary>
        public abstract MirDirection Direction { get; set; }
        /// <summary>
        /// 等级
        /// <para> 抽象属性 </para>
        /// </summary>
        public abstract ushort Level { get; set; }
        /// <summary>
        /// 生命
        /// <para> 抽象属性 </para>
        /// </summary>
        public abstract int Health { get; }
        /// <summary>
        /// 最大生命
        /// <para> 抽象属性 </para>
        /// </summary>
        public abstract int MaxHealth { get; }
        /// <summary>
        /// 生命百分比
        /// </summary>
        public byte PercentHealth
        {
            get { return (byte)(Health / (float)MaxHealth * 100); }
        }
        /// <summary>
        /// 光照范围
        /// </summary>
        public byte Light;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public int AttackSpeed;

        protected long brownTime;
        /// <summary>
        /// 棕名时间
        /// </summary>
        public virtual long BrownTime
        {
            get { return brownTime; }
            set { brownTime = value; }
        }
        /// <summary>
        /// 控制角色怪物移动的动作冷却时间
        /// </summary>
		public long CellTime;
        /// <summary>
        /// 控制pk点数减少的频率
        /// </summary>
		public long PKPointTime;
        /// <summary>
        /// 记录最后一次受到攻击的时间
        /// </summary>
        public long LastHitTime;
        /// <summary>
        /// 经验归属有效时间
        /// </summary>
		public long EXPOwnerTime;
        /// <summary>
        /// 名字显示的颜色
        /// </summary>
        public Color NameColour = Color.White;
		/// <summary>
		/// Dead: 是否死亡状态
		/// </summary>
		public bool Dead;
        /// <summary>
		/// Undead: 是否是不死生物
		/// </summary>
		public bool Undead;
        /// <summary>
		/// Harvested: 是否被采集过
		/// </summary>
		public bool Harvested;
        /// <summary>
		/// AutoRev: 是否允许广播生命值
		/// </summary>
		public bool AutoRev;

        public List<KeyValuePair<string, string>> NPCVar = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// PK值
        /// </summary>
        public virtual int PKPoints { get; set; }
        /// <summary>
		/// 药水恢复生命总量
		/// </summary>
		public ushort PotHealthAmount;
		/// <summary>
		/// 药水恢复法力总量
		/// </summary>
		public ushort PotManaAmount;
		/// <summary>
		/// 治疗总量
		/// </summary>
		public ushort HealAmount;
		/// <summary>
		/// 吸血总量
		/// </summary>
		public ushort VampAmount;
		/// <summary>
		/// 控制对象是否能够看破隐身状态
		/// </summary>
		public bool CoolEye;
        private bool _hidden;
        /// <summary>
        /// 是否隐藏状态
        /// <para>只对怪物有效</para>
        /// </summary>
        public bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                if (_hidden == value) return;
                _hidden = value;
                CurrentMap.Broadcast(new S.ObjectHidden { ObjectID = ObjectID, Hidden = value }, CurrentLocation);
            }
        }

        private bool _observer;
        /// <summary>
		///  是否观察状态
		/// </summary>
        public bool Observer
        {
            get
            {
                return _observer;
            }
            set
            {
                if (_observer == value) return;
                _observer = value;
                if (!_observer)
                    BroadcastInfo();
                else
                    Broadcast(new S.ObjectRemove { ObjectID = ObjectID });
            }
        }

        #region Sneaking
        private bool _sneakingActive;
        /// <summary>
        /// 潜行的激活状态
        /// </summary>
        public bool SneakingActive
        {
            get { return _sneakingActive; }
            set
            {
                if (_sneakingActive == value) return;
                _sneakingActive = value;

                Observer = _sneakingActive;
            }
        }

        private bool _sneaking;
        /// <summary>
        /// 是否潜行
		/// <para>对怪物不100%隐身，对玩家100%隐身</para>
		/// <para>注意：怪物等级高于玩家或怪物有看破隐身的能力则隐身无效</para>
        /// </summary>
        public bool Sneaking
        {
            get { return _sneaking; }
            set { _sneaking = value; SneakingActive = value; }
        }
        #endregion

        public MapObject _target;
        /// <summary>
        /// 攻击的目标
        /// </summary>
        public virtual MapObject Target
        {
            get { return _target; }
            set
            {
                if (_target == value) return;
                _target = value;
            }

        }

        protected MapObject master;
        /// <summary>
        /// 标识着属于谁, 比如：玩家召唤的宠物 Master 就等于玩家实例
        /// </summary>
        public virtual MapObject Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// LastHitter: 最后攻击者。谁最后一次攻击了我，用于击杀归属、计分、反击、pk惩罚，宝宝攻击时归属主人
        /// </summary>
        public MapObject LastHitter;
        /// <summary>
        /// 经验归属
        /// </summary>
        public MapObject EXPOwner;
        /// <summary>
        /// 物品短期归属
        /// </summary>
        public MapObject Owner;
        /// <summary>
        /// 对象的过期时间
        /// </summary>
        public long ExpireTime;
        /// <summary>
        /// 对象的归属时间
        /// </summary>
        public long OwnerTime;
		/// <summary>
		/// 对象的冷却时间
		/// </summary>
		public long OperateTime;
        /// <summary>
        /// 生成对象或默认0-100ms后再执行交互
        /// <para>的主要作用是: 为新生成对象随机分配首次 AI 执行时间，使大量对象不会在同一时刻执行逻辑，从而分散服务器负载、降低性能峰值</para>
        /// </summary>
        public int OperateDelay = 100;
        /// <summary>
        /// 存放人物、怪物属性的
        /// </summary>
        public Stats Stats;
        /// <summary>
        /// 宠物集合
        /// </summary>
        public List<MonsterObject> Pets = new List<MonsterObject>();
        /// <summary>
        /// Buff 集合
        /// </summary>
        public virtual List<Buff> Buffs { get; set; } = new List<Buff>();
        /// <summary>
        /// 组队集合
        /// </summary>
        public List<PlayerObject> GroupMembers;
        /// <summary>
        /// 攻击模式
        /// </summary>
        public virtual AttackMode AMode { get; set; }
        /// <summary>
        /// 宠物模式
        /// </summary>
        public virtual PetMode PMode { get; set; }

        private bool _inSafeZone;
        /// <summary>
        /// 是否在安全区
        /// </summary>
        public bool InSafeZone {
            get { return _inSafeZone; }
            set
            {
                if (_inSafeZone == value) return;
                _inSafeZone = value;
                OnSafeZoneChanged();
            }
        }
		/// <summary>
		/// 护甲系数
		/// <para>实际护甲 = 基础护甲 × ArmourRate</para>
		/// </summary>
		public float ArmourRate; //recieved not given
		/// <summary>
		/// 伤害系数
		/// <para>最终伤害 = 基础伤害 × DamageRate</para>
		/// </summary>
		public float DamageRate; //recieved not given
        /// <summary>
        /// 中毒列表
        /// </summary>
        public virtual List<Poison> PoisonList { get; set; } = new List<Poison>();
        /// <summary>
        /// 当前的中毒状态，用位运算添加或删除
        /// </summary>
        public PoisonType CurrentPoison = PoisonType.None;
        /// <summary>
        /// 动作列表
		/// <para>比如：雷电术施法需要多久后计算伤害</para>
        /// </summary>
        public List<DelayedAction> ActionList = new List<DelayedAction>();
        /// <summary>
		/// 当前对象在世界对象的节点
		/// </summary>
        public LinkedListNode<MapObject> Node;
        /// <summary>
		/// 怪物所在 ObjectList 中的节点
        /// <para> ObjectList: 所有的怪物链表 </para>
		/// </summary>
        public LinkedListNode<MapObject> NodeThreaded;
        /// <summary>
        /// 心灵起始专用属性
        /// </summary>
        public long RevTime;

		/// <summary>
		/// 是否可以穿越
		/// <para>true ：表示该对象 可以阻挡 （不可穿越）</para>
        /// <para>false ：表示该对象 不可阻挡 （可以穿越）</para>
		/// </summary>
		public virtual bool Blocking
        {
            get { return true; }
        }
        /// <summary>
        /// 前方1格的坐标
        /// </summary>
        public Point Front
        {
            get { return Functions.PointMove(CurrentLocation, Direction, 1); }
        }
        /// <summary>
        /// 后方1格的坐标
        /// </summary>
        public Point Back
        {
            get { return Functions.PointMove(CurrentLocation, Direction, -1); }

        }
        /// <summary>
        /// 每帧维护对象状态、管理 PK 与战斗归属、执行延迟动作的主循环
        /// </summary>
        public virtual void Process()
        {
			// Master 引用可能还存在，但对象已从地图移除（Node == null），因此将 Master 置空
			if (Master != null && Master.Node == null) Master = null;

            if (LastHitter != null && LastHitter.Node == null) LastHitter = null;

            if (EXPOwner != null && EXPOwner.Node == null) EXPOwner = null;

            if (Target != null && (Target.Node == null || Target.Dead)) Target = null;

            if (Owner != null && Owner.Node == null) Owner = null;

            // PKPoints 大于 0 且超过 PKPointTime 时减少 1 点 PK 值（默认每 12 秒减少 1 点）
            if (PKPoints > 0 && Envir.Time > PKPointTime)
            {
                // 更新下一次扣减时间：PKDelay 秒后（默认 12 秒，每秒为 Settings.Second 毫秒）
                PKPointTime = Envir.Time + Settings.PKDelay * Settings.Second;
                PKPoints--;
            }
            // 存在最后攻击者且超过最后攻击者的时间
            if (LastHitter != null && Envir.Time > LastHitTime)
            {
                LastHitter = null;
            }
            // 是否清理经验归属
            if (EXPOwner != null && Envir.Time > EXPOwnerTime)
            {
                EXPOwner = null;
            }
            // 遍历待执行的动作列表找到可执行的动作进行执行
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (Envir.Time < ActionList[i].Time) continue;
                Process(ActionList[i]);
                ActionList.RemoveAt(i);
            }
        }

        public virtual void OnSafeZoneChanged()
        {

        }

        public abstract void SetOperateTime();

        public int GetAttackPower(int min, int max)
        {
            if (min < 0) min = 0;
            if (min > max) max = min;
            // 有幸运的时候
            if (Stats[Stat.Luck] > 0)
            {
                // Stats[Stat.Luck] 角色幸运值
                // Settings.MaxLuck 10
                if (Stats[Stat.Luck] > Envir.Random.Next(Settings.MaxLuck))
                    return max;
            }
            // 有诅咒的时候
            else if (Stats[Stat.Luck] < 0)
            {
                if (Stats[Stat.Luck] < -Envir.Random.Next(Settings.MaxLuck))
                    return min;
            }
            // 没有幸运的时候
            return Envir.Random.Next(min, max + 1);
        }

        public int GetRangeAttackPower(int min, int max, int range)
        {
            //maxRange = highest possible damage
            //minRange = lowest possible damage

            decimal x = ((decimal)min / (Globals.MaxAttackRange)) * (Globals.MaxAttackRange - range);

            min -= (int)Math.Floor(x);

            return GetAttackPower(min, max);
        }
        /// <summary>
        /// 获取防御力（魔法防御，物理防御）
        /// <para> 通过最小值和最大值进行随机取一个值 </para>
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>防御值</returns>
        public int GetDefencePower(int min, int max)
        {
            if (min < 0) min = 0;
            if (min > max) max = min;

            return Envir.Random.Next(min, max + 1);
        }

        public virtual void Remove(HumanObject player)
        {
            player.Enqueue(new S.ObjectRemove { ObjectID = ObjectID });
        }
        public virtual void Add(HumanObject player)
        {
            if (player.Race != ObjectType.Player) return;

            if (Race == ObjectType.Merchant)
            {
                NPCObject npc = (NPCObject)this;
                npc.CheckVisible((PlayerObject)player, true);
                return;
            }

            player.Enqueue(GetInfo());

            //if (Race == ObjectType.Player)
            //{
            //    PlayerObject me = (PlayerObject)this;
            //    player.Enqueue(me.GetInfoEx(player));
            //}
            //else
            //{
            //    player.Enqueue(GetInfo());
            //}
        }
        public virtual void Remove(MonsterObject monster)
        {

        }
        public virtual void Add(MonsterObject monster)
        {

        }
        /// <summary>
        /// 子类从写执行动作方法
        /// </summary>
        /// <param name="action">延迟执行的动作</param>
        public abstract void Process(DelayedAction action);

        /// <summary>
        /// 是否能飞跃
        /// <para>当前坐标到目标坐标是否是一条无障碍的直线</para>
        /// </summary>
        /// <param name="target">目标坐标</param>
        /// <returns></returns>
        public bool CanFly(Point target)
        {
            Point location = CurrentLocation;

            while (location != target)
            {
                MirDirection dir = Functions.DirectionFromPoint(location, target);
                // 每次向目标方向查找一格
                location = Functions.PointMove(location, dir, 1);
                // 判断边界，越界返回 false
                if (location.X < 0 || location.Y < 0 || location.X >= CurrentMap.Width || location.Y >= CurrentMap.Height) return false;
                // 当前单元格是否能通过， 不能通过返回fasle
                if (!CurrentMap.GetCell(location).Valid) return false;
            }
            // 是一条无障碍的直线
            return true;
        }
        /// <summary>
        /// 标记当前对象已生成（Spawn）并正式加入游戏世界。
        /// <para>虚方法，允许子类重写</para>
        ///
        /// 执行流程：
        /// 1. 将对象加入全局对象链表（Envir.Objects）
        /// 2. 若为怪物且开启多线程，则加入对应 AI 线程的对象列表
        /// 3. 初始化下一次可执行（Operate）时间，用于分散 AI 处理负载
        /// 4. 向可视范围内的玩家广播对象信息与血量状态
        /// 5. 检查当前位置是否处于安全区
        ///
        /// 注意：
        /// - 若对象已存在于世界（Node != null），会抛出异常，防止重复 Spawn。
        /// - 此方法通常在对象创建或复活时调用。
        ///
        /// 副作用：
        /// - 对象开始参与地图逻辑、AI、渲染广播等所有游戏流程。
        /// </summary>
        public virtual void Spawned()
        {
            // 放置重复添加
            if (Node != null)
                throw new InvalidOperationException("Node is not null, Object already spawned");
            // 添加到游戏世界总对象中, 并记录自身在游戏对象中的节点
            Node = Envir.Objects.AddLast(this);
            if ((Race == ObjectType.Monster) && Settings.Multithreaded)
            {
                // 获取当前地图所在的线程
                SpawnThread = CurrentMap.Thread;
                // 把怪物放到当前地图所在的线程的ObjectsList中返回当前怪物所在怪物对象的节点
                NodeThreaded = Envir.MobThreads[SpawnThread].ObjectsList.AddLast(this);
            }

            /*
             * 设置下一次可执行操作时间
             * 的主要作用是: 为新生成对象随机分配首次 AI 执行时间，使大量对象不会在同一时刻执行逻辑，从而分散服务器负载、降低性能峰值
             */
            OperateTime = Envir.Time + Envir.Random.Next(OperateDelay);
            // 向可视范围内的玩家广播对象信息
            BroadcastInfo();
            // 向可视范围内的玩家广播对象血量状态
            BroadcastHealthChange();
            // 是否在安全区
            InSafeZone = CurrentMap != null && CurrentMap.GetSafeZone(CurrentLocation) != null;
        }
        public virtual void Despawn()
        {
            // 当前对象在世界对象中是否存在，如果不存在则报错
            if (Node == null)
                throw new InvalidOperationException("Node is null, Object already Despawned");
            // 告诉客户端移除ID为ObjectID的对象
            Broadcast(new S.ObjectRemove { ObjectID = ObjectID });
			// 全局对象链表移除 Node
			Envir.Objects.Remove(Node);
            if (Settings.Multithreaded && (Race == ObjectType.Monster))
            {
                // 线程中移除 NodeThreaded
                Envir.MobThreads[SpawnThread].ObjectsList.Remove(NodeThreaded);
            }

            ActionList.Clear();
            // 遍历所有的随从，调用他们的死亡方法
            for (int i = Pets.Count - 1; i >= 0; i--)
                Pets[i].Die();
            // 释放Node
            Node = null;
        }
        /// <summary>
        /// 根据ID查找对象
        /// <para>以中心点正方形向外查找对象</para>
        /// </summary>
        /// <param name="targetID">目标ID</param>
        /// <param name="dist">查找距离</param>
        /// <returns></returns>
        public MapObject FindObject(uint targetID, int dist)
        {
            for (int d = 0; d <= dist; d++)
            {
                for (int y = CurrentLocation.Y - d; y <= CurrentLocation.Y + d; y++)
                {
                    if (y < 0) continue;
                    if (y >= CurrentMap.Height) break;

                    for (int x = CurrentLocation.X - d; x <= CurrentLocation.X + d; x += Math.Abs(y - CurrentLocation.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0) continue;
                        if (x >= CurrentMap.Width) break;

                        Cell cell = CurrentMap.GetCell(x, y);
                        if (!cell.Valid || cell.Objects == null) continue;

                        for (int i = 0; i < cell.Objects.Count; i++)
                        {
                            MapObject ob = cell.Objects[i];
                            if (ob.ObjectID != targetID) continue;

                            return ob;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 个当前地图的玩家进行广播
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="p">要广播的包裹</param>
        public virtual void Broadcast(Packet p)
        {
            if (p == null || CurrentMap == null) return;
            // 遍历当前地图的玩家
            for (int i = CurrentMap.Players.Count - 1; i >= 0; i--)
            {
                PlayerObject player = CurrentMap.Players[i];
                if (player == this) continue; // 如果是自己不广播
                // 在符合的区域里才广播 默认是以自己为中心大小16个的正方形广播
                if (Functions.InRange(CurrentLocation, player.CurrentLocation, Globals.DataRange))
                    player.Enqueue(p);// 放到消息队列中等待逻辑帧处理
            }
        }
        /// <summary>
        /// 向“附近的其他玩家”广播一个指定的数据包
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        public virtual void BroadcastInfo()
        {

            Broadcast(GetInfo());
            return;
        }
        // 是否可以攻击目标
        public bool IsAttackTarget(MapObject attacker)
        {
            // 如果目标不存在或者已经不在游戏世界中了则不能攻击
            if (attacker == null || attacker.Node == null) return false;
            if (Dead || attacker == this) return false; // 玩家死亡或攻击目标是自身则不能攻击

            var flag = true; // 默认在安全区不能攻击
            if (Race == ObjectType.Monster)// 如果是怪物
            {
                // Check if we are a training AI - we can be attacked in safezones
                if (((MonsterObject)this).Info.AI == 56) // 如果目标是练功师则可以进行攻击
                    flag = false; // 标记为fasle（在安全区可以攻击）
            }
            if (flag && (InSafeZone || attacker.InSafeZone)) return false; // 不是练功师且在安全区不能攻击

            switch (attacker.Race)
            {
                case ObjectType.Player:
                    return IsAttackTarget((PlayerObject)attacker); // 如果是玩家则调用玩家的攻击目标方法
                case ObjectType.Hero:
                    return IsAttackTarget((HeroObject)attacker); // 如果是英雄则调用英雄的攻击目标方法
                case ObjectType.Monster:
                    return IsAttackTarget((MonsterObject)attacker); // 如果是怪物则调用怪物的攻击目标方法
                default:
                    throw new NotSupportedException(); // 不支持的攻击目标（报错）
            }
        }
        /// <summary>
        /// 是否可以攻击人形目标
        /// <para>抽象方法，需要子类重写</para>
        /// </summary>
        /// <param name="attacker">目标</param>
        /// <returns></returns>
        public abstract bool IsAttackTarget(HumanObject attacker);
        /// <summary>
        /// 是否可以攻击怪物
        /// <para>抽象方法，需要子类重写</para>
        /// </summary>
        /// <param name="attacker">目标</param>
        /// <returns></returns>
        public abstract bool IsAttackTarget(MonsterObject attacker);
        /// <summary>
        /// 人类目标攻击了我
        /// </summary>
        /// <param name="attacker"> 发起攻击的是人类对象 </param>
        /// <param name="damage"> 攻击的原始伤害 </param>
        /// <param name="type"> 防御类型，默认为 </param>
        /// <param name="damageWeapon"> 武器是否有耐久度, 如果没有耐久则不计算武器伤害 </param>
        /// <returns></returns>
        public abstract int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true);
        /// <summary>
        /// 怪物攻击了我
        /// <para>抽象方法，需要子类重写</para>
        /// </summary>
        /// <param name="attacker">发起攻击的是怪物</param>
        /// <param name="damage">攻击的原始伤害</param>
        /// <param name="type">防御类型</param>
        /// <returns></returns>
        public abstract int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility);
        /// <summary>
        /// 根据防御类型，计算本次攻击的防御值（护甲/魔防），并判断本次攻击是否命中。
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="type">防御类型</param>
        /// <param name="attacker">发起攻击的对象</param>
        /// <param name="hit">是否命中</param>
        /// <returns>计算得到的最终防御值</returns>
        public virtual int GetArmour(DefenceType type, MapObject attacker, out bool hit)
        {
            var armour = 0;
            hit = true; // 默认命中
            switch (type)
            {
                case DefenceType.ACAgility:
                	// 从0到用户属性表的敏捷值进行随机，随机结果大于攻击者的命中则攻击失败
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy])
                    {
                        // 把Miss通知放到通知队列中
                        BroadcastDamageIndicator(DamageType.Miss);
                        hit = false; // 标记为攻击未命中
                    }
                    // 获取防御力
                    armour = GetDefencePower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.AC:
                    armour = GetDefencePower(Stats[Stat.MinAC], Stats[Stat.MaxAC]);
                    break;
                case DefenceType.MACAgility:
                    if (Envir.Random.Next(Settings.MagicResistWeight) < Stats[Stat.MagicResist])
                    {
                        BroadcastDamageIndicator(DamageType.Miss);
                        hit = false;
                    }
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy])
                    {
                        BroadcastDamageIndicator(DamageType.Miss);
                        hit = false;
                    }
                    armour = GetDefencePower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.MAC:
                    if (Envir.Random.Next(Settings.MagicResistWeight) < Stats[Stat.MagicResist])
                    {
                        BroadcastDamageIndicator(DamageType.Miss);
                        hit = false;
                    }
                    armour = GetDefencePower(Stats[Stat.MinMAC], Stats[Stat.MaxMAC]);
                    break;
                case DefenceType.Agility:
                    if (Envir.Random.Next(Stats[Stat.Agility] + 1) > attacker.Stats[Stat.Accuracy])
                    {
                        BroadcastDamageIndicator(DamageType.Miss);
                        hit = false;
                    }
                    break;
            }
            return armour;
        }

        /// <summary>
        /// 应用负面效果/施加异常状态(物理攻击)
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="type">防御类型</param>
        /// <param name="levelOffset">等级压制</param>
        public virtual void ApplyNegativeEffects(HumanObject attacker, DefenceType type, ushort levelOffset)
        {
            // type != DefenceType.MAC && type != DefenceType.MACAgility 没有攻击类型，通过防御类型进行判断是否是物理攻击
            // 如果有麻痹属性则 14 分之 1 触发麻痹效果
            if (attacker.SpecialMode.HasFlag(SpecialItemMode.Paralize) && type != DefenceType.MAC && type != DefenceType.MACAgility && 1 == Envir.Random.Next(1, 15))
            {
                ApplyPoison(new Poison { PType = PoisonType.Paralysis, Duration = 5, TickSpeed = 1000 }, attacker);
            }
            if ((attacker.Stats[Stat.Freezing] > 0) && (Settings.PvpCanFreeze || Race != ObjectType.Player) && type != DefenceType.MAC && type != DefenceType.MACAgility)
            {
                if ((Envir.Random.Next(Settings.FreezingAttackWeight) < attacker.Stats[Stat.Freezing]) && (Envir.Random.Next(levelOffset) == 0))
                    ApplyPoison(new Poison { PType = PoisonType.Slow, Duration = Math.Min(10, (3 + Envir.Random.Next(attacker.Stats[Stat.Freezing]))), TickSpeed = 1000 }, attacker);
            }
            if (attacker.Stats[Stat.PoisonAttack] > 0 && type != DefenceType.MAC && type != DefenceType.MACAgility)
            {
                if ((Envir.Random.Next(Settings.PoisonAttackWeight) < attacker.Stats[Stat.PoisonAttack]) && (Envir.Random.Next(levelOffset) == 0))
                    ApplyPoison(new Poison { PType = PoisonType.Green, Duration = 5, TickSpeed = 1000, Value = Math.Min(10, 3 + Envir.Random.Next(attacker.Stats[Stat.PoisonAttack])) }, attacker);
            }
        }
        /// <summary>
        /// 处理对象受到伤害的核心方法
        /// <para>抽象方法，允许子类重写</para>
        /// </summary>
        /// <param name="damage">原始伤害值，表示攻击者造成的基础伤害</param>
        /// <param name="type">防御类型</param>
        /// <returns></returns>
        public abstract int Struck(int damage, DefenceType type = DefenceType.ACAgility);

        /// <summary>
        /// 判断当前对象是否将传入的MapObject视为友好目标
        /// </summary>
        /// <param name="ally">需要判断友好关系的对象</param>
        /// <returns>如果是友好目标则返回true，否则返回false</returns>
        public bool IsFriendlyTarget(MapObject ally)
        {
            switch (ally.Race)
            {
                case ObjectType.Player:
                    return IsFriendlyTarget((PlayerObject)ally);
                case ObjectType.Hero:
                    return IsFriendlyTarget((HeroObject)ally);
                case ObjectType.Monster:
                    return IsFriendlyTarget((MonsterObject)ally);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 判断当前对象是否将传入的HumanObject视为友好目标
        /// <para>抽象方法，允许子类重写</para>
        /// </summary>
        /// <param name="ally">需要判断友好关系的人类对象</param>
        /// <returns>如果是友好目标则返回true，否则返回false</returns>
        public abstract bool IsFriendlyTarget(HumanObject ally);

        /// <summary>
        /// 判断当前对象是否将传入的MonsterObject视为友好目标
        /// <para>抽象方法，允许子类重写</para>
        /// </summary>
        /// <param name="ally">需要判断友好关系的怪物对象</param>
        /// <returns>如果是友好目标则返回true，否则返回false</returns>
        public abstract bool IsFriendlyTarget(MonsterObject ally);
        /// <summary>
        /// 接收聊天
        /// <para>抽象方法，子类要重写</para>
        /// </summary>
        /// <param name="text">接收的文本</param>
        /// <param name="type">聊天类型</param>
        public abstract void ReceiveChat(string text, ChatType type);

        /// <summary>
        /// 获取 Packet ObjectPlayer
        /// </summary>
        /// <returns></returns>
        public abstract Packet GetInfo();
        /// <summary>
        /// 处理对象获得经验值的逻辑
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="amount">要增加的经验值</param>
        /// <param name="targetLevel">目标等级（可选），如果指定则在升级到目标等级后触发事件</param>
        public virtual void WinExp(uint amount, uint targetLevel = 0)
        {


        }
        /// <summary>
        /// 用于验证对象是否可以获得指定数量的金币
        /// <para> 默认不可以 </para>
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="gold">金币</param>
        /// <returns></returns>
        public virtual bool CanGainGold(uint gold)
        {
            return false;
        }
        /// <summary>
        /// 最终增加金币的方法
        /// <para>虚方法，允许子类重写</para>
        /// </summary>
        /// <param name="gold">金币</param>
        public virtual void WinGold(uint gold)
        {

        }
        /// <summary>
        /// 处理玩家对当前地图对象的采集操作
        /// <para>虚方法，允许子类重写实现特定的采集逻辑</para>
        /// </summary>
        /// <param name="player">执行采集操作的玩家对象</param>
        /// <returns>采集是否成功</returns>
        public virtual bool Harvest(PlayerObject player) { return false; }
		/// <summary>
		///  应用中毒效果
		/// </summary>
		/// <param name="p">毒药的对象</param>
		/// <param name="Caster">要对谁施加中毒</param>
		/// <param name="NoResist">是否要忽略中毒几率</param>
		/// <param name="ignoreDefence">是否忽略目标的防御减免（只对绿毒有用）</param>
		public abstract void ApplyPoison(Poison p, MapObject Caster = null, bool NoResist = false, bool ignoreDefence = true);

        /// <summary>
        /// 为地图对象添加或更新一个buff效果
        /// <para>虚方法，允许子类重写实现特定的buff添加逻辑</para>
        /// </summary>
        /// <param name="type">buff类型</param>
        /// <param name="owner">buff的施加者</param>
        /// <param name="duration">buff持续时间</param>
        /// <param name="stats">buff带来的属性变化</param>
        /// <param name="refreshStats">是否刷新属性</param>
        /// <param name="updateOnly">是否执行叠加：true（不叠加，新增）false（叠加）</param>
        /// <param name="values">额外的buff参数数组</param>
        /// <returns>添加或更新后的buff对象</returns>
        public virtual Buff AddBuff(BuffType type, MapObject owner, int duration, Stats stats, bool refreshStats = true, bool updateOnly = false, params int[] values)
        {
            // 不包含就创建一个buff
            if (!HasBuff(type, out Buff buff))
            {
                // 创建buff对象
                buff = new Buff(type)
                {
                    Caster = owner,
                    ObjectID = ObjectID,
                    ExpireTime = duration,
                    LastTime = Envir.Time,
                    Stats = stats
                };

                Buffs.Add(buff);
            }
            else
            {
                // 已有 buff了 叠加 updateOnly 为 false
                if (!updateOnly)
                {

                    switch (buff.StackType)
                    {
                        // 不叠加只刷新时间
                        case BuffStackType.ResetDuration:
                            {
                                // 重置时间
                                buff.ExpireTime = duration;
                            }
                            break;
                        // 持续时间可以叠加
                        case BuffStackType.StackDuration:
                            {
                                buff.ExpireTime += duration;
                            }
                            break;
                        // 属性叠加时间不变
                        case BuffStackType.StackStat:
                            {
                                if (stats != null)
                                {
                                    buff.Stats.Add(stats);
                                }
                            }
                            break;
                        // 属性时间都叠加
                        case BuffStackType.StackStatAndDuration:
                            {
                                if (stats != null)
                                {
                                    buff.Stats.Add(stats);
                                }

                                buff.ExpireTime += duration;
                            }
                            break;
                        // 属性重置时间不变
                        case BuffStackType.ResetStat:
                        {
                            if (stats != null)
                            {
                                buff.Stats = stats;
                            }
                        }
                            break;
                        // 属性和时间全部重置
                        case BuffStackType.ResetStatAndDuration:
                        {
                            buff.ExpireTime = duration;
                            if (stats != null)
                            {
                                buff.Stats = stats;
                            }
                        }
                            break;
                        // 无限时间和不允许None不处理
                        case BuffStackType.Infinite:
                        case BuffStackType.None:
                            break;
                    }
                }
            }
            // 在安全区是否暂停buff
            if (buff.Properties.HasFlag(BuffProperty.PauseInSafeZone) && InSafeZone)
            {
                buff.Paused = true;
            }
            // buff.Stats 没有值这赋默认值
            buff.Stats ??= new Stats();
            // Values没有值赋默认值
            buff.Values = values ?? new int[0];
            // 如果施法者下线了，施法者指向自己(当前的MMO为了解决这个问题改成了进入战斗后不允许施法者下线)
            if (buff.Caster?.Node == null)
                buff.Caster = owner;
            switch (buff.Type)
            {
                case BuffType.MoonLight:
                case BuffType.DarkBody:
                    // 月光术、暗影术
                    Hidden = true;
                    Sneaking = true;
                    HideFromTargets();
                    break;
                case BuffType.Hiding:
                case BuffType.ClearRing:
                    // 隐身术、隐身戒指
                    Hidden = true;
                    HideFromTargets();
                    break;
            }
            // 返回buff引用
            return buff;
        }

        /// <summary>
        /// 移除指定类型的buff效果
        /// <para>虚方法，允许子类重写实现特定的buff移除逻辑</para>
        /// </summary>
        /// <param name="b">要移除的buff类型</param>
        public virtual void RemoveBuff(BuffType b)
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Type != b) continue;

                Buffs[i].FlagForRemoval = true;
                Buffs[i].Paused = false;
                Buffs[i].ExpireTime = 0;

                switch(b)
                {
                    case BuffType.Hiding:
                    case BuffType.MoonLight:
                    case BuffType.DarkBody:
                        if (!HasAnyBuffs(b, BuffType.ClearRing, BuffType.Hiding, BuffType.MoonLight, BuffType.DarkBody))
                        {
                            Hidden = false;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 检查对象是否拥有指定类型的buff效果
        /// </summary>
        /// <param name="type">要检查的buff类型</param>
        /// <returns>如果拥有该类型buff则返回true，否则返回false</returns>
        public bool HasBuff(BuffType type)
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Type != type) continue;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查对象是否拥有指定类型的buff效果，如果有则返回该buff对象
        /// </summary>
        /// <param name="type">要检查的buff类型</param>
        /// <param name="buff">输出参数，返回找到的buff对象</param>
        /// <returns>如果拥有该类型buff则返回true，否则返回false</returns>
        public bool HasBuff(BuffType type, out Buff buff)
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Type != type) continue;

                buff = Buffs[i];
                return true;
            }

            buff = null;
            return false;
        }

        /// <summary>
        /// 检查对象是否拥有除指定buff外的其他任一指定类型buff效果
        /// </summary>
        /// <param name="exceptBuff">要排除的buff类型</param>
        /// <param name="types">要检查的buff类型数组</param>
        /// <returns>如果拥有任一指定类型buff（除了exceptBuff）则返回true，否则返回false</returns>
        public bool HasAnyBuffs(BuffType exceptBuff, params BuffType[] types)
        {
            return Buffs.Select(x => x.Type).Except(new List<BuffType> { exceptBuff }).Intersect(types).Any();
        }

        /// <summary>
        /// 暂停指定的buff效果
        /// <para>虚方法，允许子类重写实现特定的buff暂停逻辑</para>
        /// </summary>
        /// <param name="b">要暂停的buff对象</param>
        public virtual void PauseBuff(Buff b)
        {
            if (b.Paused) return;

            b.Paused = true;
        }

        /// <summary>
        /// 恢复暂停的buff效果
        /// <para>虚方法，允许子类重写实现特定的buff恢复逻辑</para>
        /// </summary>
        /// <param name="b">要恢复的buff对象</param>
        public virtual void UnpauseBuff(Buff b)
        {
            if (!b.Paused) return;

            b.Paused = false;
        }
        /// <summary>
        /// 清除仇恨（清理不能破隐的怪物并且等级要小于自身等级，否则不能清除仇恨）
        /// <para>清理以自身为中心检测1089个格子所在的怪物的仇恨</para>
        /// </summary>
        protected void HideFromTargets()
        {
            for (int y = CurrentLocation.Y - Globals.DataRange; y <= CurrentLocation.Y + Globals.DataRange; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = CurrentLocation.X - Globals.DataRange; x <= CurrentLocation.X + Globals.DataRange; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;
                    if (x < 0 || x >= CurrentMap.Width) continue;

                    Cell cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int i = 0; i < cell.Objects.Count; i++)
                    {
                        MapObject ob = cell.Objects[i];
                        if (ob.Race != ObjectType.Monster) continue;

                        if (ob.Target == this && (!ob.CoolEye || ob.Level < Level)) ob.Target = null;
                    }
                }
            }
        }
        /// <summary>
        /// 是否不可以越过
        /// </summary>
        /// <returns></returns>
        public bool CheckStacked()
        {
            Cell cell = CurrentMap.GetCell(CurrentLocation);

            if (cell.Objects != null)
                for (int i = 0; i < cell.Objects.Count; i++)
                {
                    MapObject ob = cell.Objects[i];
                    if (ob == this || !ob.Blocking) continue;
                    return true;
                }

            return false;
        }
        /// <summary>
        /// 传送到目标地图
        /// </summary>
        /// <param name="temp">目标地图</param>
        /// <param name="location">目标位置</param>
        /// <param name="effects">是否播放传送特效</param>
        /// <param name="effectnumber">特效编号</param>
        /// <returns>是否传送成功</returns>
        public virtual bool Teleport(Map temp, Point location, bool effects = true, byte effectnumber = 0)
        {
            // 目标地图不为 null || 坐标不存在， 不允许传送
            if (temp == null || !temp.ValidPoint(location)) return false;
            // 当前地图移除自身对象
            CurrentMap.RemoveObject(this);
            // 是否通知客户端播放传送特效
            if (effects) Broadcast(new S.ObjectTeleportOut {ObjectID = ObjectID, Type = effectnumber});
            Broadcast(new S.ObjectRemove {ObjectID = ObjectID}); // 通知客户端移除 ObjectID
            // 绑定到目标地图
            CurrentMap = temp;
            // 绑定当前坐标
            CurrentLocation = location;
            // 解除受困
            InTrapRock = false;
            // 把自身添加到当前地图
            CurrentMap.AddObject(this);
            // 通知客户端
            BroadcastInfo();
            // 是否通知客户端播放特效编号
            if (effects) Broadcast(new S.ObjectTeleportIn { ObjectID = ObjectID, Type = effectnumber });

            BroadcastHealthChange();

            return true;
        }
        /// <summary>
        /// 随机传送
        /// </summary>
        /// <param name="attempts"></param>
        /// <param name="distance"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public virtual bool TeleportRandom(int attempts, int distance, Map map = null)
        {
            if (map == null) map = CurrentMap;
            if (map.Cells == null) return false;
            if (map.WalkableCells.Count == 0) return false;

            int cellIndex = Envir.Random.Next(map.WalkableCells.Count);

            return Teleport(map, map.WalkableCells[cellIndex]);
        }

        public Point GetRandomPoint(int attempts, int distance, Map map)
        {
            byte edgeoffset = 0;

            if (map.Width < 150)
            {
                if (map.Height < 30) edgeoffset = 2;
                else edgeoffset = 20;
            }

            for (int i = 0; i < attempts; i++)
            {
                Point location;

                if (distance <= 0)
                    location = new Point(edgeoffset + Envir.Random.Next(map.Width - edgeoffset), edgeoffset + Envir.Random.Next(map.Height - edgeoffset)); //Can adjust Random Range...
                else
                    location = new Point(CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                         CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));


                if (map.ValidPoint(location)) return location;
            }

            return new Point(0, 0);
        }
        /// <summary>
		/// 广播生命值变化
		/// </summary>
        public virtual void BroadcastHealthChange()
        {
            // 如果不是玩家 || 怪物， 不允许广播
            if (Race != ObjectType.Player && Race != ObjectType.Monster) return;
            // 默认回显示 5s 的血量（百分比显示），如果用了心灵启示的技能则显示的时间更长
            byte time = Math.Min(byte.MaxValue, (byte)Math.Max(5, (RevTime - Envir.Time) / 1000));
            Packet p = new S.ObjectHealth { ObjectID = ObjectID, Percent = PercentHealth, Expire = time };
            // 如果主环境的时间小于等于 RevTime， 则广播
            if (Envir.Time <= RevTime)
            {
                CurrentMap.Broadcast(p, CurrentLocation);
                return;
            }
            // 如果是怪物 && 不允许广播生命值 && 没有主人，则不允许广播
            if (Race == ObjectType.Monster && !AutoRev && Master == null) return;
            // 如果是玩家
            if (Race == ObjectType.Player)
            {
                // 有分组成员
                if (GroupMembers != null) //Send HP to group
                {
                    for (int i = 0; i < GroupMembers.Count; i++)
                    {
                        PlayerObject member = GroupMembers[i];

                        if (this == member) continue;
                        // 组员不在当前地图或者不在附近范围不广播
                        if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                        member.Enqueue(p);
                    }
                }

                return;
            }
            // 当前对象有主人，且主人是玩家
            if (Master != null && Master.Race == ObjectType.Player)
            {
                // 拿到主人实例对象
                PlayerObject player = (PlayerObject)Master;
                // 广播给主人
                player.Enqueue(p);
                // 玩家有组队， 则广播给组队成员
                if (player.GroupMembers != null) //Send pet HP to group
                {
                    for (int i = 0; i < player.GroupMembers.Count; i++)
                    {
                        PlayerObject member = player.GroupMembers[i];

                        if (player == member) continue; // 自身不通知

                        if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                        member.Enqueue(p);
                    }
                }
            }

            // 当前对象的血量变化广播给拥有经验归属的玩家及队友
            if (EXPOwner != null && EXPOwner.Race == ObjectType.Player)
            {
                // 找到玩家实例
                PlayerObject player = (PlayerObject)EXPOwner;

				/* 去重（如果经验归属跟宝宝归属是同一个玩家则不再通知，因为前一个判断已经发过通知了）
                 * 如果 Master 有值 在同一个逻辑帧里指向肯定是固定的一位玩家（法师的诱惑之光可以诱惑走别的法师宝宝，这个在同一逻辑帧里不考虑）
                 * Master != null && Master.Race == ObjectType.Player 这个判断已经发过通知了,也就是说宝宝主人及所在的队伍已经通知过了
                 * EXPOwner 有两种情况
                 * 1. 指向的对象跟Master一样，这个时间不需要通知了，因为已经通知过了
                 * 2. 指向的对象不是Master， 没有通知过需要再次通知
                 * player.IsMember(Master): 指向的对象跟Master一样 不需要通知了
                 */
				if (player.IsMember(Master)) return;

                player.Enqueue(p); // 单独发送给 EXPOwner

                if (player.GroupMembers != null)
                {
                    for (int i = 0; i < player.GroupMembers.Count; i++)
                    {
                        PlayerObject member = player.GroupMembers[i];

                        if (player == member) continue; // 去重
                        if (member.CurrentMap != CurrentMap || !Functions.InRange(member.CurrentLocation, CurrentLocation, Globals.DataRange)) continue;
                        member.Enqueue(p); // 发送通知
                    }
                }
            }
        }
        /// <summary>
        /// 广播伤害指示器
        /// </summary>
        /// <param name="type">伤害类型</param>
        /// <param name="damage">伤害值</param>
        public void BroadcastDamageIndicator(DamageType type, int damage = 0)
        {
            Packet p = new S.DamageIndicator { ObjectID = ObjectID, Damage = damage, Type = type };

            if (Race == ObjectType.Player)
            {
                // 如果是玩家则通知给玩家发送
                PlayerObject player = (PlayerObject)this;
                player.Enqueue(p);
            }
            // 通知给周围的玩家
            Broadcast(p);
        }
        /// <summary>
        /// 死亡
        /// <para>抽象方法，子类需要重写</para>
        /// </summary>
        public abstract void Die();
        /// <summary>
        /// 推动效果
        /// </summary>
        /// <param name="pusher">推动者</param>
        /// <param name="dir">方向</param>
        /// <param name="distance">距离</param>
        /// <returns></returns>
        public abstract int Pushed(MapObject pusher, MirDirection dir, int distance);
        /// <summary>
		/// 是否是组队成员
		/// </summary>
		/// <param name="member">玩家对象</param>
		/// <returns>是否是队伍中的成员</returns>
        public bool IsMember(MapObject member)
        {
            if (member == this) return true;
            if (GroupMembers == null || member == null) return false;

            for (int i = 0; i < GroupMembers.Count; i++)
                if (GroupMembers[i] == member) return true;

            return false;
        }

        public abstract void SendHealth(HumanObject player);
        /// <summary>
        /// 是否受困
        /// </summary>
        public bool InTrapRock
        {
            set
            {
                if (this is PlayerObject)
                {
                    var player = (PlayerObject)this;
                    player.Enqueue(new S.InTrapRock { Trapped = value });
                }
            }
            get
            {
                Point checklocation;

                for (int i = 0; i <= 6; i += 2)
                {
                    checklocation = Functions.PointMove(CurrentLocation, (MirDirection)i, 1);

                    if (checklocation.X < 0) continue;
                    if (checklocation.X >= CurrentMap.Width) continue;
                    if (checklocation.Y < 0) continue;
                    if (checklocation.Y >= CurrentMap.Height) continue;

                    Cell cell = CurrentMap.GetCell(checklocation.X, checklocation.Y);
                    if (!cell.Valid || cell.Objects == null) continue;

                    for (int j = 0; j < cell.Objects.Count; j++)
                    {
                        MapObject ob = cell.Objects[j];
                        switch (ob.Race)
                        {
                            case ObjectType.Monster:
                                if (ob is TrapRock)
                                {
                                    TrapRock rock = (TrapRock)ob;
                                    if (rock.Dead) continue;
                                    if (rock.Target != this) continue;
                                    if (!rock.Visible) continue;
                                }
                                else continue;

                                return true;
                            default:
                                continue;
                        }
                    }
                }
                return false;
            }
        }

    }
    /// <summary>
    /// 毒物类
    /// 作用原理：Envir.time(主环境时间)-TickTime(上一次生效时间) >= TickSpeed（间隔时间）
    /// </summary>
    public class Poison
    {
        /// <summary>
        /// 毒效果的施放者
        /// 如果是 HeroObject，则自动归属到 Hero 的主人（玩家）
        /// 用于击杀归属、仇恨计算、PK 判定等
        /// </summary>
        private MapObject owner;

        /// <summary>
        /// 对外暴露的毒效果拥有者
        /// Hero 施放的毒，实际视为其主人施放
        /// </summary>
        public MapObject Owner
        {
            get
            {
                return owner switch
                {
                    HeroObject hero => hero.Owner,
                    _ => owner
                };
            }
            set { owner = value; }
        }
        /// <summary>
        /// 毒的类型
        /// 如：绿毒、红毒、麻痹、中毒减防等
        /// 决定毒的具体逻辑处理方式
        /// </summary>
        public PoisonType PType;
        /// <summary>
        /// 毒效果数值
        /// 通常表示：
        /// - 每跳伤害值
        /// - 减少的属性数值
        /// - 状态强度
        /// </summary>
        public int Value;
        /// <summary>
        /// 毒的总持续时间（毫秒）
        /// 超过该时间后毒效果自动移除
        /// </summary>
        public long Duration;
        /// <summary>
        /// 毒开始生效的时间戳（服务器时间）
        /// 用于判断毒是否已过期
        /// </summary>
        public long Time;
        /// <summary>
        /// 上一次触发毒效果的时间
        /// 用于控制 Tick 间隔
        /// </summary>
        public long TickTime;
        /// <summary>
        /// 毒效果的触发间隔（毫秒）
        /// 每隔该时间触发一次扣血 / 效果
        /// </summary>
        public long TickSpeed;
        /// <summary>
        /// 默认构造函数
        /// 通常用于运行时创建毒效果
        /// </summary>
        public Poison() { }
        /// <summary>
        /// 从网络/存档数据中反序列化毒效果
        /// 用于状态同步或角色数据恢复
        /// </summary>
        public Poison(BinaryReader reader)
        {
            Owner = null;
            PType = (PoisonType)reader.ReadByte();
            Value = reader.ReadInt32();
            Duration = reader.ReadInt64();
            Time = reader.ReadInt64();
            TickTime = reader.ReadInt64();
            TickSpeed = reader.ReadInt64();
        }
    }
}
