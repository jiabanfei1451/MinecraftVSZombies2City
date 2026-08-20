using System;
using Godot;

public partial class VBoxContainer : Node {
	public override void _Ready() {
		base._Ready();
		async void PrintHello() {
			for (int i = 0; i < 9; i++)
			{
				var Scene = GD.Load<PackedScene>("res://Scene/Misc Scene/Music room/List.tscn");
				var Scenei = Scene.Instantiate();
				Scenei.Set("id", i);
				AddChild(Scenei);
			}
		}
		PrintHello();
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}
}
