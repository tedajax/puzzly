using UnityEngine;
using System.Collections;

public class PushRegion : MonoBehaviour 
{	
	public Vector3 pushDirectionVector = Vector3.zero;
	public float forcePower = 10.0f;
	
	public void ApplyForce(Collider collider)
	{
		if (collider.rigidbody != null)
		{
			collider.rigidbody.AddForce(pushDirectionVector.normalized * forcePower);
		}
		else
		{
			CharacterImpactReceiver script = collider.GetComponent<CharacterImpactReceiver>();
			if (script != null)
			{
				script.AddImpact(pushDirectionVector.normalized, forcePower);
			}
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + pushDirectionVector.normalized * forcePower);
	}
}
