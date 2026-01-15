public enum MouseCursor : byte
{
    None,
    Default,
    Attack,
    AttackRed,
    NPCTalk,
    TextPrompt,
    Trash,
    Upgrade
}
//[Flags]
/// <summary>
/// 天气设置枚举
/// </summary>
public enum WeatherSetting : ushort
{
    /// <summary>无天气效果</summary>
    None = 0,
    /// <summary>雾</summary>
    Fog = 1,
    /// <summary>红色灰烬</summary>
    RedEmber = 2,
    /// <summary>白色灰烬</summary>
    WhiteEmber = 4,
    /// <summary>黄色灰烬</summary>
    YellowEmber = 8,
    /// <summary>火焰粒子</summary>
    FireParticle = 16,
    /// <summary>雪</summary>
    Snow = 32,
    /// <summary>雨</summary>
    Rain = 64,
    /// <summary>落叶</summary>
    Leaves = 128,
    /// <summary>火焰落叶</summary>
    FireyLeaves = 256,
    /// <summary>紫色落叶</summary>
    PurpleLeaves = 512,
}
public enum PanelType : byte
{
    Buy,
    BuySub,
    Craft,

    Sell,
    Repair,
    SpecialRepair,
    Consign,
    Refine,
    CheckRefine,
    Disassemble,
    Downgrade,
    Reset,
    CollectRefine,
    ReplaceWedRing,
}

public enum MarketItemType : byte
{
    Consign,
    Auction,
    GameShop
}

public enum MarketPanelType : byte
{
    Market,
    Consign,
    Auction,
    GameShop
}

public enum MarketPriceFilter : byte
{
    Normal,
    High,
    Low
}

public enum BlendMode : sbyte
{
    NONE = -1,
    NORMAL = 0,
    LIGHT = 1,
    LIGHTINV = 2,
    INVNORMAL = 3,
    INVLIGHT = 4,
    INVLIGHTINV = 5,
    INVCOLOR = 6,
    INVBACKGROUND = 7
}

public enum DamageType : byte
{
    Hit = 0,
    Miss = 1,
    Critical = 2
}

public enum MonsterType : byte
{
    Normal = 0,
    Uncommon = 1,
    Rare = 2,
    Elite = 3
}

[Flags]
public enum GMOptions : byte
{
    None = 0,
    GameMaster = 0x0001,
    Observer = 0x0002,
    Superman = 0x0004
}

public enum AwakeType : byte
{
    /// <summary>
    /// 没有选择觉醒类型
    /// </summary>
    None = 0,
    /// <summary>
    /// 攻击
    /// </summary>
    DC,
    /// <summary>
    /// 魔法
    /// </summary>
    MC,
    /// <summary>
    /// 道术
    /// </summary>
    SC,
    /// <summary>
    /// 物理防御
    /// </summary>
    AC,
    /// <summary>
    /// 魔法防御
    /// </summary>
    MAC,
    /// <summary>
    /// 生命值、魔法值
    /// </summary>
    HPMP,
}

[Flags]
public enum LevelEffects : ushort
{
    None = 0,
    Mist = 1,
    RedDragon = 2,
    BlueDragon = 4,
    Rebirth1 = 8,
    Rebirth2 = 16,
    Rebirth3 = 32,
    NewBlue = 64,
    YellowDragon = 128,
    Phoenix = 256
}

public enum OutputMessageType : byte
{
    Normal,
    Quest,
    Guild
}
/// <summary>
/// 物品品质
/// </summary>
public enum ItemGrade : byte
{
    /// <summary>
    /// 无品质
    /// </summary>
    None = 0,
    /// <summary>
    /// 普通品质
    /// </summary>
    Common = 1,
    /// <summary>
    /// 稀有品质
    /// </summary>
    Rare = 2,
    /// <summary>
    /// 传说品质
    /// </summary>
    Legendary = 3,
    /// <summary>
    /// 神话品质
    /// </summary>
    Mythical = 4,
    /// <summary>
    /// 史诗品质（最高级品质）
    /// </summary>
    Heroic = 5,
}



public enum RefinedValue : byte
{
    None = 0,
    DC = 1,
    MC = 2,
    SC = 3,
}

public enum QuestType : byte
{
    General = 0,
    Daily = 1,
    Repeatable = 2,
    Story = 3
}

public enum QuestIcon : byte
{
    None = 0,
    QuestionWhite = 1,
    ExclamationYellow = 2,
    QuestionYellow = 3,
    ExclamationBlue = 5,
    QuestionBlue = 6,
    ExclamationGreen = 52,
    QuestionGreen = 53
}

public enum QuestState : byte
{
    Add,
    Update,
    Remove
}

public enum QuestAction : byte
{
    TimeExpired
}

public enum DefaultNPCType : byte
{
    Login,
    LevelUp,
    UseItem,
    MapCoord,
    MapEnter,
    Die,
    Trigger,
    CustomCommand,
    OnAcceptQuest,
    OnFinishQuest,
    Daily,
    Client
}

/// <summary>
/// 智能宠物
/// </summary>
public enum IntelligentCreatureType : byte
{
    None = 99,
    BabyPig = 0,
    Chick = 1,
    Kitten = 2,
    BabySkeleton = 3,
    Baekdon = 4,
    Wimaen = 5,
    BlackKitten = 6,
    BabyDragon = 7,
    OlympicFlame = 8,
    BabySnowMan = 9,
    Frog = 10,
    BabyMonkey = 11,
    AngryBird = 12,
    Foxey = 13,
    MedicalRat = 14,
}

