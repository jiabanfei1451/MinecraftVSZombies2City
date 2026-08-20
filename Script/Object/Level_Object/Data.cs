using System;
using System.Threading.Tasks;
using DEBUG;
using Godot;
using Microsoft.VisualBasic;
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
    /// 生命类
    /// </summary>
    [ExportGroup("status")]
    #region 生命组件
    [ExportSubgroup("Health")] [Export] public Health Health = new Health();
    /// <summary>
    /// 启用生命组件
    /// </summary>
    [Export] public bool Enable_Health = true;
    #endregion
    /// <summary>
    /// 启用状态
    /// </summary>
    
    [ExportGroup("status")] [Export] public bool Enable = true;
    /// <summary>
    /// 伤害值
    /// </summary>
    [Export] public float Damage;
    /// <summary>
    /// 已检测到的object
    /// </summary>
    [Export] public Godot.Collections.Array<Level.Object.Data> Current_detection_object = new Godot.Collections.Array<Level.Object.Data>(){};
    /// <summary>
    /// 检测阵营
    /// </summary>
    [Export]public Godot.Collections.Array<StringName> detection_Group = new Godot.Collections.Array<StringName>(){};
    /// <summary>
    /// 排除阵营
    /// </summary>
    [Export] public Godot.Collections.Array<StringName> Exclude_Group = new Godot.Collections.Array<StringName>(){"Projectile","Area"};
    /// <summary>
    /// 速度
    /// </summary>
    [ExportGroup("Vector")]
    [Export] public Godot.Vector2 Speed = Vector2.Zero;
    [Export] public float Speed_Multiplication = 1;
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
        if (!Enable_Health)
        {
            Health.Free();
        }
        else
        {
            Health.Reset();
        }
        if (!Enable){return;}
        practical_Position = Position;
        Area = GetNode<Godot.Area2D>("Area");
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
            Position = practical_Position + new Vector2(0,Height);
            if (Move_Ing == true){
               practical_Position += (Speed * new Vector2(Speed_Multiplication,Speed_Multiplication)) * new Vector2((float)delta,(float)delta);
        }
    }
    public async Task Reset_Area()
    {
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        Area.Monitoring = false;
        Area.Monitorable = false;
        await ToSignal(GetTree().CreateTimer(0.05),SceneTreeTimer.SignalName.Timeout);
        Area.Monitoring = true;
        Area.Monitorable = true;
    }
    /// <summary>
    /// 检测
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool Check_Object_Group(Node2D node)
    {
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return false;
        }
        Godot.Collections.Array<StringName> Group_String = node.GetGroups();
        foreach (StringName @string in Group_String)
        {
            if (Exclude_Group.IndexOf(@string) != -1)
            {
                return false;
            }
        }
        foreach (StringName @string in Group_String)
        {
            if (detection_Group.IndexOf(@string) != -1)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 移除空物体
    /// </summary>
    public void Remove_Null_Object()
    {
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
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
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
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
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
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
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        Current_detection_object.RemoveAt(Index);
    }
    /// <summary>
    /// 启动物理
    /// </summary>
    public async Task Physics_ON()
    {
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        Physics_Enable = true;
        while (Physics_Enable && Enable)
        {
            await Task.Delay((int)Math.Round(1000 / Performance.GetMonitor(Performance.Monitor.TimeFps),0));
        }
    }
    /// <summary>
    /// 生命
    /// </summary>

}
