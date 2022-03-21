using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Inventory
{
	const int SLOTSIZE = 64;

	public Transform content;
	public GameObject elementPrefab;  
	
	Vector3 startPosition;

	public List<Pickable> ground = new();

	public Transform slot;
	List<Slot> slots = new();
	int slotIndex = 0;

	public List<Vector2> current = new();
	public List<Element> elements = new();

	private void Start()
	{
		SetSlot();
		ShowElement();
	}

	protected override void Init()
	{
		SetMapSize();

		startPosition = content.transform.position;
	}

	protected override void SetMapSize()
	{
		mapSize = new Vector2Int(10, 15);
		map = new bool[mapSize.y, mapSize.x];
	}

	protected override void ShowElement()
	{
		for(int i = 0; i < ground.Count; i++)
		{
			GameObject gameObject = Instantiate(elementPrefab, transform.position, Quaternion.identity);
			RectTransform rect = gameObject.GetComponent<RectTransform>();
			Element element = gameObject.GetComponent<Element>();
			elements.Add(element);

			gameObject.transform.SetParent(content);
			element.Cache(ground[i]);

			FindEmpty(element, Vector2Int.zero);
			SetElementPosition(element, rect);
		}

		int index = 0;
		foreach(Element el in elements)
		{
			el.SetCurrentIndex(current[index]);
			index++;
		}
	}

	void FindEmpty(Element _element, Vector2Int _mapIndex)
	{
		#region find empty index
		bool foundEmptyIndex = false;

		for (int y = _mapIndex.y; y < mapSize.y; y++)
		{
			for (int x = _mapIndex.x; x < mapSize.x; x++)
			{
				if (map[y, x] == false)
				{
					emptyIndex = new Vector2Int(x, y);
					foundEmptyIndex = true;
					break;
				}
			}
			_mapIndex.x = 0;
			if (foundEmptyIndex) break;
		}
		#endregion

		#region check collision
		bool foundEmptySpace = true;

		if (emptyIndex.x + _element.Matrix().x > mapSize.x)
		{
			emptyIndex.x = 0;
			emptyIndex.y++;

			FindEmpty(_element, emptyIndex);
			return;
		}

		// check empty space
		for (int y = (int)emptyIndex.y; y < (int)emptyIndex.y + _element.Matrix().y; y++)
		{
			for (int x = (int)emptyIndex.x; x < (int)emptyIndex.x + _element.Matrix().x; x++)
			{
				if (map[y, x] != false)
				{
					foundEmptySpace = false;
					break;
				}
			}
			if (!foundEmptySpace) break;
		}

		// mark empty space
		if (foundEmptySpace)
		{
			for (int y = (int)emptyIndex.y; y < emptyIndex.y + _element.Matrix().y; y++)
			{
				for (int x = (int)emptyIndex.x; x < emptyIndex.x + _element.Matrix().x; x++)
				{
					map[y, x] = true;
				}
			}

			SetElementSlot(_element);
			
			current.Add(emptyIndex);
		}

		else
		{
			emptyIndex.x++;
			FindEmpty(_element, emptyIndex);
		}
		#endregion
	}

	void SetElementPosition(Element _element, RectTransform _elementRect)
	{
		float x = emptyIndex.x * SLOTSIZE + _elementRect.sizeDelta.x * .5f * _element.Matrix().x;
		float y = -((emptyIndex.y + _element.Matrix().y) * SLOTSIZE - _elementRect.sizeDelta.y * .5f * _element.Matrix().y);

		_elementRect.position = startPosition + new Vector3(x, y, 0);
	}

	void SetSlot()
	{
		int x = 0, y = 0;
		for (int i = 0; i < slot.childCount; i++)
		{
			slot.GetChild(i).GetComponent<Slot>().index = new Vector2(x, y);
			slots.Add(slot.GetChild(i).GetComponent<Slot>());

			if (x == mapSize.x - 1) y = (y + 1) % mapSize.y;
			x = (x + 1) % mapSize.x;
		}
	}

	void SetElementSlot(Element _element)
	{
		_element.slot = slots[emptyIndex.x + mapSize.x * emptyIndex.y];
		_element.slot.isEmpty = false;
	}
}
