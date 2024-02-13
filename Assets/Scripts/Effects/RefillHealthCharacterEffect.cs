using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Refill Health")]
    public class RefillHealthCharacterEffect : InstantCharacterEffect
    {
        [Header("Values")]
        public float healthValue = 0;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (character.isDead.Value)
            {
                return;
            }

            character.characterNetworkManager.currentHealth.Value = Mathf.Clamp(character.characterNetworkManager.currentHealth.Value + healthValue, 0, character.characterNetworkManager.maxHealth.Value);
        }
    }
}