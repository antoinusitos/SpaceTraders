using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager instance = null;

        [Header("Weapon Item Actions")]
        public WeaponItemAction[] weaponItemActions = null;

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

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            for(int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemACtionWithID(int ID)
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                if (weaponItemActions[i].actionID == ID)
                {
                    return weaponItemActions[i];
                }
            }

            return null;
        }
    }
}