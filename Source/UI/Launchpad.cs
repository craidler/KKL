using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Launchpad : Window
    {
        private bool Rigid
        {
            get { return bool.Parse(Data.GetValue("rigid")); }
            set { Data.SetValue("rigid", value); }
        }
        
        public Launchpad(int id) : base(id)
        {
            Scenes = new List<GameScenes> { GameScenes.FLIGHT };
            
            GameEvents.onLevelWasLoadedGUIReady.Add(OnLevelLoaded);
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();
            Rigid = GUILayout.Toggle(Rigid, "Rigid");
            GUILayout.EndVertical();
        }

        private void OnLevelLoaded(GameScenes scene)
        {
            if (scene != GameScenes.FLIGHT || !Rigid) return;
            foreach (var p in FlightGlobals.ActiveVessel.Parts) p.rigidAttachment = true;
            Util.Message("Rigid attachment has been set throughout the vessel");
        }
    }
}