using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	public enum DoorStates
	{
		Closed,
		Open
	}
	
	public bool playAudio = true;
	
	public DoorStates doorState = DoorStates.Closed;
	public DoorStates defaultState = DoorStates.Closed;
	public Vector3 openPosition = Vector3.zero;
	public Vector3 closedPosition = Vector3.zero;
	
	public float smoothSpeed = 0.1f;
	
	public float secondsTilDefault = -1.0f;
	private float defaultTimer = 0.0f;
	
	private Vector3 targetPosition = Vector3.zero;
	private Vector3 currentVelocity = Vector3.zero;
	
	private Vector3 basePosition = Vector3.zero;
	
	private int doorOpenCount = 0;
	
	void Start()
	{
		basePosition = transform.position;
		
		if (doorState == DoorStates.Closed)
		{
			transform.position += closedPosition;
			targetPosition = closedPosition;
		}
		else
		{
			transform.position += openPosition;
			targetPosition = openPosition;
		}
		
		targetPosition += basePosition;
		
		defaultTimer = secondsTilDefault;
	}
	
	void Update()
	{
		if (secondsTilDefault > 0.0f)
		{
			if (defaultTimer > 0.0f)
				defaultTimer -= Time.deltaTime;
			
			if (defaultTimer <= 0.0f && doorState != defaultState)
			{
				if (defaultState == DoorStates.Closed)
				{
					targetPosition = closedPosition + basePosition;
					doorState = DoorStates.Closed;
					doorOpenCount = 0;
				}
				else
				{
					targetPosition = openPosition + basePosition;
					doorState = DoorStates.Open;
				}
			}
		}
		
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
						   					    ref currentVelocity, smoothSpeed * Time.deltaTime);
	}
	
	public void DoorToggle()
	{
		if (doorState == DoorStates.Closed)
			doorState = DoorStates.Open;
		else
			doorState = DoorStates.Closed;
		
		if (doorState == DoorStates.Closed)
			targetPosition = closedPosition + basePosition;
		else
			targetPosition = openPosition + basePosition;
		
		if (secondsTilDefault > 0.0f)
		{
			if (defaultState != doorState)
				defaultTimer = secondsTilDefault;
		}
	}
	
	public void DoorSetState(int state)
	{
		if (secondsTilDefault > 0.0f)
			if ((DoorStates)state == defaultState || defaultTimer > 0.0f)
				return;
		
		doorState = (DoorStates)state;
		
		if (doorState == DoorStates.Open)
			doorOpenCount++;
		else
			doorOpenCount--;
		
		if (doorOpenCount < 0) doorOpenCount = 0;
		
		if (doorOpenCount <= 0)
		{
			targetPosition = closedPosition + basePosition;
		}
		else
		{
			targetPosition = openPosition + basePosition;
			if (playAudio && doorOpenCount == 1 && doorState == DoorStates.Open)
				audio.PlayOneShot(audio.clip);
		}
		
		if (secondsTilDefault > 0.0f)
		{
			if (defaultState != doorState)
				defaultTimer = secondsTilDefault;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position + openPosition, 0.5f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position + closedPosition, 0.5f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + openPosition, transform.position + closedPosition);
	}
}