//2 blank mob files
public enum Monster : ushort
{
    Guard = 0,
    TaoistGuard = 1,
    Guard2 = 2,
    Hen = 3,
    Deer = 4,
    Scarecrow = 5,
    HookingCat = 6,
    RakingCat = 7,
    Yob = 8,
    Oma = 9,
    CannibalPlant = 10,
    ForestYeti = 11,
    SpittingSpider = 12,
    ChestnutTree = 13,
    EbonyTree = 14,
    LargeMushroom = 15,
    CherryTree = 16,
    OmaFighter = 17,
    OmaWarrior = 18,
    CaveBat = 19,
    CaveMaggot = 20,
    Scorpion = 21,
    Skeleton = 22,
    BoneFighter = 23,
    AxeSkeleton = 24,
    BoneWarrior = 25,
    BoneElite = 26,
    Dung = 27,
    Dark = 28,
    WoomaSoldier = 29,
    WoomaFighter = 30,
    WoomaWarrior = 31,
    FlamingWooma = 32,
    WoomaGuardian = 33,
    WoomaTaurus = 34, //BOSS
    WhimperingBee = 35,
    GiantWorm = 36,
    Centipede = 37,
    BlackMaggot = 38,
    Tongs = 39,
    EvilTongs = 40,
    EvilCentipede = 41,
    BugBat = 42,
    BugBatMaggot = 43,
    WedgeMoth = 44,
    RedBoar = 45,
    BlackBoar = 46,
    SnakeScorpion = 47,
    WhiteBoar = 48,
    EvilSnake = 49,
    BombSpider = 50,
    RootSpider = 51,
    SpiderBat = 52,
    VenomSpider = 53,
    GangSpider = 54,
    GreatSpider = 55,
    LureSpider = 56,
    BigApe = 57,
    EvilApe = 58,
    GrayEvilApe = 59,
    RedEvilApe = 60,
    CrystalSpider = 61,
    RedMoonEvil = 62,
    BigRat = 63,
    ZumaArcher = 64,
    ZumaStatue = 65,
    ZumaGuardian = 66,
    RedThunderZuma = 67,
    ZumaTaurus = 68, //BOSS
    DigOutZombie = 69,
    ClZombie = 70,
    NdZombie = 71,
    CrawlerZombie = 72,
    ShamanZombie = 73,
    Ghoul = 74,
    KingScorpion = 75,
    KingHog = 76,
    DarkDevil = 77,
    BoneFamiliar = 78,
    Shinsu = 79,
    Shinsu1 = 80,
    SpiderFrog = 81,
    HoroBlaster = 82,
    BlueHoroBlaster = 83,
    KekTal = 84,
    VioletKekTal = 85,
    Khazard = 86,
    RoninGhoul = 87,
    ToxicGhoul = 88,
    BoneCaptain = 89,
    BoneSpearman = 90,
    BoneBlademan = 91,
    BoneArcher = 92,
    BoneLord = 93, //BOSS
    Minotaur = 94,
    IceMinotaur = 95,
    ElectricMinotaur = 96,
    WindMinotaur = 97,
    FireMinotaur = 98,
    RightGuard = 99,
    LeftGuard = 100,
    MinotaurKing = 101, //BOSS
    FrostTiger = 102,
    Sheep = 103,
    Wolf = 104,
    ShellNipper = 105,
    Keratoid = 106,
    GiantKeratoid = 107,
    SkyStinger = 108,
    SandWorm = 109,
    VisceralWorm = 110,
    RedSnake = 111,
    TigerSnake = 112,
    Yimoogi = 113,
    GiantWhiteSnake = 114,
    BlueSnake = 115,
    YellowSnake = 116,
    HolyDeva = 117,
    AxeOma = 118,
    SwordOma = 119,
    CrossbowOma = 120,
    WingedOma = 121,
    FlailOma = 122,
    OmaGuard = 123,
    YinDevilNode = 124,
    YangDevilNode = 125,
    OmaKing = 126, //BOSS
    BlackFoxman = 127,
    RedFoxman = 128,
    WhiteFoxman = 129,
    TrapRock = 130,
    GuardianRock = 131,
    ThunderElement = 132,
    CloudElement = 133,
    GreatFoxSpirit = 134, //BOSS
    HedgeKekTal = 135,
    BigHedgeKekTal = 136,
    RedFrogSpider = 137,
    BrownFrogSpider = 138,
    ArcherGuard = 139,
    KatanaGuard = 140,
    ArcherGuard2 = 141,
    Pig = 142,
    Bull = 143,
    Bush = 144,
    ChristmasTree = 145,
    HighAssassin = 146,
    DarkDustPile = 147,
    DarkBrownWolf = 148,
    Football = 149,
    GingerBreadman = 150,
    HalloweenScythe = 151,
    GhastlyLeecher = 152,
    CyanoGhast = 153,
    MutatedManworm = 154,
    CrazyManworm = 155,
    MudPile = 156,
    TailedLion = 157,
    Behemoth = 158, //BOSS
    DarkDevourer = 159,
    PoisonHugger = 160,
    Hugger = 161,
    MutatedHugger = 162,
    DreamDevourer = 163,
    Treasurebox = 164,
    SnowPile = 165,
    Snowman = 166,
    SnowTree = 167,
    GiantEgg = 168,
    RedTurtle = 169,
    GreenTurtle = 170,
    BlueTurtle = 171,
    Catapult1 = 172, //SPECIAL TODO
    Catapult2 = 173, //SPECIAL TODO
    OldSpittingSpider = 174,
    SiegeRepairman = 175, //SPECIAL TODO
    BlueSanta = 176,
    BattleStandard = 177,
    WingedBullLord = 178,
    RedYimoogi = 179,
    LionRiderMale = 180, //Not Monster - Skin / Transform
    LionRiderFemale = 181, //Not Monster - Skin / Transform
    Tornado = 182,
    FlameTiger = 183,
    WingedTigerLord = 184, //BOSS
    TowerTurtle = 185,
    FinialTurtle = 186,
    TurtleKing = 187, //BOSS
    DarkTurtle = 188,
    LightTurtle = 189,
    DarkSwordOma = 190,
    DarkAxeOma = 191,
    DarkCrossbowOma = 192,
    DarkWingedOma = 193,
    BoneWhoo = 194,
    DarkSpider = 195, //AI 8
    ViscusWorm = 196,
    ViscusCrawler = 197,
    CrawlerLave = 198,
    DarkYob = 199,
    FlamingMutant = 200,
    StoningStatue = 201, //BOSS
    FlyingStatue = 202,
    ValeBat = 203,
    Weaver = 204,
    VenomWeaver = 205,
    CrackingWeaver = 206,
    ArmingWeaver = 207,
    CrystalWeaver = 208,
    FrozenZumaStatue = 209,
    FrozenZumaGuardian = 210,
    FrozenRedZuma = 211,
    GreaterWeaver = 212,
    SpiderWarrior = 213,
    SpiderBarbarian = 214,
    HellSlasher = 215,
    HellPirate = 216,
    HellCannibal = 217,
    HellKeeper = 218, //BOSS
    HellBolt = 219,
    WitchDoctor = 220,
    ManectricHammer = 221,
    ManectricClub = 222,
    ManectricClaw = 223,
    ManectricStaff = 224,
    NamelessGhost = 225,
    DarkGhost = 226,
    ChaosGhost = 227,
    ManectricBlest = 228,
    ManectricKing = 229,
    Blank2 = 230,
    IcePillar = 231,
    FrostYeti = 232,
    ManectricSlave = 233,
    TrollHammer = 234,
    TrollBomber = 235,
    TrollStoner = 236,
    TrollKing = 237, //BOSS
    FlameSpear = 238,
    FlameMage = 239,
    FlameScythe = 240,
    FlameAssassin = 241,
    FlameQueen = 242, //BOSS
    HellKnight1 = 243,
    HellKnight2 = 244,
    HellKnight3 = 245,
    HellKnight4 = 246,
    HellLord = 247, //BOSS
    WaterGuard = 248,
    IceGuard = 249,
    ElementGuard = 250,
    DemonGuard = 251,
    KingGuard = 252,
    Snake10 = 253,
    Snake11 = 254,
    Snake12 = 255,
    Snake13 = 256,
    Snake14 = 257,
    Snake15 = 258,
    Snake16 = 259,
    Snake17 = 260,
    DeathCrawler = 261,
    BurningZombie = 262,
    MudZombie = 263,
    FrozenZombie = 264,
    UndeadWolf = 265,
    DemonWolf = 266,
    WhiteMammoth = 267,
    DarkBeast = 268,
    LightBeast = 269,//AI 112
    BloodBaboon = 270, //AI 112
    HardenRhino = 271,
    AncientBringer = 272,
    FightingCat = 273,
    FireCat = 274, //AI 44
    CatWidow = 275, //AI 112
    StainHammerCat = 276,
    BlackHammerCat = 277,
    StrayCat = 278,
    CatShaman = 279,
    Jar1 = 280,
    Jar2 = 281,
    SeedingsGeneral = 282,
    RestlessJar = 283,
    GeneralMeowMeow = 284, //BOSS
    Bunny = 285,
    Tucson = 286,
    TucsonFighter = 287, //AI 44
    TucsonMage = 288,
    TucsonWarrior = 289,
    Armadillo = 290,
    ArmadilloElder = 291,
    TucsonEgg = 292, //EFFECT 0/1
    PlaguedTucson = 293,
    SandSnail = 294,
    CannibalTentacles = 295,
    TucsonGeneral = 296, //BOSS
    GasToad = 297,
    Mantis = 298,
    SwampWarrior = 299,

    AssassinBird = 300,
    RhinoWarrior = 301,
    RhinoPriest = 302,
    ElephantMan = 303,
    StoneGolem = 304,
    EarthGolem = 305,
    TreeGuardian = 306,
    TreeQueen = 307,
    PeacockSpider = 308,
    DarkBaboon = 309, //AI 112
    TwinHeadBeast = 310, //AI 112
    OmaCannibal = 311,
    OmaBlest = 312,
    OmaSlasher = 313,
    OmaAssassin = 314,
    OmaMage = 315,
    OmaWitchDoctor = 316,
    LightningBead = 317, //Effect 0, AI 149
    HealingBead = 318, //Effect 1, AI 149
    PowerUpBead = 319, //Effect 2, AI 14
    DarkOmaKing = 320, //BOSS
    CaveStatue = 321,
    Mandrill = 322,
    PlagueCrab = 323,
    CreeperPlant = 324,
    FloatingWraith = 325, //AI 8
    ArmedPlant = 326,
    AvengerPlant = 327,
    Nadz = 328,
    AvengingSpirit = 329,
    AvengingWarrior = 330,
    AxePlant = 331,
    WoodBox = 332,
    ClawBeast = 333, //AI 8
    DarkCaptain = 334, //BOSS
    SackWarrior = 335,
    WereTiger = 336, //AI 112
    KingHydrax = 337,
    Hydrax = 338,
    HornedMage = 339,
    BlueSoul = 340,
    HornedArcher = 341,
    ColdArcher = 342,
    HornedWarrior = 343,
    FloatingRock = 344,
    ScalyBeast = 345,
    HornedSorceror = 346,
    BoulderSpirit = 347,
    HornedCommander = 348, //BOSS

