using Godot;
using System;
using System.Threading.Tasks;
using DEBUG;
using System.Diagnostics;
namespace Level.Module;
public partial class ObjectPhysics : Node2D
{
    /// <summary>
    /// 当前草坪行数索引
    /// </summary>
    [ExportGroup("Index")]
    [Export] public int Lawn_Index = -1;
    /// <summary>
    /// 检测范围
    /// </summary>
    [Export] public Godot.Vector2I Cheak_Index_Scoop = new(0,0);
    [Export] public Godot.Vector2I Cheak_Lawn_Index_Scoop = new(0,0);
    /// <summary>
    /// 自动设置草坪行数索引
    /// </summary>
    [Export] public bool AutoSet_Lawn_Index = true;
    /// <summary>
    /// 检测
    /// </summary>
    /// <summary>
    /// 可检测高度等级
    /// </summary>
    [ExportGroup("Cheak")]
    [Export] public Godot.Vector2 Cheak_Height_Level_Scoop = new Vector2(-1,1);
    /// <summary>
    /// 当前高度等级
    /// </summary>
    [Export] public float Height_Level = 2;
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
    /// 可检测高度
    /// </summary>
    [Export] public Godot.Vector2 Cheak_Height_Scoop = new(-5,15);
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
    /// <summary>
    /// 临时坐标
    /// </summary>
    float Temp_Position_Y = -1;
    internal Level_Master_Script level {get;set;} = null;
   /// <summary>
   /// 重置物理坐标
   /// </summary>
    internal void Reset_Position()
    {
        practical_Position = GlobalPosition;
        Physics_Initialization = true;
    }
    /// <summary>
    /// 设置物理坐标
    /// </summary>
    /// <param name="delta"></param>
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
    /// <summary>
    /// 检测高度等级
    /// <para>
    /// 检测高度等级值 + 当前高度值 
    /// </para>
    /// </summary>
    /// <param name="physics"></param>
    /// <returns></returns>
    public bool Cheak_HeightLevel(Level.Module.ObjectPhysics physics)
    {
        if (physics == null){return false;}
        float Extra_Height_Level = 0;
        float Min_Heigit = Cheak_Height_Scoop.X + Height + Extra_Height;
        float Max_Height = Cheak_Height_Scoop.Y + Height + Extra_Height;
        if (Min_Heigit >= Max_Height)
        {
            float Temp = Min_Heigit;
            Min_Heigit = Max_Height;
            Max_Height = Temp;
        }
        if ((physics.Height + physics.Extra_Height) > Max_Height)
        {
            Extra_Height_Level = (physics.Height + physics.Extra_Height) / Max_Height;
        }
        float Max_Height_Level = Cheak_Height_Level_Scoop.Y + Height_Level;
        float Min_Height_Level = Cheak_Height_Level_Scoop.X + Height_Level;
        if (Min_Height_Level >= Max_Height_Level)
        {
            float temp = Min_Height_Level;
            Min_Height_Level = Max_Height_Level;
            Max_Height_Level = temp;
        }
        float Current_Height = physics.Height_Level + Extra_Height_Level;
        if (Current_Height <= Max_Height_Level && Current_Height >= Min_Height_Level)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检测高度是否可以判定
    /// </summary>
    /// <param name="physics">引用Level.Module.ObjectPhysics的物体</param>
    /// <returns></returns>
    public bool Cheak_Height(Level.Module.ObjectPhysics physics)
    {
        if (physics == null){return false;}
        float Min_Heigit = Cheak_Height_Scoop.X + Height + Extra_Height;
        float Max_Height = Cheak_Height_Scoop.Y + Height + Extra_Height;
        float Current_Height = physics.Height + physics.Extra_Height;
        if (Min_Heigit >= Max_Height)
        {
            float Temp = Min_Heigit;
            Min_Heigit = Max_Height;
            Max_Height = Temp;
        }
        if (Current_Height <= Max_Height && Current_Height >= Min_Heigit)
        {
            return true;
        }
        return false;

    }
    /// <summary>
    /// 检测草坪行
    /// </summary>
    /// <param name="CheakObject"></param>
    /// <returns></returns>
    public bool Cheak_Lawn_Index(Level.Object.LevelObject CheakObject)
    {
        if (CheakObject == null){return false;}
        int Object_ScoopX = CheakObject.Cheak_Index_Scoop.X;
        int Object_ScoopY = CheakObject.Cheak_Index_Scoop.Y;
        if (Object_ScoopY > Object_ScoopX)
        {
            int Temp = Object_ScoopY;
            Object_ScoopY = Object_ScoopX;
            Object_ScoopX = Temp;
        }
        int Min_Cheak_Index = Cheak_Lawn_Index_Scoop.X + Object_ScoopY + Lawn_Index;
        int Max_Cheak_Index = Cheak_Lawn_Index_Scoop.Y + Object_ScoopX + Lawn_Index;
        int CheakObject_Index = CheakObject.Lawn_Index;
        if (CheakObject_Index <= Max_Cheak_Index && CheakObject_Index >= Min_Cheak_Index)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 重新设置索引
    /// </summary>
    public void reset_Lawn_Index()
    {
        if (Lawn_Index != -1){
            level.Move_Lawn_Index(this,level.Get_LawnIndex(Position,position_Offset));
        }
        else
        {
            Lawn_Index = level.Get_LawnIndex(Position,position_Offset);
            level.Add_Lawn_Index(this,Lawn_Index);
        }
    }
    public void Get_Level()
    {
        if (level == null)
        {
            Level_Master_Script Get_Level = Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level");
            if (Get_Level != null)
            {
                level = Get_Level;
            }
        }
    }
    /// <summary>
    /// 草坪索引重定向
    /// </summary>
    public void ReSet_Index()
    {
        if (level == null){return;}
        // 高度重定向
        if (level != null && AutoSet_Lawn_Index == true)
        {
            if (Temp_Position_Y != practical_Position.Y + position_Offset.Y)
            {
                Temp_Position_Y = practical_Position.Y + position_Offset.Y;
                level.EmitSignal(Level_Master_Script.SignalName.Object_Change_Height,this);
                reset_Lawn_Index();
            }
        }
    }
}
