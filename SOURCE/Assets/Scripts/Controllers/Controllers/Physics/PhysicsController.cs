using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsController : MonoBehaviour 
{
	private Rigidbody cachedRigidbody;
	
	public Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

	protected virtual void Awake()
	{
		if(cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody>();

	}

	public void AddForce(Vector3 pForce, ForceMode pMode)
	{
		cachedRigidbody.AddForce(pForce, pMode);
	}

	#region GET/SET
	public Vector3 GetVelocity()
	{
		return this.cachedRigidbody.velocity;
	}

	public void SetVelocity(Vector3 pVelocity)
	{
		cachedRigidbody.velocity = pVelocity;
	}

	public Vector3 GetGravity()
	{
		return this.gravity;
	}

	public void SetGravity(Vector3 pGravity)
	{
		this.gravity = pGravity;
	}
	#endregion
}
