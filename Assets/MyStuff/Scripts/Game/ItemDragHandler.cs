using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parent;

	public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.DraggingObject = transform.GetComponentInParent<ItemSlot>();
        parent = transform.parent;
        transform.SetParent(HudManager.Instance.ItemDragSlot, false);
    }

	public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.DraggingObject = null;
        transform.localPosition = Vector2.zero;
        if (transform.parent == HudManager.Instance.ItemDragSlot)
        {
            transform.SetParent(parent, false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
    