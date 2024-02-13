using System;
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
        private Image item1Background = null;
        [SerializeField]
        private Image item2Image = null;
        [SerializeField]
        private Image item2Background = null;
        [SerializeField]
        private Image item3Image = null;
        [SerializeField]
        private Image item3Background = null;

        private void Start()
        {
            SetUsedSlot(0);
        }

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

        public void SetUsedSlot(int slot)
        {
            float alpha = item1Background.color.a;
            Color red = Color.red;
            red.a = alpha;
            Color black = Color.black;
            black.a = alpha;

            if (slot == 0)
            {
                item1Background.color = red;
                item2Background.color = black;
                item3Background.color = black;
            }
            else if (slot == 1)
            {
                item1Background.color = black;
                item2Background.color = red;
                item3Background.color = black;
            }
            else if (slot == 2)
            {
                item1Background.color = black;
                item2Background.color = black;
                item3Background.color = red;
            }
        }

        public void EmptyInventory()
        {
            item1Image.sprite = null;
            item2Image.sprite = null;
            item3Image.sprite = null;
        }

        public void EmptySlot(int slot)
        {
            if (slot == 0)
            {
                item1Image.sprite = null;
            }
            else if (slot == 1)
            {
                item2Image.sprite = null;
            }
            else if (slot == 2)
            {
                item3Image.sprite = null;
            }
        }
    }
}