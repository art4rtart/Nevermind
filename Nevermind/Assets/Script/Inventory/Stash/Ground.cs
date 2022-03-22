using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Ground : Inventory
{
	[Header("Ground Inventory")]
	public List<Pickable> ground = new();

	[Header("Element")]
	public Transform content;
	public Transform slot;
	public GameObject elementPrefab;  
	
	List<Vector2> currentMatrixs = new();
	List<Element> elements = new();
	List<Slot> slots = new();

	Vector3 startPosition;

	private void Awake()
	{
		if (content == null) Debug.Log("Content Transform should be assign");
		if (elementPrefab == null) Debug.Log("Content Transform should be assign");
	}

	private void Start()
	{
		Init();
		SetSlot();
		ShowElement();
		ShowMap();
	}

	protected override void Init()
	{
		SetMapSize();
		startPosition = content.transform.position;
	}

	protected override void SetMapSize()
	{
		map = new bool[inventorySize.y, inventorySize.x];
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
			el.SetCurrentMatrix(currentMatrixs[index]);
			index++;
		}
	}

	void FindEmpty(Element _element, Vector2Int _mapIndex)
	{
		#region find empty index
		bool foundEmptyIndex = false;
		for (int y = _mapIndex.y; y < inventorySize.y; y++)
		{
			for (int x = _mapIndex.x; x < inventorySize.x; x++)
			{
				if (map[y, x] == false)
				{
					emptyMatrix = new Vector2Int(x, y);
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

		if (emptyMatrix.x + _element.Matrix().x > inventorySize.x)
		{
			emptyMatrix.x = 0;
			emptyMatrix.y++;

			FindEmpty(_element, emptyMatrix);
			return;
		}

		// check empty space
		for (int y = (int)emptyMatrix.y; y < (int)emptyMatrix.y + _element.Matrix().y; y++)
		{
			for (int x = (int)emptyMatrix.x; x < (int)emptyMatrix.x + _element.Matrix().x; x++)
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
			for (int y = (int)emptyMatrix.y; y < emptyMatrix.y + _element.Matrix().y; y++)
			{
				for (int x = (int)emptyMatrix.x; x < emptyMatrix.x + _element.Matrix().x; x++)
				{
					map[y, x] = true;
				}
			}

			SetElementSlot(_element);
			
			currentMatrixs.Add(emptyMatrix);
		}

		else
		{
			emptyMatrix.x++;
			FindEmpty(_element, emptyMatrix);
		}
		#endregion
	}

	void SetElementPosition(Element _element, RectTransform _elementRect)
	{
		float x = emptyMatrix.x * SLOTSIZE + _elementRect.sizeDelta.x * .5f * _element.Matrix().x;
		float y = -((emptyMatrix.y + _element.Matrix().y) * SLOTSIZE - _elementRect.sizeDelta.y * .5f * _element.Matrix().y);

		_elementRect.position = startPosition + new Vector3(x, y, 0);
	}

	void SetSlot()
	{
		int x = 0, y = 0;
		for (int i = 0; i < slot.childCount; i++)
		{
			slot.GetChild(i).GetComponent<Slot>().Matrix = new Vector2(x, y);
			slots.Add(slot.GetChild(i).GetComponent<Slot>());

			if (x == inventorySize.x - 1) y = (y + 1) % inventorySize.y;
			x = (x + 1) % inventorySize.x;
		}
	}

	void SetElementSlot(Element _element)
	{
		_element.Slot = slots[emptyMatrix.x + inventorySize.x * emptyMatrix.y];
		_element.Slot.isEmpty = false;
	}
}
