using Data;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game.Static;
public static class PlayerData : Object
{
    /// <summary>
    /// 最大卡槽数量 实际值是当前值 + 1
    /// </summary>
    public static int Card_Quantity = 5;
    /// <summary>
    /// 难度
    /// </summary>
    public static Game.Enum.Difficult Difficult = Enum.Difficult.Normal;
    /// <summary>
    /// 由JSON存储的文本数据
    /// </summary>
    public static Godot.Collections.Array<Godot.Collections.Array> LoadJsonData = new Godot.Collections.Array<Godot.Collections.Array>()
    {
        /// NBTID
        new Godot.Collections.Array(){"CH:Name","CH:Level","EN:Name","EN:Level"},
        // JSONResource
        new Godot.Collections.Array(){
            ResourceLoader.Load("res://2/Text/Zh_CN/Level_Name.json"),
            ResourceLoader.Load("res://2/Text/Zh_CN/Level_Text.json"),
            ResourceLoader.Load("res://2/Text/EN_US/Level_Name.json"),
            ResourceLoader.Load("res://2/Text/EN_US/Level_Text.json")},
    };
    /// <summary>
    /// 玩家数据
    /// </summary>
    public static Godot.Collections.Dictionary<String,Variant> Player_Data = new Godot.Collections.Dictionary<string, Variant>();
    /// <summary>
    /// 临时数据名称
    /// </summary>
    internal static List<String> Temp_Data_Key = new();
    /// <summary>
    /// 临时数据
    /// </summary>
    internal static List<Variant> Temp_Data = new List<Variant>();
    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Data"></param>
    public static void Add_Data(String Key,Variant Data)
    {
        Temp_Data.Add(Data);
        Temp_Data_Key.Add(Key);
        While_Add();
    }
    /// <summary>
    /// 循环添加
    /// </summary>
    internal static void While_Add()
    {
        for (int List_Index = 0;List_Index < Temp_Data.Count; List_Index++)
        {
            Player_Data[Temp_Data_Key[0]] = Temp_Data[0];
            Temp_Data.RemoveAt(0);
            Temp_Data_Key.RemoveAt(0);
        }
        if (Temp_Data.Count > 0)
        {
            While_Add();
        }
    }
}