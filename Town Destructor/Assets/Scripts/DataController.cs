using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	//Variable to for total cash to pay at the end of cycle
	public float sumToPay = 0;
	public UnityEngine.Texture ConstrT;
	public UnityEngine.Texture ConstrT1;
	public UnityEngine.Texture ConstrT2;
	public UnityEngine.Texture RubbleT;
	public UnityEngine.Texture AbandonedT;
	public UnityEngine.Texture MainT;
	public UnityEngine.Texture MainT1;
	public UnityEngine.Texture MainT2;
	public UnityEngine.Texture RoofTexture;
	public UnityEngine.Texture RoofTexture1;
	public UnityEngine.Texture RoofTexture2;
	public UnityEngine.Texture RoofTextureAbandoned;
	public Material	roofMat;
	public PlayerController playerController;
	//private BuildingBehaviour buildingBehavior;

	void Start () {
		//For every child in this object
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{//If tagged "Data"
				child.gameObject.AddComponent("BuildingBehaviour");
				//Adds the building behaviour script to the child game object.

				child.gameObject.AddComponent(typeof(BoxCollider));

				//Adds a Mesh collider to the child game object so that it is clickable.
			}
		}
		playerController = Camera.main.gameObject.GetComponent<PlayerController>();
		//Starts the coroutine to pay the player in 15 seconds from the start of the game
		
		calcSumToPay ();
		playerController.incomePerCycle = sumToPay;
		sumToPay = 0;

		playerController.timerState = 0;
		StartCoroutine(setTimerState(1f,0));
	}

	void calcSumToPay ()
	{
		
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{
				//If the child game object is at its "optimum" or green state.
				if(child.GetComponent<BuildingBehaviour>().currentState == 3)
				{
					//Adds this amount to pay to the sum to pay according to the building's level
					sumToPay+=(child.GetComponent<BuildingBehaviour>().level);
					if((child.GetComponent<BuildingBehaviour>().level) > 1)
						sumToPay+=12.5f*(child.GetComponent<BuildingBehaviour>().level);
					
					
					foreach (Transform areaOfInfluence in transform)
					{
						if(areaOfInfluence.tag == "AoI")
						{
							
							areaOfInfluence.collider.enabled = true;
							
							if(areaOfInfluence.collider.bounds.Contains(child.position))
							{
								Debug.Log("Is in collider");
								if(child.GetComponent<BuildingBehaviour>().infLevel < areaOfInfluence.GetComponent<AreaOfInfluence>().level)
									child.GetComponent<BuildingBehaviour>().infLevel = areaOfInfluence.GetComponent<AreaOfInfluence>().level;
								sumToPay+=(areaOfInfluence.GetComponent<AreaOfInfluence>().level)/2;
							}
							areaOfInfluence.collider.enabled = false;
						}
					}
				}
			}
		}
		
		//Gets the PlayerController script applied to the main camera
		//Adds the sum to pay to the player's cash
	}
	
	IEnumerator setTimerState (float waitTime, int timerState)
	{
		yield return new WaitForSeconds (waitTime);
		playerController.timerState = timerState;
		if(timerState == 0)	StartCoroutine(setTimerState(1f,1));
		else if(timerState == 1)	StartCoroutine(setTimerState(1f,2));
		else if(timerState == 2)	StartCoroutine(setTimerState(1f,3));
		else if(timerState == 3)	StartCoroutine(setTimerState(1f,4));
		else if(timerState == 4)	StartCoroutine(setTimerState(1f,5));
		else if(timerState == 5)	StartCoroutine(setTimerState(1f,6));
		else if(timerState == 6)	StartCoroutine(setTimerState(1f,7));
		else if(timerState == 7)	StartCoroutine(payPlayer(1f));
	}

	public void checkArea()
	{
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{
				//If the child game object is at its "optimum" or green state.
	
					//Adds this amount to pay to the sum to pay according to the building's level
					
					
					foreach (Transform areaOfInfluence in transform)
					{
						if(areaOfInfluence.tag == "AoI")
						{
							
							areaOfInfluence.collider.enabled = true;
							
							if(areaOfInfluence.collider.bounds.Contains(child.position))
							{
								//Debug.Log("Checked collider");
								if(child.GetComponent<BuildingBehaviour>().infLevel < areaOfInfluence.GetComponent<AreaOfInfluence>().level)
									child.GetComponent<BuildingBehaviour>().infLevel = areaOfInfluence.GetComponent<AreaOfInfluence>().level;
							}else{
								if(child.GetComponent<BuildingBehaviour>().infLevel > 0)
									child.GetComponent<BuildingBehaviour>().infLevel = 1;
							}
							areaOfInfluence.collider.enabled = false;
						}
					}

			}
		}

	}

	IEnumerator payPlayer (float waitTime)
	{
		//Applies the wait time set
		yield return new WaitForSeconds (waitTime);
		calcSumToPay ();
		Camera.main.gameObject.GetComponent<PlayerController> ().cash += sumToPay ;
		playerController.timerState = 8;
		//For each child in this game object
		Debug.Log ("Paid Player £" + sumToPay + ", Total Cash ="+ Camera.main.gameObject.GetComponent<PlayerController>().cash);
		playerController.incomePerCycle = sumToPay;
		//Resets the sum to pay so that it does not stack for the next cycle
		sumToPay = 0;

		//Calls the coroutine to pay the player for the next cycle
		StartCoroutine(setTimerState(1f,0));
	}

}
