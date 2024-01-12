using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AG
{
    public class UI_Match_Scroll_Wheel_To_Select_Button : MonoBehaviour
    {
        [SerializeField]
        private GameObject currentSelected = null;
        [SerializeField]
        private GameObject previouslySelected = null;
        [SerializeField]
        private RectTransform currentSelectedTransform = null;
        
        [SerializeField]
        private RectTransform contentPanel = null;
        [SerializeField]
        private ScrollRect scrollRect = null;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if(currentSelected != null)
            {
                if(currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}