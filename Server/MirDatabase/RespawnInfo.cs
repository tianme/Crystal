using System.Drawing;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    /// <summary>
    /// 怪物重生点
    /// <para>用于维护当前地图上某一类怪物的刷新。例如</para>
    /// <para> 刷新数量 </para>
    /// <para> 刷新时间 </para>
    /// <para> 刷新位置 </para>
    /// <para> AI路径 </para>
    /// </summary>
    public class RespawnInfo
    {
        /// <summary>
        /// 主环境
        /// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 怪物索引
        /// </summary>
        public int MonsterIndex;
        /// <summary>
        /// 重生点的精确坐标位置
        /// </summary>
        public Point Location;
        /// <summary>
        /// 每次重生时生成的怪物数量
        /// </summary>
        public ushort Count;
        /// <summary>
        /// 重生范围扩散值，决定怪物在重生点周围随机分布的范围
        /// <para>如果是1， 则以中心点向外扩展一格</para>
        /// </summary>
        public ushort Spread;
        /// <summary>
        /// 基础重生延迟时间，控制怪物死亡后多久再次生成。
        /// </summary>
        public ushort Delay;
        /// <summary>
        /// 随机延迟时间，在基础延迟上增加随机性。
        /// </summary>
        public ushort RandomDelay;
        /// <summary>
        /// 怪物重生时的初始朝向
        /// </summary>
        public byte Direction;
        /// <summary>
        /// 主要用于地图编辑器
        /// <para>怪物行走路线的路径标识，关联到预设路线</para>
        /// </summary>
        public string RoutePath = string.Empty;
        /// <summary>
        /// 重生点的唯一标识符
        /// </summary>
        public int RespawnIndex;
        /// <summary>
        /// 是否保存重生计时状态，可能用于服务器重启后保持重生周期
        /// </summary>
        public bool SaveRespawnTime = false;

        /// <summary>
        /// 重生计数器，为0时表示不使用此系统。
		/// <para> 当 RespawnTicks > 0 时，怪物重生不是基于固定时间，而是基于 RespawnTimer 类维护的全局"重生时钟"计数器。 </para>
        /// </summary>
        public ushort RespawnTicks; //leave 0 if not using this system!
        /// <summary>
        /// 用于编辑器中创建重生点
        /// </summary>
        public RespawnInfo()
        {

        }
        /// <summary>
        /// 从二进制读取
        /// </summary>
        /// <param name="reader">读取器</param>
        /// <param name="Version">版本</param>
        /// <param name="Customversion">自定义版本</param>
        public RespawnInfo(BinaryReader reader, int Version, int Customversion)
        {
            MonsterIndex = reader.ReadInt32();

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());

            Count = reader.ReadUInt16();
            Spread = reader.ReadUInt16();

            Delay = reader.ReadUInt16();
            Direction = reader.ReadByte();

            RoutePath = reader.ReadString();

            if (Version > 67)
            {
                RandomDelay = reader.ReadUInt16();
                RespawnIndex = reader.ReadInt32();
                SaveRespawnTime = reader.ReadBoolean();
                RespawnTicks = reader.ReadUInt16();
            }
            else
            {
                RespawnIndex = ++Envir.RespawnIndex;
            }
        }
        /// <summary>
		/// TODO: 这可能是为地图编辑器或数据管理界面提供的功能，方便用户在编辑地图时动态添加新的重生点配置。
		/// </summary>
		/// <param name="text">文本，必须要逗号分割，分割后的长度必须是7位</param>
		/// <returns></returns>
        public static RespawnInfo FromText(string text)
        {
            string[] data = text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 7) return null;

            RespawnInfo info = new RespawnInfo();

            int x,y ;

            if (!int.TryParse(data[0], out info.MonsterIndex)) return null;
            if (!int.TryParse(data[1], out x)) return null;
            if (!int.TryParse(data[2], out y)) return null;

            info.Location = new Point(x, y);

            if (!ushort.TryParse(data[3], out info.Count)) return null;
            if (!ushort.TryParse(data[4], out info.Spread)) return null;
            if (!ushort.TryParse(data[5], out info.Delay)) return null;
            if (!byte.TryParse(data[6], out info.Direction)) return null;
            if (!ushort.TryParse(data[7], out info.RandomDelay)) return null;
            if (!int.TryParse(data[8], out info.RespawnIndex)) return null;
            if (!bool.TryParse(data[9], out info.SaveRespawnTime)) return null;
            if (!ushort.TryParse(data[10], out info.RespawnTicks)) return null;

            return info;
        }

		/// <summary>
		/// 保存到二进制
		/// </summary>
		/// <param name="writer">二进制写入器</param>
        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterIndex);

            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Count);
            writer.Write(Spread);

            writer.Write(Delay);
            writer.Write(Direction);

            writer.Write(RoutePath);

            writer.Write(RandomDelay);
            writer.Write(RespawnIndex);
            writer.Write(SaveRespawnTime);
            writer.Write(RespawnTicks);
        }

        /// <summary>
        /// 重写 ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var monsterName = Envir.MonsterInfoList.Find(o => o.Index == MonsterIndex)?.Name ?? "Unknown";
            return string.Format("Monster: {0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9} - {10}",
                MonsterIndex,
                monsterName,
                Functions.PointToString(Location),
                Count,
                Spread,
                Delay,
                Direction,
                RandomDelay,
                RespawnIndex,
                SaveRespawnTime,
                RespawnTicks);
        }
    }
    /// <summary>
    /// 怪物行走路径
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// 行走的位置
        /// </summary>
        public Point Location;
        /// <summary>
        /// 在原点停留多久
        /// </summary>
        public int Delay;
        /// <summary>
        /// 解析文件生成怪物行走的路径
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static RouteInfo FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            // 如果不是两个值，即：x y 则没有行走路径
            if (data.Length < 2) return null;
            // 实例化
            RouteInfo info = new RouteInfo();

            int x, y;
            // 把 data[0] 和 data[1] 分别复制给 x y 变量
            if (!int.TryParse(data[0], out x)) return null;
            if (!int.TryParse(data[1], out y)) return null;
            // 把要移动的坐标赋值给 Location，也就是标准化坐标格式
            info.Location = new Point(x, y);

            if (data.Length <= 2) return info;
            // 如果data有第三个值复制给 Delay
            return !int.TryParse(data[2], out info.Delay) ? info : info;
        }
    }
}
