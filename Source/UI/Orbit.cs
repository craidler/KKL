using System.Collections.Generic;

namespace KKL.UI
{
    public class Orbit : Window
    {
        public Orbit(int id) : base(id)
        {
            Scenes = new List<GameScenes> { GameScenes.FLIGHT };
        }

        protected override void DrawContent()
        {
            var orbit = FlightGlobals.ActiveVessel.GetOrbit();

            Util.Columns(new [] { "Parameter", "Value" });
            Util.Table(new Dictionary<string, string>
            {
                { "ApA", string.Format("{0:F3} km", orbit.ApA / 1000) },
                { "PeA", string.Format("{0:F3} km", orbit.PeA / 1000) },
                { "ApT", KSPUtil.PrintDateCompact(orbit.timeToAp, false, true) },
                { "PeT", KSPUtil.PrintDateCompact(orbit.timeToPe, false, true) },
                { "Alt", string.Format("{0:F3} km", orbit.altitude / 1000) },
                { "Inc", string.Format("{0:F3} °", orbit.inclination) },
                { "Ecc", string.Format("{0:F3}", orbit.eccentricity) },
                { "Per", KSPUtil.PrintDateCompact(orbit.period, false, true) },
            });
        }
    }
}