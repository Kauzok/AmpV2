using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace AmpMod.SkillStates.BaseStates
{
    class StopLoop : MonoBehaviour
    {
        public uint SoundId;
        public bool Played;
        public string SoundEventToPlay;

        void OnEnable()
        {
            if (!Played)
            {
                Played = true;
                SoundId = AkSoundEngine.PostEvent(SoundEventToPlay, base.gameObject);
            }
        }


        void OnDisable()
        {
            AkSoundEngine.StopPlayingID(SoundId);
            Destroy(gameObject);
        }
    }
}

