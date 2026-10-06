namespace SaveData.Struct;
/// <summary>
/// 向量
/// </summary>
public struct Vector2i
{
    /// <summary>
    /// X向量
    /// </summary>
    public int X;
    /// <summary>
    /// Y向量
    /// </summary>
    public int Y;
    public static Vector2 New(float x,float y)
    {
        Vector2 T = new();
        T.X = x;
        T.Y = y;
        return T;
    }
}