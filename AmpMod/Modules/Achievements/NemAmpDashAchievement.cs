using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Achievements;
using RoR2.Stats;
using UnityEngine;


namespace AmpMod.Modules.Achievements
{
    [RegisterAchievement("NemAmpDashUnlock", "Skills.VoidDash", null, typeof(NemAmpDashAchievement.NemAmpDashServerAchievement))]
    public class NemAmpDashAchievement : BaseAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("NemAmpBody");
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        private static readonly float window = 180f;

        private class NemAmpDashServerAchievement : BaseServerAchievement
        {
            private Run.FixedTimeStamp expirationTimeStamp;

            //begins the timer and starts watching for when the stage is began/completed
            private void BeginStageTimer()
            {
                this.expirationTimeStamp = Run.FixedTimeStamp.now + NemAmpDashAchievement.window;
                Debug.Log("starting stage timer");
                Debug.Log("current time is  " + Run.FixedTimeStamp.tNow);
                Debug.Log("current expiration time is " + this.expirationTimeStamp.t);
            }

          
            public override void OnInstall()
            {
                base.OnInstall();
                Stage.onStageStartGlobal += this.OnStageStart;
                //Run.onRunStartGlobal += this.OnRunStart;
                //since onRunStart doesn't give us any results (is it not working?) and onStageStart only begins on the second stage, we use a manual check to start the timer on the first stage
                if (Run.instance)
                {
                    OnRunDiscovered(Run.instance);
                } 
                SceneExitController.onBeginExit += this.OnSceneBeginExit;


            }

            private void OnRunDiscovered(Run run)
            {
                Debug.Log("Run discovered");
                if (run.stageClearCount == 0)
                {
                    Debug.Log("running timer on first stage");
                    BeginStageTimer();
                }
            }

            public override void OnUninstall()
            {
                Stage.onStageStartGlobal -= this.OnStageStart;
                SceneExitController.onBeginExit -= this.OnSceneBeginExit;
                base.OnUninstall();
            }


            //if player exits stage in under 180 seconds grant achievement
            private void OnSceneBeginExit(SceneExitController exitController)
            {
                Debug.Log("doing scene exit");
                Debug.Log("exit time is  " + Run.FixedTimeStamp.tNow);
                Debug.Log("max exit time is " + this.expirationTimeStamp.t);

                if (!this.expirationTimeStamp.hasPassed)
                {
                    base.Grant();
                    Debug.Log("granting achievement");
                }

                Reset();
            }

            //don't begin the timer if we're in an 'illegitimate' stage
            private void OnStageStart(Stage stage)
            {
                Debug.Log("Running onStageStart");
                List<String> forbiddenStages = new List<string>{ "bazaar", "artifactworld", "limbo", "mysteryspace", "goldshores"};

                if (!forbiddenStages.Contains(stage.name))
                {
                    BeginStageTimer();
                }
                
            }

            private void OnRunStart(Run run)
            {
                Debug.Log("Running onRunStart");
                this.BeginStageTimer();
            }

            //reset timer method 
            private void Reset()
            {
                this.expirationTimeStamp = Run.FixedTimeStamp.negativeInfinity;
            }

        }

    }
}
