using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using R2API;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
	[RequireComponent(typeof(ProjectileController))]
	public class StaticFieldDamage : MonoBehaviour
    {

		private ProjectileController projectileController;
		private ProjectileDamage projectileDamage;
		public float damageCoefficient = 1f;
		private float fireStopwatch;
		private float resetStopwatch;
		private float totalStopwatch;
		private BlastAttack blastAttack;
		public float resetFrequency = 4f;
		private float strikeRadius = 13f;
		public float procCoefficient;
		public float lifetime = 4f;
		public float fireFrequency = 1f;
		private ChildLocator childLocator;

		private void Start()
        {
			this.projectileController = base.GetComponent<ProjectileController>();
			this.projectileDamage = base.GetComponent<ProjectileDamage>();
			childLocator = base.gameObject.GetComponent<ChildLocator>();
			this.Fire();
			Debug.Log("childlocator is " + childLocator);
		}
		public void Fire()
        {
			//create blastattack
			BlastAttack staticHit = new BlastAttack
			{
				attacker = this.projectileController.owner,
				baseDamage = this.damageCoefficient * this.projectileDamage.damage,
				baseForce = 2f,
				attackerFiltering = AttackerFiltering.NeverHitSelf,
				crit = this.projectileDamage.crit,
				damageColorIndex = DamageColorIndex.Item,
				damageType = projectileDamage.damageType,
				falloffModel = BlastAttack.FalloffModel.None,
				inflictor = base.gameObject,
				position = childLocator.FindChild("Center").position,
				procChainMask = default(ProcChainMask),
				procCoefficient = this.procCoefficient,
				radius = this.strikeRadius,
				teamIndex = this.projectileController.teamFilter.teamIndex,
			};

			staticHit.AddModdedDamageType(DamageTypes.controlledChargeProcProjectile);
			staticHit.AddModdedDamageType(DamageTypes.nemAmpSlowOnHit);
			staticHit.Fire();
		}

		public void FixedUpdate()
		{
			if (NetworkServer.active)
			{
				this.totalStopwatch += Time.fixedDeltaTime;
				this.resetStopwatch += Time.fixedDeltaTime;
				this.fireStopwatch += Time.fixedDeltaTime;
				if (this.resetStopwatch >= 1f / this.resetFrequency)
				{
					this.Fire();
					this.resetStopwatch -= 1f / this.resetFrequency;
				}
				if (this.lifetime > 0f && this.totalStopwatch >= this.lifetime)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	} 
	
}




