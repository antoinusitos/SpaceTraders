using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AIAnimationEventHandler : MonoBehaviour
    {
        public AudioSource audioSourceFootStep = null;
        public AudioSource audioSourceRoar = null;

        public void PlayFootStep()
        {
            audioSourceFootStep.Play();
        }

        public void PlayRoar()
        {
            audioSourceRoar.Play();
        }
    }
}