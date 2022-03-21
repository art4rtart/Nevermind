using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	Image image;
	public bool isEmpty = true;

	public Color defaultColor;
	public Color emptyColor;
	public Color notEmptyColor;

	public Vector2 index;

	private void Awake()
	{
		image = GetComponent<Image>();
		defaultColor = image.color;
	}

	public void HoverEnter()
	{
		image.color = emptyColor;
		InventoryUIController.Instance.slot = this;

		if (InventoryUIController.Instance.slot == null)
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
	}

	public void SetImageColor(bool _hover)
	{
		if (_hover) image.color = emptyColor;
		else image.color = defaultColor;
	}
}
