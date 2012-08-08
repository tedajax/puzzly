using UnityEngine;
using System.Collections;

public class InteractableButton : MonoBehaviour
{
	public Transform[] linkedObjects;
	
	public FunctionInput[] functionInputs;
		
	private Vector3 basePosition;
	private Vector3 targetButtonPosition;
	
	public Vector3 pushOffset = Vector3.zero;
	
	public float pressInterval = 0.5f; //how often the button can be pressed;
	private float pressDelayTimer = 0.0f;
	
	public float pushMoveSeconds = 0.1f; //approximate number of seconds it takes for the button to move to push position
	
	private bool push = false;
	
	void Start()
	{
		basePosition = transform.localPosition;
		targetButtonPosition = basePosition;
	}
	
	void Update()
	{
		if (pressDelayTimer > 0.0f)
			pressDelayTimer -= Time.deltaTime;
		
		if (push)
		{
			targetButtonPosition = basePosition + pushOffset;
			if (Vector3.Distance(transform.localPosition, targetButtonPosition) < 0.01f)
			{
				push = false;
			}
		}
		else
			targetButtonPosition = basePosition;
		
		Vector3 velocity = Vector3.zero;
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetButtonPosition, ref velocity, pushMoveSeconds);
	}
	
	public void Interact(int input)
	{
		if (pressDelayTimer <= 0.0f)
		{
			foreach (Transform linkedObject in linkedObjects)
				foreach (FunctionInput fInput in functionInputs)
					linkedObject.SendMessage(fInput.function.ToString("g"), fInput.functionIntegerInput, SendMessageOptions.DontRequireReceiver);
			
			push = true;
			pressDelayTimer = pressInterval;
		}
	}
}
