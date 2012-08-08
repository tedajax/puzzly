using UnityEngine;
using System.Collections;

public class TwoPlayerCamera : MonoBehaviour 
{
	public Transform playerOne;
	public Transform playerTwo;
	
	public Vector3 minOffset = new Vector3(0.0f, 20.0f, 5.0f);
	public Vector3 maxOffset = new Vector3(0.0f, 100.0f, 15.0f);
	
	public float maxBaseDistance = 10.0f;
	public float maxDistance = 50.0f;
	
	void Start()
	{
		
	}
	
	void Update()
	{
		Vector3 averagePosition = (playerOne.position + playerTwo.position) / 2.0f;
		Vector3 playerOnePosition = playerOne.position;
		playerOnePosition.y = 0.0f;
		Vector3 playerTwoPosition = playerTwo.position;
		playerTwoPosition.y = 0.0f;
		float playerDistance = (playerOnePosition - playerTwoPosition).magnitude;
				
		if (playerDistance < maxBaseDistance)
			transform.position = averagePosition + minOffset;
		else
			transform.position = averagePosition + Vector3.Lerp(minOffset, maxOffset, (playerDistance - maxBaseDistance) / (maxDistance - maxBaseDistance));
	}
}
