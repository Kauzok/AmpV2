using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;
using RoR2.Achievements;

namespace AmpMod.Modules.Achievements
{
	[RegisterAchievement("AmpWormUnlock", "Skills.SummonWurm", null, typeof(AmpWormAchievement.AmpWormServerAchievement))]
	public class AmpWormAchievement : BaseAchievement
    {

		public override BodyIndex LookUpRequiredBodyIndex()
		{
			return BodyCatalog.FindBodyIndex("AmpBody");
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x0015D621 File Offset: 0x0015B821
		public override void OnBodyRequirementMet()
		{
			base.OnBodyRequirementMet();
			base.SetServerTracked(true);
		}

		// Token: 0x0600577E RID: 22398 RVA: 0x0015D630 File Offset: 0x0015B830
		public override void OnBodyRequirementBroken()
		{
			base.SetServerTracked(false);
			base.OnBodyRequirementBroken();
		}
		private class AmpWormServerAchievement : BaseServerAchievement
		{
			private BodyIndex overloadingWormBodyIndex;

			// Token: 0x06005780 RID: 22400 RVA: 0x001618C0 File Offset: 0x0015FAC0
			public override void OnInstall()
			{
				base.OnInstall();
				this.overloadingWormBodyIndex = BodyCatalog.FindBodyIndex("ElectricWormBody");
				GlobalEventManager.onCharacterDeathGlobal += this.OnCharacterDeathGlobal;
			}

			// Token: 0x06005781 RID: 22401 RVA: 0x00161935 File Offset: 0x0015FB35
			public override void OnUninstall()
			{
				GlobalEventManager.onCharacterDeathGlobal -= this.OnCharacterDeathGlobal;
				base.OnUninstall();
			}

			// Token: 0x06005783 RID: 22403 RVA: 0x001619C0 File Offset: 0x0015FBC0
			private void OnCharacterDeathGlobal(DamageReport damageReport)
			{
				if (damageReport.victimBody && damageReport.victimBody.bodyIndex == this.overloadingWormBodyIndex && base.IsCurrentBody(damageReport.damageInfo.attacker) && DoesDamageQualify(damageReport))
				{
					base.Grant();
				}
			}

			// Token: 0x06005784 RID: 22404 RVA: 0x00161A2C File Offset: 0x0015FC2C
			private bool DoesDamageQualify(DamageReport damageReport)
			{
				return (damageReport.damageInfo.HasModdedDamageType(DamageTypes.apply2Charge));
			}

		}
	}


}

