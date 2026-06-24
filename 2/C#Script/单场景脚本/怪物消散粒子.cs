using Godot;
using System;
using System.Threading.Tasks.Dataflow;
public partial class 怪物消散粒子 : Sprite2D
{
	[Export] private Godot.AtlasTexture Type{get;set;}
	private Godot.Vector2 位置;
	private Random random = new Random();
	private float 速度;
	public override void _Ready() {
		base._Ready();
		Type = new();
		速度 = random.NextSingle() * 30 + 10;
		位置 = Position;
		Type.Atlas = GD.Load<Godot.Texture2D>("res://Object/素材图/sactx-0-1024x1024-DXT5_BC3-effects-5ecf9be5.png");
		int rand = random.Next(0, 3);
		switch (rand)
		{
			case 0:
				Type.Region = new Godot.Rect2(908, 604, 8, 10);
				break;
			case 1:
				Type.Region = new Godot.Rect2(924, 602, 14, 14);
				break;
			case 2:
				Type.Region = new Godot.Rect2(944, 678, 4, 4);
				break;
		}
		Texture = Type;
		async void 延迟销毁()
		{
			await ToSignal(GetTree().CreateTimer(0.5f + new Random().NextSingle() * 0.5f), "timeout");
			QueueFree();
		}
		
		延迟销毁();
	}
	public override void _Process(double delta) {
		base._Process(delta);
		Position = 位置;
		位置.Y -= 速度  * (float)delta;
	}
}
