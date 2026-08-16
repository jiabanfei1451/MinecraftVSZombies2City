using System;
using Godot;
namespace Game{
/// <summary>
/// 关卡数据存储
/// </summary>
static class Level_Script : Object
{
	public static bool Card_Drag = false;
	/// <summary>
	/// 器械能
	/// </summary>
	public static short Equipment_Capable = 200;
	public static Godot.ColorRect Lawn;
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
	}
}
}