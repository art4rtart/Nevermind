using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Pickable : MonoBehaviour
{
	[SerializeField] protected PickableType pickableType;
	[SerializeField] protected new string name;

	public Sprite Sprite { get { return sprtie; } }
	[SerializeField] protected Sprite sprtie;

	public int Amount { get { return amount; } set { amount = value; } }
	[SerializeField] protected int amount;

	[SerializeField] protected int useAmount;

	[SerializeField] protected int price;
	[SerializeField] protected int sellAmount;

	public int SpacePerAmount { get { return spacePerAmount; } set { } }
	[SerializeField] protected int spacePerAmount;

	[SerializeField] protected PickableState pickableState;
	public Vector2 Matrix { get { return matrix; } set { matrix = value; } }
	[SerializeField] protected Vector2 matrix;

	public PickableMatrix PickableMatrix { get { return pickableMatrix; } }
	[SerializeField] protected PickableMatrix pickableMatrix;

	public abstract void Get();
	public abstract void Drop();
	public abstract void Use();
	public abstract void Sell();

	// please remind this methods...
	public virtual void InitMatrix() { }

	public virtual void SetMatrix(int x, int y) 
	{
		matrix = new Vector2Int(x, y);

		if(x == y)
		{
			pickableMatrix = PickableMatrix.Square;
		}
		else if (x < y)
		{
			pickableMatrix = PickableMatrix.Vertical;
		}
		else if (x > y)
		{
			pickableMatrix = PickableMatrix.Horizontal;
		}
	}

	public void RotateMatrix()
	{
		int value;

		value = (int)matrix.x;
		matrix.x = matrix.y;
		matrix.y = value;

		if (matrix.x - matrix.y < 0) pickableMatrix = PickableMatrix.Vertical;
		else if (matrix.x - matrix.y > 0) pickableMatrix = PickableMatrix.Horizontal;
	}

	private void Awake()
	{
		Init();
	}

	public virtual void Init()
	{
		this.gameObject.name = name;
		this.gameObject.layer = LayerMask.NameToLayer("Item");
		this.gameObject.tag = "Item";

		SetMatrix((int)matrix.x, (int)matrix.y);
	}

	public int SpaceAmount()
	{
		return amount * spacePerAmount;
	}

	public void AddToPlayer(Transform _player)
	{
		this.transform.SetParent(_player);
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.identity;
		this.gameObject.SetActive(false);
	}
}
