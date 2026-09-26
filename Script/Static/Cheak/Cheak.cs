using System;

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
    public static bool Cheak_Data(Level.Object.LevelObject levelObject,Data.GlobalData CardData)
	{
        if (levelObject == null){return false;}
		if (CardData.Reliant_Tag.Count > 0)
		{
			if (Cheak.Cheak_Tag(CardData.Reliant_Tag, levelObject.Tags))
			{
				return true;
			}
		}
		if (CardData.Reliant_UUID.Count > 0)
		{
			if (CardData.Reliant_UUID.IndexOf(levelObject.Object_UUID) != -1)
			{
				return true;
			}
		}
		return false;
	}
}