    MoonStone = 349,
    SunStone = 350,
    LightningStone = 351,
    Turtlegrass = 352,
    ManTree = 353,
    Bear = 354,  //Effect 1, AI 112
    Leopard = 355,
    ChieftainArcher = 356,
    ChieftainSword = 357, //BOSS TODO
    StoningSpider = 358, //Archer Spell mob (not yet coded)
    VampireSpider = 359, //Archer Spell mob
    SpittingToad = 360, //Archer Spell mob
    SnakeTotem = 361, //Archer Spell mob
    CharmedSnake = 362, //Archer Spell mob
    FrozenSoldier = 363,
    FrozenFighter = 364, //AI 44
    FrozenArcher = 365, //AI 8
    FrozenKnight = 366,
    FrozenGolem = 367,
    IcePhantom = 368, //TODO - AI needs revisiting (blue explosion and snakes)
    SnowWolf = 369,
    SnowWolfKing = 370, //BOSS
    WaterDragon = 371,
    BlackTortoise = 372,
    Manticore = 373, //TODO
    DragonWarrior = 374, //Done (DG)
    DragonArcher = 375, //TODO - Wind Arrow spell
    Kirin = 376, // Done (jxtulong)
    Guard3 = 377,
    ArcherGuard3 = 378,
    Bunny2 = 379,
    FrozenMiner = 380, // Done (jxtulong)
    FrozenAxeman = 381, // Done (jxtulong)
    FrozenMagician = 382, // Done (jxtulong)
    SnowYeti = 383, // Done (jxtulong)
    IceCrystalSoldier = 384, // Done (jxtulong)
    DarkWraith = 385, // Done (jxtulong)
    DarkSpirit = 386, // Use AI 8 (AxeSkeleton)
    CrystalBeast = 387,
    RedOrb = 388,
    BlueOrb = 389,
    YellowOrb = 390,
    GreenOrb = 391,
    WhiteOrb = 392,
    FatalLotus = 393,
    AntCommander = 394,
    CargoBoxwithlogo = 395, // Done - Use CargoBox AI.
    Doe = 396, // TELEPORT = EFFECT 9
    Reindeer = 397, //frames not added
    AngryReindeer = 398,
    CargoBox = 399, // Done - Basically a Pinata.

    Ram1 = 400,
    Ram2 = 401,
    Kite = 402,
    PurpleFaeFlower = 403,
    Furball = 404,
    GlacierSnail = 405,
    FurbolgWarrior = 406,
    FurbolgArcher = 407,
    FurbolgCommander = 408,
    RedFaeFlower = 409,
    FurbolgGuard = 410,
    GlacierBeast = 411,
    GlacierWarrior = 412,
    ShardGuardian = 413,
    WarriorScroll = 414, // Use AI "HoodedSummonerScrolls" - Info.Effect = 0
    TaoistScroll = 415, // Use AI "HoodedSummonerScrolls" - Info.Effect = 1
    WizardScroll = 416, // Use AI "HoodedSummonerScrolls" - Info.Effect = 2
    AssassinScroll = 417, // Use AI "HoodedSummonerScrolls" - Info.Effect = 3
    HoodedSummoner = 418, //Summons Scrolls
    HoodedIceMage = 419,
    HoodedPriest = 420,
    ShardMaiden = 421,
    KingKong = 422,
    WarBear = 423,
    ReaperPriest = 424,
    ReaperWizard = 425,
    ReaperAssassin = 426,
    LivingVines = 427,
    BlueMonk = 428,
    MutantBeserker = 429,
    MutantGuardian = 430,
    MutantHighPriest = 431,
    MysteriousMage = 432,
    FeatheredWolf = 433,
    MysteriousAssassin = 434,
    MysteriousMonk = 435,
    ManEatingPlant = 436,
    HammerDwarf = 437,
    ArcherDwarf = 438,
    NobleWarrior = 439,
    NobleArcher = 440,
    NoblePriest = 441,
    NobleAssassin = 442,
    Swain = 443,
    RedMutantPlant = 444,
    BlueMutantPlant = 445,
    UndeadHammerDwarf = 446,
    UndeadDwarfArcher = 447,
    AncientStoneGolem = 448,
    Serpentirian = 449,

    Butcher = 450,
    Riklebites = 451,
    FeralTundraFurbolg = 452,
    FeralFlameFurbolg = 453,
    ArcaneTotem = 454,
    SpectralWraith = 455,
    BabyMagmaDragon = 456,
    BloodLord = 457,
    SerpentLord = 458,
    MirEmperor = 459,
    MutantManEatingPlant = 460,
    MutantWarg = 461,
    GrassElemental = 462,
    RockElemental = 463,

    //Special
    EvilMir = 900,
    EvilMirBody = 901,
    DragonStatue = 902,
    HellBomb1 = 903,
    HellBomb2 = 904,
    HellBomb3 = 905,

    //Siege
    Catapult = 940,
    ChariotBallista = 941,
    Ballista = 942,
    Trebuchet = 943,
    CanonTrebuchet = 944,

    //Gates
    SabukGate = 950,
    PalaceWallLeft = 951,
    PalaceWall1 = 952,
    PalaceWall2 = 953,
    GiGateSouth = 954,
    GiGateEast = 955,
    GiGateWest = 956,
    SSabukWall1 = 957,
    SSabukWall2 = 958,
    SSabukWall3 = 959,
    NammandGate1 = 960, //Not Coded
    NammandGate2 = 961, //Not Coded
    SabukWallSection = 962, //Not Coded
    NammandWallSection = 963, //Not Coded
    FrozenDoor = 964, //Not Coded

    //Flags 1000 ~ 1100

    //Creatures
    BabyPig = 10000,//Permanent
    Chick = 10001,//Special
    Kitten = 10002,//Permanent
    BabySkeleton = 10003,//Special
    Baekdon = 10004,//Special
    Wimaen = 10005,//Event
    BlackKitten = 10006,//unknown
    BabyDragon = 10007,//unknown
    OlympicFlame = 10008,//unknown
    BabySnowMan = 10009,//unknown
    Frog = 10010,//unknown
    BabyMonkey = 10011,//unknown
    AngryBird = 10012,
    Foxey = 10013,
    MedicalRat = 10014,
}

public enum MirAction : byte
{
    Standing,
    Walking,
    Running,
    Pushed,
    DashL,
    DashR,
    DashFail,
    Stance,
    Stance2,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    AttackRange1,
    AttackRange2,
    AttackRange3,
    Special,
    Struck,
    Harvest,
    Spell,
    Die,
    Dead,
    Skeleton,
    Show,
    Hide,
    Stoned,
    Appear,
    Revive,
    SitDown,
    Mine,
    Sneek,
    DashAttack,
    Lunge,

    WalkingBow,
    RunningBow,
    Jump,

    MountStanding,
    MountWalking,
    MountRunning,
    MountStruck,
    MountAttack,

    FishingCast,
    FishingWait,
    FishingReel
}

/// <summary>
/// 单元格类型
/// <para>Walk: 可行走</para>
/// <para>HighWall: 高墙</para>
/// <para>LowWall: 低墙</para>
/// </summary>
public enum CellAttribute : byte
{
    /// <summary>
    /// 可行走
    /// </summary>
    Walk = 0,
    /// <summary>
    /// 高墙
    /// </summary>
    HighWall = 1,
    /// <summary>
    /// 低墙
    /// </summary>
    LowWall = 2,
}
/// <summary>
/// 光照设置
/// <para>Normal: 普通光照</para>
/// <para>Dawn: 黎明（清晨破晓）</para>
/// <para>Dawn: 白天</para>
/// <para>Day: 黄昏/傍晚</para>
/// <para>Evening: 夜晚</para>
/// </summary>
public enum LightSetting : byte
{
    /// <summary>
    /// 普通光照
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 黎明（清晨破晓）
    /// </summary>
    Dawn = 1,
    /// <summary>
    /// 白天
    /// </summary>
    Day = 2,
    /// <summary>
    /// 黄昏/傍晚
    /// </summary>
    Evening = 3,
    /// <summary>
    /// 夜晚
    /// </summary>
    Night = 4
}

public enum MirGender : byte
{
    Male = 0,
    Female = 1
}

public enum MirClass : byte
{
    /// <summary>
    /// 战士
    /// </summary>
    Warrior = 0,
    /// <summary>
    /// 法师
    /// </summary>
    Wizard = 1,
    /// <summary>
    /// 道士
    /// </summary>
    Taoist = 2,
    /// <summary>
    /// 刺客
    /// </summary>
    Assassin = 3,
    /// <summary>
    /// 弓箭手
    /// </summary>
    Archer = 4
}
/// <summary>
/// 方向
/// </summary>
public enum MirDirection : byte
{
    /// <summary>
    /// 上
    /// </summary>
    Up = 0,
    /// <summary>
    /// 右上
    /// </summary>
    UpRight = 1,
    /// <summary>
    /// 右
    /// </summary>
    Right = 2,
    /// <summary>
    /// 右下
    /// </summary>
    DownRight = 3,
    /// <summary>
    /// 下
    /// </summary>
    Down = 4,
    /// <summary>
    /// 左下
    /// </summary>
    DownLeft = 5,
    /// <summary>
    /// 左
    /// </summary>
    Left = 6,
    /// <summary>
    /// 左上
    /// </summary>
    UpLeft = 7
}

