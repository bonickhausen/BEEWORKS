using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PawnWeaponHandler : PawnComponent
{
	[Header("Objects")]
	public Transform WorldModelParent;
	public Transform WorldModelSway;
	[Header("Settings")]
	public AnimationCurve WorldModelAimCurve;
	public float WorldModelSwayRate = 1f;
	public float WorldModelSwayIntensity = 1f;
	[Header("Status")]
	[SyncVar]
	public int CurrentWeaponSlot;

	public ItemWeapon CurrentWeapon { get; private set; }

	private PawnMotor _motor;
	private PawnViewRigFPSTPS _viewFps;
	private PawnInventory _inventory;
	private ItemWeapon[] _weapons;
	private short _lastWeaponSwapAttempt;
	private int _lastFrameWeaponSlot;
	private float _lerpedMotorVelocity;

	private const float MAX_WEAPON_SOCKET_ANGLE = 20;

	protected override void Initialize()
	{
		base.Initialize();
		_viewFps = GetComponent<PawnViewRigFPSTPS>();
		_inventory = GetComponent<PawnInventory>();
		_motor = GetComponent<PawnMotor>();
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
			TryWeaponUsage();
		}

		CheckForChanges();
		RotateWeaponWorldModelSocket();
		SwayWeaponWorldModelSocket();
	}

	private void SwayWeaponWorldModelSocket()
	{
		float lerpRate = Time.deltaTime * 20f;
		_lerpedMotorVelocity = Mathf.Lerp(_lerpedMotorVelocity, _motor.GetLocalVelocity().magnitude, lerpRate);

		Vector3 pos = WorldModelSway.localPosition;
		if (_lerpedMotorVelocity > 0.1f)
		{
			pos.y = Mathf.Lerp(pos.y, (Mathf.Sin(WorldModelSwayRate * Time.timeSinceLevelLoad) * WorldModelSwayIntensity), lerpRate);
		}
		else
		{
			pos.y = Mathf.Lerp(pos.y, 0f, lerpRate);
		}
		WorldModelSway.localPosition = pos;
	}

	private void RotateWeaponWorldModelSocket()
	{
		float curPitch = _viewFps.NetworkedPitch;
		Vector3 leuler = WorldModelParent.localEulerAngles;
		float modulatedPitch = (WorldModelAimCurve.Evaluate(Mathf.Abs(curPitch) / PawnViewRigRotation.ANGLE_LIMIT)) * MAX_WEAPON_SOCKET_ANGLE * Mathf.Sign(curPitch);
		leuler.x = Mathf.LerpAngle(leuler.x, modulatedPitch, Time.deltaTime * 20f);
		WorldModelParent.localEulerAngles = leuler;
	}

	private void CheckForChanges()
	{
		if (_inventory.IsInitialized == false) return;

		if (CurrentWeaponSlot != _lastFrameWeaponSlot)
		{
			OnWeaponChanged();
		}

		_lastFrameWeaponSlot = CurrentWeaponSlot;

		void OnWeaponChanged()
		{
			CurrentWeapon = GetWeaponInSlot(CurrentWeaponSlot);

			CurrentWeapon.SetHolder(this);

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

	private void TryWeaponUsage()
	{
		if (!CurrentWeapon) return;

		if (_cmd.Attack)
		{
			CurrentWeapon.TryAttack();
		}

		if (_cmd.AltAttack)
		{
			CurrentWeapon.TryAltAttack();
		}
	}

	[Command(requiresAuthority = false)]
	private void TrySwapWeapon(int slot)
	{
		ItemWeapon w = GetWeaponInSlot(slot);
		if (w) CurrentWeaponSlot = slot;
	}

	private ItemWeapon GetWeaponInSlot(int slot)
	{
		if (_weapons == null || _weapons.Length <= 0)
		{
			RegenerateWeaponsArray();
		}

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