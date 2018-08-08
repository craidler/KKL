using System;
using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Setting : Window
    {
        private static ConfigNode Ui
        {
            get { return KKL.Setting.Instance.Load("UI"); }
        }

        private static byte Alpha
        {
            get { return byte.Parse(Ui.GetValue("alpha")); }
            set { if (Alpha != value) Ui.SetValue("alpha", Alpha); }
        }

        private static byte Tint
        {
            get { return byte.Parse(Ui.GetValue("tint")); }
            set { if (Tint != value) Ui.SetValue("tint", Tint); }
        }
        
        private static bool Grid
        {
            get { return bool.Parse(Ui.GetValue("grid")); }
            set { if (Grid != value) Ui.SetValue("grid", value); }
        }
        
        public Setting()
        {
            Scenes = new List<GameScenes>
            {
                GameScenes.EDITOR,
                GameScenes.FLIGHT,
            };
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();

            Alpha = (byte) Math.Floor(GUILayout.HorizontalSlider(Alpha, 0, 255));
            Tint = (byte) Math.Floor(GUILayout.HorizontalSlider(Tint, 0, 255));
            Grid = GUILayout.Toggle(Grid, "Snap to grid");
            
            GUILayout.EndVertical();
        }
    }
}