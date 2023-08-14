using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("NemAmpMasteryUnlock", "Skins.Origin", null, null)]
    public class NemAmpMasteryAchievement : BasePerSurvivorClearGameMonsoonAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("NemAmpBody");
        }

        /* public override bool ShouldGrant(RunReport runReport)
        {
            
            DifficultyDef runDifficulty = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
            
            bool isObliteration = false;
            if (runReport.gameEnding = RoR2Content.GameEndings.ObliterationEnding) isObliteration = true;
            bool validEnding = runReport.gameEnding.isWin || isObliteration;

            //RoR2.Console.print("Whether or not we have a valid ending:" + validEnding);
            if (runReport.gameEnding && validEnding && this.localUser.cachedBody.bodyIndex == this.requiredBodyIndex && runDifficulty.scalingValue >= 3)
            {
                return true;
            }
            return false;
        } */
    }
}
