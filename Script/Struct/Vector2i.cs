namespace SaveData.Struct;
/// <summary>
/// 向量
/// </summary>
public struct Vector2i
{
    /// <summary>
    /// X向量
    /// </summary>
    public int X {get;set;}
    /// <summary>
    /// Y向量
    /// </summary>
    public int Y {get;set;}
    public static Vector2i New(int x,int y)
    {
        Vector2i T = new();
        T.X = x;
        T.Y = y;
        return T;
    }
    public static Vector2i New(Godot.Vector2I vector)
    {
        Vector2i T = new();
        T.X = vector.X;
        T.Y = vector.Y;
        return T;
    }
}