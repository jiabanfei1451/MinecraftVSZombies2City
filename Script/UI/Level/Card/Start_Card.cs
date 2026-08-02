using Godot;
using Touch;

public partial class Start_Card : TouchPad
{
	Tween een = null;
	[Export]public StyleBoxTexture UI = GD.Load<StyleBoxTexture>("uid://cmegohxjbl1ju");
	[Export] public StyleBoxTexture UI_Foucs = GD.Load<StyleBoxTexture>("uid://d1p6irqo1kmry");
	public override void _Ready() {
		base._Ready();
		Button_Pressedvoid += pressed;
	}
	public async void pressed()
	{
		var it = GetNode<UIObject.LevelUi>("../../..")?.hide_Select_CardUI();
		await it;
		CreateTween().TweenProperty(GetNode<Control>("../../../Card_Slot"),new NodePath(Control.PropertyName.Position),GetNode<Control>("../../../Y").Position - new Vector2((GetNode<Control>("../../../Card_Slot").Size.X+80) / 2,0),2).SetTrans(Tween.TransitionType.Cubic);
		Game.Get_GlobalNode.Node_Data.Get_Node<Level.Level_Master_Script>("Level").Completed_Selected_Card();;
	}
	public override void _Process(double delta)
	{
		if (Focus == true || Pressed == true)
		{
			GetNode<Panel>("Panel6").AddThemeStyleboxOverride("panel",UI_Foucs);
		}
		else
		{
			GetNode<Panel>("Panel6").AddThemeStyleboxOverride("panel",UI);
		}
		if (Pressed == true)
		{
			if (een != null){een.Kill();}
			een = CreateTween();
			een.TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(0.5f,0.5f,0.5f,1),0.01);
		}
		else
		{
			een = CreateTween();
			een.TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.1);
		}
	}
}
