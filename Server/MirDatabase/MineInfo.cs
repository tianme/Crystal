using System.Drawing;
﻿namespace Server.MirDatabase
{
    /// <summary>
    /// 矿脉
    /// </summary>
    public class MineSet
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// 重生率
        /// </summary>
        public byte SpotRegenRate = 5;
        /// <summary>
        /// 矿点最大矿石数量
        /// </summary>
        public byte MaxStones = 80;
        /// <summary>
        /// 开采命中率
        /// </summary>
        public byte HitRate = 25;
        /// <summary>
        /// 掉落率
        /// </summary>
        public byte DropRate = 10;
        /// <summary>
        /// 总资源槽位
        /// </summary>
        public byte TotalSlots = 100;
        /// <summary>
        /// 掉落物品列表
        /// </summary>
        public List<MineDrop> Drops = new List<MineDrop>();
        /// <summary>
        /// 确保矿脉的掉落物品配置只被设置一次
        /// </summary>
        private bool DropsSet = false;
        /// <summary>
        /// 构造矿脉
        /// </summary>
        /// <param name="mineType">
        /// 矿类型
        /// <para> mineType: 1 </para>
        /// GoldOre（金矿） ：掉落概率槽位1-2
        /// SilverOre（银矿） ：掉落概率槽位3-20
        /// CopperOre（铜矿） ：掉落概率槽位21-45
        /// BlackIronOre（黑铁矿） ：掉落概率槽位46-56
        /// <para> mineType: 2 </para>
        /// PlatinumOre（铂金矿） ：掉落概率槽位1-2
        /// RubyOre（红宝石矿） ：掉落概率槽位3-20
        /// NephriteOre（软玉矿） ：掉落概率槽位21-45
        /// AmethystOre（紫水晶矿） ：掉落概率槽位46-56
        /// </param>
        public MineSet(byte mineType = 0)
        {
            switch (mineType)
            {
                case 1:
                    TotalSlots = 120;
                    Drops.Add(new MineDrop() { ItemName = "GoldOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "SilverOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "CopperOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "BlackIronOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    break;
                case 2:
                    TotalSlots = 100;
                    Drops.Add(new MineDrop() { ItemName = "PlatinumOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "RubyOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "NephriteOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "AmethystOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    break;
            }
        }

        public void SetDrops(List<ItemInfo> items)
        {
            if (DropsSet) return;
            for (int i = 0; i < Drops.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    ItemInfo info = items[j];
                    if (String.Compare(info.Name.Replace(" ", ""), Drops[i].ItemName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                    Drops[i].Item = info;
                    break;
                }
            }
            DropsSet = true;
        }
    }
    /// <summary>
    /// 矿点
    /// </summary>
    public class MineSpot
    {
        /// <summary>
        /// 表示当前矿点剩余的可开采矿石数量
        /// </summary>
        public byte StonesLeft = 0;
        /// <summary>
        /// 记录矿点上次矿石重生的时间戳
        /// </summary>
        public long LastRegenTick = 0;
        /// <summary>
        /// 引用当前矿点所属的矿脉配置信息
        /// </summary>
        public MineSet Mine;
    }
    /// <summary>
    /// 掉落物品配置
    /// </summary>
    public class MineDrop
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string ItemName;
        /// <summary>
        /// 物品信息引用
        /// </summary>
        public ItemInfo Item;
        /// <summary>
        /// 掉落概率最小值
        /// </summary>
        public byte MinSlot = 0;
        /// <summary>
        /// 掉落概率最大值
        /// </summary>
        public byte MaxSlot = 0;
        /// <summary>
        /// 物品最小耐久度
        /// </summary>
        public byte MinDura = 1;
        /// <summary>
        /// 物品最大耐久度
        /// </summary>
        public byte MaxDura = 1;
        /// <summary>
        /// 额外耐久度奖励概率
        /// </summary>
        public byte BonusChance = 0;
        /// <summary>
        /// 最大额外耐久度
        /// </summary>
        public byte MaxBonusDura = 1;
    }
    /// <summary>
    /// 矿区
    /// <para>矿脉类型</para>
    /// <para>Location: 矿脉中心点</para>
    /// <para>Size: 矿脉范围</para>
    /// </summary>
    public class MineZone
    {
        /// <summary>
        /// 定义矿脉类型的标识
        /// <para>0：默认矿脉类型（基础矿石）</para>
        /// <para>1：高级矿脉类型（如GoldOre、SilverOre等）</para>
        /// <para>2：稀有矿脉类型（如PlatinumOre、RubyOre等）</para>
        /// </summary>
        public byte Mine;

        /// <summary>
        /// 定义矿脉区域的中心点在游戏地图上的坐标
        /// </summary>
        public Point Location;
        /// <summary>
        /// 定义矿脉区域的覆盖范围大小
        /// </summary>
        public ushort Size;

        public MineZone()
        {
        }
        /// <summary>
        /// 读取二进制配置
        /// </summary>
        /// <param name="reader">二级制读取器</param>
        public MineZone(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Mine = reader.ReadByte();
        }
        /// <summary>
        /// 保存二进制配置
        /// </summary>
        /// <param name="writer">二级制写入器</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(Mine);
        }
        public override string ToString()
        {
            return string.Format("Mine: {0}- {1}", Functions.PointToString(Location), Mine);
        }
    }
}
