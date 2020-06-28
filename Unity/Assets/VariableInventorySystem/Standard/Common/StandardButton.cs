using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardButton : Button, IVariableInventoryCellActions
    {
        Action onPointerClick;
        Action onPointerOptionClick;
        Action onPointerEnter;
        Action onPointerExit;
        Action onPointerDown;
        Action onPointerUp;

        Coroutine longPointerCoroutine;

        public void SetActive(bool value)
        {
            enabled = value;
            foreach (var graphic in GetComponentsInChildren<Graphic>())
            {
                graphic.raycastTarget = value;
            }
        }

        public void SetCallback(Action onPointerClick)
        {
            this.onPointerClick = onPointerClick;
        }

        public void SetCallback(
            Action onPointerClick,
            Action onPointerOptionClick,
            Action onPointerEnter,
            Action onPointerExit,
            Action onPointerDown,
            Action onPointerUp)
        {
            this.onPointerClick = onPointerClick;
            this.onPointerOptionClick = onPointerOptionClick;
            this.onPointerEnter = onPointerEnter;
            this.onPointerExit = onPointerExit;
            this.onPointerDown = onPointerDown;
            this.onPointerUp = onPointerUp;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

#if (UNITY_IOS || UNITY_ANDROID)
            onPointerClick?.Invoke();
#else
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onPointerClick?.Invoke();
            }
            else
            {
                onPointerOptionClick?.Invoke();
            }
#endif
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onPointerEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            onPointerExit?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (longPointerCoroutine != null)
            {
                StopCoroutine(longPointerCoroutine);
            }

            longPointerCoroutine = StartCoroutine(LongPointerDownCoroutine(eventData));
#endif

            base.OnPointerDown(eventData);
            onPointerDown?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            onPointerUp?.Invoke();
        }

#if (UNITY_IOS || UNITY_ANDROID)
        IEnumerator LongPointerDownCoroutine(PointerEventData eventData)
        {
            var pressTime = Time.unscaledTime;
            var pressPosition = eventData.position;

            while (Time.unscaledTime < pressTime + 1.0f)
            {
                if ((eventData.position - pressPosition).magnitude > 10.0f)
                {
                    longPointerCoroutine = null;
                    yield break;
                }

                yield return null;
            }

            onPointerOptionClick?.Invoke();
            longPointerCoroutine = null;
            yield break;
        }
#endif
    }
}
