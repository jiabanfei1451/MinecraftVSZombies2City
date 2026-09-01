using System;
using System.Threading.Tasks;
using DEBUG;
using Godot;
namespace Level.Object;
/// <summary>
/// 用于器械，怪物BOSS在关卡内的整体数据
/// <para>
/// 列如:
/// </para>
/// <para>草坪基类 当前草坪行索引 自动索引开关</para>
/// <para>区域检测物体 生命组件 启用生命组件</para>
/// <para>启用 伤害 >是否检测/排除xx阵营</para>
/// </summary>
public partial class LevelObject : Level.Module.ObjectPhysics
{
    #region 信号
    /// <summary>
    /// 物体进入时
    /// </summary>
    /// <param name="Object"></param>
    [Signal] public delegate void Object_JoinEventHandler(Node2D Object);
    /// <summary>
    /// 物体离开时
    /// </summary>
    /// <param name="Object"></param>
    [Signal] public delegate void Object_ExitEventHandler(Node2D Object);
    /// <summary>
    /// 血量减少
    /// </summary>
    /// <param name="Damage_Object"></param>
    [Signal] public delegate void Health_ReduceEventHandler(Node Damage_Object);
    #endregion
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
    /// 当前血量
    /// </summary>
    [ExportGroup("status")]
    #region 生命组件
    [ExportSubgroup("Health_Module")]
    [Export] public int HP = 200;
    /// <summary>
    /// 最大血量
    /// </summary>
    [Export] public int Max_HP = 200;
    /// <summary>
    /// 最小血量
    /// </summary>
    [Export] public int Min_HP = 0;
    /// <summary>
    /// 已死亡
    /// </summary>
    [Export] public bool kill = false;
    /// <summary>
    /// 启用生命组件
    /// </summary>
    [Export] public bool Enable_Health = true;
    #endregion
    /// <summary>
    /// 启用状态
    /// </summary>
    #region 移动
    [ExportGroup("status")]
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
    /// 自动设置移动状态
    /// </summary>
    [Export] public bool auto_Move = true;
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
    [ExportGroup("status")] 
    [Export] public bool Enable = true;
    /// <summary>
    /// 伤害值
    /// </summary>
    [Export] public int Damage = 0;
    /// <summary>
    /// 已检测到的object
    /// </summary>
    [Export] public Godot.Collections.Array<Level.Object.LevelObject> Current_detection_object = new Godot.Collections.Array<Level.Object.LevelObject>(){};
    /// <summary>
    /// 检测阵营
    /// </summary>
    [Export]public Godot.Collections.Array<StringName> detection_Group = new Godot.Collections.Array<StringName>(){};
    /// <summary>
    /// 排除阵营
    /// </summary>
    [Export] public Godot.Collections.Array<StringName> Exclude_Group = new Godot.Collections.Array<StringName>(){"Projectile","Area"};
    /// <summary>
    /// 临时坐标
    /// </summary>
    float Temp_Position_Y = -1;
    internal Level_Master_Script level {get;set;} = null;

    public override void _ExitTree()
    {
        base._ExitTree();
        if (!Enable){return;}
        Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level").Remove_Lawn_Index(this,Lawn_Index);
    }
    public override async void _Ready() {
        base._Ready();
        if (Enable_Health == true)
        {
            HP = Max_HP;
        }
        if (!Enable){return;}
        Reset_Position();
        Area = GetNode<Godot.Area2D>("Area");
    }
    public void Object_Move(double delta)
    {
        switch (MoveType){
            case Move_Type.Linear_Motion:
                if (Move_Ing == true){
                    practical_Position += (Speed * new Vector2(Speed_Multiplication,Speed_Multiplication)) * new Vector2((float)delta,(float)delta);
                }
            break;
        }
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        Object_Move(delta);
        if (level == null)
        {
            Level_Master_Script Get_Level = Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level");
            if (Get_Level != null)
            {
                level = Get_Level;
            }
        }
        // 高度重定向
        if (level != null && AutoSet_Lawn_Index == true)
        {
            if (Temp_Position_Y != practical_Position.Y + position_Offset.Y)
            {
                Temp_Position_Y = practical_Position.Y + position_Offset.Y;
                if (Lawn_Index != -1){
                    level.Move_Lawn_Index(this,level.Get_LawnIndex(this));
                }
                else
                {
                    Lawn_Index = level.Get_LawnIndex(this);
                    level.Add_Lawn_Index(this,Lawn_Index);
                }
            }
        }
        SetPhysics_Position(delta);
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
    /// 移除空物体
    /// </summary>
    public void Remove_Null_Object()
    {
        if (!Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        foreach(Level.Object.LevelObject Body in Current_detection_object)
        {
            if (Body == null)
            {
                Current_detection_object.Remove(Body);
            }
            else
            {
                if (Body.Enable_Health == true)
                {
                    if (Body.kill == true)
                    {
                        Current_detection_object.Remove(Body);
                    }
                }
                else
                {
                    Current_detection_object.Remove(Body);
                }
                if (Body.Enable == false)
                {
                    Body.QueueFree();
                }
            }
        }
    }
    /// <summary>
    /// 添加物体
    /// </summary>
    public void Add_Object(Node2D node)
    {
        if (!Enable || kill)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        if (node is Level.Object.LevelObject && node != this)
        {
            bool Cheak = Game.Cheak.CheakGroup.Cheak_Object_Group(node,detection_Group,Exclude_Group);
            if (!Cheak){return;}
            Current_detection_object.Add((Level.Object.LevelObject)node);
            EmitSignal("Object_Join",node);
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
        if (node is Level.Object.LevelObject && node != this){
            Level.Object.LevelObject Body = (Level.Object.LevelObject)node;
            if (Current_detection_object.IndexOf(Body) != -1)
            {
                EmitSignal("Object_Exit",Body);
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
    public void Reduce_Health(int Reduce_Number,Node Damage_Object = null)
    {
        HP -= Reduce_Number;
        EmitSignal("Health_Reduce",Damage_Object);
    }
}
