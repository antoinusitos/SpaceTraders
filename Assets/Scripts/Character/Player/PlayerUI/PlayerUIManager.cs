using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance = null;

        [Header("Network Join")]
        [SerializeField]
        private bool startGameAsClient = false;

        [HideInInspector]
        public PlayerUIHUDManager playerUIHUDManager = null;
        [HideInInspector]
        public PlayerUICraftManager playerUICraftManager = null;
        [HideInInspector]
        public PlayerUIInventoryManager playerUIInventoryManager = null;

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
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(startGameAsClient)
            {
                startGameAsClient = false;
                NetworkManager.Singleton.Shutdown();
                NetworkManager.Singleton.StartClient();
            }
        }

        public void CloseMenus()
        {
            playerUICraftManager.gameObject.SetActive(false);
        }
    }
}