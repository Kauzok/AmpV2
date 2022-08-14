using RoR2;
using System.Runtime.InteropServices;
using UnityEngine;
using RoR2.Audio;
using System.Collections.Generic;
using HG;
using UnityEngine.Networking;
using RoR2.Projectile;
using R2API;
using System;

namespace AmpMod.SkillStates
{
	[RequireComponent(typeof(ProjectileDamage))]
	public class RadialDamage : MonoBehaviour
	{

        
		[Header("Damage Tick Parameters")]
		public float tickDamageCoefficient = Modules.StaticValues.vortexDamageCoefficient;
		private float interval = 1f;
		private float damageTimer;
		private String hitboxName = "VortexHitbox";
        public DamageInfo damageInfo = new DamageInfo();
		public GameObject explosionEffect;
		private OverlapAttack attack;
		private float resetStopwatch;
		private float fireStopwatch;
		public float resetFrequency = 1f;
		public float fireFrequency = 1f;

		[Header("Damaging Object Parameters")]
		protected Transform transform;
		protected TeamFilter teamFilter;

		[Header("Final Blast Parameters")]
		public BlastAttack radialBlast;
		public float finalBlastDamageCoefficient = Modules.StaticValues.vortexExplosionCoefficient;
		private float timer;
		private string explosionString = Modules.StaticValues.vortexExplosionString;        
		private string loopString = Modules.StaticValues.vortexLoopString;

		[Header("Damage Owner/Positional Parameters")]
		public ProjectileDamage projectileDamage;
		public ProjectileController projectileController;
		private ChildLocator childLocator;
		private HitBoxGroup vortexHitbox;
		private bool hasSetAttacker;
		public GameObject attacker;
		public CharacterBody charBody;
		public Vector3 position;
		public float radius = 10f;
		private SphereSearch sphereSearch;
		public float duration = 3f;
		private string spawnSound = Modules.StaticValues.vortexSpawnString;

		private void Awake()
		{
			projectileDamage = base.GetComponent<ProjectileDamage>();
			projectileController = base.GetComponent<ProjectileController>();

			this.transform = base.GetComponent<Transform>();
			this.teamFilter = base.GetComponent<TeamFilter>();
			this.sphereSearch = new SphereSearch();


			//play spawn sound i don't care if this isn't a good way to code things i'm not making a separate component for this
			//AkSoundEngine.PostEvent(spawnSound, gameObject);
			PointSoundManager.EmitSoundServer(Modules.Assets.vortexSpawnSoundEvent.index, this.transform.position);

			//NetworkSoundEventDef spawnEvent;

			//PointSoundManager.EmitSoundServer(vortexS.index, base.transform.position);

			//play looping vortex sound
			//AkSoundEngine.PostEvent(loopString, gameObject);
			PointSoundManager.EmitSoundServer(Modules.Assets.vortexLoopSoundEvent.index, this.transform.position);


		}
		private void Start()
        {
			attacker = (this.projectileController.owner ? this.projectileController.owner.gameObject : null);
			charBody = (attacker ? attacker.GetComponent<CharacterBody>() : null);
			this.ResetOverlap();
			explosionEffect = Modules.Assets.vortexExplosionEffect;

			radialBlast = new BlastAttack
			{
				attacker = this.attacker,
				baseDamage = finalBlastDamageCoefficient * charBody.damage,
				baseForce = 0f,
				attackerFiltering = AttackerFiltering.NeverHitSelf,
				crit = charBody.RollCrit(),
				damageColorIndex = DamageColorIndex.Item,
				damageType = DamageType.Generic,
				falloffModel = BlastAttack.FalloffModel.None,
				inflictor = base.gameObject,
				position = this.transform.position,
				procChainMask = default(ProcChainMask),
				procCoefficient = 1f,
				radius = radius,
				teamIndex = this.teamFilter.teamIndex
			};

		}
		private void ResetOverlap()
		{
			this.attack = new OverlapAttack();
			//this.attack.procChainMask = this.projectileController.procChainMask;
			this.attack.procCoefficient = 1f;
			this.attack.attacker = attacker;
			this.attack.inflictor = base.gameObject;
			this.attack.teamIndex = TeamIndex.Player;
			this.attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
			this.attack.damage = tickDamageCoefficient * charBody.damage;
			this.attack.forceVector = Vector3.zero;
			this.attack.isCrit = charBody.RollCrit();
			this.attack.damageColorIndex = DamageColorIndex.Default;
			this.attack.damageType = DamageType.Generic;
			this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(base.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
		}


		private void FixedUpdate()
		{


			if (NetworkServer.active)
            {
				timer += Time.fixedDeltaTime;
				this.damageTimer -= Time.fixedDeltaTime;

				//calls the damage function three times, once every second starting on object spawn
					if (this.damageTimer <= 0f)// && NetworkServer.active)
					{
					damageTimer = interval;
					ResetOverlap();
					attack.Fire(null);
					//searchAndDamage();

					}

			}
			


			//fires the final vortex explosion after the damage function has been called thrice
			if (timer >= duration - Time.fixedDeltaTime) //&& NetworkServer.active)
            {
				radialBlast.Fire();
				//play explosion sound
				//AkSoundEngine.PostEvent(explosionString, gameObject);

				EffectData effectData = new EffectData
				{
					origin = this.transform.position,
					scale = 1.5f
				};

				EffectManager.SpawnEffect(explosionEffect, effectData, true);
				timer = 0f;
            }

		}


	

	}
}

