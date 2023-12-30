using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using AmpMod.Modules;
using UnityEngine.Networking;
using RoR2.HudOverlay;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
     class DashCrosshairController : MonoBehaviour
    {
        private OverlayController overlayController;
        private GameObject overlayPrefab = Assets.plasmaCrosshair;
        private string overlayChildLocatorEntry = "CrosshairExtras";

        public void EnableCrosshair()
        {
            if (overlayController == null)
            {
                OverlayCreationParams overlayCreationParams = new OverlayCreationParams
                {
                    prefab = this.overlayPrefab,
                    childLocatorEntry = this.overlayChildLocatorEntry
                };
                this.overlayController = HudOverlayManager.AddOverlay(base.gameObject, overlayCreationParams);
            }
        
        }


        public void DisableCrosshair()
        {         
             HudOverlayManager.RemoveOverlay(overlayController);
             this.overlayController = null;
        }
     }
}
