using System.Drawing;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    /// <summary>
    /// 地图蓝图
    /// </summary>
    public class MapInfo
    {
        /// <summary>
		/// 主环境
		/// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
		/// 编辑环境
		/// </summary>
        protected static Envir EditEnvir
        {
            get { return Envir.Edit; }
        }
        /// <summary>
		/// 地图索引
		/// </summary>
        public int Index;

		/// <summary>
		/// 文件名字
		/// </summary>
		public string FileName = string.Empty;
		/// <summary>
		/// 地图标题
		/// <para>用于显示给玩家</para>
		/// </summary>
		public string Title = string.Empty;
        /// <summary>
		/// 小地图索引（对应客户端的小地图）
		/// </summary>
		public ushort MiniMap;
        /// <summary>
		/// 大地图索引（对应客户端的大地图）
		/// </summary>
		public ushort BigMap;
        /// <summary>
		/// 音乐索引（对应客户端的音乐）
		/// </summary>
		public ushort Music;

        /// <summary>
        /// 用于控制地图的光照设置，是一个枚举类型，包含Normal(0)、Dawn(1)、Day(2)、Evening(3)、Night(4)五种值
        /// <para>Normal: 普通光照</para>
        /// <para>Dawn: 黎明（清晨破晓）</para>
        /// <para>Dawn: 白天</para>
        /// <para>Day: 黄昏/傍晚</para>
        /// <para>Evening: 夜晚</para>
        /// </summary>
        public LightSetting Light;
		/// <summary>
		/// 用于进一步控制夜晚的光照颜色
		/// <para>值1-4分别对应不同的夜晚颜色：1为黑色(20,20,20)，2为浅蓝灰色，3为天蓝色，4为金黄色</para>
		/// </summary>
		public byte MapDarkLight = 0;
		/// <summary>
		/// 用于标识地图中的挖矿设置索引
		/// <para>如果为0，则表示地图没有挖矿设置</para>
		/// </summary>
		public byte MineIndex = 0;
        /// <summary>
        /// 用于标识地图中的GT设置索引
        /// </summary>
		public byte GTIndex = 0;
        /// <summary>
        /// 是否允许传送
        /// </summary>
        public bool NoTeleport;
        /// <summary>
        /// 是否允许重新进入
        /// </summary>
        public bool NoReconnect;
        /// <summary>
        /// 是否允许随机
        /// </summary>
        public bool NoRandom;
		/// <summary>
		/// 禁止玩家在该地图使用逃脱类技能/物品
		/// </summary>
		public bool NoEscape;

        /// <summary>
        /// 禁止使用召回类技能/物品，限制玩家将自己或他人传送到其他位置
        /// </summary>
        public bool NoRecall;
        /// <summary>
        /// 禁止使用药物
        /// </summary>
        public bool NoDrug;
        /// <summary>
        /// 禁止使用定位移动功能
        /// </summary>
        public bool NoPosition;
        /// <summary>
        /// 禁止在该地图进行战斗
        /// </summary>
        public bool NoFight;
        /// <summary>
        /// 禁止丢弃物品
        /// </summary>
        public bool NoThrowItem;

        /// <summary>
        /// 禁止玩家死亡时掉落物品
        /// </summary>
        public bool NoDropPlayer;
        /// <summary>
        /// 禁止怪物死亡时掉落物品
        /// </summary>
        public bool NoDropMonster;
        /// <summary>
        /// 在该地图中隐藏所有角色名称（显示为"?????"），增加战斗的不确定性
        /// </summary>
        public bool NoNames;
        /// <summary>
        /// 禁止在该地图骑乘
        /// </summary>
        public bool NoMount;

        /// <summary>
        /// 需要装备缰绳才能在该地图骑乘
        /// </summary>
        public bool NeedBridle;
        /// <summary>
        /// 是否是PVP地图
        /// </summary>
        public bool Fight;
        public bool NeedHole;
        /// <summary>
        /// 地图中有火焰效果，会对玩家造成持续伤害
        /// </summary>
        public bool Fire;
        /// <summary>
        /// 地图中有闪电效果，会对玩家造成伤害
        /// </summary>
        public bool Lightning;
        /// <summary>
        /// 禁止使用回城卷轴或回城技能
        /// </summary>
        public bool NoTownTeleport;
        /// <summary>
        /// 禁止在该地图进行转世/重生操作
        /// </summary>
        public bool NoReincarnation;
        /// <summary>
        /// 与公会地图(Guild Territory)相关，控制公会专属地图的权限和功能
        /// </summary>
        public bool GT;

        public bool NoExperience, NoGroup = false, NoPets, NoIntelligentCreatures, NoHero, RequiredGroup = false, FireWallLimit;
        public int RequiredGroupSize = 0, FireWallCount = 0;

        /// <summary>
        /// 字符串类型，与之前分析的NoReconnect属性配合使用，当玩家在NoReconnect为true的地图重连时，会被传送到此字符串指定名称的目标地图。
        /// </summary>
        public string NoReconnectMap = string.Empty;
        /// <summary>
        /// 定义地图中火焰(Fire)环境效果造成的伤害值范围。在Map.cs中，系统会随机生成0到这些值之间的伤害值应用到玩家
        /// </summary>
        public int FireDamage;
        /// <summary>
        /// 定义地图中闪电(Lightning)环境效果造成的伤害值范围。在Map.cs中，系统会随机生成0到这些值之间的伤害值应用到玩家
        /// </summary>
        public int LightningDamage;
        /// <summary>
        /// 定义地图中的安全区域。这些区域在玩家重生、绑定地点设置、使用安全传送等场景中起关键作用，也是公会战等活动的重要位置标记
        /// </summary>
        public List<SafeZoneInfo> SafeZones = new List<SafeZoneInfo>();
        /// <summary>
        /// 类型的列表，定义地图间的传送点和移动规则。包含源坐标、目标地图索引、目标坐标以及各种传送条件(如需要洞穴、需要移动等
        /// </summary>
        public List<MovementInfo> Movements = new List<MovementInfo>();
        /// <summary>
        /// RespawnInfo类型的列表，定义怪物在地图中的重生规则，包括怪物类型、重生位置、扩散范围、数量、延迟时间、方向和路径等参数。
        /// </summary>
        public List<RespawnInfo> Respawns = new List<RespawnInfo>();
        /// <summary>
        /// NPCInfo类型的列表，存储地图中所有NPC的信息，包括NPC的名称、外观、位置和交互脚本等。
        /// </summary>
        public List<NPCInfo> NPCs = new List<NPCInfo>();
        /// <summary>
        /// MineZone类型的列表，定义地图中的矿场区域，指定挖矿点的位置、大小和产出类型
        /// </summary>
        public List<MineZone> MineZones = new List<MineZone>();
        /// <summary>
        /// 动态事件坐标列表
        /// <para>支持Mir2脚本</para>
        /// </summary>
        public List<Point> ActiveCoords = new List<Point>();
        /// <summary>
        /// 天气效果
        /// <para>默认没有天气效果</para>
        /// </summary>
        public WeatherSetting WeatherParticles = WeatherSetting.None;

        public MapInfo()
        {

        }

        public MapInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            FileName = reader.ReadString();
            Title = reader.ReadString();
            MiniMap = reader.ReadUInt16();
            Light = (LightSetting)reader.ReadByte();

            BigMap = reader.ReadUInt16();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                SafeZones.Add(new SafeZoneInfo(reader) { Info = this });

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Respawns.Add(new RespawnInfo(reader, Envir.LoadVersion, Envir.LoadCustomVersion));

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Movements.Add(new MovementInfo(reader));

            NoTeleport = reader.ReadBoolean();
            NoReconnect = reader.ReadBoolean();
            NoReconnectMap = reader.ReadString();

            NoRandom = reader.ReadBoolean();
            NoEscape = reader.ReadBoolean();
            NoRecall = reader.ReadBoolean();
            NoDrug = reader.ReadBoolean();
            NoPosition = reader.ReadBoolean();
            NoThrowItem = reader.ReadBoolean();
            NoDropPlayer = reader.ReadBoolean();
            NoDropMonster = reader.ReadBoolean();
            NoNames = reader.ReadBoolean();
            Fight = reader.ReadBoolean();
            Fire = reader.ReadBoolean();
            FireDamage = reader.ReadInt32();
            Lightning = reader.ReadBoolean();
            LightningDamage = reader.ReadInt32();
            MapDarkLight = reader.ReadByte();
            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                MineZones.Add(new MineZone(reader));
            MineIndex = reader.ReadByte();
            NoMount = reader.ReadBoolean();
            NeedBridle = reader.ReadBoolean();
            NoFight = reader.ReadBoolean();
            Music = reader.ReadUInt16();

            if (Envir.LoadVersion < 78) return;
            NoTownTeleport = reader.ReadBoolean();
            if (Envir.LoadVersion < 79) return;
            NoReincarnation = reader.ReadBoolean();

            if (Envir.LoadVersion >= 110)
            {
                WeatherParticles = (WeatherSetting)reader.ReadUInt16();
            }

            if (Envir.LoadVersion >= 111)
            {
                GT = reader.ReadBoolean();
                GTIndex = reader.ReadByte();
            }
            if (Envir.LoadVersion >= 114)
            {
                NoExperience = reader.ReadBoolean();
                NoGroup = reader.ReadBoolean();
                NoPets = reader.ReadBoolean();
                NoIntelligentCreatures = reader.ReadBoolean();
                NoHero = reader.ReadBoolean();
                RequiredGroupSize = reader.ReadInt32();
                RequiredGroup = reader.ReadBoolean();
                FireWallLimit = reader.ReadBoolean();
                FireWallCount = reader.ReadInt32();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(FileName);
            writer.Write(Title);
            writer.Write(MiniMap);
            writer.Write((byte)Light);
            writer.Write(BigMap);
            writer.Write(SafeZones.Count);

            for (int i = 0; i < SafeZones.Count; i++)
                SafeZones[i].Save(writer);

            writer.Write(Respawns.Count);
            for (int i = 0; i < Respawns.Count; i++)
                Respawns[i].Save(writer);

            writer.Write(Movements.Count);
            for (int i = 0; i < Movements.Count; i++)
                Movements[i].Save(writer);

            writer.Write(NoTeleport);
            writer.Write(NoReconnect);
            writer.Write(NoReconnectMap);
            writer.Write(NoRandom);
            writer.Write(NoEscape);
            writer.Write(NoRecall);
            writer.Write(NoDrug);
            writer.Write(NoPosition);
            writer.Write(NoThrowItem);
            writer.Write(NoDropPlayer);
            writer.Write(NoDropMonster);
            writer.Write(NoNames);
            writer.Write(Fight);
            writer.Write(Fire);
            writer.Write(FireDamage);
            writer.Write(Lightning);
            writer.Write(LightningDamage);
            writer.Write(MapDarkLight);
            writer.Write(MineZones.Count);
            for (int i = 0; i < MineZones.Count; i++)
                MineZones[i].Save(writer);
            writer.Write(MineIndex);

            writer.Write(NoMount);
            writer.Write(NeedBridle);

            writer.Write(NoFight);

            writer.Write(Music);
            writer.Write(NoTownTeleport);
            writer.Write(NoReincarnation);

            writer.Write((UInt16)WeatherParticles);

            writer.Write(GT);
            writer.Write(GTIndex);

            writer.Write(NoExperience);
            writer.Write(NoGroup);
            writer.Write(NoPets);
            writer.Write(NoIntelligentCreatures);
            writer.Write(NoHero);
            writer.Write(RequiredGroupSize);
            writer.Write(RequiredGroup);
            writer.Write(FireWallLimit);
            writer.Write(FireWallCount);

        }

        /// <summary>
		/// 创建地图
		/// </summary>
        public void CreateMap()
        {
            // 遍历所有 NPC 蓝图
            for (int j = 0; j < Envir.NPCInfoList.Count; j++)
            {
                // 不在当前地图则不添加
                if (Envir.NPCInfoList[j].MapIndex != Index) continue;
                // 添加NPC
                NPCs.Add(Envir.NPCInfoList[j]);
            }
            // 创建 Map 实例
            Map map = new Map(this);
            // 如果 Load 过了就不添加了
            if (!map.Load()) return;
            /// 把地图添加到MapList
            Envir.MapList.Add(map);

            for (int i = 0; i < SafeZones.Count; i++)
                if (SafeZones[i].StartPoint) // 找到重生点
                    Envir.StartPoints.Add(SafeZones[i]); // 把重生点放到主环境的出生点列表里
        }

        public void CreateSafeZone()
        {
            SafeZones.Add(new SafeZoneInfo { Info = this });
        }

        public void CreateRespawnInfo()
        {
            Respawns.Add(new RespawnInfo { RespawnIndex = ++EditEnvir.RespawnIndex });
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Index, Title);
        }

        public void CreateNPCInfo()
        {
            NPCs.Add(new NPCInfo());
        }

        public void CreateMovementInfo()
        {
            Movements.Add(new MovementInfo());
        }

        public static void FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 8) return;

            MapInfo info = new MapInfo {FileName = data[0], Title = data[1]};


            if (!ushort.TryParse(data[2], out info.MiniMap)) return;

            if (!Enum.TryParse(data[3], out info.Light)) return;
            int sziCount, miCount, riCount, npcCount;

            if (!int.TryParse(data[4], out sziCount)) return;
            if (!int.TryParse(data[5], out miCount)) return;
            if (!int.TryParse(data[6], out riCount)) return;
            if (!int.TryParse(data[7], out npcCount)) return;


            int start = 8;

            for (int i = 0; i < sziCount; i++)
            {
                SafeZoneInfo temp = new SafeZoneInfo { Info = info };
                int x, y;

                if (!int.TryParse(data[start + (i * 4)], out x)) return;
                if (!int.TryParse(data[start + 1 + (i * 4)], out y)) return;
                if (!ushort.TryParse(data[start + 2 + (i * 4)], out temp.Size)) return;
                if (!bool.TryParse(data[start + 3 + (i * 4)], out temp.StartPoint)) return;

                temp.Location = new Point(x, y);
                info.SafeZones.Add(temp);
            }
            start += sziCount * 4;



            for (int i = 0; i < miCount; i++)
            {
                MovementInfo temp = new MovementInfo();
                int x, y;

                if (!int.TryParse(data[start + (i * 5)], out x)) return;
                if (!int.TryParse(data[start + 1 + (i * 5)], out y)) return;
                temp.Source = new Point(x, y);

                if (!int.TryParse(data[start + 2 + (i * 5)], out temp.MapIndex)) return;

                if (!int.TryParse(data[start + 3 + (i * 5)], out x)) return;
                if (!int.TryParse(data[start + 4 + (i * 5)], out y)) return;
                temp.Destination = new Point(x, y);

                info.Movements.Add(temp);
            }
            start += miCount * 5;


            for (int i = 0; i < riCount; i++)
            {
                RespawnInfo temp = new RespawnInfo();
                int x, y;

                if (!int.TryParse(data[start + (i * 7)], out temp.MonsterIndex)) return;
                if (!int.TryParse(data[start + 1 + (i * 7)], out x)) return;
                if (!int.TryParse(data[start + 2 + (i * 7)], out y)) return;

                temp.Location = new Point(x, y);

                if (!ushort.TryParse(data[start + 3 + (i * 7)], out temp.Count)) return;
                if (!ushort.TryParse(data[start + 4 + (i * 7)], out temp.Spread)) return;
                if (!ushort.TryParse(data[start + 5 + (i * 7)], out temp.Delay)) return;
                if (!byte.TryParse(data[start + 6 + (i * 7)], out temp.Direction)) return;
                if (!int.TryParse(data[start + 7 + (i * 7)], out temp.RespawnIndex)) return;
                if (!bool.TryParse(data[start + 8 + (i * 7)], out temp.SaveRespawnTime)) return;
                if (!ushort.TryParse(data[start + 9 + (i * 7)], out temp.RespawnTicks)) return;

                info.Respawns.Add(temp);
            }
            start += riCount * 7;


            for (int i = 0; i < npcCount; i++)
            {
                NPCInfo temp = new NPCInfo { FileName = data[start + (i * 6)], Name = data[start + 1 + (i * 6)] };
                int x, y;

                if (!int.TryParse(data[start + 2 + (i * 6)], out x)) return;
                if (!int.TryParse(data[start + 3 + (i * 6)], out y)) return;

                temp.Location = new Point(x, y);

                if (!ushort.TryParse(data[start + 4 + (i * 6)], out temp.Rate)) return;
                if (!ushort.TryParse(data[start + 5 + (i * 6)], out temp.Image)) return;

                info.NPCs.Add(temp);
            }



            info.Index = ++EditEnvir.MapIndex;
            EditEnvir.MapInfoList.Add(info);
        }
        public static string GetMapTitleByIndex(int index) // For Players Online tab
        {
            var mapInfo = Envir.MapInfoList.FirstOrDefault(m => m.Index == index);
            return mapInfo != null ? mapInfo.Title : $"UnknownMap({index})";
        }
    }
}
