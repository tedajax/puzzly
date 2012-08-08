using UnityEngine;
using System.Collections;

public class ClearDoorway : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag.Equals("Player"))
		{
			collision.collider.rigidbody.AddForce(Vector3.forward * 100);
		}
	}
}
