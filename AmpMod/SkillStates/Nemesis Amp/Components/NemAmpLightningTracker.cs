using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Orbs;
using EntityStates;
using UnityEngine.Networking;
using System.Linq;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpLightningTracker : MonoBehaviour
    {

		public float maxTrackingDistance = 30f;
		public float maxTrackingAngle = 20f;
		public float trackerUpdateFrequency = 10f;

		private HurtBox trackingTarget;

		private CharacterBody characterBody;

		private TeamComponent teamComponent;

		private InputBankTest inputBank;

		private float trackerUpdateStopwatch;

		private Indicator indicator;

		private readonly BullseyeSearch search = new BullseyeSearch();

		public bool enemyInStormRange;
		private HurtBox enemy;


        private void Awake()
		{
			this.indicator = new Indicator(base.gameObject, Assets.lightningCrosshair);

		}

		private void Start()
		{
			this.characterBody = base.GetComponent<CharacterBody>();
			this.inputBank = base.GetComponent<InputBankTest>();
			this.teamComponent = base.GetComponent<TeamComponent>();
		}

		public HurtBox GetTrackingTarget()
		{
			return this.trackingTarget;
		}

		private void OnEnable()
		{
			this.indicator.active = true;
		}

		private void OnDisable()
		{
			this.indicator.active = false;
		}

		private void FixedUpdate()
		{
			if (characterBody.skillLocator.primary.skillNameToken != "NT_NEMESISAMP_BODY_PRIMARY_LIGHTNING_NAME") return;

			this.trackerUpdateStopwatch += Time.fixedDeltaTime;
			if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
			{
				this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
				HurtBox hurtBox = this.trackingTarget;
				Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
				this.SearchForTarget(aimRay);
				this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
				//Debug.Log(indicator + " is indicator");
			}

			checkNearbyEnemies();
		}

		private void SearchForTarget(Ray aimRay)
		{
			this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
			this.search.filterByLoS = true;
			this.search.searchOrigin = aimRay.origin;
			this.search.searchDirection = aimRay.direction;
			this.search.sortMode = BullseyeSearch.SortMode.Distance;
			this.search.maxDistanceFilter = this.maxTrackingDistance;
			this.search.maxAngleFilter = this.maxTrackingAngle;
			this.search.RefreshCandidates();
			this.search.FilterOutGameObject(base.gameObject);
			this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
		}

		private void checkNearbyEnemies() {
            if (NetworkServer.active)
            {
				HurtBox[] lightningTargets = new HurtBox[1];
                BullseyeSearch lightningSearch = new BullseyeSearch();
                lightningSearch.filterByDistinctEntity = true;
                lightningSearch.filterByLoS = false;
				lightningSearch.maxDistanceFilter = StaticValues.stormRadius;
                lightningSearch.minDistanceFilter = 0f;
                lightningSearch.minAngleFilter = 0f;
                lightningSearch.maxAngleFilter = 180f;
                lightningSearch.sortMode = BullseyeSearch.SortMode.Distance;
                lightningSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(characterBody.teamComponent.teamIndex);
                lightningSearch.searchOrigin = characterBody.corePosition;
                lightningSearch.viewer = null;
                lightningSearch.RefreshCandidates();
                lightningSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = lightningSearch.GetResults();
                if (results.Any()) { 
					enemyInStormRange = true; 
				}
				else
				{
					enemyInStormRange = false;
				}
				
            }
        }


	}
    
}
