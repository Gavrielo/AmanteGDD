using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	public Mesh demolishedMesh;
	public Mesh constructionMesh;
	public Mesh completeMesh;
	public Mesh abandonedMesh;
	public Mesh rubbleMesh;

	public int sumToPay = 0;

	//private BuildingBehaviour buildingBehavior;

	void Start () {
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{
				child.gameObject.AddComponent("BuildingBehaviour");
			}
			foreach( Transform childOfChild in child.transform)
			if(childOfChild.tag == "Data")
			{
				childOfChild.gameObject.AddComponent("BuildingBehaviour");
			}
		}
		StartCoroutine(payPlayer(15f));
	}

	IEnumerator payPlayer (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		foreach (Transform child in transform){
			if(child.tag == "Data")
			{

			}
			foreach( Transform childOfChild in child.transform)
			{
				if(childOfChild.tag == "Data")
				{
					if(childOfChild.GetComponent<BuildingBehaviour>().currentState == 3)
					{
						sumToPay+=4*(childOfChild.GetComponent<BuildingBehaviour>().level);
					}
				}
			}
		}
		Camera.main.gameObject.GetComponent<PlayerController> ().cash += sumToPay;
		Debug.Log ("Paid Player £" + sumToPay + ", Total Cash ="+ Camera.main.gameObject.GetComponent<PlayerController>().cash);
		sumToPay = 0;
		StartCoroutine(payPlayer(15f));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
