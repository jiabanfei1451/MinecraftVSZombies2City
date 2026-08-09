using Godot;
using Touch;
using DEBUG;
using Game;
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
		public Level.Object.Data Equipment_Object = null;
	}
	public override void _Ready() {
		base._Ready();
		Current_Object = new CurrentObject();
		Get_GlobalNode.Get_Card_Data(GetTree()).Selected_Change += Card_Change;
		pad = GetNode<TouchPad>("TouchPad");
		pad.Focus_Joinvoid += focus_Join;
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
	public void Free_Object(){
		for (int i = 0; i < this.GetChildCount(); ++i)
			{
				Node Get = this.GetChild(i);
				if (Get.GetScript().ToString() == "" && !(Get is Level.Object.Data)){
					Info.ERROR(Info.ERROR_Info.NOScript);
					Get.QueueFree();
				}
				else
				{
					if (Get is Level.Object.Data){
						Level.Object.Data Temp_Get = (Level.Object.Data)Get;
						if (Temp_Get.Enable == false)
						{
							Info.Print(Temp_Get.Enable);
							Temp_Get.QueueFree();
						}
					}
				}
			}
		}
	}
}