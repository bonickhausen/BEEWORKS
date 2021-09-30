using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkThing : NetworkBehaviour
{
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