using UnityEngine;
using System.Collections;

public class BuildingBehaviour : MonoBehaviour {

	float doubleClickStart = 0;
	GameObject mainController;
	PlayerController playerController;
	DataController dataController;
	bool CR_running = false;
	// 0-5 >>  demolished, contruction, complete, abandoned, rubble
	public int currentState = 3; 
	public int level = 1;
	public int infLevel = 0;
	GameObject AreaOfInfluence;
	GameObject AreaInstance;
	bool upgradeable = false;
	bool areaSpawned = false;
	private UnityEngine.Texture ConstrT;
	private UnityEngine.Texture ConstrT1;
	private UnityEngine.Texture ConstrT2;
	private UnityEngine.Texture RubbleT;
	private UnityEngine.Texture MainT;
	private UnityEngine.Texture MainT1;
	private UnityEngine.Texture MainT2;
	private UnityEngine.Texture AbandonedT;
	private int randomNum;
	private GameObject buildingRoof;
	private Vector3 lastTap;

	// Use this for initializa1tion
	void Start () {
		//Finds the Gameobject tagged GameController and sets it to the variable
		mainController = GameObject.FindWithTag("GameController");
		//GameObject instance = Instantiate(Resources.Load<GameObject>("AreaOI"));
		//AreaOfInfluence =  (Resources.Load ("AreaOI") as GameObject);
		//Finds the player controller game object and sets the script 
		//PlayerController to the variable
		playerController = Camera.main.gameObject.GetComponent<PlayerController>();
		dataController = GameObject.FindWithTag("GameController").GetComponent<DataController>();
		//Sets renderer colour to white
		//gameObject.renderer.material.color = Color.white;

		//Sets the local scale of the building 
		gameObject.transform.localScale = new Vector3(2f, 0.1f, 2f);

		//Sets the building state to 0 so that it can regenerate
		currentState = 0;

		//Starts the Coroutine to regenerate the building with 
		//a ranging range of 1-5 seconds)
		StartCoroutine(initRegenerate(Random.Range(2f,5f)));
		
		ConstrT = dataController.ConstrT;
		ConstrT1 = dataController.ConstrT1;
		ConstrT2 = dataController.ConstrT2;
		RubbleT = dataController.RubbleT;
		MainT = dataController.MainT;
		MainT1 = dataController.MainT1;
		MainT2 = dataController.MainT2;
		AbandonedT = dataController.AbandonedT;
		gameObject.GetComponent<BoxCollider>().size = new Vector3(0.3f, 0.2f, 0.3f);
		randomNum = Random.Range(1, 4); //-0.5975929
		if (gameObject.name != "roof") 
		{
			buildingRoof = Instantiate (gameObject, transform.position, transform.rotation) as GameObject;
			buildingRoof.gameObject.name = "roof";
			buildingRoof.transform.parent = gameObject.transform;
			buildingRoof.gameObject.transform.localPosition = new Vector3(0f, 0.12f, 0f);
			buildingRoof.transform.localScale = new Vector3(1f, 0.026f, 1f);
			buildingRoof.GetComponent<BuildingBehaviour>().enabled = false;
			buildingRoof.GetComponent<BoxCollider>().enabled = false;
			buildingRoof.renderer.material = dataController.roofMat;
			//buildingRoof.transform.localScale = new Vector3 (buildingRoof.transform.localScale.x, 0.6f, buildingRoof.transform.localScale.z);
		}
	}

	void calcLoss()
	{
		if(level > 1)
			playerController.incomePerCycle -= 12.5f*level;
		else
			playerController.incomePerCycle -=level;

		if(infLevel > 1)
			playerController.incomePerCycle -=infLevel/2;
		if (playerController.incomePerCycle < 0) 
			playerController.incomePerCycle = 0;
	}
	
	void calcGain()
	{
		if(level > 1)
			playerController.incomePerCycle += 12.5f*level;
		else
			playerController.incomePerCycle +=level;
		
		if(infLevel > 1)
			playerController.incomePerCycle +=infLevel/2;
	}
	// Update is called once per frame
	void Update () {

	}
	IEnumerator waitForTimeSet (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		
		playerController.lossNotification = false;
	}

