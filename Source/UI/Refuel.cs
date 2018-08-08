using System.Collections.Generic;

namespace KKL.UI
{
    public class Refuel : Window
    {
        public Refuel() : base("REFUEL")
        {
            Scenes = new List<GameScenes>
            {
                GameScenes.FLIGHT,
            };
        }

        protected override void DrawContent()
        {
            var resources = new Dictionary<string, string>();
            var vessel = FlightGlobals.ActiveVessel;

            foreach (var p in vessel.Parts)
            {
                foreach (var r in p.Resources)
                {
                    if (!resources.ContainsKey(r.resourceName)) resources.Add(r.resourceName, "0");
                    resources[r.resourceName] = string.Format("{0:F3}", float.Parse(resources[r.resourceName]) + r.maxAmount - r.amount);
                }
            }
            
            Util.Table(resources);
        }
    }
}