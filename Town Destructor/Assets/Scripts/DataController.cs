using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	//Variable to for total cash to pay at the end of cycle
	public float sumToPay = 0;

	//private BuildingBehaviour buildingBehavior;

	void Start () {
		//For every child in this object
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{//If tagged "Data"
				child.gameObject.AddComponent("BuildingBehaviour");
				//Adds the building behaviour script to the child game object.

				child.gameObject.AddComponent(typeof(MeshCollider));
				//Adds a Mesh collider to the child game object so that it is clickable.
			}
		}

		//Starts the coroutine to pay the player in 15 seconds from the start of the game
		StartCoroutine(payPlayer(15f));
	}

	IEnumerator payPlayer (float waitTime)
	{
		//Applies the wait time set
		yield return new WaitForSeconds (waitTime);

		//For each child in this game object
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{
				//If the child game object is at its "optimum" or green state.
				if(child.GetComponent<BuildingBehaviour>().currentState == 3)
				{
					//Adds this amount to pay to the sum to pay according to the building's level
					sumToPay+=(child.GetComponent<BuildingBehaviour>().level);

					
					foreach (Transform areaOfInfluence in transform)
					{
						if(areaOfInfluence.tag == "AoI")
						{
							
							areaOfInfluence.collider.enabled = true;
							if(areaOfInfluence.collider.bounds.Contains(child.position))
							{
								Debug.Log("Is in collider");
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
		Camera.main.gameObject.GetComponent<PlayerController> ().cash += sumToPay;
		Debug.Log ("Paid Player £" + sumToPay + ", Total Cash ="+ Camera.main.gameObject.GetComponent<PlayerController>().cash);
	
		//Resets the sum to pay so that it does not stack for the next cycle
		sumToPay = 0;

		//Calls the coroutine to pay the player for the next cycle
		StartCoroutine(payPlayer(15f));
	}

}
