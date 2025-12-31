public sealed class Stats : IEquatable<Stats>
{
    public SortedDictionary<Stat, int> Values { get; set; } = new SortedDictionary<Stat, int>();
    public int Count => Values.Sum(pair => Math.Abs(pair.Value));

    public int this[Stat stat]
    {
        get
        {
            return !Values.TryGetValue(stat, out int result) ? 0 : result;
        }
        set
        {
            if (value == 0)
            {
                if (Values.ContainsKey(stat))
                {
                    Values.Remove(stat);
                }

                return;
            }

            Values[stat] = value;
        }
    }

    public Stats() { }

    public Stats(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public Stats(BinaryReader reader, int version = int.MaxValue, int customVersion = int.MaxValue)
    {
        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            Values[(Stat)reader.ReadByte()] = reader.ReadInt32();
    }

    public void Add(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Values.Count);

        foreach (KeyValuePair<Stat, int> pair in Values)
        {
            writer.Write((byte)pair.Key);
            writer.Write(pair.Value);
        }
    }

    public void Clear()
    {
        Values.Clear();
    }

    public bool Equals(Stats other)
    {
        if (Values.Count != other.Values.Count) return false;

        foreach (KeyValuePair<Stat, int> value in Values)
            if (other[value.Key] != value.Value) return false;

        return true;
    }
}
/// <summary>
/// 属性公式
/// </summary>
public enum StatFormula : byte
{
    /// <summary>
    /// 生命值
    /// </summary>
    Health,
    /// <summary>
    /// 魔法值
    /// </summary>
    Mana,
    /// <summary>
    /// 负重
    /// </summary>
    Weight,
    /// <summary>
    /// 基础属性值
    /// </summary>
    Stat
}

/// <summary>
/// 角色属性枚举 - 定义游戏中所有可能的角色属性类型
/// </summary>
public enum Stat : byte
{
    /// <summary>
    /// 最小物理攻击
    /// </summary>
    MinAC = 0,
    /// <summary>
    /// 最大物理攻击
    /// </summary>
    MaxAC = 1,
    /// <summary>
    /// 最小魔法攻击
    /// </summary>
    MinMAC = 2,
    /// <summary>
    /// 最大魔法攻击
    /// </summary>
    MaxMAC = 3,
    /// <summary>
    /// 最小道术攻击
    /// </summary>
    MinDC = 4,
    /// <summary>
    /// 最大道术攻击
    /// </summary>
    MaxDC = 5,
    /// <summary>
    /// 最小魔攻
    /// </summary>
    MinMC = 6,
    /// <summary>
    /// 最大魔攻
    /// </summary>
    MaxMC = 7,
    /// <summary>
    /// 最小神圣攻击
    /// </summary>
    MinSC = 8,
    /// <summary>
    /// 最大神圣攻击
    /// </summary>
    MaxSC = 9,

    /// <summary>
    /// 命中率
    /// </summary>
    Accuracy = 10,
    /// <summary>
    /// 敏捷度
    /// </summary>
    Agility = 11,
    /// <summary>
    /// 生命值
    /// </summary>
    HP = 12,
    /// <summary>
    /// 魔法值/法力值
    /// </summary>
    MP = 13,
    /// <summary>
    /// 攻击速度
    /// </summary>
    AttackSpeed = 14,
    /// <summary>
    /// 幸运值
    /// </summary>
    Luck = 15,
    /// <summary>
    /// 背包负重上限
    /// </summary>
    BagWeight = 16,
    /// <summary>
    /// 手持物品负重上限
    /// </summary>
    HandWeight = 17,
    /// <summary>
    /// 穿戴装备负重上限
    /// </summary>
    WearWeight = 18,
    /// <summary>
    /// 伤害反射
    /// </summary>
    Reflect = 19,
    /// <summary>
    /// 强度（物理防御增强）
    /// </summary>
    Strong = 20,
    /// <summary>
    /// 神圣属性
    /// </summary>
    Holy = 21,
    /// <summary>
    /// 冰冻攻击
    /// </summary>
    Freezing = 22,
    /// <summary>
    /// 中毒攻击
    /// </summary>
    PoisonAttack = 23,

