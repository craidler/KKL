using System;
using System.Collections.Generic;
using System.IO;
using KSP.UI.Screens;
using UnityEngine;

namespace KKL.UI
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, true)]
    public class Windows : MonoBehaviour
    {
        private ApplicationLauncherButton _launcher;
        
        private void Awake()
        {
            Util.Log(this, "Awake");
            foreach (var w in Window.Windows) Util.Log(w, "awoke");
        }

        private void Start()
        {
            Util.Log(this, "Start");
            DontDestroyOnLoad(this);
            GameEvents.onGUIApplicationLauncherReady.Add(OnLauncherReady);
        }

        private void OnGUI()
        {
            foreach (var w in Window.Windows) w.OnGUI();
        }

        private void OnLauncherReady()
        {
            if (_launcher) return;

            var texture = new Texture2D(0, 0);
            texture.LoadImage(File.ReadAllBytes(KKL.Setting.Path + "/Plugins/Icons/Launcher.png"));

            _launcher = ApplicationLauncher.Instance.AddModApplication(
                () => Window.Manager.Open(),
                () => Window.Manager.Close(),
                null,
                null,
                null,
                null,
                ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.FLIGHT,
                texture
            );
        }
    }
    
    public class Window
    {
        public static readonly List<Window> Windows = new List<Window>
        {
            new Launchpad(),
            new Manager(),
            new Manifest(),
            new Orbit(),
            new Refuel(),
            new Setting(),
        };

        protected static ConfigNode Ui
        {
            get { return KKL.Setting.Load("UI"); }
        }

        public static Window Manager
        {
            get
            {
                foreach (var w in Windows) if (w is Manager) return w;
                return null;
            }
        }

        protected int Id = 0;

        protected internal string Key
        {
            get { return Name.ToUpper(); }
        }
        
        protected internal string Name
        {
            get { return GetType().Name; }
        }

        protected internal bool Show
        {
            get { return bool.Parse(Config.GetValue("show")); }
            set
            {
                if (Show == value) return;
                Config.SetValue("show", value, true);
                Config.SetValue("x", _area.x);
                Config.SetValue("y", _area.y);
                KKL.Setting.Save();
            }
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        protected internal bool Visible { get; set; }

        protected internal bool Allow
        {
            get { return Scenes.Contains(HighLogic.LoadedScene); }   
        }

        protected ConfigNode Data
        {
            get { return Config.GetNode("DATA"); }
        }

        protected readonly ConfigNode Config;
        protected float[] Size = { 200f, 100f };
        protected List<GameScenes> Scenes;
        
        private Rect _area;

        protected Window()
        {
            GameEvents.onGameSceneLoadRequested.Add(scene => { Visible = false; });
            GameEvents.onLevelWasLoadedGUIReady.Add(scene => { Visible = true; });
            GameEvents.onHideUI.Add(() => { Visible = false; });
            GameEvents.onShowUI.Add(() => { Visible = true; });
            
            Config = KKL.Setting.Load(this);
            
            _area.x = float.Parse(Config.GetValue("x"));
            _area.y = float.Parse(Config.GetValue("y"));
            _area.width = Size[0];
            _area.height = Size[1];
        }

        private void Draw(int id)
        {
            GUI.DragWindow(new Rect(0, 0, _area.width - 20, 20));
            
            DrawContent();
        }

        protected virtual void DrawContent()
        {
        }

        // ReSharper disable once InconsistentNaming
        public void OnGUI()
        {
            try
            {
                if (!Allow || !Show || !Visible) return;
            
                // Snap to grid
                if (bool.Parse(Ui.GetValue("snap")))
                {
                    var s = float.Parse(Ui.GetValue("grid"));
                    _area.x = (float) Math.Floor(_area.x / s) * s;
                    _area.y = (float) Math.Floor(_area.y / s) * s;
                }
                
                // Stick within screen
                if (_area.x < 0) _area.x = 0;
                if (_area.y < 0) _area.y = 0;
                if (_area.x + _area.width > Screen.width) _area.x = Screen.width - _area.width;
                if (_area.y + _area.height > Screen.height) _area.y = Screen.height - _area.height;
            
                _area = GUILayout.Window(Id, _area, Draw, Name);
            }
            catch (Exception e)
            {
                Util.Log(this, e.Message);
            }
        }

        internal void Close()
        {
            Show = false;
        }

        internal void Open()
        {
            Show = true;
        }
    }
}