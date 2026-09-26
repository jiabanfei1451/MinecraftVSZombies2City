using Godot;
using Touch;
using DEBUG;
using Game;
using Game.AutoLoad;
using GameUI;
using Game.Cheak;
using System;
using System.Collections.Generic;
using System.Linq;
using Level.Object;
using Game.Static;
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
			List<String> strings = Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public Level.Object.LevelObject Has_Key_Object(String Key)
		{
			List<String> strings = Objects.Keys.ToList<String>();
			if (strings.IndexOf(Key) != -1)
			{
				return Objects[Key];
			}
			else
			{
				return null;
			}
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
	/// 放置
	/// </summary>
	public Level.Object.LevelObject Placed(bool Sousume = true)
	{
		Card_Data GetCard = Game.Get_GlobalNode.Get_Card_Data(GetTree());
		if (GetCard.Selected_raw_Object == null){return null;}
		if (GetCard.Selected_raw_Object.Card_Mode != Card.Mode.Gameing){return null;}
		Data.GlobalData GetCard_Data = GetCard.Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
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
		String Object_Type = GetCard_Data.Object_Type.ToString();
		if ((GetCard_Data.Reliant_UUID.Count > 0 || GetCard_Data.Reliant_Tag.Count > 0) && Current_Object.Has_Key_Object(Object_Type) != null)
		{
			if (!Cheak.Cheak_Data(Current_Object.Has_Key_Object(Object_Type),GetCard_Data)){return null;}
			if (GetCard_Data.Auto_Free_Reliant == true)
			{
				Current_Object.Objects[Object_Type].QueueFree();
			}
		}
		else if(GetCard_Data.Reliant_UUID.Count > 0 || GetCard_Data.Reliant_Tag.Count > 0)
		{
			return null;
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