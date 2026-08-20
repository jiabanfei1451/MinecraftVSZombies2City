using System;
using System.Linq;
using System.IO;
using Godot;
using System.Collections.Generic;

namespace DEBUG{
	static class Data
	{
		public static bool Enable = true;
        public static bool Auto_Add_Info = false;
	}
    /// <summary>
    /// 调试信息
    /// </summary>
	static class Info
	{
        public enum ERROR_Info
        {
            /// <summary>
            /// 没有脚本 错误代码: 0
            /// </summary>
            NOScript = 0,
            /// <summary>
            /// 物体没有脚本 错误代码 :1
            /// </summary>
            Object_no_Script = 1,
            /// <summary>
            /// 此方式,在某些情况下无法调用
            /// </summary>
            Invalid_method = 2,
        }
        public static string[] Info_Text = [];
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="what"></param>
        
        public static void PrintRich(params object[] what)
		{
			if (Data.Enable == true) {
			GD.PrintRich(what);
			}
            String sd = "";
            foreach (var ds in what)
            {
                sd += ds.ToString();
            }
            if (!Data.Auto_Add_Info){return;}
            List<String> s = new List<string>(Info_Text);
            s.Add("["+DateTimeOffset.Now.ToUniversalTime().ToString()+"]" + sd);
            Info_Text = s.ToArray<String>();
		}
        
        public static void Print(params object[] what)
		{
			if (Data.Enable == true) {
			GD.Print(what);
			}
            String sd = "";
            foreach (var ds in what)
            {
                if (ds != null){
                    sd += ds.ToString();
                }
            }
            if (!Data.Auto_Add_Info){return;}
            List<String> s = new List<string>(Info_Text);
            s.Add("["+DateTimeOffset.Now.ToUniversalTime().ToString()+"]" + sd);
            Info_Text = s.ToArray<String>();
		}
        public static void ERROR(ERROR_Info ERROR)
        {
            switch (ERROR)
            {
                case ERROR_Info.NOScript:
                    PrintErr("ERROR - 0:Script is Null");
                    break;
                case ERROR_Info.Object_no_Script:
                    PrintErr("ERROR - 1:Object not has Script");
                    break;
                case ERROR_Info.Invalid_method:
                    PrintErr($"ERROR - 2:Object Method is Invalid Because Method Script a Enable not true");
                    break;
            }
        }
		public static void PrintErr(params object[] what)
		{
			if (Data.Enable == true) {
			GD.PrintErr(what);
			}
            String sd = "";
            foreach (var ds in what)
            {
                sd += ds.ToString();
            }
            if (!Data.Auto_Add_Info){return;}
            List<String> s = new List<string>(Info_Text);
            s.Add("["+DateTimeOffset.Now.ToUniversalTime().ToString()+"]" + sd);
            Info_Text = s.ToArray<String>();
		} 
        public static void Save_Info(string Path)
        {
            StreamWriter stream = new StreamWriter(Path);
            foreach (String Text in Info_Text)
            {
                stream.WriteLine(Text);
            }
            stream.Close();
        }
	}
}