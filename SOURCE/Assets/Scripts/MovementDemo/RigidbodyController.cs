using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour 
{
	public float gravityForce = 3.5f;

	public float accelerationWalking = 35.0f;
	public float maxSpeedWalking = 15.0f;

	private float stoppingForce = 0.0f;
	public float speedToStopAt = 5.0f;
	public float moveFriction = 0.9f;
	public float airFriction = 0.98f;

	private Vector3 groundDirection = Vector3.right;
	public Vector3 inputDirection;

	private float characterHeight;
	private float characterWidth;

	public bool isGrounded;
	private float maxGroundWalkingAngle = 30.0f;

	public Transform[] groundCheckers;

	void Start()
	{
//		rigidbody.centerOfMass = new Vector3(0.0f, -1.0f, 0.0f);
		RecalculateBounds();
	}

	void Update()
	{
		inputDirection.x = Input.GetAxisRaw("Horizontal");
	}

	void FixedUpdate()
	{
		UpdateGroundInfo();
		ApplyGravity();
		ApplyMovementFriction();
		Walk(inputDirection);
	}

	void Walk(Vector3 pDirection)
	{
		Vector3 moveForce = groundDirection * pDirection.x * accelerationWalking;

//		Debug.DrawRay(transform.position, moveForce, Color.blue);

		GetComponent<Rigidbody>().AddForce(moveForce, ForceMode.Acceleration);
		stoppingForce = 1 - Mathf.Abs(pDirection.x);
	}

	void ApplyGravity()
	{
		if(!isGrounded)
			GetComponent<Rigidbody>().AddForce(Physics.gravity * gravityForce, ForceMode.Acceleration);
	}

	void ApplyMovementFriction()
	{
		Vector3 velocity = GetComponent<Rigidbody>().velocity;

		if(isGrounded && stoppingForce > 0.0f)
		{
			Vector3 velocityInGroundDir = Vector3.Dot(velocity, groundDirection) * groundDirection;
			Vector3 newVelocityInGroundDir = velocityInGroundDir * Mathf.Lerp(1.0f, moveFriction, stoppingForce);

			velocity -= (velocityInGroundDir - newVelocityInGroundDir);
		}

		velocity *= airFriction;

		float absSpeed = Mathf.Abs(velocity.x);
		float maxSpeed = maxSpeedWalking;

		if(absSpeed > maxSpeed)
			velocity.x *= maxSpeed / absSpeed;

		if(absSpeed < speedToStopAt && stoppingForce == 1.0f)
			velocity.x = 0.0f;

		GetComponent<Rigidbody>().velocity = velocity;
		stoppingForce = 1.0f;
	}

	#region GROUND INFO
	void UpdateGroundInfo()
	{
		float epsilon = 0.05f;
		float extraHeight = characterHeight * 0.75f;
		float halfPlayerWidth = characterWidth * 0.49f;

		CheckGround();
	}

	void HitGround(Vector3 pOrigin, RaycastHit hit)
	{
		groundDirection = new Vector3(hit.normal.y, -hit.normal.x, 0.0f);
		float groundAngle = Vector3.Angle(groundDirection, new Vector3(groundDirection.x, 0.0f, 0.0f));

		if(groundAngle <= maxGroundWalkingAngle)
		{
			isGrounded = true;
		}
	}

	void CheckGround()
	{
		RaycastHit hit;
		isGrounded = false;

		for(int i = 0; i < groundCheckers.Length; i++)
		{
			Ray ray = new Ray(groundCheckers[i].position, Vector3.down);
			Debug.DrawRay(groundCheckers[i].position, Vector3.down * 0.3f, Color.black);

			if(Physics.Raycast(ray, out hit, 0.3f))
			{
				groundDirection = new Vector3(hit.normal.y, -hit.normal.x, 0.0f);
				isGrounded = true;
			}
		}
	}
	#endregion

	void RecalculateBounds()
	{
		characterHeight = GetComponent<Collider>().bounds.size.y;
		characterWidth = GetComponent<Collider>().bounds.size.x;
	}
}
