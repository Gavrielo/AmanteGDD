using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float cash = 0;
	public int goal = 2000;
	public int approval = 0;
	
	public  AudioSource[] soundList;
	private GUISkin _playerGUI;
	private Vector3 scale;
	private float originalWidth = 1280.0f;  // define here the original resolution
	private float originalHeight = 800.0f;
	public int menuState=0;
	public float scrollSpeed = 5;
	private Rect screenRect;
	public int timerState = 0;
	public float incomePerCycle = 0;
	public bool lossNotification = false;
	public Vector2 lastTapLocation;
	public float sumOfLoss = 0; 
	public float dragSpeed = 0.01f;
	public float perspectiveZoomSpeed = 0.5f;  
	private Vector3 dragOrigin;
	private int howToState = 0;
	static bool sfxOn = true;
	static bool bgOn = true;
	bool gamePaused = false;
	// Use this for initialization
	void Start () {
		_playerGUI = Resources.Load<GUISkin>("PlayerGUI");
		lastTapLocation = new Vector2 (0, 0);
		screenRect = new Rect(0,0, Screen.width, Screen.height);
		soundList = gameObject.GetComponents<AudioSource>() ;
		if(!bgOn)
			soundList[0].Stop();
	}

	public void playRandomPop()
	{
		if(sfxOn)
			audio.PlayOneShot(soundList[Random.Range (1, 5)].clip);
	}

	public void playRandomDing()
	{
		if(sfxOn)
			audio.PlayOneShot(soundList[Random.Range (6, 10)].clip);
	}

	void switchBGM()
	{
		if (!bgOn) 
		{
			bgOn = true;
			soundList[0].Play();
		} else 
		{
			bgOn = false;
			soundList[0].Stop();
		}

	}
	
	// Update is called once per frame
	void OnGUI() {
		if (menuState == 0) {
				GUI.skin = _playerGUI;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
				GUI.Label (new Rect (10, 20, 590, 75), "Bank:   " + cash + " / " + goal, "GuiText"); 
				GUI.Label (new Rect (17, 70, 430, 65), " ", "GuiText");
				GUI.Label (new Rect (35, 80, 575, 75), "  " + incomePerCycle + " per cycle", "GuiText3"); 
				GUI.Label (new Rect (155, 17, 400, 100), "£", "GuiTextNum"); 
				if (lossNotification) {
					
						GUI.Label (new Rect (lastTapLocation.x, lastTapLocation.y - 17, 400, 100), "£", "GuiTextNum3"); 
						GUI.Label (new Rect (lastTapLocation.x + 15, lastTapLocation.y, 75, 65), "-  " + sumOfLoss, "sumOfLoss"); 
				}
				if (GUI.Button (new Rect (1150, 25, 80, 80), " ", "MenuButton")) { //(1150, 25, 80, 80)
						Time.timeScale = 0;
						playRandomPop();
						menuState = 5;
				}
	
				if (sfxOn && GUI.Button (new Rect (30, 700, 80, 80), " ", "sfxButton") ) {
					sfxOn = false;
				}
				if (!sfxOn && GUI.Button (new Rect (30, 700, 80, 80), " ", "sfxButton2")) {
					sfxOn = true;
					playRandomPop();
				}
				if (bgOn && GUI.Button (new Rect (125, 700, 80, 80), " ", "bgButton")) {
					switchBGM();
				}
				if (!bgOn && GUI.Button (new Rect (125, 700, 80, 80), " ", "bgButton2")) {
					switchBGM();
				}

				if (GUI.Button (new Rect (1125, 690, 110, 100), "?", "GuiText2")) {
						Time.timeScale = 0;
						menuState = 4;
				}
				if (timerState == 0)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer0");
				if (timerState == 1)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer1");
				if (timerState == 2)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer2");
				if (timerState == 3)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer3");
				if (timerState == 4)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer4");
				if (timerState == 5)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer5");
				if (timerState == 6)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer6");
				if (timerState == 7)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer7");
				if (timerState == 8)
						GUI.Box (new Rect (350, 65, 65, 65), " ", "timer8");
				GUI.Label (new Rect (20, 65, 400, 100), "£", "GuiTextNum2"); 
			
				//GUI.Label (new Rect (10, 20, 200, 250), "Bank: £" + cash + "\nGoal: £" + goal,"GuiText");
				//GUI.Label (new Rect (650, 20, 150, 600), "Click - \nDemolish (To rebuild)\n \nDouble Click - \nUpgrade (Earn more cash per building) \n \nR - Reset Level");
		} else if (menuState == 3) {
				GUI.skin = _playerGUI;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
				GUI.Label (new Rect (0, 0, 1280, 800), " ", "credits");
				if (GUI.Button (new Rect (430, 665, 185, 60), "Replay", "GuiButton2"))
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel(1);
				}

				if (GUI.Button (new Rect (630, 665, 235, 60), "Main Menu", "GuiButton2"))
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel(0);
				}

		} else if (menuState == 1) {
				GUI.skin = _playerGUI;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
	
				GUI.Box (new Rect (0, 65, 500, 500), "urban ascent", "MainTextShadow");
				GUI.Box (new Rect (0, 50, 500, 500), "urban ascent", "MainText");
				GUI.Box (new Rect (865, 160, 160, 40), "Play", "StartButtonShadow");
				GUI.Box (new Rect (865, 255, 160, 40), "Help", "StartButtonShadow");
				GUI.Box (new Rect (865, 360, 160, 40), "Quit", "StartButtonShadow");
				if (GUI.Button (new Rect (865, 155, 160, 40), "Play", "StartButton1")) {
						playRandomPop();
						Application.LoadLevel (1);
				}
				if (GUI.Button (new Rect (865, 250, 160, 40), "Help", "StartButton1")) {
						playRandomPop();
						menuState = 2;
				}
				if (GUI.Button (new Rect (865, 355, 160, 40), "Quit", "StartButton1")) {
						playRandomPop();
						Application.Quit ();
				}

			
	
				if (sfxOn && GUI.Button (new Rect (80, 25, 80, 80), " ", "sfxButton") ) {
					sfxOn = false;
				}
				if (!sfxOn && GUI.Button (new Rect (80, 25, 80, 80), " ", "sfxButton2")) {
					sfxOn = true;
					playRandomPop();
				}
				if (bgOn && GUI.Button (new Rect (185, 25, 80, 80), " ", "bgButton")) {
					switchBGM();
				}
				if (!bgOn && GUI.Button (new Rect (185, 25, 80, 80), " ", "bgButton2")) {
					switchBGM();
				}
		} else if (menuState == 2) {
				GUI.skin = _playerGUI;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
				if (howToState == 0) {
						GUI.Box (new Rect (0, 0, 1280, 800), " ", "HowTo1");
						if (GUI.Button (new Rect (820, 65, 155, 60), "close", "GuiBox")) 
							menuState = 1;
						if (GUI.Button (new Rect (830, 690, 135, 60), "next", "GuiBox"))
								howToState = 1;
				} else {
				
						GUI.Box (new Rect (0, 0, 1280, 800), " ", "HowTo2");
						if (GUI.Button (new Rect (830, 690, 135, 60), "back", "GuiBox"))
								howToState = 0;
						if (GUI.Button (new Rect (820, 65, 155, 60), "close", "GuiBox")) {
								howToState = 0;
								menuState = 1;
						}
				}	

		} else if (menuState == 4) {
				GUI.skin = _playerGUI;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));
				if (howToState == 0) {
						GUI.Box (new Rect (0, 0, 1280, 800), " ", "HowTo1");
						if (GUI.Button (new Rect (820, 65, 155, 60), "close", "GuiBox")) {
								menuState = 1;
								Time.timeScale = 1.0f;
								menuState = 0;
						}
						if (GUI.Button (new Rect (830, 690, 135, 60), "next", "GuiBox"))
								howToState = 1;
				} else {
				
						GUI.Box (new Rect (0, 0, 1280, 800), " ", "HowTo2");
						if (GUI.Button (new Rect (830, 690, 135, 60), "back", "GuiBox"))
								howToState = 0;
						if (GUI.Button (new Rect (820, 65, 155, 60), "close", "GuiBox")) {
								menuState = 1;
								Time.timeScale = 1.0f;
								menuState = 0;
						}
				}	
			
		} else if (menuState == 5) 
		{
			GUI.skin = _playerGUI;
			GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / originalWidth, Screen.height / originalHeight, 1f));

			GUI.Label (new Rect (415, 250, 430, 85), "Quit to menu?", "GuiText");
			if (GUI.Button (new Rect (470, 350, 140, 75), "Yes", "GuiButton1"))
			{
				Time.timeScale = 1.0f;
				Application.LoadLevel(0);
			}
			if (GUI.Button (new Rect (670, 350, 110, 75), "No", "GuiButton1"))
			{
				playRandomPop();
				Time.timeScale = 1.0f;
				menuState = 0;
			}


		}		
		
	}
	
	
	void Update () {	
		if(menuState ==0)
		{

			if (Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				
				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
				

					// Otherwise change the field of view based on the change in distance between the touches.
					camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
					
					// Clamp the field of view to make sure it's between 0 and 180.
					camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 15f, 80f);

			}
			
			
			
			if (Input.GetKeyDown (KeyCode.R))
				Application.LoadLevel (Application.loadedLevel);
			if (cash >= goal) {
				Application.LoadLevel ("Win");
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0 && transform.position.y < 12) { // back
				camera.fieldOfView += scrollSpeed * 15 * Time.deltaTime ;
				
				// Clamp the field of view to make sure it's between 0 and 180.
				camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 15f, 80f);
				//Debug.Log ("test");
			}
			if (Input.GetAxis ("Mouse ScrollWheel") > 0 && transform.position.y > 3) { // forward
				camera.fieldOfView -= scrollSpeed * 15 * Time.deltaTime ;
				
				// Clamp the field of view to make sure it's between 0 and 180.
				camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 15f, 80f);
				//Debug.Log ("test");
			}

								if (transform.position.x >= -7 && transform.position.x <= 10 && transform.position.z >= -17.5 && transform.position.z <= 7 ) {
									// Dampen towards the target rotation

									if (Input.GetMouseButtonDown(0))
										
									{
										
										dragOrigin = Input.mousePosition;
										
										return;
										
									}
									
									
									
									if (!Input.GetMouseButton(0)) return;
									
									
									
									Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
									
									Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

									transform.Translate(move, Space.World);  
								}
									if(transform.position.x < -7)
										transform.position = new Vector3(-7, transform.position.y,transform.position.z);
									if(transform.position.x > 10)
										transform.position = new Vector3(10f, transform.position.y,transform.position.z);
									if(transform.position.z < -17.5)
										transform.position = new Vector3(transform.position.x, transform.position.y,-17.5f);
									if(transform.position.z > 7)
										transform.position = new Vector3(transform.position.x, transform.position.y,7f);
								
				
									if (transform.position.y < 12) { // back
										transform.position += new Vector3 (0.0f, scrollSpeed * 5 * Time.deltaTime, 0.0f);
										//Debug.Log ("test");
								}
								if ( transform.position.y > 3) { // forward
										transform.position -= new Vector3 (0.0f, scrollSpeed * 5 * Time.deltaTime, 0.0f);
										//Debug.Log ("test");
								}

		}
	}
}
