using Godot;
using Touch;
using DEBUG;
using Game;
using GameUI;
using Game.Cheak;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Level;
public partial class Lawn : ColorRect{
	[Export] public Vector2I ArrayPosition = new Vector2I();
	[Export] public TouchPad pad;
	[Export] public Godot.Vector2 Array2D_Position = Godot.Vector2.Zero;
	public CurrentObject Current_Object = null;
	public class CurrentObject
	{
		public Godot.Collections.Dictionary<String,Level.Object.LevelObject> Objects = new ();
		public Godot.Collections.Dictionary<String,Level.Object.LevelObject> Misc_Objects= new();
		/// <summary>
		/// 获取所有字典密钥
		/// </summary>
		/// <returns></returns>
		public Godot.Collections.Array<String> Get_AllKey()
		{
			Godot.Collections.Array<String> Keys = new();
			foreach(String Key in Objects.Keys){
				Keys.Add(Key);
			}
			foreach(String Key in Misc_Objects.Keys){
				Keys.Add(Key);
			}
			return Keys;
		}
		/// <summary>
		/// 自动检索添加物体
		/// </summary>
		/// <param name="Key"></param>
		/// <param name="Object"></param>
		/// <returns></returns>
		public String Add_Object(String Key,Level.Object.LevelObject Object)
		{
			var Temp_Keys = Objects.Keys.ToList<String>();
			if (Temp_Keys.IndexOf(Key) != -1)
			{
				Level.Object.LevelObject Temp_LevelObject = Objects[Key];
				String RandomString = Game.Get.Random.Random_String(15);
				while (true){
					if (Misc_Objects.Keys.ToList<String>().IndexOf(RandomString) != -1)
					{
						RandomString = Game.Get.Random.Random_String(15);
					}
					else
					{
						break;
					}
					
				}
				Misc_Objects.Add(RandomString,Temp_LevelObject);
				Objects[Key] = Object;
			}
			else
			{
				Objects.Add(Key,Object);
			}
			return Key;
		}
		/// <summary>
		/// 清空空值物体
		/// </summary>
		public void clear_Null_Objects()
		{
			foreach(String Key in Objects.Keys)
			{
				if (Objects[Key] == null)
				{
					Objects.Remove(Key);
				}
			}
			foreach(String Key in Misc_Objects.Keys)
			{
				if (Misc_Objects[Key] == null)
				{
					Misc_Objects.Remove(Key);
				}
			}
		}
		/// <summary>
		/// 清空杂项
		/// </summary>
		public void clear_Misc_Objects()
		{
			Misc_Objects.Clear();
		}
		/// <summary>
		/// 清空物体
		/// </summary>
		public void clear_Objects()
		{
			Objects.Clear();
		}
		/// <summary>
		/// 移除物体
		/// </summary>
		/// <param name="level"></param>
		public void Remove_Object(Level.Object.LevelObject level)
		{
			foreach (String Key in Objects.Keys)
			{
				if (Objects[Key] == level)
				{
					Objects.Remove(Key);
					break;
				}
			}
			foreach (String Key in Objects.Keys)
			{
				try{
					if (Misc_Objects[Key] == level)
					{
						Misc_Objects.Remove(Key);
						break;
					}
				}catch{}finally{}
			}
		}
		/// <summary>
		/// 查找字典
		/// </summary>
		/// <param name="Key"></param>
		/// <returns></returns>
		public bool Has_Key(String Key)
		{
			bool returns = false;
			List<String> strings = Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				returns = true;
			}
			strings = Misc_Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				returns = true;
			}
			return returns;
		}
		/// <summary>
		/// 拥有类型
		/// </summary>
		/// <param name="Key"></param>
		/// <returns></returns>
		public Level.Object.LevelObject Has_Key_Object(String Key)
		{
			Level.Object.LevelObject returns = null;
			List<String> strings = Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				returns = Objects[Key];
			}
			strings = Misc_Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				returns = Misc_Objects[Key];
			}
			return returns;
		}
		/// <summary>
		/// 检测不可放置类型
		/// </summary>
		/// <returns>如果存在该类型的器械返回false 否则 true</returns>
		public bool Cheak_Cannot_Placed()
		{
			List<MVZ2.Type.ObjectType> Equipment_Types = new();
			var s = Get_GlobalNode.Get_Card_Data().Selected_raw_Object.Mode_Data.gameing_Mode;
			var Temp_Equipment = s.Card_Data.Scene.Instantiate<Level.Module.ObjectPhysics>();
			
			bool returns = true;
			if (Temp_Equipment.Cannot_place_Type.Count > 0){
				foreach (var Obj in Objects.Values)
				{
					if (Obj != null){
						Equipment_Types.Add(Obj.Object_Type);
					}
				}
				foreach (var Obj in Misc_Objects.Values)
				{
					if (Obj != null){
						Equipment_Types.Add(Obj.Object_Type);
					}
				}
				foreach (var Cannot in Temp_Equipment.Cannot_place_Type)
				{
					if (Equipment_Types.IndexOf(Cannot) != -1)
					{
						returns = false;
					}
				}
				Equipment_Types.Clear();
			}
			if (Objects.Values.Count > 0 || Misc_Objects.Count > 0)
			{
				foreach(Level.Object.LevelObject levelObject in Objects.Values){
					if (levelObject != null){
						foreach(MVZ2.Type.ObjectType Temp_Type in levelObject.Cannot_place_Type)
						{
							Equipment_Types.Add(Temp_Type);
						}
					}
				}
				foreach(Level.Object.LevelObject levelObject in Misc_Objects.Values){
					if (levelObject != null){
						foreach(MVZ2.Type.ObjectType Temp_Type in levelObject.Cannot_place_Type)
						{
							Equipment_Types.Add(Temp_Type);
						}
					}
				}
				if (Equipment_Types.IndexOf(Temp_Equipment.Object_Type) != -1)
				{
					returns = false;
				}
			}
			return returns;
		}
		/// <summary>
		/// 获取依赖字典索引
		/// </summary>
		/// <returns></returns>
		public String Get_Reliant_Object_Key(Lawn This)
		{
			Card_Data GetCard = Game.Get_GlobalNode.Get_Card_Data();
			Data.GlobalData GetCard_Data = GetCard.Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
			var Temp_Keys = Get_AllKey();
			Level.Module.ObjectPhysics Temp_Object = GetCard_Data.Scene.Instantiate<Level.Module.ObjectPhysics>();
			foreach(String Key in Temp_Keys){
				if (This.cheak_Reliant() == false && Has_Key_Object(Key) != null)
				{
					if (Cheak.Cheak_Data(Has_Key_Object(Key),GetCard_Data)){
						return Key;
					}
				}
			}
			return "";
		}
	}
	public override void _Ready() {
		base._Ready();
		Current_Object = new CurrentObject();
		Get_GlobalNode.Get_Card_Data(GetTree()).Selected_Change += Card_Change;
		pad = GetNode<TouchPad>("TouchPad");
		pad.Focus_Joinvoid += focus_Join;
		pad.Button_Pressedvoid += pressed;
	}
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (!pad.Focus){return;}
		if (Level_Script.Level_Object != null)
		{
			Level_Script.Level_Object.Lawn_Change_Color(this);			
		}
	}
	/// <summary>
	/// 检查依赖
	/// </summary>
	/// <returns>检查卡槽数据是否拥有依赖如果有为true 否则为 false</returns>
	public bool cheak_Reliant()
	{
		Data.GlobalData Temp_data = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
		Level.Module.ObjectPhysics Temp_Object = Temp_data.Scene.Instantiate<Level.Module.ObjectPhysics>();
		return (Temp_Object.Reliant_Tag.Count <= 0 && Temp_Object.Reliant_UUID.Count <= 0);
	}
	/// <summary>
	/// 放置
	/// </summary>
	public Level.Object.LevelObject Placed(bool Sousume = true)
	{
		Card_Data GetCard = Game.Get_GlobalNode.Get_Card_Data(GetTree());
		if (GetCard.Selected_raw_Object == null){return null;}
		if (GetCard.Selected_raw_Object.Card_Mode != Card.Mode.Gameing){return null;}
		Data.GlobalData GetCard_Data = GetCard.Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
		Level.Module.ObjectPhysics Temp_Object = GetCard_Data.Scene.Instantiate<Level.Module.ObjectPhysics>();
		String Object_Type = GetCard_Data.Object_Type.ToString();

		if (!Current_Object.Cheak_Cannot_Placed() && Temp_Object.Enable_Placed_Reliant == false)
		{
			return null;
		}
		if (Temp_Object.Enable_Placed_Reliant == true)
		{
			String Temp_Key = Current_Object.Get_Reliant_Object_Key(this);
			if (Temp_Key != "")
			{
				if (Temp_Object.Auto_Free_Reliant){
					Current_Object.Has_Key_Object(Temp_Key).QueueFree();
				}
			}
			else
			{
				return null;
			}
		}
		else if(Current_Object.Has_Key_Object(Object_Type) != null){
				return null;
		}
		if (Sousume == true)
		{
			if (Level_Script.Equipment_Capable >= GetCard_Data.Sonsume)
			{
				Level_Script.Equipment_Capable -= GetCard_Data.Sonsume;
				GetCard.Selected_raw_Object.Start_CD();
			}
			else
			{
				return null;
			}
		}
		Level.Object.LevelObject levelObject = GetCard_Data.Scene.Instantiate<Level.Object.LevelObject>();
		levelObject.Scale = GetCard_Data.Map_Scale;
		bool CheakGroup = Game.Cheak.CheakGroup.Cheak_Object_Group(levelObject,new(){"Monster"},new ());
		if (CheakGroup == false)
		{
			Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Equipment").AddChild(levelObject);
			Current_Object.Add_Object(Object_Type,levelObject);
		}
		else
		{
			Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Monster").AddChild(levelObject);
		}
		levelObject.Position = Position + GetCard_Data.Map_Offset;
		levelObject.practical_Position = GlobalPosition + GetCard_Data.Map_Offset;
		GetCard.Selected_raw_Object = null;
		return levelObject;
	}
	public void pressed()
	{
		Placed();
	}
	public void Object_Kill(Level.Object.LevelObject levelObject)
	{
		Current_Object.Remove_Object(levelObject);
	}
	public void focus_Join()
	{
		Game.Level_Script.Lawn = this;
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
	/// <summary>
	/// 生成虚影
	/// </summary>
	public void Summand_Phantom()
	{
		Data.GlobalData Temp_Data = Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
		PackedScene Scene = Temp_Data.Scene;
		Node2D new_Node2d = Scene.Instantiate<Node2D>();
		new_Node2d.Name = "-1+1-1+1_CS";
		if (new_Node2d is Level.Object.LevelObject)
		{
			Level.Object.LevelObject Temp_Node = (Level.Object.LevelObject)new_Node2d;
			Temp_Node.Enable = false;
			Temp_Node.Enable_Health = false;
		}
		new_Node2d.Position = Temp_Data.Map_Offset;
		this.AddChild(new_Node2d);
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