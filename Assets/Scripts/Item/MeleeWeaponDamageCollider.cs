using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage = null;

        [Header("Weapon Attack Midifiers")]
        public float lightAttackModifier = 0.0f;

        protected override void Awake()
        {
            base.Awake();

            if(damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget)
            {
                if (damageTarget == characterCausingDamage)
                {
                    return;
                }

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (characterDamaged.Contains(damageTarget))
            {
                return;
            }

            characterDamaged.Add(damageTarget);

            Debug.Log("TakeDamageEffect");
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.fireDamage = fireDamage;

            ApplyAttackDamageModifiers(lightAttackModifier, damageEffect);
            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if(characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.fireDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.fireDamage *= modifier;


        }
    }
}