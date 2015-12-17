﻿using System;
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
    public class ModuleSASUpgrade : PartModule
    {
        [KSPField]
        public string levelOneNode;

        [KSPField]
        public string levelTwoNode;

        [KSPField]
        public string levelThreeNode;

        protected bool upgradeChecked = false;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX)
            {
                ModuleSAS sasModule = this.part.FindModuleImplementing<ModuleSAS>();
                if (sasModule != null)
                    sasModule.SASServiceLevel = 3;

                sasModule.OnAwake();
                sasModule.OnActive();

                //Switch ourself off.
                this.isEnabled = false;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (HighLogic.CurrentGame.Mode != Game.Modes.SANDBOX)
                checkForUpgrade();
        }

        protected void checkForUpgrade()
        {
            ModuleSAS sasModule = this.part.FindModuleImplementing<ModuleSAS>();
            if (sasModule == null)
                return;

            if (upgradeChecked)
                return;

            if (ResearchAndDevelopment.Instance != null && !upgradeChecked)
            {
                upgradeChecked = true;

                if (ResearchAndDevelopment.GetTechnologyState(levelThreeNode) != RDTech.State.Available)
                    sasModule.SASServiceLevel = 3;

                else if (ResearchAndDevelopment.GetTechnologyState(levelTwoNode) != RDTech.State.Available)
                    sasModule.SASServiceLevel = 2;

                else
                    sasModule.SASServiceLevel = 1;

                sasModule.OnAwake();
                sasModule.OnActive();

                //Switch ourself off.
                this.isEnabled = false;
            }
        }

    }
}