using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage")]
    public class TakeHealthDamageCharacterEffect : InstantCharacterEffect
    {
        public float healthDamage = 0;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateHealthDamage(character);
        }

        private void CalculateHealthDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentHealth.Value -= healthDamage;
            }
        }
    }
}