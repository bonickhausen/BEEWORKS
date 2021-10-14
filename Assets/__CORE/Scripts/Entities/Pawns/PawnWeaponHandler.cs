using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PawnWeaponHandler : PawnComponent
{
	[Header("Objects")]
	public Transform WorldModelParent;
	[Header("Status")]
	[SyncVar]
	public int CurrentWeaponSlot;

	public ItemWeapon CurrentWeapon { get; private set; }

	private PawnViewRigFPSTPS _viewFps;
	private PawnInventory _inventory;
	private ItemWeapon[] _weapons;
	private short _lastWeaponSwapAttempt;

	private int _lastFrameWeaponSlot;

	protected override void Initialize()
	{
		base.Initialize();
		_viewFps = GetComponent<PawnViewRigFPSTPS>();
		_inventory = GetComponent<PawnInventory>();
		_inventory.Evnt_OnItemArrayChanged += OnItemArrayChanged;
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

	public override void Tick()
	{
		base.Tick();

		if (IsOwner())
		{
			CheckForWeaponSwapWish();
		}

		CheckForChanges();
	}

	private void CheckForChanges()
	{
		if (CurrentWeaponSlot != _lastFrameWeaponSlot)
		{
			OnWeaponChanged();
		}

		_lastFrameWeaponSlot = CurrentWeaponSlot;

		void OnWeaponChanged()
		{
			CurrentWeapon = GetWeaponInSlot(CurrentWeaponSlot);

			if (_viewFps && _viewFps.FirstPersonViewModelParent)
			{
				CurrentWeapon.AttachViewModelToTransform(_viewFps.FirstPersonViewModelParent);
			}
			
			CurrentWeapon.AttachWorldModelToTransform(WorldModelParent);
		}
	}

	private void CheckForWeaponSwapWish()
	{
		if (_cmd.SlotNumber > 0 && _cmd.SlotNumber != CurrentWeaponSlot && _cmd.SlotNumber != _lastWeaponSwapAttempt)
		{
			TrySwapWeapon(_cmd.SlotNumber);
			_lastWeaponSwapAttempt = _cmd.SlotNumber;
		}

		if (_cmd.SlotNumber <= 0) _lastWeaponSwapAttempt = 0;

		_cmd.SlotNumber = 0;
	}

	[Command(requiresAuthority = false)]
	private void TrySwapWeapon(int slot)
	{
		ItemWeapon w = GetWeaponInSlot(slot);
		if (w) CurrentWeaponSlot = slot;
	}

	private ItemWeapon GetWeaponInSlot(int slot)
	{
		if (_weapons != null && _weapons.Length > 0)
		{
			foreach (ItemWeapon w in _weapons)
			{
				if (w.Slot == slot) return w;
			}
		}

		return null;
	}
}