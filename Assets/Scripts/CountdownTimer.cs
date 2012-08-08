using UnityEngine;
using System.Collections;

public class CountdownTimer : MonoBehaviour 
{
	public float startTime = 5.0f;
	public Transform linkedObject;
	DigitalNumber gameObjectOutput;
	
	public bool doCountdown = false;
	
	void Start()
	{
		gameObjectOutput = linkedObject.GetComponent<DigitalNumber>();
		
		if (gameObjectOutput != null)
			gameObjectOutput.SendMessage("SetDigitValue", (int)startTime);
	}
	
	void Update()
	{
		if (doCountdown)
			if (startTime > 0.0f)
				startTime -= Time.deltaTime;
			else
				doCountdown = false;
		
		if (gameObjectOutput != null)
			gameObjectOutput.SendMessage("SetDigitValue", Mathf.CeilToInt(Mathf.Clamp(startTime, 0.0f, 10.0f)));
	}
	
	public void SetTimeSeconds(int time)
	{
		if (!doCountdown)
		{
			doCountdown = true;
			startTime = (float)time;
		}
	}
}
