using System;
using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Setting : Window
    {
        private static byte Alpha
        {
            get { return byte.Parse(Setting.Ui.GetValue("alpha")); }
            set { Setting.Ui.SetValue("alpha", value); }
        }

        private static byte Tint
        {
            get { return byte.Parse(Setting.Ui.GetValue("tint")); }
            set { Setting.Ui.SetValue("tint", value); }
        }
        
        private static byte Grid
        {
            get { return byte.Parse(Setting.Ui.GetValue("grid")); }   
            // ReSharper disable once PossibleLossOfFraction
            set { Setting.Ui.SetValue("grid", Math.Floor(value / 10f) * 10); }   
        }
        
        private static bool Snap
        {
            get { return bool.Parse(Setting.Ui.GetValue("snap")); }   
            set { Setting.Ui.SetValue("snap", value); }   
        }

        public Setting()
        {
            Id = 6661;
            Scenes = new List<GameScenes>{ GameScenes.EDITOR, GameScenes.FLIGHT };
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Grid");
            GUILayout.Label("Size  [" + Grid + "]");
            Grid = (byte) Math.Floor(GUILayout.HorizontalSlider(Grid, 10, 100));
            Snap = GUILayout.Toggle(Snap, "Snap");

            GUILayout.Label("Tint  [" + Tint + "]");
            Tint = (byte) Math.Floor(GUILayout.HorizontalSlider(Tint, byte.MinValue, byte.MaxValue));
            GUILayout.Label("Alpha [" + Tint + "]");
            Alpha = (byte) Math.Floor(GUILayout.HorizontalSlider(Alpha, byte.MinValue, byte.MaxValue));
            
            if (GUILayout.Button("save")) Setting.Save();
            
            GUILayout.EndVertical();
        }
    }
}