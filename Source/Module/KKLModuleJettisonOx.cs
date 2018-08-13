namespace KKL
{
    // ReSharper disable once InconsistentNaming
    public class KKLModuleJettisonOx : KKLModuleJettisonResource
    {
        [KSPEvent(guiActive = true, guiName = "Jettison Oxidizer")]
        public void Jettison()
        {
            JettisonResource("Oxidizer");
        }
    }
}