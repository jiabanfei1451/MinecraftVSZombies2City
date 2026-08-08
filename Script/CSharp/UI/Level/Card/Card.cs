using Touch;
using Godot;
using System.Threading.Tasks;
using DEBUG;
using My_Csharp_Node;
namespace GameUI{
public partial class Card : Control
{
	#region Enum
	/// <summary>
	/// 模式
	/// </summary>
	public enum Mode
	{
		// 选卡模式
		Selected_Card = 0,
		/// <summary>
		/// 选定卡分身模式
		/// </summary>
		is_Seleceed_Card = 1,
		/// <summary>
		/// 游戏中
		/// </summary>
		Gameing = 2,
	}
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
	#endregion
	#region Object
	[Export] public Audio_Plus Selected_Audio = null;
	[Export] public Audio_Plus Cancel_Audio = null;
	#endregion
	#region Card_Status
	/// <summary>
	/// 缓动帧
	/// </summary>
	[ExportGroup("Card_Status")]
	[Export] public Mode Card_Mode = Mode.Selected_Card;
	/// <summary>
	/// 选定
	/// </summary>
	[Export] public bool Selected = false;
	/// <summary>
	/// 卡槽索引
	/// </summary>
	[Export] public int Card_Index = -1;
	/// <summary>
	/// 选定卡分身模式的物体 仅在Selected_Card模式可以使用
	/// </summary>
	public ModeObject Mode_Data = null;
	#endregion
	#region Easing
	[ExportGroup("Easing")] [Export] public float Easing_Time = 0.5f;
	/// <summary>
	/// 脚本所处坐标
	/// </summary>
	[Export] public Godot.Vector2 Script_Position = Vector2.Zero;
	/// <summary>
	/// 停止无限循环
	/// </summary>
	[Export] public bool Stop_While = false;
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
	#endregion
	#region misc
	[Export] public bool is_Ready = false;
	/// <summary>
	/// 父节点
	/// </summary>
	[Export] public Node parent_Object = null;
	/// <summary>
	/// 索引
	/// </summary>
	int my_index {get;set;} = -1;
	bool nono = false;
	/// <summary>
	/// 临时Tween
	/// </summary>
	private Tween Temp_Tween = null;
	#endregion
	public override async void _Ready() {
		base._Ready();
		This_Ready();
		parent_Object = Get_Parent_Object();
		is_Ready = true;
	}
	
	
	/// <summary>
	/// 节点初始化
	/// </summary>
	public async void This_Ready()
	{
		//初始化材质
		Game.Card_Data.GlobalData Data = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
		CharacterBody2D texture = Data.Scene.Instantiate<CharacterBody2D>();
		GetNode<Control>("Image").AddChild(texture);
		texture.Position = Data.Offset;
		texture.Scale = Data.Scale;
		GetNode<Label>("Reduce").Text = Data.Sonsume.ToString();
		GetNode<TouchPad>("Texture/TouchPad").Button_Pressedvoid += touchpressed;
		switch (Card_Mode){
		//选卡模式
		case Mode.Selected_Card:
			Mode_Data = new ModeObject();
			Mode_Data.Selected_Card_Mode = new ModeObject.Selected_Card();
			GetNode<Control>("Cilp_Node").GetNode<ColorRect>("CD").Scale = new Godot.Vector2(1,0);
			break;
		//选卡分身模式
		case Mode.is_Seleceed_Card:
			Modulate = new Color(0,0,0,0);
			Mode_Data.gameing_Mode = new ModeObject.Gameing();
			Mode_Data.gameing_Mode.Card_Data = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
			DEBUG.Info.Print(Mode_Data.gameing_Mode.Card_Data);
			CreateTween().TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.5f).SetTrans(Tween.TransitionType.Sine);			
			while (!Stop_While){
				if (Temp_Tween != null){Temp_Tween.Kill();}
				Temp_Tween = CreateTween();
				Temp_Tween.TweenProperty(this,new NodePath(Control.PropertyName.GlobalPosition),Get_Parent_Object().GetChild<Control>(0).GetChild<Control>(0).GetChild<Control>(Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Card_Index(this)).GlobalPosition,Easing_Time);
				await Task.Delay(1000 / 60);
			}
			break;
		}
	}
	/// <summary>
	/// 被点击时
	/// </summary>
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		switch (Card_Mode){
			case Mode.Gameing:
				if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object == this)
				{
					Modulate = new Color(0.5f,0.5f,0.5f,1);
				}else
				{
					Modulate = new Color(1,1,1,1);	
				}
				break;
		}
	}
	public async void touchpressed()
	{
		switch (Card_Mode){
			case Mode.Selected_Card:
				if (GlobalPosition.Y < 80){return;}
				// 选定状态
				if (Mode_Data.Selected_Card_Mode.is_Selected_Card_Object == null){
					if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Selected_Card_Len() > Game.PlayerData.Card_Quantity - 1){return;}
					PackedScene Temp_Scene = GD.Load<PackedScene>("uid://c2y62prxcbege");
					Card Temp_Card = Temp_Scene.Instantiate<Card>();
					Temp_Card.Card_Mode = Mode.is_Seleceed_Card;
					Temp_Card.Mode_Data = new ModeObject();
					Temp_Card.Mode_Data.is_Selected_Card_Mode = new ModeObject.Is_Selected_Card();
					Temp_Card.Mode_Data.is_Selected_Card_Mode.Parent_Object = this;
					Temp_Card.Scale = new Godot.Vector2(0.4f,0.4f);
					Temp_Card.Card_Index = Card_Index;
					Temp_Card.GlobalPosition = GlobalPosition;
					Get_Parent_Object().GetNode<Control>("Card").AddChild(Temp_Card);
					Mode_Data.Selected_Card_Mode.is_Selected_Card_Object = Temp_Card;
					CreateTween().TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(0.5f,0.5f,0.5f,1),0.5).SetTrans(Tween.TransitionType.Sine);
					Game.Get_GlobalNode.Get_Card_Data(GetTree()).Add_Card_Index(Temp_Card);
					Temp_Card.GlobalPosition = GlobalPosition;
				}
				else
				{
					DEBUG.Info.Print(Mode_Data.Selected_Card_Mode.is_Selected_Card_Object);
				}
				break;
			case Mode.is_Seleceed_Card:
				if(Selected == false){
					Selected = true;
					Tween t = CreateTween();
					Tween d = CreateTween();
					Game.Get_GlobalNode.Get_Card_Data(GetTree()).Remove_Card_Index(this);
					t.TweenProperty(this,new NodePath(Control.PropertyName.Modulate),new Color(0,0,0,0),0.5).SetTrans(Tween.TransitionType.Sine);
					d.TweenProperty(this.Mode_Data.is_Selected_Card_Mode.Parent_Object,new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.5).SetTrans(Tween.TransitionType.Sine);
					await ToSignal(t,Tween.SignalName.Finished);
					Mode_Data.is_Selected_Card_Mode.Parent_Object.Mode_Data.Selected_Card_Mode.is_Selected_Card_Object = null;
					QueueFree();
				}
				break;
			case Mode.Gameing:
				if (Selected_Audio == null)
					{
						Selected_Audio = GetNode<Audio_Plus>("Selected");
					}
				if (Cancel_Audio == null)
					{
						Cancel_Audio = GetNode<Audio_Plus>("Cancel");
					}
				if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != this){
				Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = this;
				Selected_Audio.Play();
					}
				else
				{
					Cancel_Audio.Play();
					Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null;
				}
				break;
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
	/// <summary>
	/// 模式实例
	/// </summary>
	public class ModeObject(){
		#region Data
		/// <summary>
		/// 处于分身模式下的卡槽数据实例
		/// </summary>
		public Is_Selected_Card is_Selected_Card_Mode = null;
		/// <summary>
		/// 选卡模式下的卡槽数据实例
		/// </summary>
		public Selected_Card Selected_Card_Mode = null;
		/// <summary>
		/// 游戏中的卡槽数据
		/// </summary>
		public Gameing gameing_Mode = null;
		#endregion
		
		#region Class
		/// <summary>
		///处于分身模式下的卡槽数据
		/// </summary>
		public class Is_Selected_Card()
		{
			/// <summary>
			/// 源父物体
			/// </summary>
			public Card Parent_Object = null;
		}
		/// <summary>
		/// 选卡模式下的卡槽数据
		/// </summary>
		public class Selected_Card()
		{
			/// <summary>
			/// 已实例化的处于分身模式下的卡槽数据
			/// </summary>	
			public Card is_Selected_Card_Object = null;
		}
		/// <summary>
		/// 游戏中的卡槽数据
		/// </summary>
		public class Gameing()
		{
			public Game.Card_Data.GlobalData Card_Data;
		}
		#endregion
	}
}