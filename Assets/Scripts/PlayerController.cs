using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public enum PlayerIndex
	{
		One,
		Two
	}
	
	public PlayerIndex playerIndex = PlayerIndex.One;
	
	public float moveSpeed = 5.0f;
	public float maxSpeed = 10.0f;
	public float moveSpeedCurveValue = 2.0f;
	public float dragCoefficient = 20.0f;
	
	private Vector3 movementDirection = Vector3.zero;
	
	void Start()
	{
	
	}
	
	void CharacterMovement()
	{
		float hScalar = 0.0f, vScalar = 0.0f;
		string axisPrefix = (playerIndex == PlayerIndex.One) ? "P1" : "P2";
		
		hScalar = Input.GetAxis(axisPrefix + "_Horizontal");
		vScalar = Input.GetAxis(axisPrefix + "_Vertical");
			
		movementDirection = Vector3.Lerp(movementDirection, 
										 new Vector3(hScalar, 0.0f, vScalar), 
										 moveSpeedCurveValue * Time.deltaTime);
				
		Vector3 movement = movementDirection * moveSpeed * Time.deltaTime;
		
		rigidbody.AddForce(movement, ForceMode.Impulse);
		
		Vector3 rigidVelocityXZ = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z);
		float speed = rigidVelocityXZ.magnitude;
		
		rigidbody.drag = (speed / maxSpeed) * dragCoefficient;
	}
	
	void FixedUpdate()
	{		
		CharacterMovement();
	}
	
//	void OnCollisionEnter(Collision collision)
//	{
//		
//		if (collider.gameObject.tag.Equals("Player"))
//		{
//			collider.GetComponent<CharacterImpactReceiver>().AddImpact((transform.position - collider.transform.position).normalized, 5.0f);
//		}
//	}
}
