using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UI_Craft_Activator : Button
    {
        public bool isActivated = false;

        [HideInInspector]
        public PlayerUICraftManager playerUICraftManager = null;

        protected override void Start()
        {
            base.Start();

            onClick.AddListener(Click);
        }

        public void Click()
        {
            isActivated = true;
            GetComponent<Image>().color = Color.green;
            playerUICraftManager.OnActivatorTriggered();
        }


        public void ResetActivator()
        {
            isActivated = false;
            GetComponent<Image>().color = Color.white;
        }
    }
}