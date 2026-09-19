using System;
using Godot;

namespace Data;

	/// <summary>
	/// 返回数据
	/// </summary>
	/// <param name="Scene">场景</param>
	/// <param name="sonsume">消耗</param>
	/// <param name="CD">冷却</param>
	/// <param name="First_Time_RemoveCD">游戏开始时减少冷却</param>
	/// <param name="Scale">大小</param>
	/// <param name="Offset">偏移</param>
	/// <param name="Mouse_Offset">光标坐标偏移</param>
	/// <param name="Map_Offset">地图坐标偏移</param>
	/// <param name="Map_Scale">地图大小</param>
public struct GlobalData(
    PackedScene @Scene,
    short @sonsume,
    float @CD,
    float @First_Time_RemoveCD,
    Godot.Vector2 @Scale,
    Godot.Vector2 @Offset,
    Godot.Vector2 @Mouse_Offset,
    Godot.Vector2 @Map_Offset,
    Godot.Vector2 @Map_Scale,
    MVZ2.Type.ObjectType @Object_Type = MVZ2.Type.ObjectType.Normal
){

    /// <summary>
    /// 物体场景
    /// </summary>
    public PackedScene Scene = @Scene;
    /// <summary>
    /// 消耗
    /// </summary>
    public short Sonsume = @sonsume;
    /// <summary>
    /// 冷却时间
    /// </summary>
    public float CD = @CD;
    /// <summary>
    /// 首次冷却时减少冷却
    /// </summary>
    public float First_Time_RemoveCD = @First_Time_RemoveCD;
    /// <summary>
    /// 材质大小
    /// </summary>
    public Godot.Vector2 Scale = @Scale;
    /// <summary>
    /// 材质偏移
    /// </summary>
    public Godot.Vector2 Offset = @Offset;
    /// <summary>
    /// 鼠标偏移
    /// </summary>
    public Godot.Vector2 Mouse_Offset = @Mouse_Offset;
    /// <summary>
    /// 地图偏移
    /// </summary>
    public Godot.Vector2 Map_Offset = @Map_Offset;
    /// <summary>
    /// 地图缩放
    /// </summary>
    public Godot.Vector2 Map_Scale = @Map_Scale;
    /// <summary>
    /// 物体类型
    /// </summary>
    public MVZ2.Type.ObjectType Object_Type = @Object_Type;
    /// <summary>
    /// 这是空值
    /// </summary>
    public bool is_Null = false;
    public static GlobalData Zero()
    {
        var s = new GlobalData();
        s.is_Null = true;
        return s;
    }
    /// <summary>
    /// 获取类型
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static MVZ2.Type.ObjectType GetObjectType(int Index)
    {
        MVZ2.Type.ObjectType[] types = new MVZ2.Type.ObjectType[]{
        MVZ2.Type.ObjectType.Null,
        MVZ2.Type.ObjectType.Normal};
        if (Index < types.Length)
        {
            return types[Index];
        }
        else
        {
            return types[types.Length - 1];
        }
    }
}