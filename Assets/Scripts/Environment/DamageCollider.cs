using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider = null;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float fireDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint = Vector3.zero;

        [Header("Character Damaged")]
        protected List<CharacterManager> characterDamaged = new List<CharacterManager>();

        protected virtual void Awake()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log("Damage Enter");
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            Debug.Log("DamageTarget");

            if (characterDamaged.Contains(damageTarget))
            {
                return;
            }

            characterDamaged.Add(damageTarget);

            Debug.Log("TakeDamageEffect");
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.fireDamage = fireDamage;

            if(damageTarget.characterEffectsManager)
            {
                damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
            }
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            characterDamaged.Clear();
        }
    }
}