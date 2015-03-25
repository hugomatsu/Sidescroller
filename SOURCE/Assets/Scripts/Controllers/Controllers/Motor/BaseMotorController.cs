using UnityEngine;
using System.Collections;

public class BaseMotorController : MonoBehaviour 
{
	public bool isGrounded;

	protected Transform[] groundCheckers;
	private float groundCheckersOffset;
	protected Vector3 groundNormal;
	protected Vector3 groundDirection;

	protected Collider cachedCollider;

	private Vector3 originalColliderSize;
	private Vector3 originalColliderCenter;

	public float airFriction 				= 0.98f;
	public float movementFriction 			= 0.9f;
	public float speedToStopAt 				= 5.0f;
	
	protected float stoppingForce;
	protected float currentMaxSpeed;
	protected float currentAcceleration;

	protected bool jumpPressed;
	protected bool isJumping;
	protected int jumpFrameLeft;

	public float jumpVelocity				= 12.0f;
	public int jumpTimeFrames				= 15;

	protected virtual void Awake()
	{
		CheckColliderSettings();
		SetGroundCheckers();

		Reset();
	}

	public virtual void Reset()
	{
		isGrounded = false;
		jumpPressed = false;
		isJumping = false;

		jumpFrameLeft = 0;

		groundNormal = Vector3.up;
		groundNormal = transform.forward;
	}

	/// <summary>
	/// Store the Collider settings.
	/// </summary>
	protected virtual void CheckColliderSettings()
	{
		if(cachedCollider == null)
			cachedCollider = GetComponent<Collider>();

		if(cachedCollider != null)
		{
			originalColliderSize = cachedCollider.bounds.size;
			originalColliderCenter = cachedCollider.bounds.center;
		}
	}

	/// <summary>
	/// Create the ground Checkers.
	/// </summary>
	protected virtual void SetGroundCheckers()
	{
		groundCheckersOffset = 0.05f;
		float offset = groundCheckersOffset * 0.8f;

		groundCheckers = new Transform[3];
		Vector3 position = Vector3.zero;

		groundCheckers[0] = new GameObject("P0").transform;
		groundCheckers[0].SetParent(this.transform, false);
		position = GetColliderCenter();
		position.x -= (GetColliderSize().x / 2.0f);
		position.y -= (GetColliderSize().y / 2.0f) - offset;
		groundCheckers[0].position = position;

		groundCheckers[1] = new GameObject("P1").transform;
		groundCheckers[1].SetParent(this.transform, false);
		position = GetColliderCenter();
		position.y -= (GetColliderSize().y / 2.0f) - offset;
		groundCheckers[1].position = position;

		groundCheckers[2] = new GameObject("P2").transform;
		groundCheckers[2].SetParent(this.transform, false);
		position = GetColliderCenter();
		position.x += (GetColliderSize().x / 2.0f);
		position.y -= (GetColliderSize().y / 2.0f) - offset;
		groundCheckers[2].position = position;
	}

	protected virtual void FixedUpdate()
	{
		CheckGround();
		UpdateJumping();
	}

	public virtual void Move(Vector3 pDirection, float pAccel, float pMaxSpeed)
	{
	}

	public virtual void Jump()
	{
		jumpPressed = true;
	}

	public virtual void UpdateJumping()
	{

	}

	/// <summary>
	/// Check if there object is grounded.
	/// </summary>
	protected virtual void CheckGround()
	{
		if(groundCheckers != null)
		{
			RaycastHit hit;
			isGrounded = false;

			for(int i = 0; i < groundCheckers.Length; i++)
			{
				Ray ray = new Ray(groundCheckers[i].position, -groundCheckers[i].up);
				Debug.DrawRay(ray.origin, ray.direction * groundCheckersOffset, Color.red);

				if(Physics.Raycast(ray, out hit, groundCheckersOffset))
				{
					StoreGroundInfo(hit);
				}
			}
		}
	}

	/// <summary>
	/// Store the ground infos.
	/// </summary>
	/// <param name="pHit">P hit.</param>
	protected virtual void StoreGroundInfo(RaycastHit pHit)
	{
		isGrounded = true;
		groundNormal = pHit.normal;
		groundDirection = new Vector3(pHit.normal.y, -pHit.normal.x, 0.0f);
	}



#region GET/SET
	public Vector3 GetColliderSize()
	{
		return cachedCollider.bounds.size;
	}

	public Vector2 GetColliderCenter()
	{
		return cachedCollider.bounds.center;
	}

	public Vector3 GetOriginalColliderCenter()
	{
		return originalColliderCenter;
	}

	public Vector3 GetOriginalColliderSize()
	{
		return originalColliderSize;
	}

	public bool IsGrounded()
	{
		return isGrounded;
	}
#endregion
}
