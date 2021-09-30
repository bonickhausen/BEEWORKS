using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This is a helper class: it allows rerouting of the PhysX events to another gameobject/script.
/// </summary>
public class ColliderBroadcaster : MonoBehaviour
{
	public delegate void CollisionDelegate(Collision col);
	public delegate void TriggerDelegate(Collider other);

	public event CollisionDelegate CollisionEnter;
	public event CollisionDelegate CollisionStay;
	public event CollisionDelegate CollisionExit;

	public event TriggerDelegate TriggerEnter;
	public event TriggerDelegate TriggerStay;
	public event TriggerDelegate TriggerExit;

	private void OnTriggerEnter(Collider other)
	{
		if (TriggerEnter != null) TriggerEnter.Invoke(other);
	}

	private void OnTriggerStay(Collider other)
	{
		if (TriggerStay != null) TriggerStay.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		if (TriggerExit != null) TriggerExit.Invoke(other);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (CollisionEnter != null) CollisionEnter.Invoke(other);
	}

	private void OnCollisionStay(Collision other)
	{
		if (CollisionStay != null) CollisionStay.Invoke(other);
	}

	private void OnCollisionExit(Collision other)
	{
		if (CollisionExit != null) CollisionExit.Invoke(other);
	}
}