using Game;
using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MVZ2_City.Type;
using Data;
using Game.Cheak;
using Data.Level;

namespace Level.Static;
/// <summary>
/// 生成组件
/// </summary>
public static class Summand
{
    /// <summary>
    /// 生成数据
    /// </summary>
    public static Data.Level.Summand_Data Data = null;
    /// <summary>
    /// 循环模式
    /// </summary>
    public static MVZ2_City.Type.WhileMode whileMode = WhileMode._While;
    /// <summary>
    /// 循环中
    /// </summary>
    public static bool While_Ing = true;
    /// <summary>
    /// 生成物体
    /// </summary>
    /// <param name="Number"></param>
    public static async void Summand_Object(int Number)
    {
        // 条件判断
        if (Cheak.is_null<Summand_Data>(Summand.Data)){return;}
        if (Cheak.is_null<List<ID>>(Summand.Data.Summand_List)){return;}
        if (Data.Summand_List.Count <= 0){return;}
        if (Cheak.is_null<Level_Master_Script>(Level_Script.Level_Object)){return;}
        // 获取生成坐标
        Vector2 Summand_Position = Level_Script.Level_Object.Lawn_Spawn_Position;
        Vector2 Summand_Offset = Level_Script.Level_Object.Lawn_Spawn_Offect;
        Random Ran = new Random();
        if (Data.Summand_ing == false)
        {
            Data.Max_Summand_Number = Number;
            Data.Current_Summand_Number = 0;
            Data.Summand_ing = true;
        }
        while (Data.Current_Summand_Number < Data.Max_Summand_Number)
        {
            int Temp_Count = Data.Summand_List.Count - 1;
            ID Random_Get = Data.Summand_List[Ran.Next(0,Temp_Count)];
            PackedScene GetPacked = MVZ2_City.Object_List.Get_Packed(Random_Get);
            GlobalData get_carddata = Card_Data.Get_CardData(GetPacked);
            
            Vector2 Summand_Scale = new(1,1);
            Vector2 Summanc_Offset = new(40,48);

            if (get_carddata.is_Null == false)
            {
                Summand_Scale = get_carddata.Map_Scale;
                Summanc_Offset = get_carddata.Map_Offset;
            }
            // 计算生成后坐标
            Vector2 Object_Summand_Offset = new Vector2(0,Data.random.RandiRange(Data.YPosition_Offset.X,Data.YPosition_Offset.Y) * Summand_Offset.Y);
            Vector2 Data_Summand_Position = new Vector2(Data.Summand_Position.X,Data.Summand_Position.Y);
            // 求和
            Vector2 Object_Summand_Position = Summand_Position + Data_Summand_Position + Object_Summand_Offset + Summanc_Offset;
            // 开始实例化
            Level.Object.LevelObject Instantiate = get_carddata.Scene.InstantiateOrNull<Level.Object.LevelObject>();
            
            if (!Cheak.is_null<Level.Object.LevelObject>(Instantiate))
            {
                Node2D Get_Node = null;
                String layer = Instantiate.Default_generation_layer;
                if (layer != "")
                {
                    Get_Node = Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>(Instantiate.Default_generation_layer);
                }
                if (Cheak.is_null<Node2D>(Get_Node))
                {
                    if (Instantiate.IsInGroup("Monster"))
                    {
                        Get_Node = Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Monster");
                    }
                    else
                    {
                        Get_Node = Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Equipment");
                    }
                }
                if (Data.delay_Summand == false){
                    Instantiate.Physics_Enable = false;
                    Instantiate.Position = Object_Summand_Position;
                    Get_Node.AddChild(Instantiate);
                    Instantiate.Physics_Enable = true;
                }
                else
                {
                    Instantiate.Physics_Enable = false;
                    Get_Node.AddChild(Instantiate);
                    await Task.Delay((int)(1000 * Data.delay_SummandTime / 2));
                    Instantiate.Position = Object_Summand_Position;
                    Instantiate.practical_Position = Instantiate.GlobalPosition;
                    Instantiate.Physics_Enable = true;
                }
                Data.Present_Object.Add(Instantiate);
                Data.Current_Summand_Number += 1;
            }
        }
        Data.Summand_ing = false;
    }
    /// <summary>
    /// 条件判断生成
    /// </summary>
    public static void Cheak_Summand()
    {
        if (Data.Present_Object.Count <= 0 || Data.Await_Next_Wave_Timer <= 0){
            if (Data.Current_Summand_Value <= Data.Max_Summand_Value){
                Data.Get_Wave(Data.Current_Summand_Value);
                Summand_Object(Data.Max_Summand_Number);
                Data.Current_Summand_Number = Data.Max_Summand_Number;
                Data.Current_Summand_Value += 1;
            }
        }
    }
    /// <summary>
    /// 用于循环逻辑
    /// </summary>
    public static void While_s()
    {
        if (While_Ing == false){return;}
        Cheak_Summand();
        if (Cheak_Tag())
        {
            GD.Print("你过关");
            While_Ing = false;
        }
    }
    /// <summary>
    /// 标签条件判断
    /// </summary>
    public static bool Cheak_Tag()
    {
        if (Data.Current_WaveTag == Enum.WaveTag.Normal)
        {
            if (Data.Is_Final_Wave == true && Data.Summand_ing == false)
            {
                if (Data.Present_Object.Count <= 0)
                {
                    return true;
                }
            }
        }else if(Data.Current_WaveTag == Enum.WaveTag.Boss)
        {
            if (Data.Allow_victory == true)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 开始生成
    /// </summary>
    public static async void Start_Summand()
    {
        While_Ing = true;
        while (whileMode == WhileMode._While && While_Ing == true){
            While_s();
            await Task.Delay(20);
        }
    }
}