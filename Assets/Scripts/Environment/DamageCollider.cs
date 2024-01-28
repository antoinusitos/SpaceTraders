using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        public float physicalDamage = 0;
        public float fireDamage = 0;

        [Header("Contact Point")]
        private Vector3 contactPoint = Vector3.zero;

        [Header("Contact Point")]
        protected List<CharacterManager> characterManagers = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();
            if (damageTarget)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if(characterManagers.Contains(damageTarget))
            {
                return;
            }

            characterManagers.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.fireDamage = fireDamage;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}