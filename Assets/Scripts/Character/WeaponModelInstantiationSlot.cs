using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponModelSlot;
        public GameObject currentWeaponModel = null;

        public void UnloadWeapon()
        {
            if(currentWeaponModel)
            {
                Destroy(currentWeaponModel);
                currentWeaponModel = null;
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}