/// <summary>
/// 地图对象
/// </summary>
public enum ObjectType : byte
{
    /// <summary>
    /// 无
    /// </summary>
    None = 0,
    /// <summary>
    /// 玩家
    /// </summary>
    Player = 1,
    /// <summary>
    /// 物品
    /// </summary>
    Item = 2,
    /// <summary>
    /// 商人/NPC 商人
    /// </summary>
    Merchant = 3,
    /// <summary>
    /// 法术对象
    /// </summary>
    Spell = 4,
    /// <summary>
    /// 怪物
    /// </summary>
    Monster = 5,
    /// <summary>
    /// 装饰/场景物件
    /// </summary>
    Deco = 6,
    /// <summary>
    /// 生物（宠物/坐骑类）
    /// </summary>
    Creature = 7,
    /// <summary>
    /// 英雄（玩家召唤/独立英雄）
    /// </summary>
    Hero = 8
}
/// <summary>
/// 聊天类型
/// </summary>
public enum ChatType : byte
{
    /// <summary>
    /// 普通聊天(近聊)
    /// <para>最基础的聊天类型，通常用于玩家之间的本地聊天或附近范围内的交流</para>
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 喊话（有CD）
    /// <para>用于向较大范围的玩家发送消息，可能需要消耗特殊道具或有冷却时间</para>
    /// </summary>
    Shout = 1,
    /// <summary>
    /// 系统消息
    /// <para>游戏系统发送的重要通知，如服务器维护、重大事件等</para>
    /// </summary>
    System = 2,
    /// <summary>
    /// 提示信息
    /// <para>游戏功能提示，如操作指南、任务提示等辅助信息</para>
    /// </summary>
    Hint = 3,
    /// <summary>
    /// 公告
    /// <para>官方或管理员发布的全服公告，通常具有较高的显示优先级</para>
    /// </summary>
    Announcement = 4,
    /// <summary>
    /// 组队聊天
    /// <para>仅限当前队伍成员可见的内部交流</para>
    /// </summary>
    Group = 5,
    /// <summary>
    /// 收到的私聊
    /// <para>表示玩家接收到的私人消息</para>
    /// </summary>
    WhisperIn = 6,
    /// <summary>
    /// 发出的私聊
    /// <para>表示玩家发送出去的私人消息</para>
    /// </summary>
    WhisperOut = 7,
    /// <summary>
    /// 行会聊天
    /// <para>仅限当前行会成员可见的内部交流</para>
    /// </summary>
    Guild = 8,
    /// <summary>
    /// 师徒聊天
    /// <para>师徒关系之间的专属聊天频道</para>
    /// </summary>
    Trainer = 9,
    /// <summary>
    /// 升级消息
    /// <para>玩家升级时系统自动发送的通知</para>
    /// </summary>
    LevelUp = 10,
    /// <summary>
    /// 系统消息2
    /// <para>可能用于区分不同级别的系统消息，如次要系统通知</para>
    /// </summary>
    System2 = 11,
    /// <summary>
    /// 关系消息
    /// <para>处理好友、婚姻等特殊关系相关的消息</para>
    /// </summary>
    Relationship = 12,
    /// <summary>
    /// 师徒消息
    /// <para>与Trainer类似，可能用于师徒系统的特定消息</para>
    /// </summary>
    Mentor = 13,
    /// <summary>
    /// 呐喊/喊话2
    /// <para>可能是具有不同显示效果或范围的喊话类型</para>
    /// </summary>
    Shout2 = 14,
    /// <summary>
    /// 呐喊/喊话3
    /// <para>可能是具有不同显示效果或范围的喊话类型</para>
    /// </summary>
    Shout3 = 15,
    /// <summary>
    /// 行消息
    /// <para>可能是指在屏幕特定位置（如中间区域）显示的滚动消息</para>
    /// </summary>
    LineMessage = 16,
}

/// <summary>
/// 表示游戏中物品的类型，用于对物品进行分类管理
/// </summary>
public enum ItemType : byte
{
    /// <summary>无物品类型</summary>
    Nothing = 0,
    /// <summary>武器物品，用于攻击</summary>
    Weapon = 1,
    /// <summary>盔甲物品，用于防御</summary>
    Armour = 2,
    /// <summary>头盔物品，用于头部防护</summary>
    Helmet = 4,
    /// <summary>项链物品</summary>
    Necklace = 5,
    /// <summary>手镯物品</summary>
    Bracelet = 6,
    /// <summary>戒指物品</summary>
    Ring = 7,
    /// <summary>护身符物品</summary>
    Amulet = 8,
    /// <summary>腰带物品</summary>
    Belt = 9,
    /// <summary>靴子物品，用于脚部防护</summary>
    Boots = 10,
    /// <summary>石头物品，可能用于升级或合成</summary>
    Stone = 11,
    /// <summary>火把物品，用于照明</summary>
    Torch = 12,
    /// <summary>药水物品，用于恢复HP/MP或提供 buff</summary>
    Potion = 13,
    /// <summary>矿石物品，用于合成的原材料</summary>
    Ore = 14,
    /// <summary>肉类物品，可能是食物或合成材料</summary>
    Meat = 15,
    /// <summary>合成材料物品</summary>
    CraftingMaterial = 16,
    /// <summary>卷轴物品，可能用于法术或传送</summary>
    Scroll = 17,
    /// <summary>宝石物品，用于升级或附魔</summary>
    Gem = 18,
    /// <summary>坐骑物品，用于运输</summary>
    Mount = 19,
    /// <summary>书籍物品，可能用于学习技能</summary>
    Book = 20,
    /// <summary>脚本物品，可能用于调用自定义脚本</summary>
    Script = 21,
    /// <summary>缰绳物品，与坐骑相关</summary>
    Reins = 22,
    /// <summary>铃铛物品，可能与宠物或坐骑相关</summary>
    Bells = 23,
    /// <summary>马鞍物品，与坐骑相关</summary>
    Saddle = 24,
    /// <summary>丝带物品，可能是装饰性的</summary>
    Ribbon = 25,
    /// <summary>面具物品，可能用于伪装或提供 buff</summary>
    Mask = 26,
    /// <summary>食物物品，用于恢复饥饿或提供 buff</summary>
    Food = 27,
    /// <summary>鱼钩物品，用于钓鱼</summary>
    Hook = 28,
    /// <summary>浮漂物品，用于钓鱼</summary>
    Float = 29,
    /// <summary>鱼饵物品，用于钓鱼</summary>
    Bait = 30,
    /// <summary>探测器物品，可能用于定位资源</summary>
    Finder = 31,
    /// <summary>鱼线轮物品，用于钓鱼</summary>
    Reel = 32,
    /// <summary>鱼类物品，通过钓鱼获得</summary>
    Fish = 33,
    /// <summary>任务物品，用于完成任务</summary>
    Quest = 34,
    /// <summary>觉醒物品，用于升级或觉醒其他物品</summary>
    Awakening = 35,
    /// <summary>宠物物品，与宠物相关</summary>
    Pets = 36,
    /// <summary>变形物品，用于改变外观</summary>
    Transform = 37,
    /// <summary>装饰性物品</summary>
    Deco = 38,
    /// <summary>插槽物品，用于为装备添加插槽</summary>
    Socket = 39,
    /// <summary>怪物召唤物品，用于召唤怪物</summary>
    MonsterSpawn = 40,
    /// <summary>攻城弹药物品</summary>
    SiegeAmmo = 41, //TODO
    /// <summary>封印英雄物品，可能包含英雄角色或相关内容</summary>
    SealedHero = 42
}

public enum MirGridType : byte
{
    None = 0,
    Inventory = 1,
    Equipment = 2,
    Trade = 3,
    Storage = 4,
    BuyBack = 5,
    DropPanel = 6,
    Inspect = 7,
    TrustMerchant = 8,
    GuildStorage = 9,
    GuestTrade = 10,
    Mount = 11,
    Fishing = 12,
    QuestInventory = 13,
    AwakenItem = 14,
    Mail = 15,
    Refine = 16,
    Renting = 17,
    GuestRenting = 18,
    Craft = 19,
    Socket = 20,
    HeroEquipment = 21,
    HeroInventory = 22,
    HeroHPItem = 23,
    HeroMPItem = 24
}

