using System;
using System.Threading.Tasks;
using DEBUG;
using Godot;
namespace Level.Object;
/// <summary>
/// 用于器械，怪物BOSS的整体数据
/// </summary>
public partial class Data : CharacterBody2D
{
    /// <summary>
    /// 检测
    /// </summary>
    [ExportCategory("看什么看?变量在Data中")]
    [ExportGroup("Object")]
    [Export] public Godot.Area2D Area = null;
    /// <summary>
    /// 启用状态
    /// </summary>
    [ExportGroup("status")]
    [Export] public bool Enable = true;
    /// <summary>
    /// 伤害值
    /// </summary>
    [Export] public float Damage;
    /// <summary>
    /// 生命类
    /// </summary>
    public _Health Health = new _Health();
    /// <summary>
    /// 已检测到的object
    /// </summary>
    [Export] public Godot.Collections.Array<Level.Object.Data> Current_detection_object = new Godot.Collections.Array<Level.Object.Data>(){};
    [Export]public Godot.Collections.Array<StringName> detection_Group = new Godot.Collections.Array<StringName>(){};
    
    /// <summary>
    /// 速度
    /// </summary>
    [ExportGroup("Vector")]
    [Export] public Godot.Vector2 Speed = Vector2.Zero;
    [Export] public bool Move_Ing = false;
    /// <summary>
    /// 启用物理
    /// </summary>
    [ExportGroup("Physics")]
    [Export] public bool Physics_Enable = false;
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
    /// 加速度
    /// </summary>
    [Export] public float Acceleration = 0;
    public override void _Ready() {
        base._Ready();
        GD.Print(GetGroups()[0]);
        if (!Enable){return;}
        practical_Position = Position;
        Area = GetNode<Godot.Area2D>("Area");
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
            if (Move_Ing == true){
               practical_Position += Speed;
        }
    }
    /// <summary>
    /// 让Area变量索引的物体初始化
    /// </summary>
    /// <summary>
    /// 移除空物体
    /// </summary>
    public void Remove_Null_Object()
    {
        foreach(Level.Object.Data Body in Current_detection_object)
        {
            if (Body == null)
            {
                Current_detection_object.Remove(Body);
            }
        }
    }
    /// <summary>
    /// 添加物体
    /// </summary>
    public void Add_Object(Node2D node)
    {
        if (node is Level.Object.Data && node != this)
        {
            Current_detection_object.Add((Level.Object.Data)node);
        }
    }
    /// <summary>
    /// 移除已检测到的其中一位物体
    /// </summary>
    /// <param name="node"></param>
    public void Remove_Object(Node2D node)
    {
        if (node is Level.Object.Data && node != this){
            Level.Object.Data Body = (Level.Object.Data)node;
            if (Current_detection_object.IndexOf(Body) != -1)
            {
                Current_detection_object.Remove(Body);
            }
        }
    }
    /// <summary>
    /// 移除索引的物体
    /// </summary>
    /// <param name="Index"></param>
    public void Remove_Object(int Index)
    {
        Current_detection_object.RemoveAt(Index);
    }
    /// <summary>
    /// 启动物理
    /// </summary>
    public async Task Physics_ON()
    {
        Physics_Enable = true;
        while (Physics_Enable)
        {
            if (Height >= 0)
            {
                Position = practical_Position;
            }
            await Task.Delay((int)Math.Round(1000 / Performance.GetMonitor(Performance.Monitor.TimeFps),0));
        }
    }
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
