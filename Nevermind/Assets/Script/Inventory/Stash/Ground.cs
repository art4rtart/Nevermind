using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Inventory
{
	const int SLOTSIZE = 64;

	public Transform content;
	public GameObject elementPrefab;  
	
	Vector3 currentPosition;
	Vector3 startPosition;
	Vector2Int emptyIndex;

	public List<Pickable> ground = new();

	private void Start()
	{
		Init();
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

			gameObject.transform.SetParent(content);
			element.Cache(ground[i]);

			FindEmpty(element, Vector2Int.zero);
			SetElementPosition(element, rect);
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

		if (emptyIndex.x + _element.SizeAmount().x > mapSize.x)
		{
			emptyIndex.x = 0;
			emptyIndex.y++;

			FindEmpty(_element, emptyIndex);
			return;
		}

		// check empty space
		for (int y = (int)emptyIndex.y; y < (int)emptyIndex.y + _element.SizeAmount().y; y++)
		{
			for (int x = (int)emptyIndex.x; x < (int)emptyIndex.x + _element.SizeAmount().x; x++)
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
			for (int y = (int)emptyIndex.y; y < emptyIndex.y + _element.SizeAmount().y; y++)
			{
				for (int x = (int)emptyIndex.x; x < emptyIndex.x + _element.SizeAmount().x; x++)
				{
					map[y, x] = true;
				}
			}
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
		currentPosition = startPosition + new Vector3(emptyIndex.x * SLOTSIZE, -((emptyIndex.y + _element.SizeAmount().y) * SLOTSIZE), 0);
		_elementRect.position = currentPosition;
	}

	void ShowMap()
	{
		string value = "\n";
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < mapSize.x; j++)
			{
				if (map[i, j] == false) value += "0 ";
				else value += "1 ";
			}
			value += "\n";
		}
		Debug.Log(value);
	}

}
