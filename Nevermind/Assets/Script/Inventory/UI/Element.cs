using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Element : MonoBehaviour, IDragHandler, IEndDragHandler
{
	public Pickable pickable;
	Image image;

	public Image viewport;
	Slot slot;

	Transform parent;
	Transform dragElement;

	Vector3 clickPosition;
	Vector3 targetPosition;
	Vector2 size;

	bool isDropable;

	private const int SLOTSIZE = 64;

	private void Awake()
	{
		image = this.transform.GetChild(0).GetComponent<Image>();
	}

	private void Start()
	{
		Init();
	}

	public void Cache(Pickable _pickable)
	{
		pickable = _pickable;
	}

	public Vector2 SizeAmount()
	{
		return pickable.Matrix;
	}

	void Init()
	{
		this.gameObject.name = pickable.name;
		image.sprite = pickable.Sprite;

		dragElement = this.transform.GetChild(1).transform;
		dragElement.transform.GetChild(0).GetComponent<Image>().sprite = pickable.Sprite;

		size = pickable.Matrix * SLOTSIZE;
		this.transform.GetComponent<RectTransform>().sizeDelta = size;
		dragElement.GetComponent<RectTransform>().sizeDelta = size;

		parent = InventoryUIController.Instance.pickedItemHolder;
		viewport = InventoryUIController.Instance.viewPort;
	}

	public void PointerDown()
	{
		clickPosition = Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f, 0);

		dragElement.gameObject.SetActive(true);
		dragElement.SetParent(parent);
		dragElement.localPosition = clickPosition;

		viewport.raycastTarget = false;

		if (slot != null) slot.isEmpty = true;
	}

	public void PointerUp()
	{
		dragElement.SetParent(this.gameObject.transform);
		dragElement.gameObject.SetActive(false);

		// check if it is dropable
		slot = InventoryUIController.Instance.slot;
		if (slot != null) isDropable = slot.isEmpty;

		targetPosition = isDropable ? slot.transform.position : this.transform.position;

		slot.isEmpty = (targetPosition == this.transform.position) ? true : false;

		viewport.raycastTarget = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		dragElement.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.transform.position = targetPosition;
		dragElement.transform.localPosition = Vector3.zero;
	}
}
