using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AIAnimationEventHandler : MonoBehaviour
    {
        private AudioSource audioSource = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayFootStep()
        {
            audioSource.Play();
        }
    }
}