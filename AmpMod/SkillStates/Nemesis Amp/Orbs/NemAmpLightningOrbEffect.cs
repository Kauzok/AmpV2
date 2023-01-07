using AmpMod.Modules;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpLightningOrbEffect : Orb
    {
        public float speed = 60f;
        private GameObject orbEffect = Assets.lightningStreamEffect;
        private float scale = 1f;

        public override void Begin()
        {
            base.duration = base.distanceToTarget / this.speed;
            if (this.orbEffect)
            {
                EffectData effectData = new EffectData
                {
                    scale = this.scale,
                    origin = this.origin,
                    genericFloat = base.duration
                };
                effectData.SetHurtBoxReference(this.target);
                EffectManager.SpawnEffect(orbEffect, effectData, true);
                //Debug.Log(orbEffect + "spawning");
            }
        }


    }
}
