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
    /// <summary>
    /// 启动物理
    /// </summary>
    public async void Physics_ON()
    {
        if (!Physics_Enable)
        {
            Info.ERROR(Info.ERROR_Info.Invalid_method);
            return;
        }
        Physics_Enable = true;
        while (Physics_Enable && Physics_Enable)
        {
            await Task.Delay((int)Math.Round(1000 / Performance.GetMonitor(Performance.Monitor.TimeFps),0));
        }
    }
    public void Reset_Position()
    {
        practical_Position = Position;
    }
    public void SetPhysics_Position(double delta) {
        Position = practical_Position + new Vector2(0,Height);
        switch (MoveType){
            case Move_Type.Linear_Motion:
                if (Move_Ing == true){
                    practical_Position += (Speed * new Vector2(Speed_Multiplication,Speed_Multiplication)) * new Vector2((float)delta,(float)delta);
                }
            break;
        }
    }
}
