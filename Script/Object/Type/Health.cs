using DEBUG;
using Godot;
using System;
namespace Level.Object;
[Icon("res://Image/Icon/Script/Health_Script.png")]
[GlobalClass]
public partial class Health : Godot.Resource
{
    /// <summary>
    /// 最大血量
    /// </summary>
    [Export] public float MaxHP = 10;
    /// <summary>
    /// 最小血量
    /// </summary>
    [Export] public float MinHP = 0;
    /// <summary>
    /// 当前血量
    /// </summary>
    [Export] public float HP = -1;
    /// <summary>
    /// 初始化当前生命
    /// </summary>
    public void Reset()
    {
        HP = MaxHP;
        Info.Print(MaxHP);
    }
}