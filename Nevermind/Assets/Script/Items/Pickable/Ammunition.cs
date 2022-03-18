using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : Pickable
{
	public override void Get()
	{
		Debug.Log($"Player got {name} +{amount}!!");

		// player's total_item_amount += amount;
	}
	public override void Drop()
	{
		Debug.Log($"Player dropped {name} +{amount}!!");

		// player's total_item_amount -= amount;
	}
	public override void Use()
	{
		if (amount <= 0) return;
		Debug.Log($"Player used {name} +{useAmount}!!");

		amount = Mathf.Clamp(amount -= useAmount, 0, amount);
	}

	public override void Sell()
	{
		// player's total_item_amount -= amount;
		// player's total_gold_amount += price;

		Debug.Log($"Player sold {name} in {price}!!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player is here!");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player is not here..");
		}
	}
}
