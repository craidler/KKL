namespace KKL
{
    // ReSharper disable once InconsistentNaming
    public class KKLModuleJettisonLf : KKLModuleJettisonResource
    {
        [KSPEvent(guiActive = true, guiName = "Jettison Liquid Fuel")]
        public void Jettison()
        {
            JettisonResource("LiquidFuel");
        }
    }
}