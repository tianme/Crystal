using System.Drawing;
﻿using Server.MirEnvir;

namespace Server.MirDatabase
{
    /// <summary>
    /// 地图传送点
    /// <para> 比如: 比奇传送到毒蛇山谷的坐标点 </para>
    /// </summary>
    public class MovementInfo
    {
        /// <summary>
        /// 地图索引
        /// </summary>
        public int MapIndex;
        /// <summary>
        /// 传送起始点
        /// </summary>
        public Point Source;
        /// <summary>
        /// 传送点的目标位置坐标
        /// </summary>
        public Point Destination;
        /// <summary>
        /// 标识该传送点是否需要洞穴/通道才能使用
        /// </summary>
        public bool NeedHole;
        /// <summary>
        /// 标识是否需要玩家主动移动才能触发传送
        /// </summary>
        public bool NeedMove;
        /// <summary>
        /// 标识该传送点是否在游戏大地图上显示
        /// </summary>
        public bool ShowOnBigMap;
        /// <summary>
        /// 征服区域索引，与游戏中的攻城/征服系统相关
        /// </summary>
        public int ConquestIndex;
        /// <summary>
        /// 传送点图标索引，用于在大地图上显示特定图标
        /// </summary>
        public int Icon;
        /// <summary>
        /// TODO: 这可能是为地图编辑器或数据管理界面提供的功能，方便用户在编辑地图时动态添加新的传送点配置。
        /// </summary>
        public MovementInfo()
        {

        }

        public MovementInfo(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            Source = new Point(reader.ReadInt32(), reader.ReadInt32());
            Destination = new Point(reader.ReadInt32(), reader.ReadInt32());

            NeedHole = reader.ReadBoolean();
            NeedMove = reader.ReadBoolean();

            if (Envir.LoadVersion < 69) return;
            ConquestIndex = reader.ReadInt32();

            if (Envir.LoadVersion < 95) return;
            ShowOnBigMap = reader.ReadBoolean();
            Icon = reader.ReadInt32();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            writer.Write(Source.X);
            writer.Write(Source.Y);
            writer.Write(Destination.X);
            writer.Write(Destination.Y);
            writer.Write(NeedHole);
            writer.Write(NeedMove);
            writer.Write(ConquestIndex);
            writer.Write(ShowOnBigMap);
            writer.Write(Icon);
        }


        public override string ToString()
        {
            return string.Format("{0} -> Map :{1} - {2}", Source, MapIndex, Destination);
        }
    }
}
