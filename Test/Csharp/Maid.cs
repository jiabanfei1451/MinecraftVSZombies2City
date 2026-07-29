using Godot;
using System;
using System.IO;
using System.Text.Json;

namespace Test{
public partial class Maid : Object
{
	public void Saves(){
		var person = new Person { Name = "张三", Age = 25 };
		string jsonString = JsonSerializer.Serialize(person);
		File.WriteAllText("C:/person.json", jsonString);
		GD.Print("person.json", jsonString);
	}

	public void Loads()
	{
		string jsonFromFile = File.ReadAllText("C:/person.json");
		Person p = JsonSerializer.Deserialize<Person>(jsonFromFile);
		GD.Print($"{p.Name}, {p.Age}");
		GD.Print(JsonSerializer.Deserialize<String>(jsonFromFile));
	}
}
}