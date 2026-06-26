using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnClick;

    public void OnPointerEnter(PointerEventData eventData) => Debug.Log($"[UIButton] Hover ENTER: {gameObject.name}");
    public void OnPointerExit(PointerEventData eventData)  => Debug.Log($"[UIButton] Hover EXIT: {gameObject.name}");

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[UIButton] Click: {gameObject.name}");
        OnClick?.Invoke();
    }
}
