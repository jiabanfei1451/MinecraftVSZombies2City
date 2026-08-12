using System;
using Godot;
namespace Game{
/// <summary>
/// 关卡数据存储
/// </summary>
static class Level_Script : Object
{
	public static bool Card_Drag = false;
	public static short Equipment_Capable = 91;
	public static Godot.ColorRect Lawn;
	public enum Calculation_Type
	{
		add = 0,
		Remove = 1,
		Selection = 2,
	}
	public static short Remove_Equipment_Capable(short Number)
	{
		Equipment_Capable -= Number;
		return Equipment_Capable;
	}
	public static short Add_Equipment_Capable(short Number)
	{
		Equipment_Capable += Number;
		return Equipment_Capable;
	}
	public static short Set_Equipment_Capable(short Number)
	{
		Equipment_Capable -= Number;
		return Equipment_Capable;
	}
	public static void initialize()
	{
		Equipment_Capable = 50;	
	}
}
}