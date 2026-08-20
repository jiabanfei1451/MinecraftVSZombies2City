using Godot;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
namespace 粒子系统;
public partial class 粒子 : Sprite2D
{
	[Export] private bool 开启缓动 = false;
	[Export] private float 持续时间 = 3f;
	[Export] private Godot.Vector2 x偏移 = new Godot.Vector2(50, 50);
	[Export] private Godot.Vector2 随机偏移{get;set;} = new Godot.Vector2(50, 25);
	[Export] private float 上升速度{get;set;} = 1.5f;
	[Export] private float 增加幅度 = 15;
	[Export] private float 下降速度 = 1;
	[Export] private Godot.Vector2 最大高度倍率 = new Godot.Vector2(0.75f, 1.5f);
	[Export] private float speedy{get;set;} = 1.0f;
	[Export] private float plusspeed{get;set;} = 10f;
	[Export] private float 弹跳力{get;set;} = 1f;
	[Export] private int 最大除数{get;set;} = 6;
	float 上升幅度 = 0.2f,Y加速度 = 10f,speed,弹跳阈值 = -1f,减少阈值=60f,高度倍率,目标x,当前X;
	
	bool 函数上升 = true,赋值完成 = false;
	int 除数 = 2;
	private Godot.Vector2 当前位置,最终位置;
	Random rand = new();
	public override void _Ready() {
		base._Ready();
		Y加速度 = 3f + rand.NextSingle() * 7f;
		当前位置 = Position;
		最终位置 = 当前位置 + new Godot.Vector2((float)Math.Round(rand.NextSingle() * (随机偏移.X * 2) - 随机偏移.X,2)
		,(float)Math.Round(rand.NextSingle() * (随机偏移.Y * 2) - 随机偏移.Y,2));
		当前X = 当前位置.X;
		目标x = 最终位置.X + rand.NextSingle() * (x偏移.Y - x偏移.X) + x偏移.X;
		当前位置 = 最终位置;
		高度倍率 = rand.NextSingle() * 最大高度倍率.X;
		async void 延迟销毁(){
			Tween tween = CreateTween();
			tween.TweenProperty(this, "当前X", 目标x*  最大高度倍率.X, (rand.NextSingle() * 高度倍率) * 3f).SetTrans(Tween.TransitionType.Linear);
			await ToSignal(GetTree().CreateTimer((持续时间 / 2f) + rand.NextSingle() * 持续时间), "timeout");
			QueueFree();
		}
		延迟销毁();
	}
	public override void _Process(double delta) {
		base._Process(delta);
		当前位置.X = 当前X;
		switch (开启缓动){
			case false:
				Position = 当前位置;
				break;
		}
		if (函数上升 == true){
			if (Math.Sin(上升幅度) <= -0.05f){
				if (赋值完成 == false){
					speed = 2f;
					speedy = 1f;
					函数上升 = false;
					赋值完成 = true;
				}
			}
			else
			{
				当前位置.Y += Convert.ToSingle(Math.Sin(上升幅度)* -Y加速度 * 高度倍率 * (delta * 60));
				上升幅度 += 增加幅度 * (float)delta;
			}
		}
		else
		{
			if (当前位置.Y != 最终位置.Y && 当前位置.Y < 最终位置.Y || 弹跳阈值 > 0){
				当前位置.Y += speedy * Convert.ToSingle(Math.Clamp(speed, 0, 15) * -弹跳阈值) * 下降速度 * (60f * (float)delta);
				speed += plusspeed * (float)delta;
			}
			else
			{
				if (除数 != 最大除数){
					speed = 10f;
					弹跳阈值 = 弹跳力 / (float)除数;
					减少阈值 = 6f;
					除数 += 1;
				}
			}
		if (弹跳阈值 > -1)
			{
				弹跳阈值 -= 减少阈值 * (float)delta;
			}else if(弹跳阈值 < -1)
			{
				弹跳阈值 = -1f;
			}
			switch (开启缓动){
			case false:
				Position = 当前位置;
				break;
			当前位置.X = 当前X;
		}
		}
	}
}
