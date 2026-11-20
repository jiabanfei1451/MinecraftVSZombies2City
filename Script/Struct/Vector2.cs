namespace SaveData.Struct;
/// <summary>
/// 向量
/// </summary>
public struct Vector2
{
    /// <summary>
    /// X向量
    /// </summary>
    public float X {get;set;}
    /// <summary>
    /// Y向量
    /// </summary>
    public float Y {get;set;}
    public static Vector2 New(float x,float y)
    {
        Vector2 T = new();
        T.X = x;
        T.Y = y;
        return T;
    }
    public static Vector2 New(Godot.Vector2 vector)
    {
        Vector2 T = new();
        T.X = vector.X;
        T.Y = vector.Y;
        return T;
    }
    public static Vector2 New(System.Numerics.Vector2 vector)
    {
        Vector2 T = new();
        T.X = vector.X;
        T.Y = vector.Y;
        return T;
    }
}