public enum EquipmentSlot : byte
{
    Weapon = 0,
    Armour = 1,
    Helmet = 2,
    Torch = 3,
    Necklace = 4,
    BraceletL = 5,
    BraceletR = 6,
    RingL = 7,
    RingR = 8,
    Amulet = 9,
    Belt = 10,
    Boots = 11,
    Stone = 12,
    Mount = 13
}

public enum MountSlot : byte
{
    Reins = 0,
    Bells = 1,
    Saddle = 2,
    Ribbon = 3,
    Mask = 4
}

public enum FishingSlot : byte
{
    Hook = 0,
    Float = 1,
    Bait = 2,
    Finder = 3,
    Reel = 4
}

/// <summary>
/// 攻击模式
/// </summary>
public enum AttackMode : byte
{
    /// <summary>
    /// 和平
    /// </summary>
    Peace = 0,
    /// <summary>
    /// 编组
    /// </summary>
    Group = 1,
    /// <summary>
    /// 公会: 允许攻击公会成员之外的玩家
    /// </summary>
    Guild = 2,
    /// <summary>
    /// 敌对公会模式：只能攻击敌对公会成员
    /// </summary>
    EnemyGuild = 3,
    /// <summary>
    /// 红名
    /// </summary>
    RedBrown = 4,
    /// <summary>
    /// 全体
    /// </summary>
    All = 5
}

/// <summary>
/// 宠物模式
/// </summary>
public enum PetMode : byte
{
    /// <summary>
    /// 全模式：宠物会跟随主人移动并主动攻击目标
    /// </summary>
    Both = 0,
    /// <summary>
    /// 只移动模式：宠物只跟随主人移动，不主动攻击
    /// </summary>
    MoveOnly = 1,
    /// <summary>
    /// 只攻击模式：宠物独立攻击，但不跟随主人移动
    /// </summary>
    AttackOnly = 2,

    /// <summary>
    /// 关闭模式：宠物不移动、不攻击，处于静止状态
    /// </summary>
    None = 3,

    /// <summary>
    /// 集中主人目标：宠物只攻击主人的目标，跟随主人移动
    /// </summary>
    FocusMasterTarget = 4
}

/// <summary>
/// 中毒类型
/// </summary>
[Flags]
public enum PoisonType : ushort
{
    /// <summary>
    /// 无中毒状态
    /// </summary>
    None = 0,
    /// <summary>
    /// 绿毒
    /// </summary>
    Green = 1,
    /// <summary>
    /// 红毒
    /// </summary>
    Red = 2,
    /// <summary>
    /// 减速
    /// </summary>
    Slow = 4,
    /// <summary>
    /// 冰冻
    /// </summary>
    Frozen = 8,
    /// <summary>
    /// 眩晕
    /// </summary>
    Stun = 16,
    /// <summary>
    /// 麻痹
    /// </summary>
    Paralysis = 32,
    /// <summary>
    /// 延迟爆炸
    /// </summary>
    DelayedExplosion = 64,
    /// <summary>
    /// 流血
    /// </summary>
    Bleeding = 128,
    /// <summary>
    /// 左右麻痹（仅针对怪物，受击后马上解除）
    /// </summary>
    LRParalysis = 256,
    /// <summary>
    /// 失明
    /// </summary>
    Blindness = 512,
    /// <summary>
    /// daze效果
    /// <para>类似眩晕，但没有伤害</para>
    /// </summary>
    Dazed = 1024
}

[Flags]

public enum BindMode : short
{
    None = 0,
    DontDeathdrop = 1,//0x0001
    DontDrop = 2,//0x0002
    DontSell = 4,//0x0004
    DontStore = 8,//0x0008
    DontTrade = 16,//0x0010
    DontRepair = 32,//0x0020
    DontUpgrade = 64,//0x0040
    DestroyOnDrop = 128,//0x0080
    BreakOnDeath = 256,//0x0100
    BindOnEquip = 512,//0x0200
    NoSRepair = 1024,//0x0400
    NoWeddingRing = 2048,//0x0800
    UnableToRent = 4096,
    UnableToDisassemble = 8192,
    NoMail = 16384,
    NoHero = -32768
}

[Flags]
public enum SpecialItemMode : short
{
    /// <summary>
    /// 无特殊效果
    /// </summary>
    None = 0,
    /// <summary>
    /// 麻痹效果
    /// </summary>
    Paralize = 0x0001,
    /// <summary>
    /// 传送
    /// </summary>
    Teleport = 0x0002,
    /// <summary>
    /// 隐身戒指效果
    /// </summary>
    ClearRing = 0x0004,
    /// <summary>
    /// 保护效果
    /// </summary>
    Protection = 0x0008,
    /// <summary>
    /// 复活效果
    /// </summary>
    Revival = 0x0010,
    /// <summary>
    /// 肌肉力量
    /// </summary>
    Muscle = 0x0020,
    /// <summary>
    /// 火焰效果
    /// </summary>
    Flame = 0x0040,
    /// <summary>
    /// 治疗效果
    /// </summary>
    Healing = 0x0080,
    /// <summary>
    /// 探测效果
    /// </summary>
    Probe = 0x0100,
    /// <summary>
    /// 技巧戒指(技能增益)
    /// </summary>
    Skill = 0x0200,
    /// <summary>
    /// 无耐久损耗
    /// </summary>
    NoDuraLoss = 0x0400,
    /// <summary>
    /// 闪烁效果
    /// </summary>
    Blink = 0x800,
}

[Flags]
public enum RequiredClass : byte
{
    Warrior = 1,
    Wizard = 2,
    Taoist = 4,
    Assassin = 8,
    Archer = 16,
    WarWizTao = Warrior | Wizard | Taoist,
    None = WarWizTao | Assassin | Archer
}

[Flags]
public enum RequiredGender : byte
{
    Male = 1,
    Female = 2,
    None = Male | Female
}

public enum RequiredType : byte
{
    Level = 0,
    MaxAC = 1,
    MaxMAC = 2,
    MaxDC = 3,
    MaxMC = 4,
    MaxSC = 5,
    MaxLevel = 6,
    MinAC = 7,
    MinMAC = 8,
    MinDC = 9,
    MinMC = 10,
    MinSC = 11,
}
/// <summary>
/// 装备套装
/// </summary>
public enum ItemSet : byte
{
    None = 0,
    Spirit = 1,
    Recall = 2,
    RedOrchid = 3,
    RedFlower = 4,
    Smash = 5,
    HwanDevil = 6,
    Purity = 7,
    FiveString = 8,
    Mundane = 9,
    NokChi = 10,
    TaoProtect = 11,
    Mir = 12,
    Bone = 13,
    Bug = 14,
    WhiteGold = 15,
    WhiteGoldH = 16,
    RedJade = 17,
    RedJadeH = 18,
    Nephrite = 19,
    NephriteH = 20,
    Whisker1 = 21,
    Whisker2 = 22,
    Whisker3 = 23,
    Whisker4 = 24,
    Whisker5 = 25,
    Hyeolryong = 26,
    Monitor = 27,
    Oppressive = 28,
    Paeok = 29,
    Sulgwan = 30,
    BlueFrost = 31,
    DarkGhost = 38,
    BlueFrostH = 39
}
/// <summary>
/// 技能类型
/// </summary>
public enum Spell : byte
{
    None = 0,

    //Warrior
    Fencing = 1,
    Slaying = 2,
    Thrusting = 3,
    HalfMoon = 4,
    ShoulderDash = 5,
    TwinDrakeBlade = 6,
    Entrapment = 7,
    FlamingSword = 8,
    LionRoar = 9,
    CrossHalfMoon = 10,
    BladeAvalanche = 11,
    ProtectionField = 12,
    Rage = 13,
    CounterAttack = 14,
    SlashingBurst = 15,
    Fury = 16,
    ImmortalSkin = 17,

    //Wizard
    FireBall = 31,
    Repulsion = 32,
    ElectricShock = 33,
    GreatFireBall = 34,
    HellFire = 35,
    ThunderBolt = 36,
    Teleport = 37,
    FireBang = 38,
    FireWall = 39,
    Lightning = 40,
    FrostCrunch = 41,
    ThunderStorm = 42,
    MagicShield = 43,
    TurnUndead = 44,
    Vampirism = 45,
    IceStorm = 46,
    FlameDisruptor = 47,
    Mirroring = 48,
    FlameField = 49,
    Blizzard = 50,
    MagicBooster = 51,
    MeteorStrike = 52,
    IceThrust = 53,
    FastMove = 54,
    StormEscape = 55,

