using UnityEngine;
using System.Collections;

public class CharacterImpactReceiver : MonoBehaviour 
{
	public float mass = 3.0f;
	private Vector3 impact = Vector3.zero;
	
	private CharacterController controller;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		
		if (controller == null) enabled = false;
	}
	
	public void AddImpact(Vector3 direction, float force)
	{
		impact += direction.normalized * force / mass;
	}
	
	void Update()
	{
		if (impact.magnitude > 0.2f) controller.Move(impact * Time.deltaTime);
		impact = Vector3.Lerp(impact, Vector3.zero, 1 * Time.deltaTime);
	}
}
