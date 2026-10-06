using System;
namespace MVZ2_City.Type;
public class ID()
{
    /// <summary>
    /// 对象名称
    /// </summary>
    public String Object_Name_ID {get;set;}
    /// <summary>
    /// 中文名称
    /// </summary>
    public String CH_Name {get;set;}
    /// <summary>
    /// 物体ID
    /// </summary>
    public int Object_ID {get;set;}
    public IndexMode Index_Mode {get;set;}  = IndexMode.index;
    public bool this_null {get;set;}
}