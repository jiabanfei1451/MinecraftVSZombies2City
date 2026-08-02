using Godot;
using Level;
using System;
using System.Text;
namespace Game;
/// <summary>
/// 关卡数据存储
/// </summary>
static class Level_Script : Object
{
	public static Godot.ColorRect Lawn;
	public enum Calculation_Type
	{
		add = 0,
		Remove = 1,
		Selection = 2,
	}
	/// <summary>器械能</summary>
	/// <summary>
	/// 获取器械能
	/// </summary>
	/// <param name="Show_Label_Equipment_Capavle"></param>
	/// <returns></returns>
	public static int Get_Equipment_Capable(Godot.Label Show_Label_Equipment_Capavle = null)
	{
		if (Show_Label_Equipment_Capavle == null){return 0;}
		int Equipment_capable = int.Parse(Show_Label_Equipment_Capavle.Text);
		
		return Equipment_capable;
	}
	/// <summary>
	/// 设定器械能
	/// </summary>
	/// <param name="Show_Label_Equipment_Capavle"></param>
	/// <param name="Value"></param>
	/// <param name="calculation"></param>
	public static void Sel_Equipment_Capable(Godot.Label Show_Label_Equipment_Capavle = null,int Value = 0,Calculation_Type calculation = Calculation_Type.Selection)
	{
		if (Show_Label_Equipment_Capavle == null){return;}
		int Equipment_Capable = Get_Equipment_Capable(Show_Label_Equipment_Capavle);
		switch (calculation)
		{
			case Calculation_Type.add:
				Equipment_Capable += Value;
				break;
			case Calculation_Type.Remove:
				Equipment_Capable -= Value;
				break;
			case Calculation_Type.Selection:
				Equipment_Capable = Value;
				break;
		}
		Show_Label_Equipment_Capavle.Text = " " + Equipment_Capable .ToString() + " ";
		
	}

}
