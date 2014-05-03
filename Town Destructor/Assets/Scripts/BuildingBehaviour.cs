using UnityEngine;
using System.Collections;

public class BuildingBehaviour : MonoBehaviour {

	float doubleClickStart = 0;
	GameObject mainController;
	PlayerController playerController;
	bool CR_running = false;
	// 0-5 >>  demolished, contruction, complete, abandoned, rubble
	public int currentState = 3; 
	public int level = 1;
	GameObject AreaOfInfluence;
	GameObject AreaInstance;
	bool areaSpawned = false;
	// Use this for initializa1tion
	void Start () {
		//Finds the Gameobject tagged GameController and sets it to the variable
		mainController = GameObject.FindWithTag("GameController");
		//GameObject instance = Instantiate(Resources.Load<GameObject>("AreaOI"));
		//AreaOfInfluence =  (Resources.Load ("AreaOI") as GameObject);
		//Finds the player controller game object and sets the script 
		//PlayerController to the variable
		playerController = Camera.main.gameObject.GetComponent<PlayerController>();

		//Sets renderer colour to white
		gameObject.renderer.material.color = Color.white;

		//Sets the local scale of the building 
		gameObject.transform.localScale = new Vector3(1f, 0f, 1f);

		//Sets the building state to 0 so that it can regenerate
		currentState = 0;

		//Starts the Coroutine to regenerate the building with 
		//a ranging range of 1-5 seconds)
		StartCoroutine(initRegerate(Random.Range(1F,5f)));

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnDoubleClick()
	{	//When a building is double clicked
		//Checks if player has enough cash to purchase upgrade
		if ((playerController.cash > (25 * level)+2+(level))) {
						Debug.Log ("Double Clicked!");
						//removes cash from the player controller script
						playerController.cash -= 25 * level;
						//levels the building up
						level++;
						if(!areaSpawned)
						{
							AreaInstance = Instantiate((Resources.Load ("AreaOI") as GameObject), transform.position, transform.rotation) as GameObject;
							areaSpawned = true;
							AreaInstance.transform.parent = transform.parent;
						}
						AreaInstance.transform.localScale = new Vector3(0.5f*level, 0f,0.5f*level);
						AreaInstance.GetComponent<AreaOfInfluence>().level++;
				//AreaClone.transform.parent = gameObject.transform;
				}
	}

	void OnMouseDown()
	{	//When a building is clicked on once

		//checks if it is a double click or not
		if ((Time.time - doubleClickStart) < 0.6f) {
						//Calls the double click function
						this.OnDoubleClick ();
						doubleClickStart = -1;
				} else {
						//Resets the time for double click to check
						doubleClickStart = Time.time;
						if(currentState > 0 && (playerController.cash > 2+(level)))
						{
							//Resets the render colour back to white
							gameObject.renderer.material.color = Color.white;
							//Resets its scale
							gameObject.transform.localScale = new Vector3(1f, 0f, 1f);
							//Resets the building state back to 0 - demolished state
							currentState = 0;
							//removes cash from player script
							playerController.cash -= (level);	
							
							//checks if regeneration coroutine is running
							if(CR_running)
							{
								//Stops the regeneration coroutine
								StopCoroutine("initRegerate");
								//sets the boolean to false
								CR_running = false;
								//Starts the regeneration coroutine between 2.5s to 10s
								StartCoroutine(initRegerate(Random.Range(2.5F,10F)));
							}
							else // starts the coroutine if it isn't running
								StartCoroutine(initRegerate(Random.Range(1F,5F)));
						}
				}
		//Checks if the current state is greater than 0 and if the player
		//has enough cash to demolish the building

	}

	IEnumerator initRegerate (float waitTime)
	{ //Coroutine to regenerate demolished buildings
		//checks if building state is not complete 
		if (currentState < 3) {
			//sets coroutine boolean on to check later if coroutine is running
			CR_running = true;

			//waits for the seconds set when the coroutine is called
			yield return new WaitForSeconds (waitTime);

			//Upgrades the building state to the next 
			currentState ++;

			//switch to decide what appearance the building should look like when
			//regenerating construction appearance
			switch (currentState) {
			case 1: //Construction state 1, white render, 1/2 size
					gameObject.renderer.material.color = Color.yellow;
					gameObject.transform.localScale = new Vector3(1f, 0.25f*level, 1f);
					break;

			case 2://Construction state 2, white render, 1.25 times size
					gameObject.transform.localScale = new Vector3(1f, 0.5f+level, 1f);
					gameObject.renderer.material.color = Color.yellow;
					break;

			case 3://Building complete from construction, with height according to level
					gameObject.renderer.material.color = Color.green;
					gameObject.transform.localScale = new Vector3(1f, level,1f);
					break;
			}

			//Debug.Log ("State" + currentState);

			//sets coroutine boolean check off
			CR_running = false;
			if(currentState == 3)//initiates coroutine to decide what to do next
				StartCoroutine(initRegerate(Random.Range(15F,50F)));//15-50seconds
			else
				StartCoroutine(initRegerate (Random.Range(1F,3f)));//1-3seconds

		} else if (currentState == 3 ) {
			//sets coroutine boolean true
			CR_running = true;

			//waits the set wait time
			yield return new WaitForSeconds (waitTime);

			//Sets the current state to a random range between 0-100;
			currentState = Random.Range (0, 100);
	
			//Debug.Log ("State" + currentState);
			if(currentState <= 20)
			{	//if current state is less than 20 
				//sets the building state to healthy, green render
				currentState = 3;
				gameObject.transform.localScale = new Vector3(1f, level, 1f);
				gameObject.renderer.material.color = Color.green;
			}
			else if(currentState > 20 && currentState <= 60)
			{	//if current state is between 20-60
				//sets the current state to abandoned, red render
				currentState = 4;
				gameObject.transform.localScale = new Vector3(1f, 0.5f+(level/2), 1f);
				gameObject.renderer.material.color = Color.red;
			}
			else if(currentState > 60)
			{  //if current state is over 60
				//sets current state to rubble, black render
				currentState = 5;
				gameObject.transform.localScale = new Vector3(1f, 0.5f+(level/2), 1f);
				gameObject.renderer.material.color = Color.black;
			}	
			
			
			//Debug.Log ("State" + currentState);
			
			CR_running = false;
			if(currentState == 3)//calls another regeneration coroutine if building is healthy
				StartCoroutine(initRegerate(Random.Range(20f,75F)));
		}
	} 

}
