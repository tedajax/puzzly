using UnityEngine;
using System.Collections;

public class TriggerArea : MonoBehaviour 
{
	public InteractableFunctions enterFunction;
	public FunctionInputTypes enterInputType;
	public int enterFunctionInputInteger;
	
	public InteractableFunctions stayFunction;
	public FunctionInputTypes stayInputType;
	public int stayFunctionInputInteger; 
	
	public InteractableFunctions exitFunction;
	public FunctionInputTypes exitInputType;
	public int exitFunctionInputInteger;
	
	public Transform[] linkedObjects;
	
	public bool filterByTags = false;
	public string[] allowedTags;
	
	public bool limitObjectCount = false;
	public int maxObjects = 1;
	private int objectCount = 0;
	
//	public bool filterByLayermask = false;
//	public LayerMask[] allowedLayers;
	
//	private LayerMask collisionLayers;
	
	void Update()
	{
//		collisionLayers = 0;
//		foreach (LayerMask mask in allowedLayers)
//		{
//			collisionLayers |= ~mask.value;
//		}
//		
//		if (filterByLayermask)
//			gameObject.layer = collisionLayers;
//		else
//			gameObject.layer = 0;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 0.5f);
		Gizmos.DrawCube(transform.position, transform.localScale);
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (!filterByTags || tagAllowed(collider.tag))
		{
			objectCount++;
			
			if (objectCount > maxObjects)
				return;
			
			foreach (Transform linkedObject in linkedObjects)			
				if (enterInputType == FunctionInputTypes.Collider)
					linkedObject.SendMessage(enterFunction.ToString("g"), 
				 						    collider, 
										    SendMessageOptions.DontRequireReceiver);
				else
					linkedObject.SendMessage(enterFunction.ToString("g"), 
										    enterFunctionInputInteger,
										    SendMessageOptions.DontRequireReceiver);	
		}
	}
	
	void OnTriggerStay(Collider collider)
	{
		if (!filterByTags || tagAllowed(collider.tag))
		{
			foreach (Transform linkedObject in linkedObjects)			
				if (stayInputType == FunctionInputTypes.Collider)
					linkedObject.SendMessage(stayFunction.ToString("g"), 
				 						    collider, 
										    SendMessageOptions.DontRequireReceiver);
				else
					linkedObject.SendMessage(stayFunction.ToString("g"), 
										    stayFunctionInputInteger,
										    SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerExit(Collider collider)
	{		
		if (!filterByTags || tagAllowed(collider.tag))
		{
			objectCount--;
			
			if (objectCount > 0)
				return;
				
			foreach (Transform linkedObject in linkedObjects)			
				if (exitInputType == FunctionInputTypes.Collider)
					linkedObject.SendMessage(exitFunction.ToString("g"), 
				 						    collider, 
										    SendMessageOptions.DontRequireReceiver);
				else
					linkedObject.SendMessage(exitFunction.ToString("g"), 
										    exitFunctionInputInteger,
										    SendMessageOptions.DontRequireReceiver);
		}
	}
	
	bool tagAllowed(string tag)
	{
		for (int i = 0; i < allowedTags.Length; i++)
			if (allowedTags[i].Equals(tag))
				return true;
		
		return false;
	}
}
