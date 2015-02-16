using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WildBlueIndustries
{
    public class ModuleBackseatController : WBIAnimation
    {
        protected bool isDeploying = false;

        public void RemoveParentHeatShielding()
        {
            List<PartResource> doomedResources = new List<PartResource>();

            if (this.part.parent == null)
                return;

            //Look for ablative shielding in the parent part and remove it.
            //The backseat will have the shielding.
            foreach (PartResource resource in this.part.parent.Resources)
            {
                if (resource.resourceName == "AblativeShielding")
                {
                    resource.amount = 0.001;
                    resource.maxAmount = 0.001;
                    doomedResources.Add(resource);
                }
            }
            foreach (PartResource doomed in doomedResources)
                this.part.Resources.list.Remove(doomed);
        }

        public void OnEditorAttach()
        {
            RemoveParentHeatShielding();
        }

        public override void SetupAnimations()
        {
            base.SetupAnimations();

            Events["ToggleAnimation"].guiActive = false;
            Events["ToggleAnimation"].guiActiveEditor = false;
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            this.part.OnEditorAttach += OnEditorAttach;

            RemoveParentHeatShielding();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //If RCS is activated, deploy the periscope
            if (isDeploying == false && isDeployed == false && vessel.ActionGroups[KSPActionGroup.RCS])
            {
                Log("Deploying scope");
                isDeploying = true;
                isDeployed = true;
                PlayAnimation(!isDeployed);
            }

            //If RCS is disabled, retract the periscope
            else if (isDeploying == false && isDeployed == true && !vessel.ActionGroups[KSPActionGroup.RCS])
            {
                Log("Retracting scope");
                isDeploying = true;
                isDeployed = false;
                PlayAnimation(!isDeployed);
            }

            //Done playing animation?
            if (animationState.normalizedTime == 0f)
                isDeploying = false;
        }
    }
}
