using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseMotorController), typeof(AnimatorController))]
public class BasePlayerController : MonoBehaviour 
{
	protected BaseMotorController motorController;
	protected PhysicsController physicsController;
	protected AnimatorController animatorController;

	public float walkAcceleration					= 35.0f;
	public float walkMaxSpeed						= 15.0f;

	protected virtual void Awake()
	{
		if(!motorController)
			motorController = GetComponent<BaseMotorController>();

		if(!physicsController)
			physicsController = GetComponent<PhysicsController>();

		if(!animatorController)
			animatorController = GetComponent<AnimatorController>();
	}

	protected virtual void Update()
	{
		HandleMotorController();
	}

	protected virtual void HandleMotorController()
	{
		Vector3 inputRaw = Vector3.zero;
		
		inputRaw.x = Input.GetAxis("Horizontal");
		inputRaw.y = Input.GetAxis("Vertical");
		
		motorController.Move(inputRaw, walkAcceleration, walkMaxSpeed);

		animatorController.SetFloat("horizontalSpeed", Mathf.Abs(physicsController.GetVelocity().x));

		if(Input.GetKey(KeyCode.Space))
		{
			motorController.Jump();
		}
	}
}
