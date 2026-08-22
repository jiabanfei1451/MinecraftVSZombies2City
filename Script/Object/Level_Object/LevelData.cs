using System;
using System.Threading.Tasks;
using DEBUG;
using Godot;
using Microsoft.VisualBasic;
namespace Level.Object;
/// <summary>
/// 用于器械，怪物BOSS的整体数据
/// </summary>
public partial class LevelData : CharacterBody2D
{
    /// <summary>
    /// 当前草坪行数索引
    /// </summary>
    [ExportCategory("看什么看?变量在Data中")]
    [ExportGroup("Index")]
    [Export] public int Lawn_Index = -1;
    /// <summary>
    /// 自动设置草坪行数索引
    /// </summary>
    [Export] public bool AutoSet_Lawn_Index = true;
    /// <summary>
    /// 检测
    /// </summary>
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
    
    [ExportGroup("status")] 
    [Export] public bool Enable = true;
    /// <summary>
    /// 伤害值
    /// </summary>
    [Export] public int Damage;
    /// <summary>
    /// 已检测到的object
    /// </summary>
    [Export] public Godot.Collections.Array<Level.Object.LevelData> Current_detection_object = new Godot.Collections.Array<Level.Object.LevelData>(){};
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
    #region 移动
    [ExportSubgroup("Move")]
    [Export] public Godot.Vector2 Speed = Vector2.Zero;
    /// <summary>
    /// 移动速度的倍率
    /// </summary>
    [Export] public float Speed_Multiplication = 1;
    /// <summary>
    /// 移动状态
    /// </summary>
    [Export] public bool Move_Ing = false;
    /// <summary>
    /// 移动类型
    /// </summary>
    [Export] public Move_Type MoveType = Move_Type.Linear_Motion;
    /// <summary>
    /// 移动类型
    /// </summary>
    public enum Move_Type
    {
        /// <summary>
        /// 平移
        /// </summary>
        Linear_Motion = 0,
        /// <summary>
        /// 使用脚本驱动
        /// </summary>
        Script_Driver = 1,
    }
    /// <summary>
    /// 基于Practical_Position的偏移量
    /// </summary>
    #endregion
    [ExportGroup("Vector")]
    [Export] public Godot.Vector2 position_Offset = Godot.Vector2.Zero;
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
    internal Level_Master_Script level {get;set;} = null;
    float Temp_Position_Y = -1;

    public override void _ExitTree()
    {
        base._ExitTree();
        if (!Enable){return;}
        Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level").Remove_Lawn_Index(this,Lawn_Index);
    }
    public override async void _Ready() {
        base._Ready();
        if (!Enable_Health)
        {
            Health = null;
        }
        else
        {
            Health.Reset();
        }
        if (!Enable){return;}
        practical_Position = Position;
        Area = GetNode<Godot.Area2D>("Area");
        if (level != null){
            if (level.Game_Reset_Done == true){
                await Task.Delay(100);
            }
        }
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        Position = practical_Position + new Vector2(0,Height);
        switch (MoveType){
            case Move_Type.Linear_Motion:
                if (Move_Ing == true){
                    practical_Position += (Speed * new Vector2(Speed_Multiplication,Speed_Multiplication)) * new Vector2((float)delta,(float)delta);
                }
            break;
        }
        if (Health != null)
        {
            if (Health.kill == true)
            {
                Health.Free();
                QueueFree();
            }
        }
        if (level == null)
        {
            Level_Master_Script Get_Level = Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level");
            if (Get_Level != null)
            {
                level = Get_Level;
            }
        }
        if (level != null && AutoSet_Lawn_Index == true)
        {
            if (Temp_Position_Y != practical_Position.Y + position_Offset.Y)
            {
                Temp_Position_Y = practical_Position.Y + position_Offset.Y;
                if (Lawn_Index != -1){
                    level.Move_Lawn_Index(this,AutoGet_LawnIndex());
                }
                else
                {
                    Lawn_Index = AutoGet_LawnIndex();
                    level.Add_Lawn_Index(this,Lawn_Index);
                }
            }
        }
    }
    public int AutoGet_LawnIndex()
    {
        if (level == null){return -1;}
        float Position_Y = level.Lawn_Spawn_Position.Y;
        float IndexNumber = level.Lawn_Spawn_Offect.Y;
        float Index = practical_Position.Y + position_Offset.Y;
        int Current_Lawn_Index = 0;
        int MaxIndex = level.Lawn_Object_Index.Count;
        while(Index >= Position_Y)
        {
            if (Index >= Position_Y)
            {
                Index -= IndexNumber;
                Current_Lawn_Index += 1;
            }
        }
        Current_Lawn_Index -= 1;
        if (Current_Lawn_Index >= MaxIndex)
        {
            Current_Lawn_Index = MaxIndex -1;
        }
        if (Current_Lawn_Index < 0)
        {
            Current_Lawn_Index = 0;
        }
        return Current_Lawn_Index;
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
        foreach(Level.Object.LevelData Body in Current_detection_object)
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
        if (node is Level.Object.LevelData && node != this)
        {
            Current_detection_object.Add((Level.Object.LevelData)node);
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
        if (node is Level.Object.LevelData && node != this){
            Level.Object.LevelData Body = (Level.Object.LevelData)node;
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
