using System.Drawing;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    /// <summary>
    /// NPC 蓝图
    /// <para>导出</para>
    /// <para>导入</para>
    /// <para>倍率</para>
    /// <para>根据条件显示或隐藏</para>
    /// <para>可接任务、已完成的任务</para>
    /// </summary>
    public class NPCInfo
    {
        /// <summary>
        /// 编辑环境
        /// </summary>
        protected static Envir EditEnvir
        {
            get { return Envir.Edit; }
        }

        public int Index;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName = string.Empty;

        /// <summary>
        /// 名字
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// 地图索引
        /// </summary>
        public int MapIndex;
        /// <summary>
        /// 位置
        /// </summary>
        public Point Location;
        /// <summary>
        /// npc售卖的价格倍率。默认值100， 倍率为1倍
        /// </summary>
        public ushort Rate = 100;
        /// <summary>
        /// 图片
        /// </summary>
        public ushort Image;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Colour;
        /// <summary>
        /// 定义NPC在游戏中的显示颜色
        /// </summary>
        public bool TimeVisible = false;

        #region 控制NPC在一天中的可见时间段。
        /// <summary>
        /// 控制NPC在一天中的可见时间段
        /// <para>限时NPC（如活动NPC、特定时间段出现的商人）</para>
        /// </summary>
        public byte HourStart = 0;
        public byte MinuteStart = 0;
        public byte HourEnd = 0;
        public byte MinuteEnd = 1;
        #endregion

        /// <summary>
        /// 当MinLev不为0时，玩家等级低于此值则隐藏NPC
        /// <para> 任务相关，比如说玩家等级小于 MinLev 则不显示NPC </para>
        /// </summary>
        public short MinLev = 0;
        /// <summary>
        /// 当MaxLev不为0时，玩家等级高于此值则隐藏NPC
        /// <para> 任务相关，比如说玩家等级大于 MaxLev 则不显示NPC </para>
        /// </summary>
        public short MaxLev = 0;
        /// <summary>
        /// 限制NPC只在特定星期几出现
        /// </summary>
        public string DayofWeek = "";
        /// <summary>
        /// 限制NPC只对特定职业的玩家可见
        /// </summary>
        public string ClassRequired = "";

        /// <summary>
        /// 标记NPC是否与Sabuk征服战相关
        /// </summary>
        public bool Sabuk = false;
        /// <summary>
        /// 要求玩家拥有特定标志才能看到该NPC
        /// </summary>
        public int FlagNeeded = 0;
        /// <summary>
        /// 将NPC关联到特定征服战系统
        /// <para>征服战区域内的功能性NPC（如守卫、管理员）</para>
        /// </summary>
        public int Conquest;
        /// <summary>
        /// 控制NPC是否在大地图上显示
        /// </summary>
        public bool ShowOnBigMap;
        /// <summary>
        /// 定义NPC在大地图上显示的图标ID
        /// </summary>
        public int BigMapIcon;
        /// <summary>
        /// 能否传送到当前NPC
        /// </summary>
        public bool CanTeleportTo;
        /// <summary>
        /// 控制征服战期间NPC的可见性
        /// </summary>
        public bool ConquestVisible = true;
        /// <summary>
        /// 当前NPC可接取的任务
        /// </summary>
        public List<int> CollectQuestIndexes = new List<int>();
        /// <summary>
        /// 当前NPC完成的任务
        /// </summary>
        public List<int> FinishQuestIndexes = new List<int>();

        public NPCInfo() { }
        /// <summary>
        /// 二进制构造函数
        /// </summary>
        /// <param name="reader">二进制阅读器</param>
        public NPCInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            MapIndex = reader.ReadInt32();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                CollectQuestIndexes.Add(reader.ReadInt32());

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                FinishQuestIndexes.Add(reader.ReadInt32());

            FileName = reader.ReadString();
            Name = reader.ReadString();

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());

            if (Envir.LoadVersion >= 72)
            {
                Image = reader.ReadUInt16();
            }
            else
            {
                Image = reader.ReadByte();
            }

            Rate = reader.ReadUInt16();

            if (Envir.LoadVersion >= 64)
            {
                TimeVisible = reader.ReadBoolean();
                HourStart = reader.ReadByte();
                MinuteStart = reader.ReadByte();
                HourEnd = reader.ReadByte();
                MinuteEnd = reader.ReadByte();
                MinLev = reader.ReadInt16();
                MaxLev = reader.ReadInt16();
                DayofWeek = reader.ReadString();
                ClassRequired = reader.ReadString();
                if (Envir.LoadVersion >= 66)
                    Conquest = reader.ReadInt32();
                else
                    Sabuk = reader.ReadBoolean();
                FlagNeeded = reader.ReadInt32();
            }

            if (Envir.LoadVersion > 95)
            {
                ShowOnBigMap = reader.ReadBoolean();
                BigMapIcon = reader.ReadInt32();
            }
            if (Envir.LoadVersion > 96)
                CanTeleportTo = reader.ReadBoolean();

            if (Envir.LoadVersion >= 107)
            {
                ConquestVisible = reader.ReadBoolean();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="writer">二进制写入器</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(MapIndex);

            writer.Write(CollectQuestIndexes.Count());
            for (int i = 0; i < CollectQuestIndexes.Count; i++)
                writer.Write(CollectQuestIndexes[i]);

            writer.Write(FinishQuestIndexes.Count());
            for (int i = 0; i < FinishQuestIndexes.Count; i++)
                writer.Write(FinishQuestIndexes[i]);

            writer.Write(FileName);
            writer.Write(Name);

            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Image);
            writer.Write(Rate);

            writer.Write(TimeVisible);
            writer.Write(HourStart);
            writer.Write(MinuteStart);
            writer.Write(HourEnd);
            writer.Write(MinuteEnd);
            writer.Write(MinLev);
            writer.Write(MaxLev);
            writer.Write(DayofWeek);
            writer.Write(ClassRequired);
            writer.Write(Conquest);
            writer.Write(FlagNeeded);

            writer.Write(ShowOnBigMap);
            writer.Write(BigMapIcon);
            writer.Write(CanTeleportTo);
            writer.Write(ConquestVisible);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="text"></param>
        public static void FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 6) return;

            NPCInfo info;
            bool isNew = false;
            if (!int.TryParse(data[0], out var index))
            {
                index = -1;
            }
            if (index == -1 || (info = EditEnvir.NPCInfoList.FirstOrDefault(d => d.Index == index)) == null)
            {
                info = new NPCInfo() { Index = ++EditEnvir.NPCIndex };
                isNew = true;
            }
            info.FileName = data[1];

            info.MapIndex = EditEnvir.MapInfoList.Where(d => d.FileName == data[2]).FirstOrDefault().Index;

            if (!int.TryParse(data[3], out int x)) return;
            if (!int.TryParse(data[4], out int y)) return;

            info.Location = new Point(x, y);

            info.Name = data[5];

            if (!ushort.TryParse(data[6], out info.Image)) return;
            if (!ushort.TryParse(data[7], out info.Rate)) return;

            if (!bool.TryParse(data[8], out info.ShowOnBigMap)) return;
            if (!int.TryParse(data[9], out info.BigMapIcon)) return;
            if (!bool.TryParse(data[10], out info.CanTeleportTo)) return;
            if (!bool.TryParse(data[11], out info.ConquestVisible)) return;
            if (!short.TryParse(data[12], out info.MinLev)) return;
            if (!short.TryParse(data[13], out info.MaxLev)) return;
            if (!bool.TryParse(data[14], out info.TimeVisible)) return;
            if (!byte.TryParse(data[15], out info.HourStart)) return;
            if (!byte.TryParse(data[16], out info.MinuteStart)) return;
            if (!byte.TryParse(data[17], out info.HourEnd)) return;
            if (!byte.TryParse(data[18], out info.MinuteEnd)) return;

            if (isNew) EditEnvir.NPCInfoList.Add(info);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public string ToText()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}", Index,
                FileName, EditEnvir.MapInfoList.Where(d => d.Index == MapIndex).FirstOrDefault().FileName, Location.X, Location.Y, Name, Image, Rate, ShowOnBigMap, BigMapIcon, CanTeleportTo, ConquestVisible,
                MinLev, MaxLev, TimeVisible, HourStart, MinuteStart, HourEnd, MinuteEnd);
        }

        public override string ToString()
        {
            return $" [{Index}] {FileName}: {Name}, {Functions.PointToString(Location)}";
        }
        /// <summary>
        /// 客户端显示的名称
        /// </summary>
        public string GameName
        {
            get
            {
                string s = Name;
                if (s.Contains("_"))
                {
                    string[] splitName = s.Split('_');
                    s = splitName[splitName.Length - 1];
                }
                return s;
            }
        }

        public ClientNPCInfo ClientInformation
        {
            get
            {
                return new ClientNPCInfo
                {
                    ObjectID = 0,
                    Index = Index,
                    FileName = FileName,
                    Name = Name,
                    MapIndex = MapIndex,
                    Location = Location,
                    Image = Image,
                    Rate = Rate,
                    ShowOnBigMap = ShowOnBigMap,
                    BigMapIcon = BigMapIcon,
                    Icon = BigMapIcon,
                    CanTeleportTo = CanTeleportTo
                };
            }
        }
    }
}
