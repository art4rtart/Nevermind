using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

public class Element : MonoBehaviour, IDragHandler, IEndDragHandler
{
	Pickable pickable;

	public Slot slot;

	Image image;
	Image viewport;

	Transform parent;
	public Transform dragElement;

	Vector3 clickPosition;
	Vector3 targetPosition;
	Vector2 size;

	bool isDropable;

	private const int SLOTSIZE = 64;

	public Vector2 current;

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

	public Vector2 Matrix()
	{
		return pickable.Matrix;
	}

	public void SetCurrentIndex(Vector2 _index)
	{
		current = _index;
	}

	void Init()
	{
		this.gameObject.name = pickable.name;
		image.sprite = pickable.Sprite;

		dragElement.transform.GetChild(0).GetComponent<Image>().sprite = pickable.Sprite;

		size = pickable.Matrix * SLOTSIZE;
		this.transform.GetComponent<RectTransform>().sizeDelta = size;
		dragElement.GetComponent<RectTransform>().sizeDelta = size;

		parent = InventoryUIController.Instance.pickedItemHolder;
		viewport = InventoryUIController.Instance.viewPort;

		this.GetComponent<Image>().color = Random.ColorHSV();
	}

	public void PointerEnter()
	{
		InventoryUIController.Instance.isDropable = false;
	}

	Vector3 startPosition;
	public void PointerDown()
	{
		InventoryUIController.Instance.element = this;

		startPosition = this.transform.position;
		clickPosition = Input.mousePosition - new Vector3(Screen.width * .5f, Screen.height * .5f, 0);

		this.GetComponent<Image>().raycastTarget = false;
		dragElement.gameObject.SetActive(true);
		dragElement.SetParent(parent);
		dragElement.localPosition = clickPosition;
		dragElement.GetComponent<RectTransform>().rotation = this.GetComponent<RectTransform>().rotation;

		viewport.raycastTarget = false;

		InventoryUIController.Instance.inventory.EraseElement(current, pickable.Matrix);
	}

	public void PointerUp()
	{
		this.GetComponent<RectTransform>().eulerAngles = dragElement.GetComponent<RectTransform>().eulerAngles;

		this.GetComponent<Image>().raycastTarget = true;
		dragElement.SetParent(this.gameObject.transform);
		dragElement.gameObject.SetActive(false);

		slot = InventoryUIController.Instance.slot;
		isDropable = InventoryUIController.Instance.isDropable;

		if (slot != null)
		{
			targetPosition = isDropable ? slot.transform.position : startPosition;
			InventoryUIController.Instance.inventory.MoveElement(current, slot.index, pickable.Matrix);
			current = slot.index;
		}
		else isDropable = false;

		viewport.raycastTarget = true;
		InventoryUIController.Instance.element = null;
	}

	public void OnDrag(PointerEventData eventData)
	{
		dragElement.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float x = targetPosition.x + pickable.Matrix.x * SLOTSIZE * .5f;
		float y = targetPosition.y - pickable.Matrix.y * SLOTSIZE * .5f + SLOTSIZE;

		this.transform.position = isDropable ? new Vector3(x, y, 0) : startPosition;

		dragElement.transform.localPosition = Vector3.zero;
	}

	public void RotateElement()
	{
		dragElement.GetComponent<RectTransform>().eulerAngles -= Vector3.forward * 90f;
		pickable.RotateMatrix();
		current = pickable.Matrix;
	}
}
