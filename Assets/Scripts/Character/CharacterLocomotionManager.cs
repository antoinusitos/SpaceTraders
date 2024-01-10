using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Update()
        {

        }
    }
}