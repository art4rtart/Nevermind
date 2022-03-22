using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	Image image;
	Inventory inventory;
	public bool isEmpty = true;

	public Color defaultColor;
	public Color emptyColor;
	public Color notEmptyColor;

	public Vector2 Matrix { get { return matrix; } set { matrix = value; } }
	[SerializeField] private Vector2 matrix;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Start()
	{
		defaultColor = image.color;
	}

	public void HoverEnter()
	{
		image.color = emptyColor;
		InventoryUIController.Instance.slot = this;

		if (InventoryUIController.Instance.slot != null)
		{
			InventoryUIController.Instance.CheckDropable();
		}
	}

	public void HoverClick()
	{

	}

	public void HoverExit()
	{
		image.color = defaultColor;

		InventoryUIController.Instance.slot = null;
		InventoryUIController.Instance.isDropable = false;
	}

	public void SetParent(Inventory _inventory)
	{
		this.inventory = _inventory;
	}
}
