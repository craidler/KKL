using System.Collections.Generic;

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
    }
}