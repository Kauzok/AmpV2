using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class BaseFireLightningBeam : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

		private void Fire()
		{
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();
				if (this.projectilePrefab != null)
				{
					float num = Util.Remap(this.charge, 0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
					float num2 = this.charge * this.force;
					FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
					{
						projectilePrefab = this.projectilePrefab,
						position = aimRay.origin,
						rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
						owner = base.gameObject,
						damage = this.damageStat * num,
						force = num2,
						crit = base.RollCrit()
					};
					this.ModifyProjectile(ref fireProjectileInfo);
					ProjectileManager.instance.FireProjectile(fireProjectileInfo);
				}
				if (base.characterMotor)
				{
					base.characterMotor.ApplyForce(aimRay.direction * (-this.selfForce * this.charge), false, false);
				}
			}
		}


	}
}
