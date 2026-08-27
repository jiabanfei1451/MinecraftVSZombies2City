using Touch;
using Godot;
using System.Threading.Tasks;
using My_Csharp_Node;
using Game;
using Game.AutoLoad;
using DEBUG;
using System;
namespace GameUI{
/// <summary>
/// 卡槽
/// </summary>
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
	[Export] public ColorRect CD_Mask = null;
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
	/// 卡槽在选定数据中的索引
	/// </summary>
	[Export] public int Selected_Index = -1;
	/// <summary>
	/// 选定
	/// </summary>
	[Export] public bool Selected = false;
	/// <summary>
	/// 卡槽索引，用于索引放置该卡槽放置的器械
	/// </summary>
	[Export] public int Card_Index = -1;
	int Current_Card_Index = -1;
	[Export] public int Sonsume = 0;
	/// <summary>
	/// 选定卡分身模式的物体 仅在Selected_Card模式可以使用
	/// </summary>
	public ModeObject Mode_Data = null;
	/// <summary>
	/// 冷却中
	/// </summary>
	[ExportSubgroup("CD")]
	[Export] public bool CDing = false;
	/// <summary>
	/// 最大冷却时间
	/// </summary>
	[Export] public float MAXCD_Time = 0;
	/// <summary>
	/// 冷却时间
	/// </summary>
	[Export] public float CD_Time = 0;
	[Export] public Label Show_CD_Label = null;
	/// <summary>
	/// 已开始第一次冷却
	/// </summary>
	[Export] public bool first_Time_ReduceCD = false;
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
	/// 热键
	/// </summary>
	[Export] public Key Trigger_Key = Key.None;
	[Export] public bool Key_Pressed = false;
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
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (Card_Mode != Mode.Gameing){return;}
		if (@event is InputEventKey)
		{
			if (((InputEventKey)@event).Keycode == Trigger_Key && Key_Pressed != ((InputEventKey)@event).IsPressed()){
				Key_Pressed = ((InputEventKey)@event).IsPressed();
				if (((InputEventKey)@event).IsPressed() == true){
					_Selected();
				}
			}
		}
		if (Get_Selected_rawObject() != this){return;}
		if (@event is InputEventMouseButton)
			{
				InputEventMouseButton button = (InputEventMouseButton)@event;
				if (button.Pressed && button.ButtonIndex == MouseButton.Right)
				{
					Cancel_Audio.Play();
					Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null;
				}
			}
	}
	public override void _Ready() {
		base._Ready();
		
		This_Ready();
		parent_Object = Get_Parent_Object();
		is_Ready = true;
	}
	/// <summary>
	/// 初始化材质
	/// </summary>
	public void Reset_Texture()
	{
		foreach(Node Nodes in GetNode<Control>("Image").GetChildren())
		{
			Nodes.QueueFree();
		}
		//初始化材质
		Card_Data.GlobalData Data = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
		Node2D texture = Data.Scene.Instantiate<Node2D>();
		if (texture is Level.Object.LevelObject)
		{
			((Level.Object.LevelObject)texture).Enable = false;
			((Level.Object.LevelObject)texture).Enable_Health = false;
			((Level.Object.LevelObject)texture).Physics_Enable = false;
		}
		GetNode<Control>("Image").AddChild(texture);
		texture.Position = Data.Offset;
		texture.Scale = Data.Scale;
		GetNode<Label>("Reduce").Text = Data.Sonsume.ToString();
	}
	/// <summary>
	/// 节点初始化
	/// </summary>
	public async void This_Ready()
	{
		Reset_Texture();
		GetNode<TouchPad>("Texture/TouchPad").Button_Pressedvoid += touchpressed;
		GetNode<TouchPad>("Texture/TouchPad").Button_Downvoid += TouchDown;
		GetNode<Control>("Cilp_Node").Visible =false;
		CD_Mask = GetNode<ColorRect>("Cilp_Node/CD");
		Show_CD_Label = GetNode<Label>("Cilp_Node/ShowCD");
		if (OS.GetName() != "Windows") {
		GetNode<TouchPad>("Texture/TouchPad").End_Dragvoid += TouchDragEnd;
		}
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
				if (this == null){return;}
				if (Temp_Tween != null){
					Temp_Tween.Kill();
					Temp_Tween = null;
				}
				if (Temp_Tween == null){
					Temp_Tween = this.CreateTween();
					Temp_Tween.TweenProperty(this,new NodePath(Control.PropertyName.GlobalPosition),Get_Parent_Object().GetChild<Control>(0).GetChild<Control>(0).GetChild<Control>(Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Card_Index(this)).GlobalPosition,Easing_Time);
				}
				await Task.Delay(1000 / 60);
			}
			break;
		}
	}
	/// <summary>
	/// 被点击时
	/// </summary>
	public override void _Process(double delta) {
		base._Process(delta);
		switch (Card_Mode){
			case Mode.Gameing:
				if (first_Time_ReduceCD == false)
				{
					var Temp = Start_CD();
				}
				if (Current_Card_Index != Card_Index)
					{
						GetNode<Control>("Cilp_Node").Visible =true;	
						Current_Card_Index = Card_Index;
						Sonsume = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index).Sonsume;
					}
				if (Selected_Index == -1)
					{
						Selected_Index = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Card_Index(this);
						Info.Print($"数组索引:{Selected_Index} 取值:",Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_CD[Selected_Index]);
					}
				if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object == this)
				{
					Modulate = new Color(0.5f,0.5f,0.5f,1);
				}else
				{
					Modulate = new Color(1,1,1,1);	
				}
				if (CD_Mask != null){
					if (CD_Time > 0){
						if (CDing == false)
						{
							CDing = true;
						}
						#region 显示状态切换
						if (CD_Mask.Visible == false){
							CD_Mask.Visible = true;
						}
						if (Show_CD_Label.Visible == false)
						{
							Show_CD_Label.Visible = true;	
						}
						#endregion
						Show_CD_Label.Text = Math.Round(CD_Time,1).ToString();
						CD_Mask.Scale = new Vector2(1,CD_Time / MAXCD_Time);
						CD_Time -= (float)delta;
					}
					else
					{	
						#region 显示状态切换
						if (Game.Level_Script.Equipment_Capable < Sonsume)
						{
							Show_CD_Label.Visible = true;	
							Show_CD_Label.Text = "不足";
							CD_Mask.Visible = true;
							CD_Mask.Scale = new Vector2(1,1);
						}
						else
						{
							Show_CD_Label.Visible = false;
							CD_Mask.Visible = false;
						}
						#endregion
						if (CDing == true)
						{
							GetNode<AnimationPlayer>("Animation").Play("Twinkle");
							CDing = false;
						}
					}
				}
				break;
		}
		
	}
	#region 触摸事件
	public void TouchDragEnd()
	{
		Placed();
	}
	public void TouchUP()
	{
	}
	public void TouchDown()
	{
		if (Card_Mode == Mode.Gameing)
		{
			_Selected();
		}
	}
	/// <summary>
	/// 触摸点击时
	/// </summary>
	public async void touchpressed()
	{
		switch (Card_Mode){
			case Mode.Selected_Card:
				if (GlobalPosition.Y < 80){return;}
				// 选定状态
				if (Mode_Data.Selected_Card_Mode.is_Selected_Card_Object == null){
					if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_Selected_Card_Len() > PlayerData.Card_Quantity - 1){return;}
					PackedScene Temp_Scene = Game.ResourceTool.LoadScene("uid://c2y62prxcbege");
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
					Info.Print(Mode_Data.Selected_Card_Mode.is_Selected_Card_Object);
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
		}
	}
	/// <summary>
	/// 获取选中卡槽原物体
	/// </summary>
	/// <returns></returns>
	#endregion
	public Card Get_Selected_rawObject()
	{
		Card getCard = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object;
		return getCard;	
	}
	/// <summary>
	/// 放置
	/// </summary>
	public void Placed(bool Sousume = true)
	{
		Level.Level_Master_Script level = (Level.Level_Master_Script)GetTree().CurrentScene;
		Card_Data.GlobalData data = Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
		if (CDing == true){return;}
		if (Game.Level_Script.Equipment_Capable < data.Sonsume || Sousume == false){return;}
		if (Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != this){return;}
		if (level.Selected_Lawn == null){return;}
		var Temp = Start_CD();
		Game.Level_Script.Equipment_Capable -= data.Sonsume;
		Level.Lawn Lawn = level.Selected_Lawn;
		Level.Object.LevelObject node = data.Scene.Instantiate<Level.Object.LevelObject>();
		node.Position = Lawn.Position + data.Map_Offset;
		node.Scale = data.Map_Scale;
		Lawn.Current_Object.Equipment_Object = node;
		Get_GlobalNode.Node_Data.Get_Node<Node2D>("Equipment").AddChild(Lawn.Current_Object.Equipment_Object);
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null;
	}
	/// <summary>
	/// 开始冷却
	/// </summary>
	/// <returns></returns>
	public bool Start_CD()
	{
		if (CDing == true)
		{
			Info.PrintErr("当前卡槽正在冷却请勿重复执行!");
			return false;
		}
		Card_Data.GlobalData data = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
		MAXCD_Time = data.CD;
		CD_Time = data.CD;
		if (first_Time_ReduceCD == false)
		{
			CD_Time -= data.First_Time_RemoveCD;
			first_Time_ReduceCD = true;
		}
		return true;
	}
	/// <summary>
	/// 选定
	/// </summary>
	public void _Selected()
	{
		Card_Data.GlobalData globalData = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Get_CardData(Card_Index);
		if (Game.Level_Script.Equipment_Capable < globalData.Sonsume || CDing == true)
		{
			GetNode<Audio_Plus>("buzzer").Play();
			return;
		}
		if (Selected_Audio == null)
			{
				Selected_Audio = GetNode<Audio_Plus>("Selected");
			}
		if (Cancel_Audio == null)
			{
				Cancel_Audio = GetNode<Audio_Plus>("Cancel");
			}
		if (Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object != this){
			Level.Level_Master_Script level = (Level.Level_Master_Script)GetTree().CurrentScene;
			level.Selected_Lawn = null;
			Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = this;
			Selected_Audio.Play();
		}
		else
		{
			Cancel_Audio.Play();
			Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object = null;
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
			public Card_Data.GlobalData Card_Data;
		}
		#endregion
	}
}