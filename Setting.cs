using System.IO;

namespace KKL
{
    public class Setting
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBePrivate.Global
        public const string MODID = "KKL";
        
        private const string Default = "Data/Default.cfg";
        private const string User = "Data/User.cfg";
        
        private static Setting _instance;
        
        public static Setting Instance
        {
            get
            {
                if (null != _instance) return _instance;
                if (!File.Exists(User)) File.Copy(Default, User);
                return _instance = new Setting(User);
            }
        }
        
        private readonly ConfigNode _config;

        private Setting(string path)
        {
            _config = ConfigNode.Load(path);
        }
        
        public ConfigNode Load(string key)
        {
            return _config.GetNode(MODID).GetNode(key);
        }

        public ConfigNode Load(Window window)
        {
            return _config.GetNode(MODID).GetNode("WINDOW").GetNode(window.Key);
        }

        public void Save(Window window)
        {
            _config.GetNode(MODID).GetNode("WINDOW").SetNode(window.Key, window.Config);
            _config.Save(User);
        }
    }
}