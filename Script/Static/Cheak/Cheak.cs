using System;
using System.Collections.Generic;

namespace Game.Cheak;

public static class Cheak
{
    public static bool Cheak_Tag(Godot.Collections.Array<String> CheakTag,Godot.Collections.Array<String> Object_Tag)
    {
        int Tag_Number = CheakTag.Count;
        int Detected_Tag_Number = 0;
        foreach (String on_Tag in CheakTag)
        {
            foreach (String Cheak_Tag in Object_Tag)
            {
                if (Cheak_Tag.ToLower() == "all")
                {
                    Detected_Tag_Number += 99990;
                }
                if (Cheak_Tag == on_Tag)
                {
                    Detected_Tag_Number += 1;
                    break;
                }
            }
        }
        if (Detected_Tag_Number >= Tag_Number)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检测数据
    /// </summary>
    /// <param name="levelObject"></param>
    /// <param name="CardData"></param>
    /// <returns></returns>
    public static bool Cheak_Data(Level.Object.LevelObject levelObject,Level.Module.ObjectPhysics ObjectData)
	{
        if (is_null<Level.Object.LevelObject>(levelObject)){return false;}
		if (ObjectData.Reliant_Tag.Count > 0)
		{
			if (Cheak.Cheak_Tag(ObjectData.Reliant_Tag, levelObject.Tags))
			{
				return true;
			}
		}
		if (ObjectData.Reliant_UUID.Count > 0)
		{
			if (ObjectData.Reliant_UUID.IndexOf(levelObject.Object_UUID) != -1)
			{
				return true;
			}
		}
		return false;
	}
    /// <summary>
    /// 高性能检测空值方案
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Value"></param>
    /// <returns></returns>
    public static bool is_null<T>(T Value)
    {
        return EqualityComparer<T>.Default.Equals(Value,default(T));
    }
    public static bool Cheak_Null<T>(T Value)
    {
        try{
            Value.ToString();
        }
        catch
        {
            return true;
        }finally{}
        return false;
    }
}