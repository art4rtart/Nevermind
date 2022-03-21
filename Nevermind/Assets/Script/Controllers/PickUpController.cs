using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PickUpController : MonoBehaviour
{
	Bag inventory;

	public LayerMask layerMask;
	public float hitRange = 100f;

	private void Awake()
	{
		inventory = GetComponent<Bag>();
	}

	private void Update()
	{
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, hitRange, layerMask))
		{
			if (hit.transform.CompareTag("Item") && Input.GetKeyDown(KeyCode.F))
			{
				Pickable item = hit.transform.GetComponent<Pickable>();
				GetItem(item);
			}
		}
	}

	void GetItem(Pickable _item)
	{
		if (inventory.IsFull(_item)) return;
		inventory.Add(_item);
	}

	void DropItem(Pickable _item, int _dropAmount)
	{
		inventory.Drop(_item, _dropAmount);
	}
}
