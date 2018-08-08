using System;
using System.Collections.Generic;
using UnityEngine;

namespace KKL
{
    public class Windows : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var w in Window.Windows) Debug.Log("[" + Setting.MODID + "] Window " + w.Name + " loaded");
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void OnGUI()
        {
            foreach (var w in Window.Windows) w.OnGUI();
        }
    }
    
    public class Window
    {
        public static IEnumerable<Window> Windows
        {
            get
            {
                return new List<Window>
                {
                    new UI.Manager(),
                    new UI.Manifest(),
                    new UI.Orbit(),
                    new UI.Refuel(),
                    new UI.Setting(),
                };
            }
        }
        
        protected internal string Name
        {
            get { return GetType().Name; }
        }

        protected internal bool Show
        {
            get
            {
                return bool.Parse(Config.GetValue("show"));
            }
            
            set
            {
                var show = bool.Parse(Config.GetValue("show"));
                if (show == value) return;
                Config.SetValue("show", value);
                Setting.Instance.Save(this);
            }
        }

        protected internal bool Allow
        {
            get { return Scenes.Contains(HighLogic.LoadedScene); }   
        }
        
        protected internal ConfigNode Config
        {
            get { return Setting.Instance.Load(this); }
        }

        protected List<GameScenes> Scenes;

        private Rect Area
        {
            get
            {
                return Area = new Rect(
                    float.Parse(Config.GetValue("x")),
                    float.Parse(Config.GetValue("y")),
                    100,
                    100
                );
            }

            set
            {
                Config.SetValue("x", value.x);
                Config.SetValue("y", value.y);
            }
        }

        private bool _visible;

        protected Window()
        {
            GameEvents.onGameSceneLoadRequested.Add(scene => { _visible = false; });
            GameEvents.onLevelWasLoadedGUIReady.Add(scene => { _visible = true; });
            GameEvents.onHideUI.Add(() => { _visible = false; });
            GameEvents.onShowUI.Add(() => { _visible = true; });
        }

        private void Draw(int id)
        {
            GUI.DragWindow(new Rect(0, 0, Area.width - 20, 20));
            
            DrawTitle();
            DrawContent();
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void DrawTitle()
        {
            
        }

        protected virtual void DrawContent()
        {
            
        }

        // ReSharper disable once InconsistentNaming
        public void OnGUI()
        {
            if (!Allow || !_visible || !Show) return;
            
            // Snap to grid
            if (bool.Parse(Setting.Instance.Load("UI").GetValue("grid")))
            {
                var a = Area;
                
                a.x = (float) Math.Floor(a.x / 10) * 10;
                a.y = (float) Math.Floor(a.y / 10) * 10;

                Area = a;
            }
            
            Area = GUILayout.Window(int.Parse(Config.GetValue("id")), Area, Draw, Name);
        }

        private void OnClose()
        {
            Show = false;
        }

        private void OnOpen()
        {
            Show = true;
        }
    }
}