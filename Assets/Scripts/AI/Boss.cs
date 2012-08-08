using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour 
{
	public enum BossStates
	{
		Inactive,
		Spawn,
		Roar,
		Target,
		Chase,
		Jump,
		Hover,
		Slam,
		Dead
	}
	
	private BossStates state = BossStates.Inactive;
	
	private Vector3 baseScale = Vector3.one;
	private Vector3 roarMaxSize = Vector3.zero;
	private Vector3 targetRoarSize = Vector3.zero;
	
	public Transform playerOne;
	public Transform playerTwo;
	
	private Transform currentTarget;
	private Vector3 currentVectorTarget;
	
	public float targetingTime = 2.0f;
	public float chaseTime = 5.0f;
	
	private float actionTimer = 0.0f;
	
	private float rotation = 180.0f;
	
	public AudioClip roarClip;
	
	void Start()
	{
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
		
		baseScale = transform.localScale;
		roarMaxSize = baseScale * 1.5f;
	}
	
	void StateInactive()
	{
		
	}
	
	void StateSpawn()
	{
		if (transform.position.y < 1.0f && rigidbody.velocity.magnitude < 0.1f)
			SetState(BossStates.Roar);
	}
	
	void StateRoar()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, targetRoarSize, 0.025f);

		if (Mathf.Abs(transform.localScale.magnitude - roarMaxSize.magnitude) < 0.1f)
		{
			targetRoarSize = baseScale;
		}
		
		if (Vector3.Distance(transform.localScale, targetRoarSize) < 0.1f && targetRoarSize == baseScale)
		{
			transform.localScale = baseScale;
			SetState(BossStates.Target);
		}
	}
	
	void StateTarget()
	{
		float rotVelocity = 0.0f;
		rotation = Mathf.SmoothDampAngle(rotation, Mathf.Atan2(currentTarget.position.z - transform.position.z,
									 currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90, ref rotVelocity, 0.1f);
		transform.eulerAngles = new Vector3(0.0f, -rotation, 0.0f);
		
		actionTimer -= Time.deltaTime;
		
		if (actionTimer <= 0.0f)
			SetState(BossStates.Chase);
	}
	
	void StateChase()
	{
		float rotVelocity = 0.0f;
		rotation = Mathf.SmoothDampAngle(rotation, Mathf.Atan2(currentTarget.position.z - transform.position.z,
									 currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg - 90, ref rotVelocity, 0.1f);
		transform.eulerAngles = new Vector3(0.0f, -rotation, 0.0f);
		
		Vector3 moveDirection = currentTarget.position - transform.position;
		moveDirection.Normalize();
		
		if (Physics.Raycast(transform.position, Vector3.down))
			transform.position += transform.forward * 4.0f * Time.deltaTime;
		
		actionTimer -= Time.deltaTime;
		
		if (actionTimer <= 0.0f && Vector3.Distance(transform.position, currentTarget.position) < 15.0f)
			SetState(BossStates.Jump);
	}
	
	void StateJump()
	{
		transform.position = Vector3.Lerp(transform.position, currentVectorTarget, 0.05f);
		
		if (Vector3.Distance(transform.position, currentVectorTarget) < 0.5f)
			SetState(BossStates.Hover);
	}
	
	void StateHover()
	{
		actionTimer -= Time.deltaTime;
		
		if (actionTimer <= 0.0f)
			SetState(BossStates.Slam);
	}
	
	void StateSlam()
	{
		if (transform.position.y < 1.0f && rigidbody.velocity.magnitude <= 0.1f)
			SetState(BossStates.Roar);
	}
	
	void StateDead()
	{
	}
	
	void StateMachine()
	{	
		switch (state)
		{
		case BossStates.Inactive:
			StateInactive();
			break;
			
		case BossStates.Spawn:
			StateSpawn();
			break;
			
		case BossStates.Roar:
			StateRoar();
			break;
		
		case BossStates.Target:
			StateTarget();
			break;
			
		case BossStates.Chase:
			StateChase();
			break;
			
		case BossStates.Jump:
			StateJump();
			break;
		
		case BossStates.Hover:
			StateHover();
			break;
		
		case BossStates.Slam:
			StateSlam();
			break;
		
		case BossStates.Dead:
			StateDead();
			break;
		}
	}
	
	void Update()
	{
		StateMachine();
		
		if (transform.position.y < -10.0f)
			SetState(BossStates.Dead);
	}
	
	void SetState(BossStates state)
	{
		this.state = state;
		
		switch (this.state)
		{
		case BossStates.Spawn:
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
		
			Camera.main.audio.clip = Resources.Load("Music/boss-music") as AudioClip;
			Camera.main.audio.Play();
		
			break;
			
		case BossStates.Roar:
			rigidbody.isKinematic = true;
			targetRoarSize = roarMaxSize;
		
			audio.PlayOneShot(roarClip);
			break;
			
		case BossStates.Target:
			rigidbody.isKinematic = false;
			float p1Dist = Vector3.Distance(playerOne.transform.position, transform.position);
			float p2Dist = Vector3.Distance(playerTwo.transform.position, transform.position);
			
			currentTarget = (p1Dist < p2Dist) ? playerOne.transform : playerTwo.transform;
			
			actionTimer = targetingTime;
			
			break;	
			
		case BossStates.Chase:
			rigidbody.isKinematic = false;
			actionTimer = chaseTime;
			break;
			
		case BossStates.Jump:
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
			
			currentVectorTarget = new Vector3(currentTarget.position.x, currentTarget.position.y, currentTarget.position.z);
			currentVectorTarget.y += 15.0f;
			break;
			
		case BossStates.Hover:
			actionTimer = 0.25f;
			break;
			
		case BossStates.Slam:
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			break;
			
		case BossStates.Dead:
			Camera.main.audio.clip = Resources.Load("Music/victory") as AudioClip;
			Camera.main.audio.Play();
			Camera.main.SendMessage("ActivateWin");
			Destroy(gameObject);
			break;
		}
	}
		
	void Spawn()
	{
		if (state == BossStates.Inactive)
			SetState(BossStates.Spawn);
	}
}
