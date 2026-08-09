using Godot;
using System;

namespace Game{
public static class PlayerData : Object
{
    /// <summary>
    /// 最大卡槽数量
    /// </summary>
    public static int Card_Quantity = 5;
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
}
}