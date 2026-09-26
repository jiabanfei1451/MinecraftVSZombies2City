using Godot;
using Level;
using System.Collections.Generic;
using Level.Static;
namespace SaveData;
/// <summary>
/// 生成数据
/// </summary>
public class SaveSummanddata
{
    /// <summary>
    /// 种子
    /// </summary>
    public int Seed {get;set;}
    /// <summary>
    /// 关卡节点脚本
    /// </summary>
    public Level_Master_Script Level_Object {get;set;}
    /// <summary>
    /// 波次生成怪物ID
    /// </summary>
    #region 波次数据
    public List<List<MVZ2_City.Type.ID>> Object_ID {get;set;}
    /// <summary>
    /// 生成数量
    /// </summary>
    public Godot.Collections.Array<int> Summand_Number {get;set;}
    /// <summary>
    /// 等待怪物全体死亡快速进行下一波
    /// </summary>
    public Godot.Collections.Array<bool> Await_Mouster {get;set;}
    /// <summary>
    /// 等待下一波时间
    /// </summary>.
    public Godot.Collections.Array<float> Await_Next_Time {get;set;} 
    #endregion
    #region 检测
    /// <summary>
    /// 指定物体
    /// </summary>
    public List<MVZ2_City.Type.ID> Specify_Monster_Summand {get;set;}
    /// <summary>
    /// 指定生成坐标
    /// </summary>
    public Godot.Collections.Array<Godot.Collections.Array<Godot.Vector2>> Specify_Position {get;set;}
    #endregion
    /// <summary>
    /// 已生成的怪物
    /// </summary>
    public Godot.Collections.Array<Level.Object.LevelObject> Generated_Object {get;set;}
    /// <summary>
    /// 完成检测ID
    /// </summary>
    public List<MVZ2_City.Type.ID> ENDCheck_ID {get;set;}
    /// <summary>
    /// 完成检测后存在状态
    /// </summary>
    public Godot.Collections.Array<bool> ENDCheck_bool {get;set;}
    /// <summary>
    /// 生成中
    /// </summary>
    public bool Summand_Ing {get;set;}
    /// <summary>
    /// 当前波次
    /// </summary>
    public int Current_Wave {get;set;}
    /// <summary>
    /// 随机生成器
    /// </summary>
    public Godot.RandomNumberGenerator random {get;set;}
    /// <summary>
    /// 运算逻辑
    /// </summary>
    /// <param name="delta"></param>
    public MVZ2_City.Type.WhileMode While_Mode {get;set;}
    /// <summary>
    /// 循环状态 当while_Mode = While时才会改变
    /// </summary>
    public bool While_ing {get;set;}
    /// <summary>
    /// 生成倒计时
    /// </summary>
    public float Start_Timer {get;set;}
    public short Summand_Seed {get;set;} = 1;
}