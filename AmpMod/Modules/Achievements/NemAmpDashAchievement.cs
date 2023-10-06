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
            private ToggleAction listenForStageCompleted;
            private ToggleAction listenForStageStart;

            //begins the timer and starts watching for when the stage is began/completed
            private void BeginStageTimer()
            {
                this.expirationTimeStamp = Run.FixedTimeStamp.now + NemAmpDashAchievement.window;
                //this.listenForStageStart.SetActive(false);
                Debug.Log("starting stage timer");
                //this.listenForStageCompleted.SetActive(true);
            }

          
            public override void OnInstall()
            {
                base.OnInstall();

                //adds in our onstagestart & onscenebegin methods
                //these toggleactions essentially act as "watch for something until the thing happens, then stop watching for the thing"
                // e.g. setting listenForStageStart as active adds our OnStageStart code to the global method, and unsetting it as active removes our code
                #region actiontoggles (commented out)
                /*   this.listenForStageStart = new ToggleAction(delegate ()
                   {
                       Stage.onStageStartGlobal += this.OnStageStart;
                   }, delegate ()
                   {
                       Stage.onStageStartGlobal -= this.OnStageStart;
                   });
                   this.listenForStageCompleted = new ToggleAction(delegate()
                   {
                       SceneExitController.onBeginExit += this.OnSceneBeginExit;
                   }, delegate ()
                   {
                       SceneExitController.onBeginExit -= this.OnSceneBeginExit;
                   }); 
                */
                #endregion

               
                Stage.onStageStartGlobal += this.OnStageStart;
                Run.onRunStartGlobal += this.OnRunStart;
                SceneExitController.onBeginExit += this.OnSceneBeginExit;
                this.Reset();
                //reset placement seems to be irrelevant; or at least it doesnt fix the issue im having

            }

            public override void OnUninstall()
            {
                Stage.onStageStartGlobal -= this.OnStageStart;
                SceneExitController.onBeginExit -= this.OnSceneBeginExit;
                //this.listenForStageCompleted.SetActive(false);
                //this.listenForStageStart.SetActive(false);
                base.OnUninstall();
            }


            //if player exits stage in under 180f seconds
            private void OnSceneBeginExit(SceneExitController exitController)
            {
                Debug.Log("doing scene exit");
                if (!this.expirationTimeStamp.hasPassed)
                {
                    base.Grant();
                    Debug.Log("granting achievement");
                }
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
                this.Reset();
                this.BeginStageTimer();
            }

            //reset timer method called on every stage start & run start
            private void Reset()
            {
                this.expirationTimeStamp = Run.FixedTimeStamp.negativeInfinity;
                //this.listenForStageCompleted.SetActive(false);
                //this.listenForStageStart.SetActive(false);
            }

        }

    }
}
