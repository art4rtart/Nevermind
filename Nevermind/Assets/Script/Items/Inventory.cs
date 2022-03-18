using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[Header("Baggage")]
	public List<Pickable> bag = new List<Pickable>();
	public int maxSpace = 50;
	public int currentSpace;

	public void Add(Pickable _item)
	{
		bag.Add(_item);
		currentSpace += _item.SpaceAmount();
		_item.AddToPlayer(this.transform);
	}

	public void Drop(Pickable _item, int _dropAmount)
	{
		if (_dropAmount != _item.Amount)
		{
			_dropAmount = _dropAmount <= _item.Amount ? _dropAmount : _item.Amount;
			_item.Amount -= _dropAmount;

			if(_item.Amount == 0) bag.Remove(_item);
		}

		else
		{
			bag.Remove(_item);
		}

		currentSpace -= _item.SpacePerAmount * _dropAmount;
	}

	public void Empty()
	{
		bag.Clear();
		currentSpace = 0;
	}

	public bool IsFull(Pickable _item)
	{
		if (currentSpace + _item.SpaceAmount() > maxSpace)
		{
			return true;
		}

		else return false;
	}
}
