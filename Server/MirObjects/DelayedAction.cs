using Server.MirEnvir;

namespace Server.MirObjects
{
    /// <summary>
    /// 延迟类型
    /// </summary>
    public enum DelayedType
    {
        /// <summary>
        /// 延迟施法动作
        /// </summary>
        Magic,
        /// <summary>
        /// Param0 MapObject (Target) | Param1 Damage | Param2 Defence | Param3 damageWeapon | Param4 UserMagic | Param5 FinalHit
        /// </summary>
        Damage,
        /// <summary>
        /// 远程伤害
        /// </summary>
        RangeDamage,
        /// <summary>
        /// 怪物生成
        /// </summary>
        Spawn,
        /// <summary>
        /// 死亡动作
        /// </summary>
        Die,
        /// <summary>
        /// 召回
        /// </summary>
        Recall,
        /// <summary>
        /// 地图移动
        /// </summary>
        MapMovement,
        /// <summary>
        /// 挖矿
        /// </summary>
        Mine,
        /// <summary>
        /// NPC相关
        /// </summary>
        NPC,
        /// <summary>
        /// 中毒
        /// </summary>
        Poison,
        /// <summary>
        /// 伤害指示器
        /// </summary>
        DamageIndicator,
        /// <summary>
        /// 任务
        /// </summary>
        Quest,

		// Sanjian
		/// <summary>
		/// 法术效果
		/// </summary>
		SpellEffect,
    }
    /// <summary>
    /// 延迟动作
    /// </summary>
    public class DelayedAction
    {
        /// <summary>
        /// 主环境
        /// </summary>
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }
        /// <summary>
        /// 延迟动作类型
        /// </summary>
        public DelayedType Type;
		/// <summary>
		/// 动作应该执行的时间点
		/// </summary>
        public long Time;
		/// <summary>
		/// 延迟动作创建的时间点
		/// </summary>
        public long StartTime;
        /// <summary>
		/// 动作的参数
		/// </summary>
        public object[] Params;
        /// <summary>
		/// 标记该延迟动作是否需要被移除
		/// </summary>
        public bool FlaggedToRemove;
        /// <summary>
        /// 延迟动作的构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="time">要执行的时间点</param>
        /// <param name="p">参数</param>
        public DelayedAction(DelayedType type, long time, params object[] p)
        {
            StartTime = Envir.Time;
            Type = type;
            Time = time;
            Params = p;
        }
    }
}
