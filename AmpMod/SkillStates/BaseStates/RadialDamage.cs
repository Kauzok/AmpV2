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

		public float radius;

		public float damageResetAge = 0f;
		public float damageResetFrequency = 1f;

		public float blastDamage;
		public float finalBlastDamage;
		private float interval = 1f;
		private float damageTimer;

		private float timer;

		private SphereSearch sphereSearch;

		public GameObject attacker;
		public CharacterBody charBody;
		public Vector3 position;

		protected Transform transform;
		protected TeamFilter teamFilter;

		public BlastAttack radialBlast;

		public DamageInfo damageInfo = new DamageInfo();



		private void Awake()
		{
			this.transform = base.GetComponent<Transform>();
			this.teamFilter = base.GetComponent<TeamFilter>();
			this.sphereSearch = new SphereSearch();

			radialBlast = new BlastAttack
			{
				attacker = attacker.gameObject,
				baseDamage = finalBlastDamage * charBody.damage,
				baseForce = 0f,
				attackerFiltering = AttackerFiltering.NeverHit,
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

		}



		private void FixedUpdate()
		{


			/* if (NetworkServer.active)
            {


				damageResetAge += Time.fixedDeltaTime;
				//fires the overlapattack
				if (damageResetAge >= 1f / damageResetFrequency)
				{
					radialBlast.Fire();

				}

			}*/

			timer += Time.fixedDeltaTime;
			this.damageTimer -= Time.fixedDeltaTime;
			if (this.damageTimer <= 0f && NetworkServer.active)
			{
				damageTimer = interval;	
				searchAndDamage();
			}

			if (timer >= 3-Time.fixedDeltaTime && NetworkServer.active)
            {
				radialBlast.Fire();
				timer = 0f;
            }
			


		}
		protected void ApplyDamage(HurtBox hurtBox)
		{
			if (!hurtBox)
			{
				return;
			}
			HealthComponent healthComponent = hurtBox.healthComponent;
			if (healthComponent && NetworkServer.active)
			{
				damageInfo = new DamageInfo
				{
					attacker = attacker,
					damage = charBody.damage * blastDamage,
					force = Vector3.zero,
					crit = charBody.RollCrit(),
					damageType = DamageType.Generic,
					procChainMask = default(ProcChainMask),
					inflictor = base.gameObject,
					position = hurtBox.healthComponent.body.corePosition
				};
			}

			//this causes an error with R2API, no clue why lmao

			hurtBox.healthComponent.TakeDamage(damageInfo);

		}


		private void searchAndDamage()
		{
			List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
			SearchForTargets(list);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				ApplyDamage(list[i]);
				i++;
			}

		}


		protected void SearchForTargets(List<HurtBox> dest)
		{
			this.sphereSearch.mask = LayerIndex.entityPrecise.mask;
			this.sphereSearch.origin = this.transform.position;
			this.sphereSearch.radius = this.radius + 7f;
			this.sphereSearch.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			this.sphereSearch.RefreshCandidates();
			this.sphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamFilter.teamIndex));
			this.sphereSearch.OrderCandidatesByDistance();
			this.sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
			this.sphereSearch.GetHurtBoxes(dest);
			this.sphereSearch.ClearCandidates();
		}






	}
}

