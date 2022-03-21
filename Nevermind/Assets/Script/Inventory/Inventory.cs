using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
	[Header("Inventory Setting")]
	[SerializeField] protected Vector2Int mapSize;
	[SerializeField] protected bool[,] map;

	private void Awake()
	{
		Init();
	}

	protected abstract void Init();
	protected abstract void SetMapSize();
	protected abstract void ShowElement();
}
