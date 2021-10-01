using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnWeaponHandler : PawnComponent
{
	private PawnInventory _inventory;
	private ItemWeapon[] _weapons;

	protected override void Initialize()
	{
		base.Initialize();
		_inventory = GetComponent<PawnInventory>();
		_inventory.OnItemArrayChanged += OnItemArrayChanged;
		OnItemArrayChanged();
	}

	private void OnItemArrayChanged()
	{
		RegenerateWeaponsArray();
	}

	private void RegenerateWeaponsArray()
	{
		List<ItemWeapon> weapons = new();
		_weapons = null;
		ItemBase[] items = _inventory.GetItems();
		if (items == null || items.Length <= 0) return;
		for (int index = 0; index < items.Length; index++)
		{
			ItemBase item = items[index];
			if (item == null) continue;

			ItemWeapon w = item.GetComponent<ItemWeapon>();
			if (w != null)
			{
				weapons.Add(w);
			}
		}

		_weapons = weapons.ToArray();
	}
}