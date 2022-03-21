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

	Vector3 pivot;
	bool update;

	private void Awake()
	{
		image = GetComponent<Image>();
		defaultColor = image.color;
	}

	private void Start()
	{
		Vector2 size = this.GetComponent<RectTransform>().sizeDelta * .5f;
		pivot = this.transform.position + new Vector3(size.x, size.y, 0);
	}

	private void Update()
	{
		if (!update) return;
	}

	public void HoverEnter()
	{
		image.color = emptyColor;
		InventoryUIController.Instance.slot = this;
		update = true;
	}

	public void HoverClick()
	{

	}

	public void HoverExit()
	{
		image.color = defaultColor;

		InventoryUIController.Instance.slot = this;
		update = false;
	}
}
