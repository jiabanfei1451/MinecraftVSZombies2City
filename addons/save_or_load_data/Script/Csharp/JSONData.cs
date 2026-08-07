using Godot;
using System;
using System.IO;
using System.Text.Json;
namespace Data;
[GlobalClass]
public partial class JSONData : Data_For_CSharp
{
	public override void _Ready() {
		base._Ready();
		Json_Load_Res();
		Test.Maid Test = new Test.Maid();
		Test.Saves();
		Test.Loads();
	}
	public Json Json_Load_Res()
	{
		String GlobalPath = ProjectSettings.GlobalizePath(Write_Path);
		Json json = ResourceLoader.Load<Json>(GlobalPath);
		DEBUG.Info.Print(json.Data);
		if (json is Json && json != null)
		{
			return json;
		}
		return null;
	}
	public void Save_Data()
	{
		Godot.Collections.Dictionary<String,Variant> keys = new Godot.Collections.Dictionary<String,Variant>();
		for (int Name = 0; Name < Data_array.Count; Name++)
		{
			keys.Add(Key[Name],Data_array[Name]);
		}
		String GlobalPath = ProjectSettings.GlobalizePath(Write_Path);
		var Save = Godot.FileAccess.Open(GlobalPath,Godot.FileAccess.ModeFlags.Write);
		String Data_Temp_Text = keys.ToString(); // 临时数据
		String Data_Text = ""; //主数据
		int Indent = 0; // 缩进距离
		bool is_Array = false; // 是否为数组
		foreach(Char s in Data_Temp_Text)
		{
			if (s.ToString() == "(")
			{
				Data_Text += "[";
				is_Array = true; // 检测是否是数组
			}else if(s.ToString() == ")"){
				Data_Text += "]";
				is_Array = false;
			}else if(s.ToString() == ",") // 自动换行+缩进了解一下
			{
				Data_Text += s.ToString();
				if (is_Array == false){
				Data_Text += "\n"; //换行符
				for (int d = 0;d < Indent; d++)
				{
					Data_Text += "	"; //你缩进爷爷
				}
				}
			}
			else
			{
				Data_Text += s.ToString();
			}
			if (s.ToString() == "{") // 字典？
			{
				Data_Text += "\n";
				Indent += 1;
				for (int d = 0;d < Indent; d++)
				{
					Data_Text += "	";
				}
			}else if (s.ToString() == "}") // 看起来是的？
			{
				Indent -= 1;
				Data_Text += "\n";
				for (int d = 0;d < Indent; d++)
				{
					Data_Text += "	";
				}
			}
		}
		DEBUG.Info.Print(Data_Text);
		Save.StoreString(Data_Text);
	}
}