	void OnDoubleClick()
	{	//When a building is double clicked
				//Checks if player has enough cash to purchase upgrade
			if(level < 10)
			{//&& 
				if (((playerController.cash > (25 * level) + 2 + (level)) ) || ((playerController.cash > (25 * level) + 2 + (level)) && upgradeable == true)) {
						//Debug.Log ("Double Clicked!");
						//removes cash from the player controller script
						playerController.cash -= 25 * level;
						playerController.playRandomDing();
						//levels the building up
						level++;
						playerController.incomePerCycle += 12.5f*level;
						playerController.lossNotification = true;
						lastTap = Camera.main.WorldToScreenPoint(transform.position);
						playerController.lastTapLocation = new Vector2(lastTap.x,800-lastTap.y);
						playerController.sumOfLoss = 25 * level;
						StartCoroutine(waitForTimeSet(1f));
						upgradeable = true;
						if (!areaSpawned && infLevel < 1) 
						{	
							AreaInstance = Instantiate ((Resources.Load ("AreaOI") as GameObject), new Vector3(transform.position.x,transform.position.y -0.001f,transform.position.z), transform.rotation) as GameObject;
							areaSpawned = true;
							AreaInstance.transform.parent = transform.parent;
						}

						randomNum = Random.Range (1, 3);
						determineTexture (1, randomNum);
						if(areaSpawned)
						{
							AreaInstance.transform.localScale = new Vector3 (level, 0f, level);
							AreaInstance.GetComponent<AreaOfInfluence> ().level++;
						}
						infLevel = level;
						switch (level) {
						case 2:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 5.2f);
								break;
						case 3:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 8.2f);
								break;
						case 4:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 9.35f);
								break;
						case 5:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 12.5f);
								break;
						case 6:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 15.2f);
								break;
						case 7:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 18.3f);
								break;
						case 8:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 19.35f);
								break;
						case 9:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 22.22f);
								break;
						case 10:
								transform.renderer.material.mainTextureScale = new Vector2 (100, 25f);
								break;
						}
				}
		}
	}

	void OnMouseDown()
	{	//When a building is clicked on once

		//checks if it is a double click or not
		if ((Time.time - doubleClickStart) < 0.6f) {
						//Calls the double click function
			
							if(!playerController.lossNotification)
							{
								
								this.OnDoubleClick ();
							}
						doubleClickStart = -1;
				} else {
						//Resets the time for double click to check
						doubleClickStart = Time.time;
						if(currentState > 0 && (playerController.cash > 2+(level)))
						{
							//Resets the render colour back to white
							//gameObject.renderer.material.color = Color.white;
							//Resets its scale
							playerController. playRandomPop();
							if(currentState == 4 || currentState == 5)	calcGain();
							randomNum = Random.Range(1, 3);
							determineTexture(1,randomNum);
							gameObject.transform.localScale = new Vector3(2f, 0.25f, 2f);
							//Resets the building state back to 0 - demolished state
							currentState = 0;
							//removes cash from player script
							playerController.cash -= (level);	
							if(level > 1 && areaSpawned)
							{
								AreaInstance.SetActive(false);
								dataController.checkArea();
							}
							//checks if regeneration coroutine is running
							if(CR_running)
							{
								//Stops the regeneration coroutine
								StopCoroutine("initRegenerate");
								//sets the boolean to false
								CR_running = false;
								//Starts the regeneration coroutine between 2.5s to 10s
								StartCoroutine(initRegenerate(Random.Range(2.5F,5f)));
							}
							else // starts the coroutine if it isn't running
								
								StartCoroutine(initRegenerate(Random.Range(2f,5F)));

						}
				}
		//Checks if the current state is greater than 0 and if the player
		//has enough cash to demolish the building

	}

	void determineTexture(int buildingState, int textureType)
	{
		switch (buildingState) 
		{
			case 1:
				switch(textureType)
				{
					case 1:
				buildingRoof.renderer.material.mainTexture = ConstrT;
						renderer.material.mainTexture = ConstrT;
						break;
					case 2:
				buildingRoof.renderer.material.mainTexture = ConstrT1;
						renderer.material.mainTexture = ConstrT1;
						break;
					case 3:
				buildingRoof.renderer.material.mainTexture = ConstrT2;
						renderer.material.mainTexture = ConstrT2;
						break;
				}
				break;
			case 2:
				switch(textureType)
				{
						case 1:
							buildingRoof.renderer.material.mainTexture = ConstrT;
							renderer.material.mainTexture = MainT;
						break;
						case 2:
							buildingRoof.renderer.material.mainTexture = ConstrT1;
							renderer.material.mainTexture = MainT1;
						break;
						case 3:
						buildingRoof.renderer.material.mainTexture = ConstrT2;
							renderer.material.mainTexture = MainT2;
						break;
				}
				break;
			case 3:
				buildingRoof.renderer.material.mainTexture = RubbleT;
				renderer.material.mainTexture = RubbleT;
				break;
		}

	}

	IEnumerator initRegenerate (float waitTime)
	{ //Coroutine to regenerate demolished buildings
		//checks if building state is not complete 
		//Debug.Log ("Num " + randomNum);
		if (currentState < 3) {
			//sets coroutine boolean on to check later if coroutine is running
			CR_running = true;

			//waits for the seconds set when the coroutine is called
			yield return new WaitForSeconds (waitTime);

			//Upgrades the building state to the next 
			currentState ++;

			//switch to decide what appearance the building should look like when
			//regenerating construction appearance
			switch (currentState) 
			{
			case 1: //Construction state 1, white render, 1/2 size
					//gameObject.renderer.material.color = Color.yellow;
				
					randomNum = Random.Range(1, 4);
					determineTexture(1,randomNum);
					gameObject.transform.localScale = new Vector3(2f, 0.75f*level, 2f);
					break;

			case 2://Construction state 2, white render, 1.25 times size
					determineTexture(1,randomNum);
					gameObject.transform.localScale = new Vector3(2f, 1.5f*level, 2f);
					//gameObject.renderer.material.color = Color.yellow;
					break;

			case 3://Building complete from construction, with height according to level
					//gameObject.renderer.material.color = Color.green;
					determineTexture(2,randomNum);
					gameObject.transform.localScale = new Vector3(2f, 2f*level,2f);
					if(level > 1&& areaSpawned){
						AreaInstance.SetActive(true);
						infLevel = level;
						dataController.checkArea();
					}
					break;
			}

			//Debug.Log ("State" + currentState);

			//sets coroutine boolean check off
			CR_running = false;
			if(currentState == 3)//initiates coroutine to decide what to do next
				StartCoroutine(initRegenerate(Random.Range(15F+level,75F+(5*level))));//15-50seconds
			else
				StartCoroutine(initRegenerate (Random.Range(2f,3f)));//1-3seconds

		} else if (currentState == 3 ) {
			//sets coroutine boolean true
			CR_running = true;

			//waits the set wait time
			yield return new WaitForSeconds (waitTime);

			//Sets the current state to a random range between 0-100;
			currentState = Random.Range (0, 100);
	
			//Debug.Log ("State" + currentState);
			if(infLevel == 0)
			{
				if(currentState <= 20)
				{	//if current state is less than 20 
					//sets the building state to healthy, green render
					currentState = 3;
					determineTexture(2,randomNum);
					gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
					//gameObject.renderer.material.color = Color.green;
					if(level > 1&& areaSpawned){
						AreaInstance.SetActive(true);
					}
				}
				else if(currentState > 20 && currentState <= 60)
				{	//if current state is between 20-60
					//sets the current state to abandoned, red render
					currentState = 4;
					gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
					//gameObject.renderer.material.color = Color.red;
					renderer.material.mainTexture = AbandonedT;
					buildingRoof.renderer.material.mainTexture = RubbleT;
					calcLoss();
					if(level > 1&& areaSpawned){
						infLevel = 1;
						AreaInstance.SetActive(false);
						dataController.checkArea();
					}
				}
				else if(currentState > 60)
				{  //if current state is over 60
					//sets current state to rubble, black render
					currentState = 5;
					determineTexture(3,randomNum);
					gameObject.transform.localScale = new Vector3(2f, 0.2f, 2f);
					//gameObject.renderer.material.color = Color.black;
					calcLoss();
					if(level > 1&& areaSpawned){
						infLevel = 1;
						AreaInstance.SetActive(false);
						dataController.checkArea();
					}
				}	
			}else 
			{
				if(infLevel < 5)
				{
					if(currentState <= 20+(10*infLevel))
					{	//if current state is less than 20 
						//sets the building state to healthy, green render
						currentState = 3;
						determineTexture(2,randomNum);
						gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
						//gameObject.renderer.material.color = Color.green;
						if(level > 1 && areaSpawned){
							AreaInstance.SetActive(true);
						}
					}
					else if(currentState > 20+(10*infLevel) && currentState <= 87)
					{	//if current state is between 20-60
						//sets the current state to abandoned, red render
						currentState = 4;
						gameObject.transform.localScale = new Vector3(2f, 2f* level, 2f);
						//gameObject.renderer.material.color = Color.red;
						renderer.material.mainTexture = AbandonedT;
						buildingRoof.renderer.material.mainTexture = RubbleT;
						calcLoss();
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}
					else if(currentState > 87)
					{  //if current state is over 60
						//sets current state to rubble, black render
						currentState = 5;
						determineTexture(3,randomNum);
						gameObject.transform.localScale = new Vector3(2f,0.2f, 2f);
						//gameObject.renderer.material.color = Color.black;
						calcLoss();
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}
				}else if(infLevel < 8 && infLevel > 5)
				{
					if(currentState <= 20+(10*infLevel))
					{	//if current state is less than 20 
						//sets the building state to healthy, green render
						currentState = 3;
						determineTexture(2,randomNum);
						gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
						//gameObject.renderer.material.color = Color.green;
						if(level > 1&& areaSpawned){
							AreaInstance.SetActive(true);
						}
					}
					else if(currentState > 20+(10*infLevel) && currentState <= 95)
					{	//if current state is between 20-60
						//sets the current state to abandoned, red render
						currentState = 4;
						gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
						//gameObject.renderer.material.color = Color.red;
						calcLoss();
						renderer.material.mainTexture = AbandonedT;
						buildingRoof.renderer.material.mainTexture = RubbleT;
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}
					else if(currentState > 95)
					{  //if current state is over 60
						//sets current state to rubble, black render
						currentState = 5;
						determineTexture(3,randomNum);
						calcLoss();
						gameObject.transform.localScale = new Vector3(2f, 0.2f, 2f);
						//gameObject.renderer.material.color = Color.black;
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}

				}else 
				{
					if(currentState <= 90)
					{	//if current state is less than 20 
						//sets the building state to healthy, green render
						currentState = 3;
						determineTexture(2,randomNum);
						gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
						//gameObject.renderer.material.color = Color.green;
						if(level > 1&& areaSpawned){
							AreaInstance.SetActive(true);
						}
					}
					else if(currentState > 90 && currentState <= 95)
					{	//if current state is between 20-60
						//sets the current state to abandoned, red render
						currentState = 4;
						gameObject.transform.localScale = new Vector3(2f, 2f*level, 2f);
						//gameObject.renderer.material.color = Color.red;
						calcLoss();
						renderer.material.mainTexture = AbandonedT;
						buildingRoof.renderer.material.mainTexture = RubbleT;
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}
					else if(currentState > 95)
					{  //if current state is over 60
						//sets current state to rubble, black render
						currentState = 5;
						determineTexture(3,randomNum);
						calcLoss();
						gameObject.transform.localScale = new Vector3(2f, 0.2f, 2f);
						//gameObject.renderer.material.color = Color.black;
						if(level > 1&& areaSpawned)
						{
							infLevel = 1;
							AreaInstance.SetActive(false);
							dataController.checkArea();
						}
					}

				}
			}
			
			//Debug.Log ("State" + currentState);
			
			CR_running = false;
			if(currentState == 3)//calls another regeneration coroutine if building is healthy
				StartCoroutine(initRegenerate(Random.Range(20f+level,75F + (5*level))));
		}
	} 

}
