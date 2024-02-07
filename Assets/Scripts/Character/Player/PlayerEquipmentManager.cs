using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace AG
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager player = null;

        public WeaponModelInstantiationSlot rightHandSlot = null;

        [SerializeField]
        private WeaponManager rightWeaponManager = null;

        public GameObject rightHandWeaponModel = null;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlot();
        }

        protected override void Start()
        {
            base.Start();

            LoadRightWeapon();
        }

        public void InitializeWeaponSlot()
        {
            Debug.Log("InitializeWeaponSlot");
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
            foreach(WeaponModelInstantiationSlot weaponSlot in weaponSlots)
            {
                switch(weaponSlot.weaponModelSlot)
                {
                    case WeaponModelSlot.RightHand:
                        Debug.Log("found");
                        rightHandSlot = weaponSlot;
                        break;
                }
            }
        }

        public void LoadRightWeapon()
        {
            rightHandSlot.UnloadWeapon();

            if(player.playerInventoryManager.currentRightHandWeapon)
            {
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.itemPrefab);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        /*public void TryToUSeEquipment()
        {
            if(player.playerInventoryManager.currentRightHandWeapon)
            {
                Debug.Log("Using " + player.playerInventoryManager.currentRightHandWeapon.itemName);
                player.characterAnimatorManager.PlayTargetActionAnimation(player.playerInventoryManager.currentRightHandWeapon.animationToPlay, false, true, true);
            }
        }*/

        public void OpenDamageCollider()
        {
            if(player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
    }
}