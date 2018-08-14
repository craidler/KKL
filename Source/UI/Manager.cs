using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Manager : Window
    {
        public Manager(int id) : base(id)
        {
            Scenes = new List<GameScenes> { GameScenes.EDITOR, GameScenes.FLIGHT };
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();

            foreach (var w in Windows)
            {
                if (w.Key == Key || !w.Allow) continue;
                w.Show = GUILayout.Toggle(w.Show, w.Name);
            }
            
            GUILayout.EndVertical();
        }
    }
}