using System.Collections.Generic;

namespace KKL.UI
{
    public class Manifest : Window
    {
        public Manifest() : base("MANIFEST")
        {
            Scenes = new List<GameScenes>
            {
                GameScenes.EDITOR,
                GameScenes.FLIGHT,
            };
        }

        protected override void DrawContent()
        {
            var vessel = EditorLogic.RootPart.vessel;
            
            Util.Table(new Dictionary<string, string>
            {
                { "Crew", string.Format("{0:0}/{1:0}", vessel.GetCrewCount(), vessel.GetCrewCapacity())},
                { "Mass", string.Format("{0:F3} t", vessel.totalMass / 1000) },
                { "Part", string.Format("{0:0}", vessel.Parts.Count) },
            });
        }
    }
}