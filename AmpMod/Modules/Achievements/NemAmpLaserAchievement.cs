using RoR2;
using RoR2.Achievements;
using RoR2.Stats;


namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("NemAmpLaserUnlock", "Skills.Laser", null, null)]
    class NemAmpLaserAchievement : BaseStatMilestoneAchievement
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
				return 50UL;
			}
		}
	}
}

