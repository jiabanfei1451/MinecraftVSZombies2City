using Godot;
using System;
namespace UIObject{
public partial class Card : Control
{
	public enum Easing_Type{
		/// <summary>
		/// 相对于父节点的坐标
		/// </summary>
		Position = 0,
		/// <summary>
		/// 全局坐标
		/// </summary>
		GlobalPosition = 1
	} 
	/// <summary>
	/// 缓动帧
	/// </summary>
	[ExportGroup("Card_Status")]
	[Export] public bool Selected = false;
	[ExportGroup("Easing")] [Export] public int Easing_Tick = 10;
	/// <summary>
	/// 脚本所处坐标
	/// </summary>
	[Export] public Godot.Vector2 Script_Position = Vector2.Zero;
	/// <summary>
	/// 脚本所处坐标
	/// </summary>
	[Export] public Godot.Vector2 Script_Position2 = Vector2.Zero;
	/// <summary>
	/// 缓动类型
	/// </summary>
	[Export] public Easing_Type Position_Easing_type = Easing_Type.GlobalPosition;
	/// <summary>
	/// 是否完成初始化
	/// </summary>
	[Export] public bool is_Ready = false;
	public override void _Ready() {
		base._Ready();
		This_Ready();
		switch (Position_Easing_type)
		{
		case Easing_Type.Position:
			Script_Position = Position;
			break;
		case Easing_Type.GlobalPosition:
			Script_Position = GlobalPosition;
			break;
		}
		is_Ready = true;
		
	}
	public override async void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (!is_Ready){return;}
		NodePath path = "";
		switch (Position_Easing_type)
		{
		case Easing_Type.Position:
			path = new NodePath(UIObject.Card.PropertyName.Position);
			break;
		case Easing_Type.GlobalPosition:
			path = new NodePath(UIObject.Card.PropertyName.GlobalPosition);
			break;
		}
		if (Selected == false){
			ZIndex = 1;
			CreateTween().TweenProperty(this,path,Script_Position,delta * (float)Easing_Tick);
		}else
		{
			ZIndex = 2;
			CreateTween().TweenProperty(this,path,Script_Position2,delta * (float)Easing_Tick);
		}
	}
	public void This_Ready()
	{
		GetNode<Control>("Cilp_Node").GetNode<ColorRect>("CD").Scale = new Godot.Vector2(1,0);
		GetNode<TouchPad>("Texture/TouchPad").Button_Pressedvoid += touchpressed;
	}
	public void touchpressed()
	{
		Selected = !Selected;
	}
}
}