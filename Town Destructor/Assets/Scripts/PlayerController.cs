using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public int cash = 0;
	public int goal = 1000;
	public int approval = 0;
	private GUISkin _playerGUI;
	private Vector3 scale;
	private float originalWidth = 800.0f;  // define here the original resolution
	private float originalHeight = 600.0f;
	public static int menuState=0;
	// Use this for initialization
	void Start () {
		_playerGUI = Resources.Load<GUISkin>("PlayerGUI");
	}
	
	// Update is called once per frame
	void OnGUI() {
			if (menuState == 0) {
						GUI.skin = _playerGUI;
						GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
						GUI.Label (new Rect (10, 20, 200, 250), "Cash: £" + cash + "\nGoal: £" + goal);
						GUI.Label (new Rect (650, 20, 150, 500), "Click - \nDemolish (To rebuild)\n \nDouble Click - \nUpgrade (Earn more cash per building)");
				} else {
					GUI.skin = _playerGUI;
					GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
					
					GUI.Label (new Rect (0, 150, 800, 150), "Thanks for playing! \nMore features soon!");
					if (GUI.Button (new Rect (350, 275, 125, 75), "Replay"))
					{
						menuState = 0;
						Application.LoadLevel(0);
					}
				}
		
	
	}


	void Update () {
		if (cash >= goal)
		{
			menuState = 1;
			Application.LoadLevel ("Win");
		}
	}
}
