using Godot;
using System;
using MY_Type;
public partial class Particie : Sprite2D
{
    [ExportGroup("Strength")]
    /// <summary>
    /// 最大上升力度
    /// </summary>
    [Export] public float Max_Rise_Strength = 20;
    /// <summary>
    /// 上升提升速度倍率
    /// </summary>
    [Export] public float Rise_Strength_Enhance_Speed = 1;
    /// <summary>
    /// 坐标设置倍率
    /// </summary>
    [Export] public float Set_Position_Multiplication = 1;
    /// <summary>
    /// 倍率
    /// </summary>
    [Export] public Godot.Vector2 Multiplication = new Godot.Vector2(1,1);
    /// <summary>
    /// 当前上升力度
    /// </summary>
    internal float Current_Rise_Strength = 0;
    /// <summary>
    /// 最大高度
    /// </summary>
    [Export] public Godot.Vector2 MaxHeight = new Vector2(15,30); 
    /// <summary>
    /// 当前最高高度
    /// </summary>
    internal float Current_MaxHeight = 0;
    /// <summary>
    /// 当前高度
    /// </summary>
    internal float Height = 0;
    /// <summary>
    /// 最大X坐标偏移
    /// </summary>
    [Export] public float MAX_PositionX_Offset = 10;
    /// <summary>
    /// 当前最大X偏移
    /// </summary>
    [Export] public float Current_Max_Position_X_offset = 0;
    /// <summary>
    /// 当前X偏移
    /// </summary>
    [Export] internal float Current_PositionX_Offset = 0;
    /// <summary>
    /// 最大弹跳次数
    /// </summary>
    [Export] public int Max_bounce_Number = 3;
    /// <summary>
    /// 当前弹跳次数
    /// </summary>
    [Export] internal int Current_Bounce_Number = 0;
    /// <summary>
    /// 每次弹跳时减少的倍率
    /// </summary>
    [Export] public float Reduced_bounce_force = 2;
    /// <summary>
    /// 反转上升力度
    /// </summary>
    [Export] internal bool Reverse = false;
    /// <summary>
    /// 等待销毁时长
    /// </summary>
    [Export] public float QueneFree_Time = 3;
    /// <summary>
    /// 最少Y向量
    /// </summary>
    [Export] public Godot.Vector2 Reset_Position = Vector2.Zero;
    [Export] public WhileMode while_Mode = WhileMode._Process; 
    public override async void _Ready() {
        base._Ready();
        Modulate = new Color(Game.Get.Random.NextFloat_32(1,0),Game.Get.Random.NextFloat_32(1,0),Game.Get.Random.NextFloat_32(1,0),1);
        Current_MaxHeight = Game.Get.Random.NextFloat_32(MaxHeight.Y,MaxHeight.X);
        Multiplication *= 1 + Game.Get.Random.NextFloat_32(0,1.5f);
        Current_Max_Position_X_offset = MAX_PositionX_Offset * (float)(new Random().NextDouble() - 0.5) * 2;
        GD.Print(Current_Max_Position_X_offset);
        Current_PositionX_Offset = Current_Max_Position_X_offset / Max_bounce_Number;
        Reset_Position.Y = Position.Y;
        Reset_Position.X = Position.X;
        await ToSignal(GetTree().CreateTimer(Game.Get.Random.NextFloat_32(QueneFree_Time - 1,QueneFree_Time)),SceneTreeTimer.SignalName.Timeout);
        QueueFree();
    }
    public override void _Process(double delta) {
        base._Process(delta);
        if (while_Mode != WhileMode._Process){return;}
        huh(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (while_Mode != WhileMode._PhysicsProcess){return;}
        huh(delta);
    }

    public void huh(double delta)
    {
        if (delta > 0.3){return;}
        if(Current_Bounce_Number >= Max_bounce_Number)
        {
            return;
        }
        Godot.Vector2 Temp_Position = Position;
        if (Reverse){
            if (Current_Rise_Strength < Max_Rise_Strength)
            {
                Current_Rise_Strength += Max_Rise_Strength * Rise_Strength_Enhance_Speed * (float)delta;
            }
        }
        else
        {
            if (Current_Rise_Strength > -Max_Rise_Strength)
            {
                Current_Rise_Strength -= Max_Rise_Strength  * Rise_Strength_Enhance_Speed * (float)delta;
            }
        }
        Height -= Current_Rise_Strength * Rise_Strength_Enhance_Speed * Multiplication.Y * (float)delta;
        Temp_Position.X += (Current_PositionX_Offset * Set_Position_Multiplication * Multiplication.X) * (float)delta;
        Current_PositionX_Offset -= Current_PositionX_Offset * Multiplication.X *  (float)delta;
        Temp_Position.Y = Reset_Position.Y + -Height + Current_Rise_Strength * Set_Position_Multiplication * Multiplication.Y * (float)delta;
        if (Height >= Current_MaxHeight && Current_MaxHeight >0)
        {
            Reverse = true;
        }else if (Height <= 0 && Reverse == true && Current_Bounce_Number < Max_bounce_Number)
        {
            Current_Bounce_Number += 1;
            Current_PositionX_Offset = Current_Max_Position_X_offset / Current_Bounce_Number;
            Current_Rise_Strength = -Current_Rise_Strength / (Current_Bounce_Number + 1);
            Current_MaxHeight /= Reduced_bounce_force;
            Reverse = false;
        }
        Position = Temp_Position;
        if (Position.Y > Reset_Position.Y)
        {
            Position = new Vector2(Temp_Position.X,Reset_Position.Y);
        }
    }
}