    /// <summary>
    /// 魔法抗性
    /// </summary>
    MagicResist = 30,
    /// <summary>
    /// 中毒抗性
    /// </summary>
    PoisonResist = 31,
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    HealthRecovery = 32,
    /// <summary>
    /// 魔法恢复速度
    /// </summary>
    SpellRecovery = 33,
    /// <summary>
    /// 中毒恢复速度
    /// </summary>
    PoisonRecovery = 34, //TODO - Should this be in seconds or milliseconds??
    /// <summary>
    /// 暴击率
    /// </summary>
    CriticalRate = 35,
    /// <summary>
    /// 暴击伤害倍率
    /// </summary>
    CriticalDamage = 36,

    /// <summary>
    /// 最大物理攻击百分比加成
    /// </summary>
    MaxACRatePercent = 40,
    /// <summary>
    /// 最大魔法攻击百分比加成
    /// </summary>
    MaxMACRatePercent = 41,
    /// <summary>
    /// 最大道术攻击百分比加成
    /// </summary>
    MaxDCRatePercent = 42,
    /// <summary>
    /// 最大魔攻百分比加成
    /// </summary>
    MaxMCRatePercent = 43,
    /// <summary>
    /// 最大神圣攻击百分比加成
    /// </summary>
    MaxSCRatePercent = 44,
    /// <summary>
    /// 攻击速度百分比加成
    /// </summary>
    AttackSpeedRatePercent = 45,
    /// <summary>
    /// 生命值百分比加成
    /// </summary>
    HPRatePercent = 46,
    /// <summary>
    /// 魔法值百分比加成
    /// </summary>
    MPRatePercent = 47,
    /// <summary>
    /// 生命偷取百分比
    /// </summary>
    HPDrainRatePercent = 48,

    /// <summary>
    /// 经验获取百分比加成
    /// </summary>
    ExpRatePercent = 100,
    /// <summary>
    /// 物品掉落率百分比加成
    /// </summary>
    ItemDropRatePercent = 101,
    /// <summary>
    /// 金币掉落率百分比加成
    /// </summary>
    GoldDropRatePercent = 102,
    /// <summary>
    /// 挖矿成功率百分比加成
    /// </summary>
    MineRatePercent = 103,
    /// <summary>
    /// 宝石掉落率百分比加成
    /// </summary>
    GemRatePercent = 104,
    /// <summary>
    /// 钓鱼成功率百分比加成
    /// </summary>
    FishRatePercent = 105,
    /// <summary>
    /// 制作成功率百分比加成
    /// </summary>
    CraftRatePercent = 106,
    /// <summary>
    /// 技能经验获取倍数
    /// </summary>
    SkillGainMultiplier = 107,
    /// <summary>
    /// 攻击加成
    /// </summary>
    AttackBonus = 108,

    /// <summary>
    /// 情侣经验加成百分比
    /// </summary>
    LoverExpRatePercent = 120,
    /// <summary>
    /// 师徒伤害加成百分比
    /// </summary>
    MentorDamageRatePercent = 121,
    /// <summary>
    /// 师徒经验加成百分比
    /// </summary>
    MentorExpRatePercent = 123,
    /// <summary>
    /// 伤害减免百分比
    /// </summary>
    DamageReductionPercent = 124,
    /// <summary>
    /// 能量护盾吸收百分比
    /// </summary>
    EnergyShieldPercent = 125,
    /// <summary>
    /// 能量护盾生命值获取
    /// </summary>
    EnergyShieldHPGain = 126,
    /// <summary>
    /// 魔法消耗惩罚百分比
    /// </summary>
    ManaPenaltyPercent = 127,
    /// <summary>
    /// 传送魔法消耗惩罚百分比
    /// </summary>
    TeleportManaPenaltyPercent = 128,
    /// <summary>
    /// 英雄相关属性
    /// </summary>
    Hero = 129,

    /// <summary>
    /// 未知属性
    /// </summary>
    Unknown = 255
}