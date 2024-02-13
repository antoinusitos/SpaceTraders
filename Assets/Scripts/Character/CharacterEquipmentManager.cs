using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        protected int currentEquipmentIndex = 0;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        public void SetCurrentSlotUsed(int newSlot)
        {
            currentEquipmentIndex = newSlot;
        }

        public int GetCurrentSlotUsed()
        {
            return currentEquipmentIndex;
        }

        public virtual void LoadRightWeapon()
        {
            Debug.Log("char LoadRightWeapon");
        }
    }
}