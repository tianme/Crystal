using Server.MirEnvir;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class BuffInfo
    {
        /// <summary>
        /// buff 类型
        /// </summary>
        public BuffType Type { get; set; }
        /// <summary>
        /// 叠加规则
        /// </summary>
        public BuffStackType StackType { get; set; }
        /// <summary>
        /// buff 的特殊属性标志
        /// </summary>
        public BuffProperty Properties { get; set; }
		/// <summary>
		/// buff在客户端显示的图标ID
		/// </summary>
		public int Icon { get; set; }
		/// <summary>
		/// buff是否在客户端可见
		/// </summary>
		public bool Visible { get; set; }
        /// <summary>
		/// 加载buff信息
		/// </summary>
		/// <returns></returns>
        public static List<BuffInfo> Load()
        {
            List<BuffInfo> info = new List<BuffInfo>
            {
                //Magics
                new BuffInfo { Type = BuffType.TemporalFlux, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.Hiding, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.Haste, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.SwiftFeet, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.Fury, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.SoulShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.BlessedArmour, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.LightBody, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.UltimateEnhancer, Properties = BuffProperty.None, StackType = BuffStackType.ResetStatAndDuration },
                new BuffInfo { Type = BuffType.ProtectionField, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.Rage, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.Curse, Properties = BuffProperty.RemoveOnDeath | BuffProperty.Debuff, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.MoonLight, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.DarkBody, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.Concentration, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.VampireShot, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.PoisonShot, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.CounterAttack, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.MentalState, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.EnergyShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.MagicBooster, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.PetEnhancer, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.ImmortalSkin, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.MagicShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.ElementalBarrier, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },

                //Monsters
                new BuffInfo { Type = BuffType.HornedArcherBuff, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.ColdArcherBuff, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.GeneralMeowMeowShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.RhinoPriestDebuff, Properties = BuffProperty.Debuff, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.PowerBeadBuff, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.HornedWarriorShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.HornedCommanderShield, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration, Visible = true },
                new BuffInfo { Type = BuffType.Blindness, Properties = BuffProperty.RemoveOnDeath | BuffProperty.Debuff, StackType = BuffStackType.ResetDuration },

                //Special
                new BuffInfo { Type = BuffType.GameMaster, Properties = BuffProperty.None, StackType = BuffStackType.Infinite, Visible = Settings.GameMasterEffect },
                new BuffInfo { Type = BuffType.Mentee, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Mentor, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Guild, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Skill, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.ClearRing, Properties = BuffProperty.None, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Transform, Properties = BuffProperty.None, StackType = BuffStackType.None },
                new BuffInfo { Type = BuffType.Lover, Properties = BuffProperty.RemoveOnExit, StackType = BuffStackType.Infinite },
                new BuffInfo { Type = BuffType.Rested, Properties = BuffProperty.None, StackType = BuffStackType.ResetDuration },
                new BuffInfo { Type = BuffType.Prison, Properties = BuffProperty.None, StackType = BuffStackType.None }, //???
                new BuffInfo { Type = BuffType.General, Properties = BuffProperty.None, StackType = BuffStackType.None }, //???
                new BuffInfo { Type = BuffType.Newbie, Properties = BuffProperty.RemoveOnExit, StackType = BuffStackType.Infinite }, //???

                //Stats
                new BuffInfo { Type = BuffType.Exp, Properties = BuffProperty.PauseInSafeZone, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Drop, Properties = BuffProperty.PauseInSafeZone, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Gold, Properties = BuffProperty.PauseInSafeZone, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.BagWeight, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Impact, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Magic, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Taoist, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Storm, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.HealthAid, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.ManaAid, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Defence, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.MagicDefence, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.WonderDrug, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration },
                new BuffInfo { Type = BuffType.Knapsack, Properties = BuffProperty.None, StackType = BuffStackType.StackDuration }
            };

            return info;
        }
    }

    public class Buff
    {
        /// <summary>
		/// 主环境
		/// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
		/// <summary>
		/// Buff 的私有运行时状态数据（不由时间字段直接描述）。
		/// 仅供具体 Buff 逻辑使用，框架层不关心其内容。
		/// 私有字段，通过Get/Set访问。
		/// </summary
		private Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// buff 的蓝图
        /// </summary>
        public BuffInfo Info;
        /// <summary>
        /// 施法者
        /// </summary>
        public MapObject Caster;
        /// <summary>
        /// buff 的归属对象的ID
        /// </summary>
        public uint ObjectID;
        /// <summary>
        /// buff 的过期时间
        /// </summary>
        public long ExpireTime;
        /// <summary>
        /// 上一次Tick的生效时间
        /// </summary>
        public long LastTime;
        /// <summary>
        /// 下次生效时间
        /// </summary>
        public long NextTime;

        /// <summary>
        /// 属性加成系统
        /// </summary>
        public Stats Stats;
		/// <summary>
		///
		/// Buff 参数数组。
		/// 各下标的具体含义由 BuffType 的处理逻辑约定，
		/// 不同 BuffType 之间 Values 的语义可以完全不同。
		///
		/// 使用约定：
		/// 1. 同一 BuffType 内，Values 下标语义必须保持一致；
		/// 2. 不要在不同 BuffType 间复用下标含义；
		/// 3. 仅用于存放简单整数参数（配置/公式用）；
		/// 4. 如需存放运行态状态或复杂对象，请使用 Data。
		///
		/// </summary>
		public int[] Values;
        /// <summary>
		/// 标记用于标记 buff 是否需要移除
		/// </summary>
        public bool FlagForRemoval;
        /// <summary>
		/// 标记用于标记 buff 是否暂停倒计时
		/// </summary>
        public bool Paused;
        /// <summary>
        /// buff 的类型
        /// </summary>
        public BuffType Type
        {
            get { return Info.Type; }
        }

        /// <summary>
        /// buff 的叠加类型
        /// <para>护盾类(MagicShield)</para>
        /// <para>经验、掉落加成</para>
        /// <para>状态类<para>
        /// <para>身份类(GameMaster、师徒、婚姻)</para>
        /// <para>终极强化（UltimateEnhancer）</para>
        /// </summary>
        public BuffStackType StackType
        {
            get { return Info.StackType; }
        }
		/// <summary>
		/// buff的特殊属性标志，用于定义buff的行为特性
		/// <para>例如: </para>
		/// <para>1. 角色死亡时移除该buff</para>
		/// <para>2. 角色退出游戏时移除该buff</para>
		/// <para>3. 负面效果，减益buff</para>
		/// <para>4. 安全区域暂停计时， buff 在安全区域时暂停倒计时</para>
		/// </summary>
		public BuffProperty Properties
        {
            get { return Info.Properties; }
        }
        /// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="type">buff的类型</param>
        public Buff(BuffType type)
        {
            Info = Envir.GetBuffInfo(type);
            Stats = new Stats();
            Data = new Dictionary<string, object>();
        }
        /// <summary>
		/// 从二进制流加载 Buff。
		/// </summary>
		/// <param name="reader">二进制读取器</param>
		/// <param name="version">文件格式版本</param>
		/// <param name="customVersion">自定义版本号</param>
        public Buff(BinaryReader reader, int version, int customVersion)
        {
            var type = (BuffType)reader.ReadByte();

            Info = Envir.GetBuffInfo(type);

            Caster = null;

            if (version < 88)
            {
                var visible = reader.ReadBoolean();
            }

            ObjectID = reader.ReadUInt32();
            ExpireTime = reader.ReadInt64();

            if (version <= 84)
            {
                Values = new int[reader.ReadInt32()];

                for (int i = 0; i < Values.Length; i++)
                {
                    Values[i] = reader.ReadInt32();
                }

                if (version < 88)
                {
                    var infinite = reader.ReadBoolean();
                }

                Stats = new Stats();
                Data = new Dictionary<string, object>();
            }
            else
            {
                if (version < 88)
                {
                    var stackable = reader.ReadBoolean();
                }

                Values = new int[0];
                Stats = new Stats(reader, version, customVersion);
                Data = new Dictionary<string, object>();

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var key = reader.ReadString();
                    var length = reader.ReadInt32();

                    var array = new byte[length];

                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j] = reader.ReadByte();
                    }

                    Data[key] = Functions.DeserializeFromBytes(array);
                }

                if (version > 86)
                {
                    count = reader.ReadInt32();

                    Values = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        Values[i] = reader.ReadInt32();
                    }
                }
            }
        }

		/// <summary>
		/// 保存 Buff 到二进制流。
		/// </summary>
		/// <param name="writer">二进制写入器</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(ObjectID);
            writer.Write(ExpireTime);

            Stats.Save(writer);

            writer.Write(Data.Count);

            foreach (KeyValuePair<string, object> pair in Data)
            {
                var bytes = Functions.SerializeToBytes(pair.Value);

                writer.Write(pair.Key);
                writer.Write(bytes.Length);

                for (int i = 0; i < bytes.Length; i++)
                {
                    writer.Write(bytes[i]);
                }
            }

            writer.Write(Values.Length);

            for (int i = 0; i < Values.Length; i++)
            {
                writer.Write(Values[i]);
            }
        }

        public T Get<T>(string key)
        {
            if (!Data.TryGetValue(key, out object result))
            {
                return default;
            }

            return (T)result;
        }

        public void Set(string key, object val)
        {
            Data[key] = val;
        }
		/// <summary>
		/// 生成用于客户端显示的 Buff 快照。
		/// 仅包含展示所需数据，不暴露任何服务端逻辑或运行时状态。
		/// </summary>
		public ClientBuff ToClientBuff()
        {
            return new ClientBuff
            {
                Type = Type,
                Caster = Caster?.Name ?? "",
                ObjectID = ObjectID,
                Visible = Info.Visible,
                Infinite = StackType == BuffStackType.Infinite,
                Paused = Paused,
                ExpireTime = ExpireTime,
                Stats = new Stats(Stats),
                Values = Values
            };
        }
    }
}
