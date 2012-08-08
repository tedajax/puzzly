using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DigitalNumber : MonoBehaviour 
{
	public int digitValue = 0;
	
	public Material offMaterial;
	public Material onMaterial;
	
	private Transform[] digitSlices;
	
	private int[,] digitDefs = {
		{ 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 }, //0
		{ 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 }, //1
		{ 1, 1, 0, 1, 1, 1, 1, 0, 1, 1 }, //2
		{ 1, 1, 0, 1, 0, 1, 0, 1, 1, 1 }, //3
		{ 0, 0, 1, 1, 1, 1, 0, 1, 0, 0 }, //4
		{ 1, 1, 1, 0, 1, 1, 0, 1, 1, 1 }, //5
		{ 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 }, //6
		{ 1, 1, 0, 1, 0, 0, 0, 1, 0, 0 }, //7
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //8
		{ 1, 1, 1, 1, 1, 1, 0, 1, 0, 0 }  //9
	};
	
	void Start()
	{
		digitSlices = new Transform[10];
		
		foreach (Transform child in transform)
		{
			digitSlices[int.Parse(child.name)] = child;
		}
	}
	
	void Update()
	{
		while (digitValue < 0) digitValue += 10;
		while (digitValue > 9) digitValue -= 10;
			
		SetMaterialsFromValue();	
	}
	
	void SetMaterialsFromValue()
	{
		for (int i = 0; i < 10; i++)
		{
			digitSlices[i].gameObject.renderer.sharedMaterial = (digitDefs[digitValue, i] == 1) ? onMaterial : offMaterial;
		}
	}
	
	public void SetDigitValue(int digit)
	{
		digitValue = digit;
	}
}
