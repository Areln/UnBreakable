using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		GameManager.Instance.UsingUI = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameManager.Instance.UsingUI = false;
	}

}
