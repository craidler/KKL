namespace KKL
{
    // ReSharper disable once InconsistentNaming
    public class KKLModuleJettisonResource : PartModule
    {
        protected void JettisonResource(string resource)
        {
            foreach (var r in part.Resources)
            {
                if (r.resourceName != resource) continue;
                r.amount = 0;                    
                return;
            }
        }
    }
}