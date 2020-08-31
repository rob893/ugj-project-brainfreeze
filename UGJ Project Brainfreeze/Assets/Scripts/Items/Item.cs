using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
	public string ItemName = "New Item";
	public int ItemId = 1;
	public bool CanPickUp = false;

	public void SetItemId()
	{
		var itemIds = new HashSet<int>();

		foreach (var item in Resources.LoadAll<Item>("Items"))
		{
			if (item != this && !itemIds.Contains(item.ItemId))
			{
				itemIds.Add(item.ItemId);
			}
		}

		if (!itemIds.Contains(ItemId))
		{
			Debug.Log("This item has a unique id already!");
			return;
		}

		int i = 1;
		while (itemIds.Contains(i))
		{
			i++;
		}
		Debug.Log($"The next available item id is {i} Setting this item's id to {i}");
		ItemId = i;
	}
}