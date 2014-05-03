using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float cash = 0;
	public int goal = 2000;
	public int approval = 0;
	private GUISkin _playerGUI;
	private Vector3 scale;
	private float originalWidth = 800.0f;  // define here the original resolution
	private float originalHeight = 600.0f;
	public static int menuState=0;
	public float scrollSpeed = 5;
	private Rect screenRect;
	// Use this for initialization
	void Start () {
		_playerGUI = Resources.Load<GUISkin>("PlayerGUI");
		screenRect = new Rect(0,0, Screen.width, Screen.height);
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
		
		if (screenRect.Contains (Input.mousePosition)) {
						if (Input.mousePosition.x < Screen.width * 0.2 && transform.position.x > -10) {
								// Dampen towards the target rotation
								transform.position -= new Vector3 (scrollSpeed * Time.deltaTime, 0.0f, 0.0f);
						} 
						if (Input.mousePosition.x > Screen.width * 0.8 && transform.position.x < 10) {
								// Dampen towards the target rotation
								transform.position += new Vector3 (scrollSpeed * Time.deltaTime, 0.0f, 0.0f);
						}  
						if (Input.mousePosition.y < Screen.height * 0.2 && transform.position.z > -20) {
								// Dampen towards the target rotation
								transform.position -= new Vector3 (0.0f, 0.0f, scrollSpeed * Time.deltaTime);
						}  
						if (Input.mousePosition.y > Screen.height * 0.8 && transform.position.z < 10) {
								// Dampen towards the target rotation
								transform.position += new Vector3 (0.0f, 0.0f, scrollSpeed * Time.deltaTime);
						}
			if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < 15) // back
			{
				transform.position += new Vector3 (0.0f, scrollSpeed*5 * Time.deltaTime, 0.0f);
				//Debug.Log ("test");
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > 2) // forward
			{
				transform.position -= new Vector3 (0.0f, scrollSpeed*5 * Time.deltaTime, 0.0f);
				//Debug.Log ("test");
			}
		}
	}
}
