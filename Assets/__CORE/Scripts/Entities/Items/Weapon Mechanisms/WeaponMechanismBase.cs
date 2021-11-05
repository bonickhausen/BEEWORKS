using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class WeaponMechanismBase : NetworkThing
{
	[Header("Mechanism Settings")]
	[Tooltip("How long should we have to wait before re-using this mechanism? In milliseconds.")]
	public ushort MsTimeInterval = 100;
	public string AmmoType = "9mm";
	public uint MaxAmmo = 30;
	[Header("State")]
	[SyncVar]
	[ReadOnly]
	public uint CurrentAmmo;

	private ItemWeapon _weapon;
	protected float _lastUseTime;

	private void Awake()
	{
		_weapon = GetComponent<ItemWeapon>();
	}

	public override void OnStartServer()
	{
		CurrentAmmo = MaxAmmo;
	}

	protected bool IsLocalMachineWeaponHolder()
	{
		return _weapon.CurrentHolder.IsOwner();
	}

	public void TryUse()
	{
		bool hasEnoughTimePassedSinceLastUsage = TimeIntervalCheck();

		if (!hasEnoughTimePassedSinceLastUsage) return;

		if (!HasAmmo())
		{
			UseWithNoAmmo();
			return;
		}

		RegisterUseTime();
		UseMechanism();
		ProduceUsageFX();
		CmdReplicateUsageFX();
	}

	[Command(requiresAuthority = false)]
	private void CmdReplicateUsageFX()
	{
		RpcReplicateUsageFX();
	}

	[ClientRpc]
	private void RpcReplicateUsageFX()
	{
		if (IsLocalMachineWeaponHolder()) return;

		ProduceUsageFX();
	}

	protected abstract void ProduceUsageFX();

	protected abstract void UseWithNoAmmo();

	protected abstract void UseMechanism();


	protected bool TimeIntervalCheck()
	{
		return Time.timeSinceLevelLoad > _lastUseTime + (float) MsTimeInterval / 1000;
	}

	protected void RegisterUseTime()
	{
		_lastUseTime = Time.timeSinceLevelLoad;
	}

	protected bool HasAmmo()
	{
		if (MaxAmmo > 0)
		{
			return CurrentAmmo > 0;
		}

		return true;
	}
}