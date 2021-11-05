using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class NetworkThing : NetworkBehaviour
{
	[Header("Network Properties")]
	public float GeneralRPCInterval = 0.1f;

	private float _lastRpcSendTime;

	protected bool CanSendRPC(bool doNotFlag = false)
	{
		bool result = Time.timeSinceLevelLoad > _lastRpcSendTime + GeneralRPCInterval;
		if (result && !doNotFlag)
		{
			_lastRpcSendTime = Time.timeSinceLevelLoad;
		}

		return result;
	}

	protected NetworkIdentity FetchNetworkIdentity(uint id)
	{
		NetworkIdentity result = isServer ? NetworkServer.spawned[id] : NetworkClient.spawned[id];

		return result;
	}

	public bool IsOwner()
	{
		return netIdentity.hasAuthority;
	}
}