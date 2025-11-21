using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableItem_23 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private Image image;
    [SerializeField] private Item_23 item;

    private Canvas _rootCanvas;
    private Image _dragGhost;

    /// public access of item data
    public Item_23 Item => item;

    void OnValidate()
    {
        if (item != null)
            image.sprite = item.sprite;
    }

    void Awake()
    {
        _rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragGhost = new GameObject("Drag Ghost", typeof(Image)).GetComponent<Image>();
        _dragGhost.transform.SetParent(_rootCanvas.transform, false);
        _dragGhost.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        _dragGhost.sprite = GetComponent<Image>().sprite;
        _dragGhost.raycastTarget = false;
        image.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPos
        );
        _dragGhost.rectTransform.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dropPosition = _dragGhost.rectTransform.anchoredPosition;

        if (_dragGhost != null)
            Destroy(_dragGhost.gameObject);

        if(eventData.pointerEnter != null 
        && eventData.pointerEnter.GetComponent<FolderBin>() != null)
        {
            image.enabled = false;
            Debug.Log("Dropped into a Folder or Bin");

        }
        else
        {
            image.enabled = true;
            RectTransform originalRect = GetComponent<RectTransform>();
            originalRect.anchoredPosition = dropPosition; 
        }


        
    }

    public static implicit operator DraggableItem_23(Item_23 v)
    {
        throw new NotImplementedException();
    }

    public void HideSprite()
    {
        if (image != null)
        {
            image.enabled = false;
            Debug.Log("HIDING SPRITE");

        }
    }

    
}
