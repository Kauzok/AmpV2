using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;
using RoR2.Stats;

namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("AmpPlasmaUnlock", "Skills.PlasmaSlash", null, null)]
	public class AmpPlasmaAchievement: BaseStatMilestoneAchievement
	{

		public override StatDef statDef
		{
			get
			{
				return Survivors.Amp.ampTotalBurnedEnemiesKilled;
			}
		}

		public override ulong statRequirement 
		{
			get
			{
				return 100UL;
			}
		}
	}
}
