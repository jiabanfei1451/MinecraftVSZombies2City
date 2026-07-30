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
	[ExportGroup("Easing")] [Export] public float Easing_Time = 0.5f;
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
	[Export] public Easing_Type Position_Easing_type = Easing_Type.Position;
	/// <summary>
	/// 是否完成初始化
	/// </summary>
	[Export] public bool is_Ready = false;
	/// <summary>
	/// 父节点
	/// </summary>
	[Export] public Node parent_Object = null;
	/// <summary>
	/// 索引
	/// </summary>
	int my_index {get;set;} = -1;
	public override void _Ready() {
		base._Ready();
		This_Ready();
		parent_Object = Get_Parent_Object();
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
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (GlobalPosition.Y < 140 && Selected == false)
		{
			CreateTween().TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,(GlobalPosition.Y/140) * 0.2f),0.1);
		}
		else
		{
			CreateTween().TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.1);
		}
		if (!is_Ready){return;}
		NodePath path = "";
		if (my_index != Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Card_Index(this)){
			my_index = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Card_Index(this);
			Script_Position2 = parent_Object.GetNode("Card_Slot").GetChild(0).GetChild<Control>(my_index).GlobalPosition;
		}
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
			ZIndex = 0;
			CreateTween().TweenProperty(this,path,Script_Position,Easing_Time);
		}else
		{
			ZIndex = 1;
			CreateTween().TweenProperty(this,path,Script_Position2,Easing_Time);
		}
	}
	/// <summary>
	/// 节点初始化
	/// </summary>
	public void This_Ready()
	{
		GetNode<Control>("Cilp_Node").GetNode<ColorRect>("CD").Scale = new Godot.Vector2(1,0);
		GetNode<TouchPad>("Texture/TouchPad").Button_Pressedvoid += touchpressed;
	}
	public void touchpressed()
	{
		if (Modulate.A < 0.8){return;}
		// 选定状态
		if (Selected == false)
		{
			Position_Easing_type = Easing_Type.GlobalPosition;
			Selected = true;
			Game.Get_GlobalNode.Get_Card_Data(GetTree()).Add_Card_Index(this);
		}
		else
		{
			Position_Easing_type = Easing_Type.Position;
			Selected = false;
			Game.Get_GlobalNode.Get_Card_Data(GetTree()).Remove_Card_Index(this);
		}
	}
	/// <summary>
	/// 获取父节点
	/// </summary>
	/// <returns></returns>
	public CanvasLayer Get_Parent_Object()
	{
		Node getnode = this;
		while (!(getnode is CanvasLayer))
		{
			getnode= getnode.GetNode<Node>("..");
		}
		return (CanvasLayer)getnode;
	}
}
}