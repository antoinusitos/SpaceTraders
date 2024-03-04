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

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if(cross.y < 0)
            {
                viewableAngle = -viewableAngle;
            }

            return viewableAngle;
        }
    }
}