using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource = null;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundEffectManager.instance.rollSFX);
        }
    }
} 