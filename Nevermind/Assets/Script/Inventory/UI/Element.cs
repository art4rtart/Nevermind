using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

public class Element : MonoBehaviour, IDragHandler, IEndDragHandler
{
	Pickable pickable;

	public Slot Slot { get { return slot; } set { slot = value; } }
	Slot slot;

	Image background;
	Image sprite;
	Image viewport;

	RectTransform rect;
	Transform parent;

	Vector3 clickPosition;
	Vector3 targetPosition;
	Vector2 size;

	public Transform dragElement;
	public Vector2 current;

	bool dropped;

	private void Awake()
	{
		rect = this.GetComponent<RectTransform>();
		background = this.GetComponent<Image>();
		sprite = this.transform.GetChild(0).GetComponent<Image>();
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
		return pickable.MatrixSize;
	}

	public void SetCurrentMatrix(Vector2 _index)
	{
		current = _index;
	}

	void Init()
	{
		this.gameObject.name = pickable.name;
		sprite.sprite = pickable.Sprite;

		dragElement.transform.GetChild(0).GetComponent<Image>().sprite = pickable.Sprite;

		size = pickable.MatrixSize * SLOTSIZE;
		rect.sizeDelta = size;
		dragElement.GetComponent<RectTransform>().sizeDelta = size;

		parent = InventoryUIController.Instance.pickedItemHolder;
		viewport = InventoryUIController.Instance.VeiwPort;

		background.color = Random.ColorHSV();
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

		background.raycastTarget = false;

		dragElement.gameObject.SetActive(true);
		dragElement.SetParent(parent);
		dragElement.localPosition = clickPosition;
		dragElement.GetComponent<RectTransform>().rotation = this.GetComponent<RectTransform>().rotation;

		viewport.raycastTarget = false;

		InventoryUIController.Instance.Inventory.EraseElement(current, pickable.MatrixSize);
	}

	public void PointerUp()
	{
		rect.eulerAngles = dragElement.GetComponent<RectTransform>().eulerAngles;

		background.raycastTarget = true;
		dragElement.SetParent(this.gameObject.transform);
		dragElement.gameObject.SetActive(false);

		slot = InventoryUIController.Instance.slot;
		dropped = InventoryUIController.Instance.isDropable;

		viewport.raycastTarget = true;

		if (slot != null)
		{
			targetPosition = dropped ? slot.transform.position : startPosition;
			InventoryUIController.Instance.Inventory.MoveElement(current, slot.Matrix, pickable.MatrixSize);
			current = slot.Matrix;
		}
		else dropped = false;

		InventoryUIController.Instance.element = null;
	}

	public void OnDrag(PointerEventData eventData)
	{
		dragElement.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float x = targetPosition.x + pickable.MatrixSize.x * SLOTSIZE * .5f;
		float y = targetPosition.y - pickable.MatrixSize.y * SLOTSIZE * .5f + SLOTSIZE;

		this.transform.position = dropped ? new Vector3(x, y, 0) : startPosition;
		dragElement.transform.localPosition = Vector3.zero;
	}

	public void RotateElement()
	{
		dragElement.GetComponent<RectTransform>().eulerAngles -= Vector3.forward * 90f;
		pickable.RotateMatrix();
		current = pickable.MatrixSize;
	}
}
