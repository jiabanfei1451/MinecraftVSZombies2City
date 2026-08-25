using Godot;
using Touch;
using DEBUG;
using Game;
using Game.AutoLoad;
using GameUI;
namespace Level{
public partial class Lawn : ColorRect{
	[Signal]
	public delegate void ME_JoinEventHandler(Lawn This);
	[Export] public Vector2I ArrayPosition = new Vector2I();
	[Export] public TouchPad pad;
	[Export] public Godot.Vector2 Array2D_Position = Godot.Vector2.Zero;
	public CurrentObject Current_Object = null;
	public class CurrentObject
	{
		public Level.Object.LevelObject Equipment_Object = null;
	}
	public override void _Ready() {
		base._Ready();
		Current_Object = new CurrentObject();
		Get_GlobalNode.Get_Card_Data(GetTree()).Selected_Change += Card_Change;
		pad = GetNode<TouchPad>("TouchPad");
		pad.Focus_Joinvoid += focus_Join;
		pad.Button_Pressedvoid += pressed;
	}
	public void pressed()
	{
		if (Current_Object.Equipment_Object != null){return;}
		Level_Master_Script Level_ = (Level_Master_Script)GetTree().CurrentScene;
		if (Level_.Selected_Lawn != this){return;}
		Card_Data card_Data = Get_GlobalNode.Get_Card_Data(GetTree());
		if (card_Data.Selected_raw_Object == null){return;}
		card_Data.Selected_raw_Object.Placed();
	}
	public void focus_Join()
	{
		Game.Level_Script.Lawn = this;
		EmitSignal("ME_Join",this);
	}
	public void Card_Change(Card card)
	{
		if (card == null)
		{
			Free_Object();
			SelfModulate = new Color(0,0,0,0);
		}
		else
		{
			Color = new Color(1,1,1,0.2f);
			SelfModulate = new Color(1,1,1,1);
		}
	}
	public void Summand_Phantom()
	{
		Card_Data.GlobalData Temp_Data = Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
		PackedScene Scene = Temp_Data.Scene;
		Node2D new_Node2d = Scene.Instantiate<Node2D>();
		new_Node2d.Name = "-1+1-1+1_CS";
		if (new_Node2d is Level.Object.LevelObject)
		{
			Level.Object.LevelObject Temp_Node = (Level.Object.LevelObject)new_Node2d;
			Temp_Node.Enable = false;
			Temp_Node.Enable_Health = false;
		}
		this.AddChild(new_Node2d);
		new_Node2d.Position = Temp_Data.Map_Offset;
		new_Node2d.Modulate = new Color(1,1,1,0.3f);
	}
	public void Free_Object(){
		for (int i = 0; i < this.GetChildCount(); ++i)
			{
				Node Get = this.GetChild(i);
				if (Get.GetScript().ToString() == "" && !(Get is Level.Object.LevelObject)){
					Info.ERROR(Info.ERROR_Info.NOScript);
					Get.QueueFree();
				}
				else
				{
					if (Get is Level.Object.LevelObject){
						Level.Object.LevelObject Temp_Get = (Level.Object.LevelObject)Get;
						if (Temp_Get.Enable == false)
						{
							Temp_Get.QueueFree();
						}
					}
				}
			}
		}
	}
}