    //Taoist
    Healing = 61,
    SpiritSword = 62,
    Poisoning = 63,
    SoulFireBall = 64,
    SummonSkeleton = 65,
    Hiding = 67,
    MassHiding = 68,
    SoulShield = 69,
    Revelation = 70,
    BlessedArmour = 71,
    EnergyRepulsor = 72,
    TrapHexagon = 73,
    Purification = 74,
    MassHealing = 75,
    Hallucination = 76,
    UltimateEnhancer = 77,
    SummonShinsu = 78,
    Reincarnation = 79,
    SummonHolyDeva = 80,
    Curse = 81,
    Plague = 82,
    PoisonCloud = 83,
    EnergyShield = 84,
    PetEnhancer = 85,
    HealingCircle = 86,

    //Assassin
    FatalSword = 91,
    DoubleSlash = 92,
    Haste = 93,
    FlashDash = 94,
    LightBody = 95,
    HeavenlySword = 96,
    FireBurst = 97,
    Trap = 98,
    PoisonSword = 99,
    MoonLight = 100,
    MPEater = 101,
    SwiftFeet = 102,
    DarkBody = 103,
    Hemorrhage = 104,
    CrescentSlash = 105,
    MoonMist = 106,
    CatTongue = 107,

    //Archer
    Focus = 121,
    StraightShot = 122,
    DoubleShot = 123,
    ExplosiveTrap = 124,
    DelayedExplosion = 125,
    Meditation = 126,
    BackStep = 127,
    ElementalShot = 128,
    Concentration = 129,
    Stonetrap = 130,
    ElementalBarrier = 131,
    SummonVampire = 132,
    VampireShot = 133,
    SummonToad = 134,
    PoisonShot = 135,
    CrippleShot = 136,
    SummonSnakes = 137,
    NapalmShot = 138,
    OneWithNature = 139,
    BindingShot = 140,
    MentalState = 141,

    //Custom
    Blink = 151,
    Portal = 152,
    BattleCry = 153,
    FireBounce = 154,
    MeteorShower = 155,

    //Map Events
    DigOutZombie = 200,
    Rubble = 201,
    MapLightning = 202,
    MapLava = 203,
    MapQuake1 = 204,
    MapQuake2 = 205,
    DigOutArmadillo = 206,
    GeneralMeowMeowThunder = 207,
    StoneGolemQuake = 208,
    EarthGolemPile = 209,
    TreeQueenRoot = 210,
    TreeQueenMassRoots = 211,
    TreeQueenGroundRoots = 212,
    TucsonGeneralRock = 213,
    FlyingStatueIceTornado = 214,
    DarkOmaKingNuke = 215,
    HornedSorcererDustTornado = 216,
    HornedCommanderRockFall = 217,
    HornedCommanderRockSpike = 218
}

public enum SpellEffect : byte
{
    None,
    FatalSword,
    Teleport,
    Healing,
    RedMoonEvil,
    TwinDrakeBlade,
    MagicShieldUp,
    MagicShieldDown,
    GreatFoxSpirit,
    Entrapment,
    Reflect,
    Critical,
    Mine,
    ElementalBarrierUp,
    ElementalBarrierDown,
    DelayedExplosion,
    MPEater,
    Hemorrhage,
    Bleeding,
    AwakeningSuccess,
    AwakeningFail,
    AwakeningMiss,
    AwakeningHit,
    StormEscape,
    TurtleKing,
    Behemoth,
    Stunned,
    IcePillar,
    KingGuard,
    KingGuard2,
    DeathCrawlerBreath,
    FlamingMutantWeb,
    FurbolgWarriorCritical,
    Tester,
    MoonMist
}

/// <summary>
/// buff 类型
/// </summary>
public enum BuffType : byte
{
    /// <summary>
    /// 无效果
    /// </summary>
    None = 0,

    // Magics 法术

    /// <summary>
    /// 时空扭曲效果
    /// </summary>
    TemporalFlux,
    /// <summary>
    /// 隐身效果（仅对怪物生效）
    /// </summary>
    Hiding,
    /// <summary>
    /// 加速效果，色的移动速度
    /// </summary>
    Haste,
    /// <summary>
    /// 疾风步，进一步提升移动速度，比Haste效果更强
    /// </summary>
    SwiftFeet,
    /// <summary>
    /// 狂暴状态，提升角色的攻击力，可能降低防御
    /// </summary>
    Fury,
    /// <summary>
    /// 灵魂护盾，提升角色的魔法防御力
    /// </summary>
    SoulShield,
    /// <summary>
    /// 神圣护甲，提升角色的物理防御力
    /// </summary>
    BlessedArmour,
    /// <summary>
    /// 轻身术，提升敏捷度和移动速度
    /// </summary>
    LightBody,
    /// <summary>
    /// 终极强化，增强装备或角色的综合效果
    /// </summary>
    UltimateEnhancer,
    /// <summary>
    /// 保护领域，在角色周围形成防护罩，吸收或减少伤害
    /// </summary>
    ProtectionField,
    /// <summary>
    /// 愤怒状态，类似狂暴，大幅提升攻击力
    /// </summary>
    Rage,
    /// <summary>
    /// 诅咒效果，负面状态，降低角色的各项属性
    /// </summary>
    Curse,
    /// <summary>
    /// 月光术，夜间隐身效果，在特定时间提供隐身能力
    /// </summary>
    MoonLight,
    /// <summary>
    /// 暗影术，更高级的隐身效果，可能提供更好的隐形能力
    /// </summary>
    DarkBody,
    /// <summary>
    /// 专注状态，提升施法专注度，减少施法被打断的概率
	/// <para>弓箭手的专用buff</para>
	/// <para> 该buff立即获得（buff持续时间45s(基础时间) + （15s * 技能等级）），移动后中断，站立3s后重新生效</para>
    /// </summary>
    Concentration,
    /// <summarys
    /// 吸血射击，攻击时将部分伤害转化为自己的生命值
    /// </summary>
    VampireShot,
    /// <summary>
    /// 毒箭射击，攻击时对目标施加中毒效果
    /// </summary>
    PoisonShot,
    /// <summary>
    /// 反击状态，受到攻击时自动反击攻击者
    /// </summary>
    CounterAttack,
    /// <summary>
    /// 精神状态，影响角色的攻击模式（攻击/技巧/组队三种模式）
    /// </summary>
    MentalState,
    /// <summary>
    /// 能量护盾，使用魔法值吸收伤害
    /// </summary>
    EnergyShield,
    /// <summary>
    /// 魔法增幅，提升魔法技能的效果和伤害
    /// </summary>
    MagicBooster,
    /// <summary>
    /// 宠物强化，增强召唤宠物的能力
    /// </summary>
    PetEnhancer,
    /// <summary>
    /// 不朽之肤，提供额外的生命值或防御力
    /// </summary>
    ImmortalSkin,
    /// <summary>
    /// 魔法盾，提供魔法防护，减少魔法伤害
    /// </summary>
    MagicShield,
    /// <summary>
    /// 元素屏障，提供元素抗性，减少元素伤害
    /// </summary>
    ElementalBarrier,

    //Monster buff

    /// <summary>
    /// 角弓箭手的增益效果，怪物特有的buff
    /// </summary>
    HornedArcherBuff = 50,
    /// <summary>
    /// 寒冰弓箭手的增益效果，怪物特有的冰系buff
    /// </summary>
    ColdArcherBuff,
    /// <summary>
    /// 喵喵将军的护盾效果，怪物特有的防御buff
    /// </summary>
    GeneralMeowMeowShield,
    /// <summary>
    /// 犀牛祭司的负面效果，怪物施加给玩家的减益
    /// </summary>
    RhinoPriestDebuff,
    /// <summary>
    /// 能量珠的增益效果，提升怪物能力
    /// </summary>
    PowerBeadBuff,
    /// <summary>
    /// 角斗士的护盾效果，怪物战士的防御buff
    /// </summary>
    HornedWarriorShield,
    /// <summary>
    /// 角指挥官的护盾效果，怪物首领的防御buff
    /// </summary>
    HornedCommanderShield,
    /// <summary>
    /// 失明效果，降低命中率，使攻击难以命中
    /// </summary>
    Blindness,

    //Special

