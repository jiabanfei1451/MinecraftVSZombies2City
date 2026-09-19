using Game;
using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MVZ2_City.Type;
using DEBUG;

namespace Level.Static;
/// <summary>
/// 生成组件
/// </summary>
public static class Summand
{
#region 变量
    /// <summary>
    /// 种子
    /// </summary>
    public static ulong Seed = 1;
    /// <summary>
    /// 树节点
    /// </summary>
    public static SceneTree Tree;
    /// <summary>
    /// 关卡节点脚本
    /// </summary>
    public static Level_Master_Script Level_Object = null;
    /// <summary>
    /// 波次生成怪物ID
    /// </summary>
    #region 波次数据
    public static List<List<MVZ2_City.Type.ID>> Object_ID = new(){};
    /// <summary>
    /// 生成数量
    /// </summary>
    public static Godot.Collections.Array<int> Summand_Number = new Godot.Collections.Array<int>(){};
    /// <summary>
    /// 等待怪物全体死亡快速进行下一波
    /// </summary>
    public static Godot.Collections.Array<bool> Await_Mouster = new(){};
    /// <summary>
    /// 等待下一波时间
    /// </summary>.
    public static Godot.Collections.Array<float> Await_Next_Time = new(){}; 
    #endregion
    #region 检测
    /// <summary>
    /// 指定物体
    /// </summary>
    public static List<MVZ2_City.Type.ID> Specify_Monster_Summand = new(){};
    /// <summary>
    /// 指定生成坐标
    /// </summary>
    public static Godot.Collections.Array<Godot.Collections.Array<Godot.Vector2>> Specify_Position = new(){};
    #endregion
    /// <summary>
    /// 已生成的怪物
    /// </summary>
    public static Godot.Collections.Array<Level.Object.LevelObject> Generated_Object = new();
    /// <summary>
    /// 完成检测ID
    /// </summary>
    public static List<MVZ2_City.Type.ID> ENDCheck_ID = new();
    /// <summary>
    /// 完成检测后存在状态
    /// </summary>
    public static Godot.Collections.Array<bool> ENDCheck_bool = new();
    /// <summary>
    /// 生成中
    /// </summary>
    public static bool Summand_Ing = false;
    /// <summary>
    /// 当前波次
    /// </summary>
    public static int Current_Wave = -1;
    /// <summary>
    /// 随机生成器
    /// </summary>
    public static Godot.RandomNumberGenerator random = new();
    /// <summary>
    /// 运算逻辑
    /// </summary>
    /// <param name="delta"></param>
    public static WhileMode While_Mode = WhileMode.Process;
    /// <summary>
    /// 循环状态 当while_Mode = While时才会改变
    /// </summary>
    static bool While_ing = false;
    /// <summary>
    /// 生成倒计时
    /// </summary>
    public static float Start_Timer = 0;
    public static void _Ready()
    {
        Seed = (ulong)new Random().Next(0,210000000);
        random.Seed = Seed;
    }
    public enum WhileMode
    {
        While = 0,
        Process = 1
    }
    public static bool calculate(double delta)
    {
        if (Summand_Data.Finale_Wave == false){
            Summand_Data.Await_Next_Time -= delta;
        }
        if (Generated_Object.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
#endregion
    /// <summary>
    /// 一种循环模式
    /// </summary>
    /// <param name="delta"></param>
    public static void _Process(double delta)
    {
        if (!Summand_Ing){return;}
        if (While_Mode != WhileMode.Process){return;}
        calculate(delta);
    }
    /// <summary>
    /// 运行循环运算逻辑
    /// </summary>
    public static async void While_Start()
    {
        Summand_Data.Initialization();
        if (While_Mode != WhileMode.While){return;}
        Summand_Ing = true;
        While_ing = true;
        while (Current_Wave <= Object_ID.Count && Summand_Ing && While_ing){
            await Task.Delay(1000 / 60);
            Cheak_Next_Wave(1d / 60d * Engine.TimeScale);
        }
    }
    /// <summary>
    /// 检测并进行下一波
    /// </summary>
    public static void Cheak_Next_Wave(double delta)
    {
        if (Start_Timer > 0)
        {
            Start_Timer -= (float)delta;
            return;
        }
        if (calculate(delta) == true || ((Summand_Data.Await_AllMonster_Kill == false || Summand_Data.Await_Next_Time < 0) && Summand_Data.Finale_Wave == false)){
            // 判定数组是否存在
            if (Current_Wave + 1 < Object_ID.Count){
                int Index = Current_Wave + 1;
                
                var array = Object_ID[Index];
                if (Index >= Object_ID.Count - 1)
                {
                    Summand_Data.Finale_Wave = true;
                }
                Level.Static.Summand_Data.Await_Next_Time = Await_Next_Time[Index];
                Summand_Data.Await_AllMonster_Kill = Await_Mouster[Index];

                for(int i = 0;i < Summand_Number[Current_Wave + 1]; i++)
                {
                    Summand_Object(array[new Random().Next(0,array.Count - 1)]);
                }
                Current_Wave += 1;
            }
            else if (!Summand_Data.Boss_Wave)
            {
                GD.Print("你过关!");
                While_ing = false;
            }
        }
    }
    public static void While_End()
    {
        While_ing = false;
    }
    /// <summary>
    /// 清空数组
    /// </summary>
    public static void claer_Array()
    {
        Await_Next_Time.Clear();
        Object_ID.Clear();
        Summand_Number.Clear();
        ENDCheck_ID.Clear();
        ENDCheck_bool.Clear();
        Current_Wave = -1;
        Start_Timer = 0;
    }
    /// <summary>
    /// 添加波次
    /// </summary>
    /// <param name="SummandID">生成物体ID</param>
    /// <param name="SummandNumber">当前波次生成数量</param>
    /// <param name="AwaitNextTime">生成后等待一段时间进行下一波</param>
    /// <param name="AwaitMouster">怪物全部死亡时是否快速进行下一波</param>
    /// <returns></returns>
    public static void Add_wave(
        List<MVZ2_City.Type.ID> SummandID,int SummandNumber,
        float AwaitNextTime,bool AwaitMouster)
    {
        Object_ID.Add(SummandID);
        Summand_Number.Add(SummandNumber);
        Await_Next_Time.Add(AwaitNextTime);
        Await_Mouster.Add(AwaitMouster);
    }
    /// <summary>
    /// 添加指定物体
    /// </summary>
    /// <param name="ID_Object">ID物体</param>
    /// <param name="SummandPosition">生成坐标数组</param>
    public static void Add_Specify_Monster(MVZ2_City.Type.ID ID_Object,Godot.Collections.Array<Vector2> SummandPosition)
    {
        Specify_Monster_Summand.Add(ID_Object);
        Specify_Position.Add(SummandPosition);
    }
    /// <summary>
    /// 清空检测
    /// </summary>
    public static void Clear_Check()
    {
        ENDCheck_ID.Clear();
        ENDCheck_bool.Clear();
    }
    public static Data.GlobalData Get_data(MVZ2_City.Type.ID Object_ID)
    {
        Game.Card_Data card_Data = Game.Get_GlobalNode.Get_Card_Data(Tree);
        
        MVZ2_City.Object_List object_List = Game.Get_GlobalNode.object_List;
        Data.GlobalData data = card_Data.Get_CardData(object_List.Get_Packed(Object_ID));
        return data;
    }
    /// <summary>
    /// 生成物体
    /// </summary>
    /// <param name="Object_ID"></param>
    public static void Summand_Object(MVZ2_City.Type.ID Object_ID)
    {
        Data.GlobalData data = Get_data(Object_ID);
        if (Tree == null){return;}
        //检测
        bool check = Check_ID(Object_ID);
        
        int Lawn_Index = random.RandiRange(0,Level_Object.Lawn_Array.Count - 1);
        Node2D MonsterNode = Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Monster");
        Level.Object.LevelObject levelObject = data.Scene.Instantiate<Level.Object.LevelObject>();
        levelObject.Position = Level_Object.Lawn_Spawn_Position + data.Map_Offset + new Vector2(Level_Object.Lawn_Spawn_Offect.X * 9,Level_Object.Lawn_Spawn_Offect.X * Lawn_Index);
        levelObject.Scale = data.Map_Scale;
        if (!check)
        {
            
        }
        Generated_Object.Add(levelObject);
        MonsterNode.AddChild(levelObject);
        GD.Print(Generated_Object);
    }
    /// <summary>
    /// 这是一个信号函数，默认是自动连接的
    /// </summary>
    /// <param name="kill_Object"></param>
    public static void Monster_Kill(Level.Object.LevelObject kill_Object)
    {
        Generated_Object.Remove(kill_Object);
        CheakObject();
    }
    /// <summary>
    /// 检测并排除空怪物
    /// </summary>
    public static void CheakObject()
    {
        Godot.Collections.Array<Level.Object.LevelObject> Temp_Objects = new(){};
        foreach(Level.Object.LevelObject @object in Generated_Object)
        {
            if (@object != null)
            {
                Temp_Objects.Add(@object);
            }
        }
        Generated_Object = Temp_Objects;
    }
    /// <summary>
    /// 检测ID
    /// </summary>
    /// <param name="Object_ID"></param>
    /// <returns></returns>
    public static bool Check_ID(MVZ2_City.Type.ID Object_ID)
    {
        int index = ENDCheck_ID.IndexOf(Object_ID);
        if (index != -1)
        {
            return ENDCheck_bool[index];
        }
        foreach(var s in Specify_Monster_Summand)
        {
            if (Object_ID.Index_Mode == ID.IndexMode.Name)
            {
                if (Object_ID.Object_Name_ID == s.Object_Name_ID)
                {
                    ENDCheck_ID.Add(Object_ID);
                    ENDCheck_bool.Add(true);
                    return true;
                }
            }
            else if(Object_ID.Index_Mode == ID.IndexMode.CH_Name)
            {
                if (Object_ID.CH_Name == s.CH_Name)
                {
                    ENDCheck_ID.Add(Object_ID);
                    ENDCheck_bool.Add(true);
                    return true;
                }
            }
            else
            {
                if (Object_ID.Object_ID == s.Object_ID)
                {
                    ENDCheck_ID.Add(Object_ID);
                    ENDCheck_bool.Add(true);
                    return true;
                }
            }
        }
        ENDCheck_ID.Add(Object_ID);
        ENDCheck_bool.Add(false);
        return false;
    }
}