using Godot;
using System;
using System.Threading.Tasks;
using DEBUG;
using System.Diagnostics;
namespace Level.Module;
public partial class ObjectPhysics : Node2D
{
    /// <summary>
    /// 速度
    /// </summary>
    [ExportGroup("Vector")]
    [Export] public Godot.Vector2 position_Offset = Godot.Vector2.Zero;
    /// <summary>
    /// 启用物理
    /// </summary>
    [ExportGroup("Physics")]
    [Export] public bool Physics_Enable = true;
    /// <summary>
    /// 重量
    /// </summary>发`
    [Export] public float Weight = 5;
    /// <summary>
    /// 下落加速度
    /// </summary>
    [Export] public float Falling_Acceleration = 1;
    /// <summary>
    /// 高度
    /// </summary>
    [Export] public float Height = 0;
    /// <summary>
    /// 额外高度
    /// </summary>
    [Export] public float Extra_Height = 0;
    /// <summary>
    /// 实际坐标
    /// </summary>
    [Export] public Godot.Vector2 practical_Position = new Godot.Vector2();
    /// <summary>
    /// 阴影
    /// </summary>
    [Export] public Node Shadow = null;
    /// <summary>
    /// 阴影尺寸
    /// </summary>
    [Export] public Godot.Vector2 Shadow_Size = Vector2.Zero;
    /// <summary>
    /// 阴影偏移
    /// </summary>
    [Export] public Godot.Vector2 Shadow_Offset = Vector2.Zero;
    /// <summary>
    /// 加速度
    /// </summary>
    [Export] public float Acceleration = 0;
    [Export] internal bool Physics_Initialization = false;
    public void Reset_Position()
    {
        practical_Position = GlobalPosition;
        Physics_Initialization = true;
    }
    public void SetPhysics_Position(double delta) {
        float FloatDelta = (float)delta;
        if (!Physics_Enable){return;}
        if (!Physics_Initialization){return;}
        GlobalPosition = practical_Position - new Vector2(0,Height + Extra_Height);
        if (Height > 0)
        {
            Falling_Acceleration += Weight * 10 * FloatDelta;
            Height -= Falling_Acceleration;
        }

        if(Height < 0)
        {
            Falling_Acceleration = 0;
            Height = 0;
        }
    }
}
