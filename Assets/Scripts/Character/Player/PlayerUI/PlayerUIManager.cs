using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance = null;

        [HideInInspector]
        public PlayerUIHUDManager playerUIHUDManager = null;
        [HideInInspector]
        public PlayerUICraftManager playerUICraftManager = null;
        [HideInInspector]
        public PlayerUIInventoryManager playerUIInventoryManager = null;
        [HideInInspector]
        public PlayerUIEndGameResult playerUIEndGameResult = null;

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

            playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
            playerUICraftManager = GetComponentInChildren<PlayerUICraftManager>();
            playerUIInventoryManager = GetComponentInChildren<PlayerUIInventoryManager>();
            playerUIEndGameResult = GetComponentInChildren<PlayerUIEndGameResult>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void CloseMenus()
        {
            playerUICraftManager.gameObject.SetActive(false);
        }
    }
}