using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour 
{
	protected Animator animator;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}


	
	#region SET VALUES
	public void SetFloat(string pName, float pValue)
	{
		animator.SetFloat(pName, pValue);
	}

	public void SetFloat(int pID, float pValue)
	{
		animator.SetFloat(pID, pValue);
	}

	public void SetInt(string pName, int pValue)
	{
		animator.SetInteger(pName, pValue);
	}

	public void SetInt(int pID, int pValue)
	{
		animator.SetInteger(pID, pValue);
	}

	public void SetBool(string pName, bool pValue)
	{
		animator.SetBool(pName, pValue);
	}

	public void SetBool(int pID, bool pValue)
	{
		animator.SetBool(pID, pValue);
	}

	public void SetTrigger(string pName)
	{
		animator.SetTrigger(pName);
	}

	public void SetTrigger(int pID)
	{
		animator.SetTrigger(pID);
	}
	#endregion
}
