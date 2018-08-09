using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KKL
{
    public class Setting
    {
        // ReSharper disable once InconsistentNaming
        public const string MODID = "KKL";

        public static readonly Setting Instance = new Setting();

        public string Path
        {
            get { return Application.dataPath + "/../GameData/" + MODID; }
        }
        
        public ConfigNode Ui
        {
            get { return Config.GetNode(MODID).GetNode("UI"); }
        }

        private ConfigNode Config
        {
            get
            {
                if (_config != null) return _config;
                if (!File.Exists(_files["user"])) File.Copy(_files["default"], _files["user"]);
                return _config = ConfigNode.Load(_files["user"]);
            }
        }

        private readonly Dictionary<string, string> _files;
        private ConfigNode _config;

        private Setting()
        {
            _files = new Dictionary<string, string>
            {
                { "default", Path + "/Plugins/Data/Default.cfg" },
                { "user", Path + "/Plugins/Data/User.cfg" }
            };
        }

        public ConfigNode Load(UI.Window window)
        {
            var w = Config.GetNode(MODID).GetNode("WINDOW");
            
            if (w.HasNode(window.Key)) return w.GetNode(window.Key);
            
            var n = new ConfigNode(window.Key);
            
            n.SetValue("show", true, true);
            n.SetValue("x", 100, true);
            n.SetValue("y", 100, true);
            
            w.AddNode(n);
            
            return n;
        }

        public void Save()
        {
            Config.Save(_files["user"]);
        }
    }
}