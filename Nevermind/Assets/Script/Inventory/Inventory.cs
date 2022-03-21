using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
	public Vector2Int MapSize { get { return mapSize; } }
	[SerializeField] protected Vector2Int mapSize;

	public bool[,]Map { get { return map; } }
	[SerializeField] protected bool[,] map;
	
	public Vector2Int EmptyIndex { get { return emptyIndex; } }
	protected Vector2Int emptyIndex;

	private void Awake()
	{
		Init();
	}

	protected abstract void Init();
	protected abstract void SetMapSize();
	protected abstract void ShowElement();

	public void EraseElement(Vector2 _current, Vector2 _size)
	{
		if (_current.y + _size.y > mapSize.y || _current.x + _size.x > mapSize.x) return;

		for (int y = (int)_current.y; y < _current.y + _size.y; y++)
		{
			for (int x = (int)_current.x; x < _current.x + _size.x; x++)
			{
				map[y, x] = false;
			}
		}
	}

	public void MoveElement(Vector2 _current, Vector2 _target, Vector2 _size)
	{
		if (_target.y + _size.y > mapSize.y || _target.x + _size.x > mapSize.x) return;

		for (int y = (int)_target.y; y < _target.y + _size.y; y++)
		{
			for (int x = (int)_target.x; x < _target.x + _size.x; x++)
			{
				map[y, x] = true;
			}
		}

		ShowMap();
	}

	public void ShowMap()
	{
		string value = "\n";
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < mapSize.x; j++)
			{
				if (map[i, j] == false) value += "0 ";
				else value += "1 ";
			}
			value += "\n";
		}
	}
}