    /// <summary>
    /// 游戏管理员权限buff，GM专用的特殊状态
    /// </summary>
    GameMaster = 100,
    /// <summary>
    /// 通用增益效果，可能提供综合属性提升
    /// </summary>
    General,
    /// <summary>
    /// 经验值加成buff，提升打怪获得的经验值
    /// </summary>
    Exp,
    /// <summary>
    /// 掉落加成buff，提升怪物掉落物品的概率
    /// </summary>
    Drop,
    /// <summary>
    /// 金币加成buff，提升打怪获得的金币数量
    /// </summary>
    Gold,
    /// <summary>
    /// 背包负重加成，增加背包的负重上限
    /// </summary>
    BagWeight,
    /// <summary>
    /// 变身效果，改变角色的外形，可能获得新的能力
    /// </summary>
    Transform,
    /// <summary>
    /// 夫妻buff，婚姻系统中夫妻关系提供的增益效果
    /// </summary>
    Lover,
    /// <summary>
    /// 徒弟buff，师徒系统中徒弟获得的增益效果
    /// </summary>
    Mentee,
    /// <summary>
    /// 导师buff，师徒系统中导师获得的增益效果
    /// </summary>
    Mentor,
    /// <summary>
    /// 行会buff，加入行会后获得的增益效果
    /// </summary>
    Guild,
    /// <summary>
    /// 监狱状态，被禁闭时的特殊状态，可能限制行动
    /// </summary>
    Prison,
    /// <summary>
    /// 休息状态，离线休息后登录获得的经验加成
    /// </summary>
    Rested,
    /// <summary>
    /// 技能增益，技巧戒指提供的buff效果
    /// </summary>
    Skill,
    /// <summary>
    /// 隐身戒指buff，装备隐身戒指后对怪物隐形
    /// </summary>
    ClearRing,
    /// <summary>
    /// 新手保护buff，新手玩家获得的保护效果，可能提供属性加成或保护
    /// </summary>
    Newbie,

    //Stats

    /// <summary>
    /// 物理攻击加成，提升角色的物理攻击力
    /// </summary>
    Impact = 200,
    /// <summary>
    /// 魔法攻击加成，提升角色的魔法攻击力
    /// </summary>
    Magic,
    /// <summary>
    /// 道术攻击加成，提升角色的道术攻击力
    /// </summary>
    Taoist,
    /// <summary>
    /// 攻击速度加成，提升角色的攻击速度
    /// </summary>
    Storm,
    /// <summary>
    /// 生命值加成，提升角色的最大生命值
    /// </summary>
    HealthAid,
    /// <summary>
    /// 魔法值加成，提升角色的最大魔法值
    /// </summary>
    ManaAid,
    /// <summary>
    /// 物理防御加成，提升角色的物理防御力
    /// </summary>
    Defence,
    /// <summary>
    /// 魔法防御加成，提升角色的魔法防御力
    /// </summary>
    MagicDefence,
    /// <summary>
    /// 灵丹妙药效果，综合属性提升，可能同时提升多项属性
    /// </summary>
    WonderDrug,
    /// <summary>
    /// 背包容量加成，增加背包的格子数量或容量
    /// </summary>
    Knapsack,
}

/// <summary>
/// buff的特殊属性标志，用于定义buff的行为特性
/// <para>例如: </para>
/// <para>1. 角色死亡时移除该buff</para>
/// <para>2. 角色退出游戏时移除该buff</para>
/// <para>3. 负面效果，减益buff</para>
/// <para>4. 安全区域暂停计时， buff 在安全区域时暂停倒计时</para>
/// </summary>
[Flags]
public enum BuffProperty : byte
{
    /// <summary>
    /// 无特殊属性
    /// </summary>
    None = 0,
    /// <summary>
    /// 角色死亡时移除该buff
    /// </summary>
    RemoveOnDeath = 1,
    /// <summary>
    /// 角色退出游戏时移除该buff
    /// </summary>
    RemoveOnExit = 2,
    /// <summary>
    /// 负面效果，减益buff
    /// </summary>
    Debuff = 4,
    /// <summary>
    /// 在安全区域暂停计时
    /// </summary>
    PauseInSafeZone = 8
}
/// <summary>
/// buff 的叠加规则
/// </summary>
public enum BuffStackType : byte
{
    /// <summary>
    /// 不允许叠加
    /// </summary>
    None,
    /// <summary>
    /// 不叠加，只刷新时间
    /// </summary>
    ResetDuration,
    /// <summary>
    /// 持续时间可叠加
    /// </summary>
    StackDuration,
    /// <summary>
    /// 属性叠加，时间不变
    /// </summary>
    StackStat,
    /// <summary>
    /// 属性 + 时间 都叠
    /// </summary>
    StackStatAndDuration,
    /// <summary>
    /// 无限 Buff
    /// </summary>
    Infinite,
    /// <summary>
    /// 属性重置，时间不变
    /// </summary>
    ResetStat,
    /// <summary>
    /// 属性 + 时间 全部重置
    /// </summary>
    ResetStatAndDuration
}
/// <summary>
/// 防御类型
/// </summary>
public enum DefenceType : byte
{
    /// <summary>
	/// 敏捷物理防御
	/// </summary>
    ACAgility,
    /// <summary>
    /// 纯物理防御
    /// </summary>
    AC,
    /// <summary>
    /// 敏捷魔法防御
    /// </summary>
    MACAgility,
    /// <summary>
    /// 纯魔法防御
    /// </summary>
    MAC,
    /// <summary>
    /// 纯敏捷防御
    /// </summary>
    Agility,
    /// <summary>
    /// 反弹防御
    /// </summary>
    Repulsion,
    /// <summary>
    /// 无防御
    /// </summary>
    None
}

public enum ServerPacketIds : short
{
    Connected,
    ClientVersion,
    Disconnect,
    KeepAlive,
    NewAccount,
    ChangePassword,
    ChangePasswordBanned,
    Login,
    LoginBanned,
    LoginSuccess,
    NewCharacter,
    NewCharacterSuccess,
    DeleteCharacter,
    DeleteCharacterSuccess,
    StartGame,
    StartGameBanned,
    StartGameDelay,
    MapInformation,
    NewMapInfo,
    WorldMapSetup,
    SearchMapResult,
    UserInformation,
    UserSlotsRefresh,
    UserLocation,
    ObjectPlayer,
    ObjectHero,
    ObjectRemove,
    ObjectTurn,
    ObjectWalk,
    ObjectRun,
    Chat,
    ObjectChat,
    NewItemInfo,
    NewMonsterInfo,
    NewNPCInfo,
    NewHeroInfo,
    NewChatItem,
    MoveItem,
    EquipItem,
    MergeItem,
    RemoveItem,
    RemoveSlotItem,
    TakeBackItem,
    StoreItem,
    SplitItem,
    SplitItem1,
    DepositRefineItem,
    RetrieveRefineItem,
    RefineCancel,
    RefineItem,
    DepositTradeItem,
    RetrieveTradeItem,
    UseItem,
    DropItem,
    TakeBackHeroItem,
    TransferHeroItem,
    PlayerUpdate,
    PlayerInspect,
    LogOutSuccess,
    LogOutFailed,
    ReturnToLogin,
    TimeOfDay,
    ChangeAMode,
    ChangePMode,
    ObjectItem,
    ObjectGold,
    GainedItem,
    GainedGold,
    LoseGold,
    GainedCredit,
    LoseCredit,
    ObjectMonster,
    ObjectAttack,
    Struck,
    ObjectStruck,
    DamageIndicator,
    DuraChanged,
    HealthChanged,
    HeroHealthChanged,
    DeleteItem,
    Death,
    ObjectDied,
    ColourChanged,
    ObjectColourChanged,
    ObjectGuildNameChanged,
    GainExperience,
    GainHeroExperience,
    LevelChanged,
    HeroLevelChanged,
    ObjectLeveled,
    ObjectHarvest,
    ObjectHarvested,
    ObjectNpc,
    NPCResponse,
    ObjectHide,
    ObjectShow,
    Poisoned,
    ObjectPoisoned,
    MapChanged,
    ObjectTeleportOut,
    ObjectTeleportIn,
    TeleportIn,
    NPCGoods,
    NPCSell,
    NPCRepair,
    NPCSRepair,
    NPCRefine,
    NPCCheckRefine,
    NPCCollectRefine,
    NPCReplaceWedRing,
    NPCStorage,
    SellItem,
    CraftItem,
    RepairItem,
    ItemRepaired,
    ItemSlotSizeChanged,
    ItemSealChanged,
    NewMagic,
    RemoveMagic,
    MagicLeveled,
    Magic,
    MagicDelay,
    MagicCast,
    ObjectMagic,
    ObjectEffect,
    ObjectProjectile,
    RangeAttack,
    Pushed,
    ObjectPushed,
    ObjectName,
    UserStorage,
    SwitchGroup,
    DeleteGroup,
    DeleteMember,
    GroupInvite,
    AddMember,
    Revived,
    ObjectRevived,
    SpellToggle,
    ObjectHealth,
    ObjectMana,
    MapEffect,
    AllowObserve,
    ObjectRangeAttack,
    AddBuff,
    RemoveBuff,
    PauseBuff,
    ObjectHidden,
    RefreshItem,
    ObjectSpell,
    UserDash,
    ObjectDash,
    UserDashFail,
    ObjectDashFail,
    NPCConsign,
    NPCMarket,
    NPCMarketPage,
    ConsignItem,
    MarketFail,
    MarketSuccess,
    ObjectSitDown,
    InTrapRock,
    BaseStatsInfo,
    HeroBaseStatsInfo,
    UserName,
    ChatItemStats,
    GuildNoticeChange,
    GuildMemberChange,
    GuildStatus,
    GuildInvite,
    GuildExpGain,
    GuildNameRequest,
    GuildStorageGoldChange,
    GuildStorageItemChange,
    GuildStorageList,
    GuildRequestWar,
    HeroCreateRequest,
    NewHero,
    HeroInformation,
    UpdateHeroSpawnState,
    UnlockHeroAutoPot,
    SetAutoPotValue,
    SetAutoPotItem,
    SetHeroBehaviour,
    ManageHeroes,
    ChangeHero,
    DefaultNPC,
    NPCUpdate,
    NPCImageUpdate,
    MarriageRequest,
    DivorceRequest,
    MentorRequest,
    TradeRequest,
    TradeAccept,
    TradeGold,
    TradeItem,
    TradeConfirm,
    TradeCancel,
    MountUpdate,
    EquipSlotItem,
    FishingUpdate,
    ChangeQuest,
    CompleteQuest,
    ShareQuest,
    NewQuestInfo,
    GainedQuestItem,
    DeleteQuestItem,
    CancelReincarnation,
    RequestReincarnation,
    UserBackStep,
    ObjectBackStep,
    UserDashAttack,
    ObjectDashAttack,
    UserAttackMove,
    CombineItem,
    ItemUpgraded,
    SetConcentration,
    SetElemental,
    RemoveDelayedExplosion,
    ObjectDeco,
    ObjectSneaking,
    ObjectLevelEffects,
    SetBindingShot,
    SendOutputMessage,
    NPCAwakening,
    NPCDisassemble,
    NPCDowngrade,
    NPCReset,
    AwakeningNeedMaterials,
    AwakeningLockedItem,
    Awakening,
    ReceiveMail,
    MailLockedItem,
    MailSendRequest,
    MailSent,
    ParcelCollected,
    MailCost,
    ResizeInventory,
    ResizeStorage,
    NewIntelligentCreature,
    UpdateIntelligentCreatureList,
    IntelligentCreatureEnableRename,
    IntelligentCreaturePickup,
    NPCPearlGoods,
    TransformUpdate,
    FriendUpdate,
    LoverUpdate,
    MentorUpdate,
    GuildBuffList,
    NPCRequestInput,
    GameShopInfo,
    GameShopStock,
    Rankings,
    Opendoor,
    GetRentedItems,
    ItemRentalRequest,
    ItemRentalFee,
    ItemRentalPeriod,
    DepositRentalItem,
    RetrieveRentalItem,
    UpdateRentalItem,
    CancelItemRental,
    ItemRentalLock,
    ItemRentalPartnerLock,
    CanConfirmItemRental,
    ConfirmItemRental,
    NewRecipeInfo,
    OpenBrowser,
    PlaySound,
    SetTimer,
    ExpireTimer,
    UpdateNotice,
    Roll,
    SetCompass,
    GroupMembersMap,
    SendMemberLocation,
    GuildTerritoryPage,
}

