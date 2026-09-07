using System;
using Godot;
namespace MVZ2_City.Type;
public class ID(int @Object_ID,String @Object_NameID,String @CH_Name = "null")
{
    /// <summary>
    /// 对象名称
    /// </summary>
    public String Object_Name_ID = @Object_NameID;
    /// <summary>
    /// 中文名称
    /// </summary>
    public String CH_Name = @CH_Name;
    /// <summary>
    /// 物体ID
    /// </summary>
    public int Object_ID = @Object_ID;
    public IndexMode Index_Mode = IndexMode.index;
    public enum IndexMode : sbyte
    {
        index = 0,
        Name = 1,
        CH_Name = 2
    }
}