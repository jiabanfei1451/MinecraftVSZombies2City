using Godot;
using Touch;
using My_Csharp_Node;
using System.Threading.Tasks;

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
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).CD_Initialization();
		var it = GetNode<UIObject.LevelUi>("../../..")?.hide_Select_CardUI();
		await it;
		CreateTween().TweenProperty(GetNode<Control>("../../../Card"),new NodePath(Control.PropertyName.Position),GetNode<Control>("../../../Y").Position - new Vector2((GetNode<Control>("../../../Card_Slot").Size.X+80) / 2,0),2).SetTrans(Tween.TransitionType.Cubic);
		CreateTween().TweenProperty(GetNode<Control>("../../../Card_Slot"),new NodePath(Control.PropertyName.Position),GetNode<Control>("../../../Y").Position - new Vector2((GetNode<Control>("../../../Card_Slot").Size.X+80) / 2,0),2).SetTrans(Tween.TransitionType.Cubic);
		Game.Get_GlobalNode.Node_Data.Get_Node<Level.Level_Master_Script>("Level").Completed_Selected_Card();
		await Task.Delay(1000);
		Audio_Plus s = new Audio_Plus();
		s.Audio_Type = Audio_Plus.Audio.Souds;
		s.Auto_QueneFree = true;
		s.Stream = Game.Get_GlobalNode.Get_Audio_List(GetTree()).Get_Souds("MVZ2:Ready");
		AddChild(s);
		s.Play();
		Game.Get_GlobalNode.Get_Muisc_Engine(GetTree()).new_playMuisc("2");
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"好!");
		await Task.Delay(500);
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"准备!");
		await Task.Delay(500);
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"安放器械!!!");
		await Task.Delay(1000);
		GetNode<UIObject.LevelUi>("../../..")?.Card_Initialization();
		Game.Tip.Set_Ready_Text("");
		await ToSignal(GetTree().CreateTimer(2),SceneTreeTimer.SignalName.Timeout);
		GetNode<Control>("../..").QueueFree();
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).CD_Initialization();
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
