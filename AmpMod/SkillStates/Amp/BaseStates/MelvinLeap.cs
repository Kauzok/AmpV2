using System;
using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.BaseStates
{

    public class MelvinLeap : BaseState
    {
        // Token: 0x06000DC6 RID: 3526 RVA: 0x0003A322 File Offset: 0x00038522
        // Token: 0x06000DCA RID: 3530 RVA: 0x0003A5D4 File Offset: 0x000387D4
        public override void OnEnter()
        {
            base.OnEnter();
            this.wormBodyPositionsDriver = base.GetComponent<WormBodyPositionsDriver>();
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                RaycastHit raycastHit;
                if (Util.CharacterRaycast(base.gameObject, aimRay, out raycastHit, 1000f, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal))
                {
                    this.targetPosition = base.gameObject.transform.position + 30 * Vector3.up;
                }
            }
            this.targetPosition = base.gameObject.transform.position + 800 * Vector3.up;
            Debug.Log("leaping");
        }

        // Token: 0x06000DCB RID: 3531 RVA: 0x0000F039 File Offset: 0x0000D239
        public override void OnExit()
        {
            base.OnExit();
        }

        // Token: 0x06000DCC RID: 3532 RVA: 0x0003A630 File Offset: 0x00038830
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active && this.targetPosition != null)
            {
                Vector3 vector = this.targetPosition.Value - this.wormBodyPositionsDriver.chaserPosition;
                if (vector != Vector3.zero && this.wormBodyPositionsDriver.chaserVelocity != Vector3.zero)
                {
                    Vector3 normalized = vector.normalized;
                    Vector3 normalized2 = this.wormBodyPositionsDriver.chaserVelocity.normalized;
                    float num = Vector3.Dot(normalized, normalized2);
                    float num2 = 0f;
                    if (num >= MelvinLeap.slowTurnThreshold)
                    {
                        num2 = MelvinLeap.slowTurnRate;
                        if (num >= MelvinLeap.fastTurnThreshold)
                        {
                            num2 = MelvinLeap.fastTurnRate;
                        }
                    }
                    if (num2 != 0f)
                    {
                        Vector3 speedUp = 20*Vector3.up;
                        //we want 1.5 rotations done in the total duration of the skill, so thats 540 degrees or  9.42 radians


                        this.wormBodyPositionsDriver.chaserVelocity = Vector3.RotateTowards(this.wormBodyPositionsDriver.chaserVelocity, vector, 0.017453292f * num2 * Time.fixedDeltaTime, 0f);
                    }
                }
            }
            /*if (base.isAuthority && !base.IsKeyDownAuthority())
            {
                this.outer.SetNextStateToMain();
            } */
        }

        // Token: 0x06000DCD RID: 3533 RVA: 0x0003A736 File Offset: 0x00038936
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            if (this.targetPosition != null)
            {
                writer.Write(true);
                writer.Write(this.targetPosition.Value);
                return;
            }
            writer.Write(false);
        }

        // Token: 0x06000DCE RID: 3534 RVA: 0x0003A76C File Offset: 0x0003896C
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            if (reader.ReadBoolean())
            {
                this.targetPosition = new Vector3?(reader.ReadVector3());
                return;
            }
            this.targetPosition = null;
        }

        // Token: 0x040010F1 RID: 4337
        private WormBodyPositionsDriver wormBodyPositionsDriver;

        // Token: 0x040010F2 RID: 4338
        private Vector3? targetPosition;

        // Token: 0x040010F3 RID: 4339
        private static readonly float fastTurnThreshold = Mathf.Cos(0.5235988f);

        // Token: 0x040010F4 RID: 4340
        private static readonly float slowTurnThreshold = Mathf.Cos(1.0471976f);

        // Token: 0x040010F5 RID: 4341
        private static readonly float fastTurnRate = 180f;

        // Token: 0x040010F6 RID: 4342
        private static readonly float slowTurnRate = 90f;
    }
}
