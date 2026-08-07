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
                sd += ds.ToString();
            }
            if (!Data.Auto_Add_Info){return;}
            List<String> s = new List<string>(Info_Text);
            s.Add("["+DateTimeOffset.Now.ToUniversalTime().ToString()+"]" + sd);
            Info_Text = s.ToArray<String>();
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