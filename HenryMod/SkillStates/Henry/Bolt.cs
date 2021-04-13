using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.SkillStates
{
    public class Bolt : BaseSkillState
    {
        public static float duration = 3.5f;
        public static float initialSpeedCoefficient = 3f;
        public static float finalSpeedCoefficient = 10f;
        public static float moveSpeedCoefficient = 10f;
        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;


        public static HurtBoxGroup hurtbox;
        public static CharacterModel charactermodeler;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;
        private GameObject coreVfxInstance;
        private GameObject footVfxInstance;
        private ICharacterFlightParameterProvider flight;
        private ICharacterGravityParameterProvider gravity;

        /*   private void UpdateVfxPositions()
           {
               if (characterBody)
               {
                   if (coreVfxInstance)
                   {
                       coreVfxInstance.transform.position = base.characterBody.corePosition;
                   }
                   if (footVfxInstance)
                   {
                       footVfxInstance.transform.position = base.characterBody.footPosition;
                   }
               }
           } */

        public override void OnEnter()
        {
            base.OnEnter();

            gravity = gameObject.GetComponent<ICharacterGravityParameterProvider>();
            flight = gameObject.GetComponent<ICharacterFlightParameterProvider>();


            CharacterGravityParameters gravityParameters = gravity.gravityParameters;
            gravityParameters.channeledAntiGravityGranterCount++;
            gravity.gravityParameters = gravityParameters;


            CharacterFlightParameters flightParameters = flight.flightParameters;
            flightParameters.channeledFlightGranterCount++;
            flight.flightParameters = flightParameters;



            this.animator = base.GetModelAnimator();

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, Bolt.duration);
            }


        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (base.fixedAge >= Bolt.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            CharacterGravityParameters gravityParameters = gravity.gravityParameters;
            gravityParameters.channeledAntiGravityGranterCount--;
            gravity.gravityParameters = gravityParameters;


            CharacterFlightParameters flightParameters = flight.flightParameters;
            flightParameters.channeledFlightGranterCount--;
            flight.flightParameters = flightParameters;





        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);

        }

        public override void OnDeserialize(NetworkReader reader)
        {

            base.OnDeserialize(reader);

        }
    }
}