public enum ClientPacketIds : short
{
    ClientVersion,
    Disconnect,
    KeepAlive,
    NewAccount,
    ChangePassword,
    Login,
    NewCharacter,
    DeleteCharacter,
    StartGame,
    LogOut,
    Turn,
    Walk,
    Run,
    Chat,
    MoveItem,
    StoreItem,
    TakeBackItem,
    MergeItem,
    EquipItem,
    RemoveItem,
    RemoveSlotItem,
    SplitItem,
    UseItem,
    DropItem,
    DepositRefineItem,
    RetrieveRefineItem,
    RefineCancel,
    RefineItem,
    CheckRefine,
    ReplaceWedRing,
    DepositTradeItem,
    RetrieveTradeItem,
    TakeBackHeroItem,
    TransferHeroItem,
    DropGold,
    PickUp,
    RequestMapInfo,
    RequestMonsterInfo,
    RequestNPCInfo,
    RequestItemInfo,
    TeleportToNPC,
    SearchMap,
    Inspect,
    Observe,
    ChangeAMode,
    ChangePMode,
    ChangeTrade,
    Attack,
    RangeAttack,
    Harvest,
    CallNPC,
    BuyItem,
    SellItem,
    CraftItem,
    RepairItem,
    BuyItemBack,
    SRepairItem,
    MagicKey,
    Magic,
    SwitchGroup,
    AddMember,
    DellMember,
    GroupInvite,
    NewHero,
    SetAutoPotValue,
    SetAutoPotItem,
    SetHeroBehaviour,
    ChangeHero,
    TownRevive,
    SpellToggle,
    ConsignItem,
    MarketSearch,
    MarketRefresh,
    MarketPage,
    MarketBuy,
    MarketGetBack,
    MarketSellNow,
    RequestUserName,
    RequestChatItem,
    EditGuildMember,
    EditGuildNotice,
    GuildInvite,
    GuildNameReturn,
    RequestGuildInfo,
    GuildStorageGoldChange,
    GuildStorageItemChange,
    GuildWarReturn,
    MarriageRequest,
    MarriageReply,
    ChangeMarriage,
    DivorceRequest,
    DivorceReply,
    AddMentor,
    MentorReply,
    AllowMentor,
    CancelMentor,
    TradeRequest,
    TradeReply,
    TradeGold,
    TradeConfirm,
    TradeCancel,
    EquipSlotItem,
    FishingCast,
    FishingChangeAutocast,
    AcceptQuest,
    FinishQuest,
    AbandonQuest,
    ShareQuest,

    AcceptReincarnation,
    CancelReincarnation,
    CombineItem,

    AwakeningNeedMaterials,
    AwakeningLockedItem,
    Awakening,
    DisassembleItem,
    DowngradeAwakening,
    ResetAddedItem,

    SendMail,
    ReadMail,
    CollectParcel,
    DeleteMail,
    LockMail,
    MailLockedItem,
    MailCost,

    UpdateIntelligentCreature,
    IntelligentCreaturePickup,
    RequestIntelligentCreatureUpdates,

    AddFriend,
    RemoveFriend,
    RefreshFriends,
    AddMemo,
    GuildBuffUpdate,
    NPCConfirmInput,
    GameshopBuy,

    ReportIssue,
    GetRanking,
    Opendoor,

    GetRentedItems,
    ItemRentalRequest,
    ItemRentalFee,
    ItemRentalPeriod,
    DepositRentalItem,
    RetrieveRentalItem,
    CancelItemRental,
    ItemRentalLockFee,
    ItemRentalLockItem,
    ConfirmItemRental,
    GuildTerritoryPage,
    PurchaseGuildTerritory,
    DeleteItem,
}

public enum ConquestType : byte
{
    Request = 0,
    Auto = 1,
    Forced = 2,
}

public enum ConquestGame : byte
{
    CapturePalace = 0,
    KingOfHill = 1,
    Random = 2,
    Classic = 3,
    ControlPoints = 4
}

[Flags]
public enum GuildRankOptions : byte
{
    CanChangeRank = 1,
    CanRecruit = 2,
    CanKick = 4,
    CanStoreItem = 8,
    CanRetrieveItem = 16,
    CanAlterAlliance = 32,
    CanChangeNotice = 64,
    CanActivateBuff = 128
}
/// <summary>
/// 门状态
/// </summary>
public enum DoorState : byte
{
    /// <summary>
    /// 关闭
    /// </summary>
    Closed = 0,
    /// <summary>
    /// 打开中
    /// </summary>
    Opening = 1,
    /// <summary>
    /// 打开
    /// </summary>
    Open = 2,
    /// <summary>
    /// 关闭中
    /// </summary>
    Closing = 3
}

public enum IntelligentCreaturePickupMode : byte
{
    Automatic = 0,
    SemiAutomatic = 1,
}

public enum HeroSpawnState : byte
{
    None = 0,
    Unsummoned = 1,
    Summoned = 2,
    Dead = 3
}

public enum HeroBehaviour : byte
{
    Attack = 0,
    CounterAttack = 1,
    Follow = 2,
    Custom = 3
}

public enum SpellToggleState: sbyte
{
    None = -1,
    False = 0,
    True = 1
}

public enum MarketCollectionMode : byte
{
    Any = 0,
    Sold = 1,
    Expired = 2
}