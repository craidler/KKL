using System;
using System.Collections.Generic;
using System.IO;
using KSP.UI.Screens;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace KKL.UI
{
    // [KSPAddon(KSPAddon.Startup.FlightAndEditor, true)]
    public class Windows : MonoBehaviour
    {
        public static Texture2D Button
        {
            get
            {
                var texture = new Texture2D(0, 0);
                texture.LoadImage(File.ReadAllBytes(Window.Setting.Path + "/Plugins/Icons/Launcher.png"));
                return texture;
            }
        }
        
        private ApplicationLauncherButton _launcher;
        
        private void Awake()
        {
            Util.Log(this, string.Format("Awake {0:0} windows", Window.Windows.Count));
        }

        private void Start()
        {
            Util.Log(this, "Start");
            
            DontDestroyOnLoad(this);
            
            GameEvents.onGUIApplicationLauncherReady.Add(() =>
            {
                if (_launcher) return;

                _launcher = ApplicationLauncher.Instance.AddModApplication(
                    () => Window.Manager.Open(),
                    () => Window.Manager.Close(),
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.FLIGHT,
                    Button
                );
            });
        }

        private void OnGUI()
        {
            foreach (var w in Window.Windows) w.OnGUI();
        }
    }
    
    public class Window
    {
        public static readonly List<Window> Windows = new List<Window>
        {
            new Manager(6660),
            new Setting(6661),
            new Flight(6662),
            new Launchpad(6663),
            new Manifest(6664),
            new Orbit(6665),
            new Refuel(6666),
        };

        public static Window Manager
        {
            get
            {
                foreach (var w in Windows) if (w is Manager) return w;
                return null;
            }
        }

        protected internal static KKL.Setting Setting
        {
            get { return KKL.Setting.Instance; }
        }

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
                Config.SetValue("x", Area.x);
                Config.SetValue("y", Area.y);
                Setting.Save();
            }
        }
        
        protected internal bool Visible { get; set; }

        protected internal bool Allow
        {
            get { return Scenes.Contains(HighLogic.LoadedScene); }   
        }

        protected ConfigNode Data
        {
            get { return Config.GetNode("DATA"); }
        }

        protected readonly int Id;
        protected readonly ConfigNode Config;
        protected float[] Size = { 200f, 100f };
        protected List<GameScenes> Scenes;
        protected Rect Area;

        protected Window(int id)
        {
            Id = id;
            
            // Load config of window
            Config = Setting.Load(this);
            
            // Initialize area of window
            Area.x = float.Parse(Config.GetValue("x"));
            Area.y = float.Parse(Config.GetValue("y"));
            Area.width = Size[0];
            Area.height = Size[1];

            // React on some game events
            GameEvents.onGameSceneLoadRequested.Add(scene => { Visible = false; });
            GameEvents.onLevelWasLoadedGUIReady.Add(scene => { Visible = true; });
            GameEvents.onHideUI.Add(() => { Visible = false; });
            GameEvents.onShowUI.Add(() => { Visible = true; });
        }

        private void Draw(int id)
        {
            GUI.DragWindow(new Rect(0, 0, Area.width - 20, 20));
            DrawContent();
        }

        protected virtual void DrawContent()
        {
        }

        public void OnGUI()
        {
            try
            {
                // Dont render anything
                if (!Allow || !Show || !Visible) return;
            
                // Snap to grid
                if (bool.Parse(Setting.Ui.GetValue("snap")))
                {
                    var s = float.Parse(Setting.Ui.GetValue("grid"));
                    Area.x = (float) Math.Floor(Area.x / s) * s;
                    Area.y = (float) Math.Floor(Area.y / s) * s;
                }
                
                // Stick within screen
                if (Area.x < 0) Area.x = 0;
                if (Area.y < 0) Area.y = 0;
                if (Area.x + Area.width > Screen.width) Area.x = Screen.width - Area.width;
                if (Area.y + Area.height > Screen.height) Area.y = Screen.height - Area.height;
            
                Area = GUILayout.Window(Id, Area, Draw, Name);
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