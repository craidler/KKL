namespace KKL
{
    // ReSharper disable once InconsistentNaming
    public class KKLModuleScience : PartModule
    {
        // ReSharper disable once UnusedMember.Global
        [KSPEvent(guiActive = false, guiName = "Run Experiments")]
        public void Run()
        {
            foreach (var m in vessel.FindPartModulesImplementing<ModuleScienceExperiment>())
            {
                if (m.IsRerunnable()) m.Events[m.experimentActionName].Invoke();
            }
        }
    }
}