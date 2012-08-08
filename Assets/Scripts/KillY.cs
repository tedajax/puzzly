using UnityEngine;
using System.Collections;

public class KillY : MonoBehaviour 
{
	public enum KillBehavior
	{
		ResetPosition,
		Destroy
	}
	
	public static float KILL_Y = -10.0f;
	public Vector3 resetPosition = Vector3.zero;
	public KillBehavior behavior;
	
	public Transform[] additionalObjects;
	
	void Update()
	{
		if (transform.position.y <= KILL_Y)
		{
			switch (behavior)
			{
			case KillBehavior.ResetPosition:
				transform.position = resetPosition;
				foreach (Transform t in additionalObjects)
					t.transform.position = t.GetComponent<KillY>().resetPosition;
				break;
				
			case KillBehavior.Destroy:
				Destroy(gameObject);
				foreach (Transform t in additionalObjects)
					Destroy(t.gameObject);
				break;
			}
		}
	}
}
