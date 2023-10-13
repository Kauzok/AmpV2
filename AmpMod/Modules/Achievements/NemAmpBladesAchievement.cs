using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;
using RoR2.Stats;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp;


namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("NemAmpBladesUnlock", "Skills.TrackingBlades", null, null)]//typeof(NemAmpBladesServerAchievement))]
    public class NemAmpBladesAchievement : BaseAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("NemAmpBody");
        }
		public override void OnBodyRequirementMet()
		{
			base.OnBodyRequirementMet();
			//base.SetServerTracked(true);
			FireLightningBeam.onPierce += this.OnPierce;
		}
		public override void OnBodyRequirementBroken()
		{
			//base.SetServerTracked(false);
			FireLightningBeam.onPierce -= this.OnPierce;
			base.OnBodyRequirementBroken();
		}


		private void OnPierce(FireLightningBeam state)
		{
			//Debug.Log("checking pierceCount");
			if (state.piercedCount >= requirement)
			{
				base.Grant();
			}
		}

		// Token: 0x04005094 RID: 20628
		private static readonly int requirement = 5;

		/* private class NemAmpBladesServerAchievement : BaseServerAchievement
        {
			private int hitCount;
			private bool hasFiredBeam;

			public override void OnInstall()
			{
				base.OnInstall();
				RoR2Application.onFixedUpdate += this.OnFixedUpdate;
				//GlobalEventManager.onServerDamageDealt += this.onServerDamageDealt;
				//FireLightningBeam.onFireBeam += this.OnFireBeam;
		
			}
			public override void OnUninstall()
			{
				//GlobalEventManager.onServerDamageDealt += this.onServerDamageDealt;
				RoR2Application.onFixedUpdate -= this.OnFixedUpdate;
				base.OnUninstall();
			}

			private void onServerDamage(DamageReport damageReport)
			{
				(if (damageReport.attackerMaster == base.networkUser.master && damageReport.attackerMaster != null && this.hasFiredBeam)
				{
					this.hitCount++;
					Debug.Log("beam hit count is " + hitCount);
					if (NemAmpBladesAchievement.requirement <= this.hitCount)
					{
						base.Grant();
					}
				}

			}

			private void OnFixedUpdate()
			{
				this.hitCount = 0;
				this.hasFiredBeam = false;
            }

            private void OnFireBeam(FireLightningBeam state)
            {
                Debug.Log("beam achievement has fired");
                this.hasFiredBeam = (state is FireLightningBeam);
            }

			}
		} */
    }
}
