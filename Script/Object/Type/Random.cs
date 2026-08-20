using System;
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
}
