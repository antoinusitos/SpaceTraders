using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance = null;

        [SerializeField]
        private LayerMask characterLayers;

        [SerializeField]
        private LayerMask enviroLayers;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }
    }
}