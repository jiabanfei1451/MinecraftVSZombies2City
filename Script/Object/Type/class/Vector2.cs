namespace SaveData.Class;
/// <summary>
/// 实例坐标存储
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public class Vector2()
{
    public float X {get;set;}
    public float Y {get;set;}
    public static Vector2 New(float X,float Y)
    {
        Vector2 d = new();
        d.X = X;
        d.Y = Y;
        return d;
    }
}
