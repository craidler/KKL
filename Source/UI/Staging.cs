using System.Collections.Generic;
using UnityEngine;

namespace KKL.UI
{
    public class Staging : Window
    {
        private bool FairingCamera
        {
            get { return bool.Parse(Config.GetValue("fairingCamera")); }
            set { if (FairingCamera != value) Config.SetValue("fairingCamera", value); }
        }
        
        public Staging()
        {
            Scenes = new List<GameScenes>
            {
                GameScenes.FLIGHT,
            };
            
            GameEvents.onStageActivate.Add(OnStageActivate);
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();

            FairingCamera = GUILayout.Toggle(FairingCamera, "Take fairing cam screenshot");
            
            GUILayout.EndVertical();
        }

        private void OnStageActivate(int stage)
        {
            var vessel = FlightGlobals.ActiveVessel;

            foreach (var p in vessel.Parts)
            {
                if (p.originalStage != stage) continue;

                foreach (var m in p.Modules)
                {
                    if (m.name != "MuMechModuleHullCameraZoom") continue;

                    foreach (var e in m.Events)
                    {
                        Debug.Log("[" + KKL.Setting.MODID + "] found event " + e.name);
                        if (e.name != "Activate camera") continue;
                        e.Invoke();
                    }
                    
                    ScreenCapture.CaptureScreenshot("fairing-deploy.png");
                    
                    foreach (var e in m.Events)
                    {
                        if (e.name != "Deactivate camera") continue;
                        e.Invoke();
                    }

                    return;
                }
            }
        }
    }
}