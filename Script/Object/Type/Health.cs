using DEBUG;
using Godot;
namespace Level.Object;
/// <summary>
/// 生命基类
/// </summary>
[Icon("res://Image/Icon/Script/Health_Script.png")]
[GlobalClass]
public partial class Health() : Godot.Resource
{
    [Signal] public delegate void Health_ReduceEventHandler(Node Damage_Object);
    [ExportGroup("Health Points")]
    /// <summary>
    /// 最大血量
    /// </summary>
    [Export] public int MaxHP = 10;
    /// <summary>
    /// 最小血量
    /// </summary>
    [Export] public int MinHP = 0;
    /// <summary>
    /// 当前血量
    /// </summary>
    [Export] public int HP = -1;
    /// <summary>
    /// 是否死亡
    /// </summary>
    [ExportGroup("Status")]
    [Export] public bool kill = false;
    /// <summary>
    /// 初始化当前生命
    /// </summary>
    public void Reset()
    {
        if (HP == -1){
            HP = MaxHP;
        }
        Info.Print("初始化:",HP);
    }

}