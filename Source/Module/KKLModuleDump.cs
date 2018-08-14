using System.Collections.Generic;
using UnityEngine;

namespace KKL
{
    // ReSharper disable once InconsistentNaming
    public class KKLModuleDump : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false, guiName = "Dump limit", guiFormat = "S", guiUnits = "%")]
        [UI_FloatEdit(scene = UI_Scene.Flight, minValue = 0f, maxValue = 100f, incrementLarge = 10f, incrementSmall = 1f, incrementSlide = 10f)]
        public float Limit;

        [KSPField(guiActive = false, guiName = "Dump resource")]
        [UI_Cycle(stateNames = new []{ "All", "Lf", "Ox", "Mp", "Lf+Ox", "Lf+Ox+Mp", "Ore" })]
        public string Resource = "All";
        
        private readonly Dictionary<string, List<string>> _affect = new Dictionary<string, List<string>>
        {
            { "All", new List<string>{ "LiquidFuel", "Oxidizer", "MonoPropellant", "Ore" }},
            { "Lf", new List<string>{ "LiquidFuel" } },
            { "Ox", new List<string>{ "Oxidizer" } },
            { "Mp", new List<string>{ "MonoPropellant" } },
            { "Lf+Ox", new List<string>{ "LiquidFuel", "Oxidizer" } },
            { "Lf+Ox+Mp", new List<string>{ "LiquidFuel", "Oxidizer", "MonoPropellant" } },
            { "Ore", new List<string>{ "Ore" } },
        };

        // ReSharper disable once UnusedMember.Global
        [KSPEvent(guiActive = false, guiName = "Dump")]
        public void Dump()
        {
            foreach (var r in part.Resources)
            {
                if (!_affect[Resource].Contains(r.resourceName)) continue;
                var amount = 0d;
                var original = r.amount;
                if (Limit > 0) amount = r.maxAmount * Limit / 100;
                if (r.amount <= amount) return;
                r.amount = amount;
            }
        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            Setup();
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            Setup();
        }

        private void Setup()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;

            foreach (var r in part.Resources)
            {
                if (!_affect["All"].Contains(r.resourceName) || r.amount <= 0) continue;
                Fields["Limit"].guiActive = true;
                Fields["Resource"].guiActive = true;
                Events["Dump"].guiActive = true;
                return;
            }
        }
    }
}