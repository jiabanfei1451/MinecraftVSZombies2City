using System;
namespace Game{
/// <summary>
/// 关卡数据存储
/// </summary>
static class Level_Script : Object
{
	public static short Equipment_Capable = 20;
	public static Godot.ColorRect Lawn;
	public enum Calculation_Type
	{
		add = 0,
		Remove = 1,
		Selection = 2,
	}
	public static void initialize()
	{
		Equipment_Capable = 50;	
	}
}
}