using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour 
{
	bool showWin = false;
	
	public GUIStyle winStyle;
	
	void OnGUI()
	{
		if (showWin)
		{
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 800, 400), "YOU WIN!");
		}
	}
	
	void ActivateWin()
	{
		showWin = true;
	}
}
