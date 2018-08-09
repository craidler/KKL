using System.IO;
using UnityEngine;

namespace KKL
{
    public static class Setting
    {
        // ReSharper disable once InconsistentNaming
        public const string MODID = "KKL";
        
        private static readonly string Default = Path + "/Plugins/Data/Default.cfg";
        private static readonly string User = Path + "/Plugins/Data/User.cfg";
        
        private static ConfigNode _config;

        private static ConfigNode Config
        {
            get
            {
                if (_config != null) return _config;
                if (!File.Exists(User)) File.Copy(Default, User);
                return _config = ConfigNode.Load(User);
            }
        }

        public static string Path
        {
            get { return Application.dataPath + "/../GameData/" + MODID; }
        }
        
        public static ConfigNode Load(string key)
        {
            return Config.GetNode(MODID).GetNode(key);
        }

        public static ConfigNode Load(UI.Window window)
        {
            var w = Load("WINDOW");
            
            if (w.HasNode(window.Key)) return w.GetNode(window.Key);
            
            var n = new ConfigNode(window.Key);
            
            n.SetValue("show", true, true);
            n.SetValue("x", 100, true);
            n.SetValue("y", 100, true);
            
            w.AddNode(n);
            
            return n;
        }

        public static void Save()
        {
            Config.Save(User);
        }
    }
}