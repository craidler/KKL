using System.Collections.Generic;
using Contracts.Parameters;
using Contracts.Predicates;
using UnityEngine;

namespace KKL.UI
{
    public class Flight : Window
    {
        private static Vessel Vessel
        {
            get { return FlightGlobals.ActiveVessel; }
        }
        
        public Flight(int id) : base(id)
        {
            Scenes = new List<GameScenes>() { GameScenes.FLIGHT };
        }

        protected override void DrawContent()
        {
            GUILayout.BeginVertical();
            
            GUILayout.Label("Deployables");
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Antennae");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("DPLY")) Deploy<ModuleDeployableAntenna>();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("RTRT")) Retract<ModuleDeployableAntenna>();
            GUILayout.EndVertical();            
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Ladders");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("DPLY")) DeployLadder();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("RTRT")) RetractLadder();
            GUILayout.EndVertical();            
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Radiators");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("DPLY")) Deploy<ModuleDeployableRadiator>();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("RTRT")) Retract<ModuleDeployableRadiator>();
            GUILayout.EndVertical();            
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Solarpanels");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("DPLY")) Deploy<ModuleDeployableSolarPanel>();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("RTRT")) Retract<ModuleDeployableSolarPanel>();
            GUILayout.EndVertical();            
            GUILayout.EndHorizontal();

            GUILayout.Label("Engines");

            if (GUILayout.Button("Shutdown"))
            {
                foreach (var m in Vessel.FindPartModulesImplementing<ModuleEngines>()) m.Events["shutdown engine"].Invoke();
            }
            
            GUILayout.Label("Experiments");

            if (GUILayout.Button("RUN"))
            {
                foreach (var m in Vessel.FindPartModulesImplementing<ModuleScienceExperiment>()) m.Events[m.experimentActionName].Invoke();
            }
            
            GUILayout.EndVertical();
        }
        
        private static void DeployLadder()
        {
            foreach (var m in Vessel.FindPartModulesImplementing<PartModule>())
            {
                var l = m as RetractableLadder;
                if (!l) continue;
                l.Events["extend"].Invoke();
            }
        }
        
        private static void RetractLadder()
        {
            foreach (var m in Vessel.FindPartModulesImplementing<PartModule>())
            {
                var l = m as RetractableLadder;
                if (!l) continue;
                l.Events["retract"].Invoke();
            }
        }

        private static void Deploy<T>() where T : ModuleDeployablePart
        {
            foreach (var m in Vessel.FindPartModulesImplementing<T>()) m.Events[m.extendActionName].Invoke();
        }
        
        private static void Retract<T>() where T : ModuleDeployablePart
        {
            foreach (var m in Vessel.FindPartModulesImplementing<T>()) m.Events[m.retractActionName].Invoke();
        }
    }
}