using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirDatabase
{
    /// <summary>
    /// 技能蓝图
    /// </summary>
    public class MagicInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 技能名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 当前技能
        /// </summary>
        public Spell Spell;

        /// <summary>
        /// 魔法基础消耗
        /// </summary>
        public byte BaseCost;
        /// <summary>
        /// 魔法等级消耗
        /// </summary>
        public byte LevelCost;
        /// <summary>
        /// 显示的icon
        /// </summary>
        public byte Icon;
        /// <summary>
        /// 技能等级 1 级
        /// </summary>
        public byte Level1;
        /// <summary>
        /// 技能等级 2 级
        /// </summary>
        public byte Level2;
        /// <summary>
        /// 技能等级 3 级
        /// </summary>
        public byte Level3;
        /// <summary>
        /// 技能等级的经验需求
        /// </summary>
        public ushort Need1, Need2, Need3;
        /// <summary>
        /// 魔法基础延迟时间
        /// </summary>
        public uint DelayBase = 1800;
        /// <summary>
        /// 每级延迟减少值
        /// </summary>
        public uint DelayReduction;
        /// <summary>
        /// 物理攻击力基础值
        /// </summary>
        public ushort PowerBase;
        /// <summary>
        /// 物理攻击力附加值
        /// </summary>
        public ushort PowerBonus;
        /// <summary>
        /// 魔法攻击力基础值
        /// </summary>
        public ushort MPowerBase;
        /// <summary>
        /// 魔法攻击力附加值
        /// </summary>
        public ushort MPowerBonus;
        /// <summary>
        /// 基础伤害倍数
        /// </summary>
        public float MultiplierBase = 1.0f;
        /// <summary>
        /// 每级倍数增加
        /// </summary>
        public float MultiplierBonus;
        /// <summary>
        /// 魔法可以释放的距离
        /// </summary>
        public byte Range = 9;

        public override string ToString()
        {
            return Name;
        }

        public MagicInfo()
        {

        }
        /// <summary>
        /// 初始化技能
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        /// <param name="version">版本</param>
        /// <param name="Customversion">自定义版本</param>
        public MagicInfo (BinaryReader reader, int version = int.MaxValue, int Customversion = int.MaxValue)
        {
            Name = reader.ReadString();
            Spell = (Spell)reader.ReadByte();
            BaseCost = reader.ReadByte();
            LevelCost = reader.ReadByte();
            Icon = reader.ReadByte();
            Level1 = reader.ReadByte();
            Level2 = reader.ReadByte();
            Level3 = reader.ReadByte();
            Need1 = reader.ReadUInt16();
            Need2 = reader.ReadUInt16();
            Need3 = reader.ReadUInt16();
            DelayBase = reader.ReadUInt32();
            DelayReduction = reader.ReadUInt32();
            PowerBase = reader.ReadUInt16();
            PowerBonus = reader.ReadUInt16();
            MPowerBase = reader.ReadUInt16();
            MPowerBonus = reader.ReadUInt16();

            if (version > 66)
                Range = reader.ReadByte();
            if (version > 70)
            {
                MultiplierBase = reader.ReadSingle();
                MultiplierBonus = reader.ReadSingle();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)Spell);
            writer.Write(BaseCost);
            writer.Write(LevelCost);
            writer.Write(Icon);
            writer.Write(Level1);
            writer.Write(Level2);
            writer.Write(Level3);
            writer.Write(Need1);
            writer.Write(Need2);
            writer.Write(Need3);
            writer.Write(DelayBase);
            writer.Write(DelayReduction);
            writer.Write(PowerBase);
            writer.Write(PowerBonus);
            writer.Write(MPowerBase);
            writer.Write(MPowerBonus);
            writer.Write(Range);
            writer.Write(MultiplierBase);
            writer.Write(MultiplierBonus);
        }
    }
    /// <summary>
    /// 用户技能
    /// </summary>

    public class UserMagic
    {
        /// <summary>
        /// 主环境
        /// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 技能
        /// </summary>
        public Spell Spell;
        /// <summary>
        /// 技能蓝图
        /// </summary>
        public MagicInfo Info;
        /// <summary>
        /// 等级
        /// </summary>
        public byte Level;
        /// <summary>
        /// 技能绑定的快捷键
        /// </summary>
        public byte Key;
        /// <summary>
        /// 技能经验值
        /// </summary>
        public ushort Experience;
        /// <summary>
        /// 是不是临时技能
        /// </summary>
        public bool IsTempSpell;
        /// <summary>
        /// 最后释放的时间
        /// </summary>
        public long CastTime;
        /// <summary>
        /// 获取技能蓝图
        /// </summary>
        /// <param name="spell">技能枚举</param>
        /// <returns></returns>
        private MagicInfo GetMagicInfo(Spell spell)
        {
            for (int i = 0; i < Envir.MagicInfoList.Count; i++)
            {
                MagicInfo info = Envir.MagicInfoList[i];
                if (info.Spell != spell) continue;
                return info;
            }
            return null;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="spell"></param>
        public UserMagic(Spell spell)
        {
            Spell = spell;
            // 获取到技能蓝图
            Info = GetMagicInfo(Spell);
        }
        /// <summary>
        /// 从二进制中读取技能
        /// </summary>
        /// <param name="reader">二进制读取器</param>
        /// <param name="version">版本</param>
        /// <param name="customVersion">自定义版本</param>
        public UserMagic(BinaryReader reader, int version, int customVersion)
        {
            Spell = (Spell) reader.ReadByte();
            Info = GetMagicInfo(Spell);

            Level = reader.ReadByte();
            Key = reader.ReadByte();
            Experience = reader.ReadUInt16();

            if (version < 15) return;
            // version大于15读取是否是临时技能
            IsTempSpell = reader.ReadBoolean();

            if (version < 65) return;
            // version 大于 65 读最后施法时间
            CastTime = reader.ReadInt64();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="writer"></param>
        public void Save(BinaryWriter writer)
        {
            writer.Write((byte) Spell);

            writer.Write(Level);
            writer.Write(Key);
            writer.Write(Experience);
            writer.Write(IsTempSpell);
            writer.Write(CastTime);
        }
        /// <summary>
        /// 生成魔法信息的网络数据包，用于发送给客户端显示
        /// </summary>
        /// <param name="hero">是否为英雄角色的魔法</param>
        /// <returns>包含魔法信息的网络数据包</returns>
        public Packet GetInfo(bool hero)
        {
            return new S.NewMagic
                {
                    Magic = CreateClientMagic(),
                    Hero = hero
                };
        }

        public ClientMagic CreateClientMagic()
        {
            return new ClientMagic
            {
                    Name = Info.Name,
                    Spell = Spell,
                    BaseCost = Info.BaseCost,
                    LevelCost = Info.LevelCost,
                    Icon = Info.Icon,
                    Level1 = Info.Level1,
                    Level2 = Info.Level2,
                    Level3 = Info.Level3,
                    Need1 = Info.Need1,
                    Need2 = Info.Need2,
                    Need3 = Info.Need3,
                    Level = Level,
                    Key = Key,
                    Experience = Experience,
                    IsTempSpell = IsTempSpell,
                    Delay = GetDelay(),
                    Range = Info.Range,
                    CastTime = CastTime - Envir.Time
            };
        }
        /// <summary>
        /// 计算出最终伤害
        /// </summary>
        /// <param name="DamageBase">基础伤害</param>
        /// <returns>技能的最终伤害</returns>
        public int GetDamage(int DamageBase)
        {
            return (int)((DamageBase + GetPower()) * GetMultiplier());
        }
        /// <summary>
        /// 获取倍数伤害
        /// </summary>
        /// <returns></returns>
        public float GetMultiplier()
        {
            return (Info.MultiplierBase + (Level * Info.MultiplierBonus));
        }
        /// <summary>
        /// 获取混合伤害
        /// </summary>
        /// <returns>魔法伤害+物理伤害</returns>
        public int GetPower()
        {
            return (int)Math.Round((MPower() / 4F) * (Level + 1) + DefPower());
        }
        /// <summary>
        /// 获取魔法攻击力
        /// </summary>
        /// <returns>技能的魔法攻击力</returns>
        public int MPower()
        {
            if (Info.MPowerBonus > 0)
            {
                return Envir.Random.Next(Info.MPowerBase, Info.MPowerBonus + Info.MPowerBase);
            }
            else
                return Info.MPowerBase;
        }
        /// <summary>
        /// 获取物理攻击力
        /// </summary>
        /// <returns>技能物理攻击力</returns>
        public int DefPower()
        {
            if (Info.PowerBonus > 0)
            {
                return Envir.Random.Next(Info.PowerBase, Info.PowerBonus + Info.PowerBase);
            }
            else
                return Info.PowerBase;
        }

        public int GetPower(int power)
        {
            return (int)Math.Round(power / 4F * (Level + 1) + DefPower());
        }
        /// <summary>
        /// 技能的延迟时间
        /// </summary>
        /// <returns></returns>
        public long GetDelay()
        {
            // 基础延时 - （技能等级+技能缩减的延时时间）
            return Info.DelayBase - (Level * Info.DelayReduction);
        }
    }
}
