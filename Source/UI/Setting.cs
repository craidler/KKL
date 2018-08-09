using System;
using System.Collections.Generic;
using UnityEngine;


namespace KKL.UI
{
    public class Setting : Window
    {
        private static byte Alpha
        {
            get { return byte.Parse(Ui.GetValue("alpha")); }
            set { Ui.SetValue("alpha", value); }
        }

        private static byte Tint
        {
            get { return byte.Parse(Ui.GetValue("tint")); }
            set { Ui.SetValue("tint", value); }
        }
        
        private static byte Grid
        {
            get { return byte.Parse(Ui.GetValue("grid")); }   
            // ReSharper disable once PossibleLossOfFraction
            set { Ui.SetValue("grid", Math.Floor((decimal) (value / 10)) * 10); }   
        }
        
        private static bool Snap
        {
            get { return bool.Parse(Ui.GetValue("snap")); }   
            set { Ui.SetValue("snap", value); }   
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
            
            if (GUILayout.Button("save")) KKL.Setting.Save();
            
            GUILayout.EndVertical();
        }
    }
}