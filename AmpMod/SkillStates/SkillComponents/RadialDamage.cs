using RoR2;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;
using HG;
using UnityEngine.Networking;
using R2API;

namespace AmpMod.SkillStates
{

	public class RadialDamage : MonoBehaviour
	{

		[Header("Damage Tick Parameters")]
		public float blastDamage;
		private float interval = 1f;
		private float damageTimer;
		public DamageInfo damageInfo = new DamageInfo();

		[Header("Damaging Object Parameters")]
		protected Transform transform;
		protected TeamFilter teamFilter;

		[Header("Final Blast Parameters")]
		public BlastAttack radialBlast;
		public float finalBlastDamage;
		private float timer;
		public GameObject explosionEffect;
		private string explosionString = Modules.StaticValues.vortexExplosionString;        
		private string loopString = Modules.StaticValues.vortexLoopString;

		[Header("Damage Owner/Positional Parameters")]
		public GameObject attacker;
		public CharacterBody charBody;
		public Vector3 position;
		public float radius = 10f;
		private SphereSearch sphereSearch;
		public float duration;

		private void Awake()
		{
			this.transform = base.GetComponent<Transform>();
			this.teamFilter = base.GetComponent<TeamFilter>();
			this.sphereSearch = new SphereSearch();

			//play looping vortex sound
			AkSoundEngine.PostEvent(loopString, gameObject);
			//declare explosion to be used on vortex destruction
			radialBlast = new BlastAttack
			{
				attacker = attacker.gameObject,
				baseDamage = finalBlastDamage * charBody.damage,
				baseForce = 0f,
				attackerFiltering = AttackerFiltering.NeverHitSelf,
				crit = charBody.RollCrit(),
				damageColorIndex = DamageColorIndex.Item,
				damageType = DamageType.Generic,
				falloffModel = BlastAttack.FalloffModel.None,
				inflictor = attacker.gameObject,
				position = this.transform.position,
				procChainMask = default(ProcChainMask),
				procCoefficient = 1f,
				radius = radius,
				teamIndex = this.teamFilter.teamIndex
			};

			

			//exitEffectPrefab = Modules.Assets.testLightningEffect;


		}


		private void FixedUpdate()
		{

			timer += Time.fixedDeltaTime;
			this.damageTimer -= Time.fixedDeltaTime;
			
			//calls the damage function three times, once every second starting on object spawn
			if (this.damageTimer <= 0f && NetworkServer.active)
			{
				damageTimer = interval;	
				searchAndDamage();
			}

			//fires the final vortex explosion after the damage function has been called thrice
			if (timer >= duration-Time.fixedDeltaTime && NetworkServer.active)
            {
				radialBlast.Fire();
				//play explosion sound
				AkSoundEngine.PostEvent(explosionString, gameObject);

				EffectData effectData = new EffectData
				{
					origin = this.transform.position,
					scale = 1.5f
				};

				EffectManager.SpawnEffect(explosionEffect, effectData, true);
				timer = 0f;
            }

		}


		//function for applying damage to characters in a radius
		protected void ApplyDamage(HurtBox hurtBox)
		{
			if (!hurtBox)
			{
				return;
			}
			HealthComponent healthComponent = hurtBox.healthComponent;

			if (healthComponent && NetworkServer.active)
			{
				//declare damageinfo with attacker object and characterbody set through vars
				damageInfo = new DamageInfo
				{
					attacker = attacker,
					damage = charBody.damage * blastDamage,
					force = Vector3.zero,
					crit = charBody.RollCrit(),
					damageType = DamageType.Generic,
					procChainMask = default(ProcChainMask),

					//change inflictor?
					inflictor = base.gameObject,
					position = hurtBox.healthComponent.body.corePosition
				};

				//apply damage info
				hurtBox.healthComponent.TakeDamage(damageInfo);
			}
				
		}


		private void searchAndDamage()
		{
			//get a list of hurtboxes with a spheresearch 
			List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
			SearchForTargets(list);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				//apply damage to every hurtbox in the list
				ApplyDamage(list[i]);
				i++;
			}

		}

		//search for hurtboxes in a radius centered at the vortex object
		protected void SearchForTargets(List<HurtBox> dest)
		{
			sphereSearch.mask = LayerIndex.entityPrecise.mask;
			sphereSearch.origin = this.transform.position;
			sphereSearch.radius = this.radius;
			sphereSearch.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			sphereSearch.RefreshCandidates();
			sphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamFilter.teamIndex));
			sphereSearch.OrderCandidatesByDistance();
			sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
			sphereSearch.GetHurtBoxes(dest);
			sphereSearch.ClearCandidates();
		}






	}
}

