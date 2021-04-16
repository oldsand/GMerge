namespace GalaxyMerge.Archestra.Options
{
    public class DeploymentOptions
    {
        public CurrentlyDeployedOption DeployedOption { get; set; }
        public bool SkipUnDeployed { get; set; }
        public bool DeployOnScan { get; set; }
        public bool ForceOffScan { get; set; }
        public bool Cascade { get; set; }
        public bool MarkAsDeployOnStatusMismatch { get; set; }

        public DeploymentOptions WithDeployedOption(CurrentlyDeployedOption deployedOption)
        {
            DeployedOption = deployedOption;
            return this;
        }
        
        public DeploymentOptions SkipsUnDeployed()
        {
            SkipUnDeployed = true;
            return this;
        }
        
        public DeploymentOptions DeploysOnScan()
        {
            DeployOnScan = true;
            return this;
        }
        
        public DeploymentOptions ForcesOffScan()
        {
            ForceOffScan = true;
            return this;
        }
        
        public DeploymentOptions Cascades()
        {
            Cascade = true;
            return this;
        }
        
        public DeploymentOptions MarksDeployedOnMismatch()
        {
            MarkAsDeployOnStatusMismatch = true;
            return this;
        }
    }
}