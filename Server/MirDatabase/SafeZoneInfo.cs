using System.Drawing;
﻿namespace Server.MirDatabase
{
    /// <summary>
    /// 安全区蓝图
    /// <para>安全区是个正方形，以 Location 为中心点，Size为范围</para>
    /// </summary>
    public class SafeZoneInfo
    {
        /// <summary>
        /// 安全区中心点
        /// </summary>
        public Point Location;
        /// <summary>
        /// 范围
        /// </summary>
        public ushort Size;
        /// <summary>
        /// 重生点
        /// </summary>
        public bool StartPoint;
        /// <summary>
        /// 这个字段没有用到，应该是：引用该安全区域所属的地图信息对象，建立安全区域与地图之间的关联
        /// </summary>
        public MapInfo Info;

        public SafeZoneInfo() { }

        public SafeZoneInfo(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            StartPoint = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(StartPoint);
        }

        public override string ToString()
        {
            return string.Format("Map: {0}- {1}", Functions.PointToString(Location), StartPoint);
        }
    }
}
