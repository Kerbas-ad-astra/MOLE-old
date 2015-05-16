using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;

/*
Source code copyright 2015, by Michael Billard (Angel-125)
License: CC BY-NC-SA 4.0
License URL: https://creativecommons.org/licenses/by-nc-sa/4.0/
Wild Blue Industries is trademarked by Michael Billard and may be used for non-commercial purposes. All other rights reserved.
Note that Wild Blue Industries is a ficticious entity 
created for entertainment purposes. It is in no way meant to represent a real entity.
Any similarity to a real entity is purely coincidental.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace WildBlueIndustries
{
    public class ModuleWB400 : ExtendedPartModule
    {
        [KSPField(isPersistant = true)]
        public string disableRCSOnPart = null;

        bool wasAttached;
        ModuleRCS parentRCS;

        public override void OnStart(StartState state)
        {
            string parentPartName = null;
            ModuleRCS rcsModule = null;
            base.OnStart(state);

            if (!HighLogic.LoadedSceneIsFlight)
                return;
            if (this.part.parent == null)
                return;

            //Find parent part. If it is the Backseat, then disable its RCS.
            if (this.part.parent.partInfo != null)
            {
                parentPartName = this.part.parent.partInfo.name.Replace('.', '_');
                if (parentPartName == disableRCSOnPart)
                {
                    rcsModule = this.part.parent.FindModuleImplementing<ModuleRCS>();
                    if (rcsModule == null)
                        return;
                    rcsModule.Disable();
                    parentRCS = rcsModule;
                    wasAttached = this.part.isAttached;
                }
            }

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (!HighLogic.LoadedSceneIsFlight)
                return;

            try
            {
                if (wasAttached && this.part.isAttached == false && parentRCS != null)
                    parentRCS.Enable();
            }
            catch (Exception ex)
            {
                Log(ex);
            }

        }
    }
}
