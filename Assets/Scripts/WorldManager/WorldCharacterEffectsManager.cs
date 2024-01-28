using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance = null;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect = null;

        [SerializeField]
        private List<InstantCharacterEffect> instantEffects;

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

            GenerateEffectsIDs();
        }

        private void GenerateEffectsIDs()
        {
            for(int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }

        public InstantCharacterEffect GetInstantCharacterEffectWithID(int ID)
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                if (instantEffects[i].instantEffectID == ID)
                {
                    return instantEffects[i];
                }
            }

            return null;
        }
    }
}