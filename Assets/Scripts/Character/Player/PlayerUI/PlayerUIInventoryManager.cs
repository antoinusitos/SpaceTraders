using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class PlayerUIInventoryManager : MonoBehaviour
    {
        [SerializeField]
        private Image item1Image = null;
        [SerializeField]
        private Image item2Image = null;
        [SerializeField]
        private Image item3Image = null;

        public void AffectSpriteToItem(Sprite sprite, int index)
        {
            if (index == 0)
            {
                item1Image.sprite = sprite;
            }
            else if (index == 1)
            {
                item2Image.sprite = sprite;
            }
            else if (index == 2)
            {
                item3Image.sprite = sprite;
            }
        }
    }
}