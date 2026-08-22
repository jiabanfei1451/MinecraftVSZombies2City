using Godot;
using Touch;
using My_Csharp_Node;
using System.Threading.Tasks;
using Level;

public partial class Start_Card : TouchPad
{
	Tween een = null;
	[Export]public StyleBoxTexture UI = GD.Load<StyleBoxTexture>("uid://cmegohxjbl1ju");
	[Export] public StyleBoxTexture UI_Foucs = GD.Load<StyleBoxTexture>("uid://d1p6irqo1kmry");
	public override void _Ready() {
		base._Ready();
		Button_Pressedvoid += pressed;
		Game.Get_GlobalNode.Node_Data.Add_Node(GetNode<UIObject.LevelUi>("../../.."),"LevelUI");
		Game.Get_GlobalNode.Node_Data.Add_Node(GetNode<Control>("../.."),"LevelUI2");
	}
	public async void pressed()
	{
		Touch.Touch_Index.Set_Index_Enable(1,false);
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).CD_Initialization();
		var it = GetNode<UIObject.LevelUi>("../../..")?.hide_Select_CardUI();
		await it;
		CreateTween().TweenProperty(GetNode<Control>("../../../Card"),new NodePath(Control.PropertyName.Position),GetNode<Control>("../../../Y").Position - new Vector2((GetNode<Control>("../../../Card_Slot").Size.X+80) / 2,0),2).SetTrans(Tween.TransitionType.Cubic);
		CreateTween().TweenProperty(GetNode<Control>("../../../Card_Slot"),new NodePath(Control.PropertyName.Position),GetNode<Control>("../../../Y").Position - new Vector2((GetNode<Control>("../../../Card_Slot").Size.X+80) / 2,0),2).SetTrans(Tween.TransitionType.Cubic);
		Game.Get_GlobalNode.Node_Data.Get_Node<Level.Level_Master_Script>("Level").Completed_Selected_Card();
		await Task.Delay(1000);
		Game.Get_GlobalNode.Node_Data.Get_Node<Level_Master_Script>("Level").Game_Start();
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
