using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;

namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("AmpGrandMasteryUnlock", "Skins.Reformation", null, null)]
    public class AmpGrandMasteryAchievement : BaseEndingAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("AmpBody");
        }

        public override bool ShouldGrant(RunReport runReport)
        {
            //DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
            //if (runReport.gameEnding && runReport.gameEnding.isWin && this.localUser.cachedBody.bodyIndex == this.requiredBodyIndex && runDifficulty.scalingValue >= 3.5)
           // {
            //    return true;
           // }
            return false;
        }
    }
}
