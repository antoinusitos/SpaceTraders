using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldSoundEffectManager : MonoBehaviour
    {
        public static WorldSoundEffectManager instance = null;

        [Header("ActionSoundFX")]
        public AudioClip rollSFX = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}