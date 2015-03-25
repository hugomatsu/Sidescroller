using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsController))]
public class MotorControllerPhysics : BaseMotorController
{
	private PhysicsController physicsController;

	protected override void Awake ()
	{
		base.Awake ();

		if(physicsController == null)
			physicsController = GetComponent<PhysicsController>();
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();

		if(!isGrounded)
			ApplyGravity();

		ApplyMovementFriction();
	}

	public override void Move(Vector3 pDirection, float pAccel, float pMaxSpeed)
	{
		base.Move(pDirection, pAccel, pMaxSpeed);

		currentMaxSpeed = pMaxSpeed;
		currentAcceleration = pAccel;

		physicsController.AddForce(groundDirection * pDirection.x * currentAcceleration, ForceMode.Acceleration);
		stoppingForce = 1 - Mathf.Abs(pDirection.x);
	}

	public override void Jump ()
	{
		base.Jump ();

		if(jumpFrameLeft == 0 && !isJumping)
		{
			if(IsGrounded())
			{
				jumpFrameLeft = jumpTimeFrames;
				isJumping = true;
			}
		}

		if(jumpFrameLeft != 0)
		{
			Vector3 velocity = physicsController.GetVelocity();
			velocity.y = jumpVelocity;
			physicsController.SetVelocity(velocity);
		}
	}

	public override void UpdateJumping ()
	{
		base.UpdateJumping ();

		if(!jumpPressed && isJumping)
		{
			jumpFrameLeft = 0;
			isJumping = false;
		}

		jumpPressed = false;

		if(jumpFrameLeft != 0)
			jumpFrameLeft--;
	}

	public void ApplyGravity()
	{
		physicsController.AddForce(physicsController.GetGravity(), ForceMode.Acceleration);
	}

	public void ApplyMovementFriction()
	{
		Vector3 velocity = physicsController.GetVelocity();

		if(isGrounded && stoppingForce > 0.0f)
		{
			Vector3 velocityInGround = Vector3.Dot(velocity, groundDirection) * groundDirection;
			Vector3 newVelocityInGround = velocityInGround * Mathf.Lerp(1.0f, movementFriction, stoppingForce);
			velocity -= (velocityInGround - newVelocityInGround);
		}

		velocity *= airFriction;

		float absSpeed = Mathf.Abs(velocity.x);

		if(absSpeed > currentMaxSpeed)
			velocity.x *= currentMaxSpeed / absSpeed;

		//Apply Min Speed
		if(absSpeed < speedToStopAt && stoppingForce == 1.0f)
//			velocity.x = Mathf.Lerp(velocity.x, 0.0f, Time.deltaTime * currentAcceleration);
			velocity.x = 0.0f;

		physicsController.SetVelocity(velocity);
		stoppingForce = 1.0f;
	}
}
