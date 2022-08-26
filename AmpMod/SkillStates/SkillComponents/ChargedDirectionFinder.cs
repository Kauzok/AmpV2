using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using RoR2;
using RoR2.Projectile;
using System.Linq;

namespace AmpMod.SkillStates.SkillComponents
{
	[RequireComponent(typeof(TeamFilter))]
	[RequireComponent(typeof(ChargedTargeting))]
	class ChargedDirectionFinder : MonoBehaviour
    {
		[Tooltip("How far ahead the projectile should look to find a target.")]
		public float lookRange = 40f;
		
		[Tooltip("How wide the cone of vision for this projectile is in degrees. Limit is 180.")]
		[Range(0f, 180f)]
		public float lookCone = 90f;
	
		[Tooltip("How long before searching for a target.")]
		public float targetSearchInterval = 0.1f;
	
		[Tooltip("Will not search for new targets once it has one.")]
        public bool onlySearchIfNoTarget = true;
		
		[Tooltip("Allows the target to be lost if it's outside the acceptable range.")]
		public bool allowTargetLoss = false;
	
		[Tooltip("If set, targets can only be found when there is a free line of sight.")]
		public bool testLoS = false;
	
		[Tooltip("Whether or not airborne characters should be ignored.")]
		public bool ignoreAir = false;

		[Tooltip("Whether or not uncharged characters should be ignored.")]
		public bool ignoreUnCharged = true;

		[Tooltip("The difference in altitude at which a result will be ignored.")]
		[FormerlySerializedAs("altitudeTolerance")]
		public float flierAltitudeTolerance = float.PositiveInfinity;

		public UnityEvent onNewTargetFound;

		[FormerlySerializedAs("ontargetLost")]
		public UnityEvent onTargetLost;
		private new Transform transform;
		private TeamFilter teamFilter;
		private ChargedTargeting targetComponent;
		private float searchTimer;
		private bool hasTarget;
		private bool hadTargetLastUpdate;
		private BullseyeSearch bullseyeSearch;
		private HurtBox lastFoundHurtBox;
		private Transform lastFoundTransform;

		private void Start()
		{
			if (!NetworkServer.active)
			{
				base.enabled = false;
				return;
			}
			this.bullseyeSearch = new BullseyeSearch();
			this.teamFilter = base.GetComponent<TeamFilter>();
			this.targetComponent = base.GetComponent<ChargedTargeting>();
			this.transform = base.transform;
			this.searchTimer = 0f;
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00117234 File Offset: 0x00115434
		private void FixedUpdate()
		{
			this.searchTimer -= Time.fixedDeltaTime;
			if (this.searchTimer <= 0f)
			{
				this.searchTimer += this.targetSearchInterval;
				if (this.allowTargetLoss && this.targetComponent.target != null && this.lastFoundTransform == this.targetComponent.target && !this.PassesFilters(this.lastFoundHurtBox))
				{
					this.SetTarget(null);
				}
				if (!this.onlySearchIfNoTarget || this.targetComponent.target == null)
				{
					this.SearchForTarget();
				}
				this.hasTarget = (this.targetComponent.target != null);
				if (this.hadTargetLastUpdate != this.hasTarget)
				{
					if (this.hasTarget)
					{
						UnityEvent unityEvent = this.onNewTargetFound;
						if (unityEvent != null)
						{
							unityEvent.Invoke();
						}
					}
					else
					{
						UnityEvent unityEvent2 = this.onTargetLost;
						if (unityEvent2 != null)
						{
							unityEvent2.Invoke();
						}
					}
				}
				this.hadTargetLastUpdate = this.hasTarget;
			}
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x00117334 File Offset: 0x00115534
		private bool PassesFilters(HurtBox result)
		{
			CharacterBody body = result.healthComponent.body;
			if (ignoreUnCharged)
            {
				if (!body.HasBuff(Modules.Buffs.chargeBuildup) && !body.HasBuff(Modules.Buffs.electrified))
				{
					return body && false;
				}

				return body && (!this.ignoreAir || !body.isFlying) && (!body.isFlying || float.IsInfinity(this.flierAltitudeTolerance) || this.flierAltitudeTolerance >= Mathf.Abs(result.transform.position.y - this.transform.position.y));
			}
			else
			{
				return body && (!this.ignoreAir || !body.isFlying) && (!body.isFlying || float.IsInfinity(this.flierAltitudeTolerance) || this.flierAltitudeTolerance >= Mathf.Abs(result.transform.position.y - this.transform.position.y));
			}

		}

		// Token: 0x06004350 RID: 17232 RVA: 0x001173B0 File Offset: 0x001155B0
		private void SearchForTarget()
		{
			this.bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
			this.bullseyeSearch.teamMaskFilter.RemoveTeam(this.teamFilter.teamIndex);
			this.bullseyeSearch.filterByLoS = this.testLoS;
			this.bullseyeSearch.searchOrigin = this.transform.position;
			this.bullseyeSearch.searchDirection = this.transform.forward;
			this.bullseyeSearch.maxDistanceFilter = this.lookRange;
			this.bullseyeSearch.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
			this.bullseyeSearch.maxAngleFilter = this.lookCone;
			this.bullseyeSearch.RefreshCandidates();
			IEnumerable<HurtBox> source = this.bullseyeSearch.GetResults().Where(new Func<HurtBox, bool>(this.PassesFilters));
			this.SetTarget(source.FirstOrDefault<HurtBox>());
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x00117487 File Offset: 0x00115687
		private void SetTarget(HurtBox hurtBox)
		{
			this.lastFoundHurtBox = hurtBox;
			this.lastFoundTransform = ((hurtBox != null) ? hurtBox.transform : null);
			this.targetComponent.target = this.lastFoundTransform;
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x001174B4 File Offset: 0x001156B4
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Transform transform = base.transform;
			Vector3 position = transform.position;
			Gizmos.DrawWireSphere(position, this.lookRange);
			Gizmos.DrawRay(position, transform.forward * this.lookRange);
			Gizmos.DrawFrustum(position, this.lookCone * 2f, this.lookRange, 0f, 1f);
			if (!float.IsInfinity(this.flierAltitudeTolerance))
			{
				Gizmos.DrawWireCube(position, new Vector3(this.lookRange * 2f, this.flierAltitudeTolerance * 2f, this.lookRange * 2f));
			}
		}


	}
}
