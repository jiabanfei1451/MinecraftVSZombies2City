using System;
using Godot;
using Level;
namespace Game{
/// <summary>
/// 关卡数据存储
/// </summary>
static class Level_Script : Object
{
	public static Level_Master_Script Level_Object = null;
	public static bool Card_Drag = false;
	/// <summary>
	/// 器械能
	/// </summary>
	public static short Equipment_Capable = 200;
	/// <summary>
	/// 星之碎片数量
	/// </summary>
	public static byte Star_Fragment = 0;
	/// <summary>
	/// 音高
	/// </summary>
	public static byte audio_Scale = 0;
	public static Level.Object.LevelObject Selected_Object = null;
	/// <summary>
	/// 草坪
	/// </summary>
	public static Godot.ColorRect Lawn;
	/// <summary>
	/// 当前使用道具
	/// </summary>
	public static Prop Use_Prop = Prop.Not;
	/// <summary>
	/// 道具
	/// </summary>
	public enum Prop
	{
		/// <summary>
		/// 无
		/// </summary>
		Not = -1,
		/// <summary>
		/// 铁镐
		/// </summary>
		iron_pickaxe = 0,
		/// <summary>
		/// 星之碎片
		/// </summary>
		Star_Fragment = 1
	}
	public enum Calculation_Type
	{
		add = 0,
		Remove = 1,
		Selection = 2,
	}
	/// <summary>
	/// 初始化
	/// </summary>
	public static void initialize()
	{
		Equipment_Capable = 50;
		Star_Fragment = 0;
	}
}
}