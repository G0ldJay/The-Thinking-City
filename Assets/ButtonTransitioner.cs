using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

    public Color32 _NormalColor = Color.white;
    public Color32 _HoverColor = Color.grey;
    public Color32 _DownColor = Color.white;

    private Image _image = null;

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _image.color = _HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _image.color = _NormalColor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        _image.color = _DownColor;
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void OnPointerClick(PointerEventData eventData) {
        _image.color = _HoverColor;
    }
}
