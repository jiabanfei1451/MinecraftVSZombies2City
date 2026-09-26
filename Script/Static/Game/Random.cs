using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Game.Get;
public static class Random
{
    public static float NextFloat_32(float MaxVariant,float MinVariant)
    {
        System.Random ran = new System.Random();
        float variant = (float)(ran.NextDouble() * (MaxVariant - MinVariant)) + MinVariant;
        return variant;
    }
    public static double NextFloat_64(double MaxVariant,double MinVariant)
    {
        System.Random ran = new System.Random();
        double variant = (ran.NextDouble() * (MaxVariant - MinVariant)) + MinVariant;
        return variant;
    }
    public static String Random_String(int number)
    {
        String Ran = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        byte[] Ranbytes = Encoding.UTF8.GetBytes(Ran);
        List<byte> Back = new List<byte>();
        for (int i = 0;i < number; i++)
        {
            Back.Add(Ranbytes[new System.Random().Next(0,Ranbytes.Length - 1)]);
        }
        var Back_String = Encoding.UTF8.GetString(Back.ToArray<byte>());
        return Back_String;
    }
}
