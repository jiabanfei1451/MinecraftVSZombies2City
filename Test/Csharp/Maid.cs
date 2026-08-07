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
		DEBUG.Info.Print("person.json", jsonString);
	}

	public void Loads()
	{
		string jsonFromFile = File.ReadAllText("C:/person.json");
		Person p = JsonSerializer.Deserialize<Person>(jsonFromFile);
		DEBUG.Info.Print($"{p.Name}, {p.Age}");
		DEBUG.Info.Print(JsonSerializer.Deserialize<String>(jsonFromFile));
	}
}
}