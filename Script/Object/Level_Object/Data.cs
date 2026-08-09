using Godot;
namespace Level.Object;
/// <summary>
/// 用于器械，怪物BOSS的整体数据
/// </summary>
public partial class Data : CharacterBody2D
{
    /// <summary>
    /// 伤害
    /// </summary>
    [ExportCategory("看什么看?变量在Data中")]
    [ExportGroup("status")]
    [Export] public bool Enable = true;
    [Export] public float Damage;
    public _Health Health = new _Health();
    [Export] public Godot.Collections.Array<CharacterBody2D> Delectation = new Godot.Collections.Array<CharacterBody2D>(){};
    /// <summary>
    /// 生命
    /// </summary>
    public class _Health
    {
        /// <summary>
        /// 最大血量
        /// </summary>
        [Export] public static float MaxHP = 10;
        /// <summary>
        /// 最小血量
        /// </summary>
        [Export] public static float MinHP = 0;
        /// <summary>
        /// 当前血量
        /// </summary>
        [Export] public static float HP = MaxHP;
    }
}
