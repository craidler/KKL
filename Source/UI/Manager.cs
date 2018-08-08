using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Manager : Window
    {
        public Manager() : base("MANAGER")
        {
            Scenes = new List<GameScenes>{
                GameScenes.EDITOR,
                GameScenes.FLIGHT,
            };
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();

            foreach (var window in Windows)
            {
                if (window.Equals(this) || !window.Allow) continue;
                window.Show = GUILayout.Toggle(window.Show, window.Title);
            }
            
            GUILayout.EndVertical();
        }
    }
}