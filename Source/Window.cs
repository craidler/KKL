using System;
using System.Collections.Generic;
using UnityEngine;

namespace KKL
{
    public class Windows : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var w in Window.Windows) Debug.Log("[" + Setting.MODID + "] Window " + w.Key + " loaded");
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
        
        protected internal readonly string Key;

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

        protected internal string Title
        {
            get { return Config.GetValue("title"); }
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

        protected Window(string key)
        {
            Key = key;
            
            GameEvents.onGameSceneLoadRequested.Add(OnSceneLoad);
            GameEvents.onLevelWasLoadedGUIReady.Add(OnSceneReady);
            GameEvents.onHideUI.Add(OnHide);
            GameEvents.onShowUI.Add(OnShow);
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
            
            Area = GUILayout.Window(int.Parse(Config.GetValue("id")), Area, Draw, Title);
        }

        private void OnClose()
        {
            Show = false;
        }

        private void OnOpen()
        {
            Show = true;
        }

        private void OnHide()
        {
            _visible = false;
        }

        private void OnShow()
        {
            _visible = true;
        }

        private void OnSceneLoad(GameScenes scene)
        {
            _visible = false;
        }

        private void OnSceneReady(GameScenes scene)
        {
            _visible = true;
        }
    }
}