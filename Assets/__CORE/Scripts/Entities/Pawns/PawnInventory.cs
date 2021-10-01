using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PawnInventory : PawnComponent
{
	public SyncList<uint> ItemIds = new();

	public CORE_Delegates.VoidDelegate Evnt_OnItemArrayChanged;
	
	private ItemBase[] _items;

	protected override void Initialize()
	{
		base.Initialize();
		ItemIds.Callback += OnItemIdsChanged;
	}

	public ItemBase[] GetItems()
	{
		return _items;
	}
	
	private void OnItemIdsChanged(SyncList<uint>.Operation op, int itemindex, uint olditem, uint newitem)
	{
		RegenerateInternalItemArray();
	}

	private void RegenerateInternalItemArray()
	{
		_items = new ItemBase[ItemIds.Count];

		for (int i = 0; i < ItemIds.Count; i++)
		{
			NetworkIdentity nid = FetchNetworkIdentity(ItemIds[i]);
			if (nid != null)
			{
				_items[i] = nid.GetComponent<ItemBase>();
			}
			else
			{
				_items[i] = null;
			}
		}
		
		Evnt_OnItemArrayChanged?.Invoke();
	}

	public void TryAddItemToInventory(ItemBase i)
	{
		AddItemToInventory(i.netId);
	}

	[Command(requiresAuthority = false)]
	private void AddItemToInventory(uint itemNetId)
	{
		NetworkIdentity nid = FetchNetworkIdentity(itemNetId);
		if (!nid) return;
		ItemBase i = nid.GetComponent<ItemBase>();
		if (i.IsBeingHeld()) return;

		ItemIds.Add(i.netId);
		i.CurrentHolder = netId;
	}
}