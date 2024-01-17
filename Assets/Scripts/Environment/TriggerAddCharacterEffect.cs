using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TriggerAddCharacterEffect : MonoBehaviour
    {
        [SerializeField]
        private int effectID = -1;

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if(player != null && player.IsOwner)
            {
                player.playerEffectManager.ProcessInstantEffect(WorldCharacterEffectsManager.instance.GetInstantCharacterEffectWithID(effectID));
            }
        }
    }
}