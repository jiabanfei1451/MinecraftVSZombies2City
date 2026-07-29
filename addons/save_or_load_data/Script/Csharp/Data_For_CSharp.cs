using Godot;
using System;


namespace Data{
[GlobalClass]
public partial class Data_For_CSharp : Node
{
	[Export] public Godot.Collections.Array Data_array = new Godot.Collections.Array();
	[Export] public Godot.Collections.Array<String> Key = new Godot.Collections.Array<string>();
	[Export] public String Write_Path = "User://Temp_Data.json";
}
}