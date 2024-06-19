using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Projectile;

namespace AmpMod.SkillStates.Amp.BaseStates
{
    public class BaseReplenishingSkill : BaseSkillState
    {
        private GameObject lightningProjectile = Modules.Projectiles.swordOrbPrefab;
        private float meatballAngle = 60f;
        private float meatballForce = 15f;
        //private float lightningDamageCoefficient = 1.5f;
        public void FireLightningBall(int orbCount, float damage, Vector3 impactPosition)
        {
            var impactNormal = Vector3.up;
            float num = 360f / (float)orbCount;
            Vector3 normalized = Vector3.ProjectOnPlane(this.characterDirection.forward, impactNormal).normalized;
            Vector3 point = Vector3.RotateTowards(impactNormal, normalized, meatballAngle * 0.017453292f, float.PositiveInfinity);
            for (int i = 0; i < orbCount; i++)
            {
                Vector3 forward2 = Quaternion.AngleAxis(num * (float)i, impactNormal) * point;
                ProjectileManager.instance.FireProjectile(this.lightningProjectile, impactPosition, Util.QuaternionSafeLookRotation(forward2), base.gameObject, damage, meatballForce, Util.CheckRoll(this.characterBody.crit, this.characterBody.master), DamageColorIndex.Default, null, -1f);
            }
        }
    }
}
