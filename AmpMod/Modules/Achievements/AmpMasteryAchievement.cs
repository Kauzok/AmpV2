using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("AmpMasteryUnlock", "Skins.RedSprite", null, null)]
    public class AmpMasteryAchievement : BaseAchievement
    {
        public string obliterateString = "mysteryspace";
        public string currentStageName;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("AmpBody");
        }
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += this.OnClientGameOverGlobal;
        }

        private void OnStageStart(Stage stage)
        {
            currentStageName = SceneManager.GetActiveScene().name;
           // Debug.Log("stage name is " + currentStageName);

        }

        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= this.OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }

        public override void OnInstall()
        {
            base.OnInstall();
            Stage.onStageStartGlobal += this.OnStageStart;
        }


        public override void OnUninstall()
        {
            Stage.onStageStartGlobal -= this.OnStageStart;
            base.OnUninstall();
        }

        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if (!runReport.gameEnding)
            {
                return;
            }
            //Debug.Log("game is ending");
            if (runReport.gameEnding.isWin || currentStageName == obliterateString)
            {
               // Debug.Log("player has won");
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
                if (difficultyDef != null && difficultyDef.countsAsHardMode)
                {
                  //  Debug.Log("granting achievement");
                    base.Grant();
                }
            }
        }

        /*  public override BodyIndex LookUpRequiredBodyIndex()
          {
              return BodyCatalog.FindBodyIndex("AmpBody");
          }

          */
    }
}
