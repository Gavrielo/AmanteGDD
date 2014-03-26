using UnityEngine;
using System.Collections;

public class BuildingBehaviour : MonoBehaviour {
	
	Mesh meshToUse;
	Mesh origMesh ;
	bool demolished = false;
	float doubleClickStart = 0;
	int goal = 1000;
	GameObject mainController;
	PlayerController playerController;
	bool CR_running = false;
	// 0-5 >>  demolished, contruction, complete, abandoned, rubble
	public int currentState = 3; 
	public int level = 1;

	// Use this for initializa1tion
	void Start () {
		mainController = GameObject.FindWithTag("GameController");
		playerController = Camera.main.gameObject.GetComponent<PlayerController>();

		origMesh = gameObject.GetComponent<MeshFilter>().mesh;
		meshToUse = mainController.GetComponent<DataController>().demolishedMesh;

		gameObject.GetComponent<MeshFilter>().mesh = meshToUse;
		gameObject.renderer.material.color = Color.white;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		currentState = 0;
		StartCoroutine(initRegerate(Random.Range(1F,5f)));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	


	void OnDoubleClick()
	{
		if ((playerController.cash > (25 * level)+2+(level))) {
						Debug.Log ("Double Clicked!");
						level++;
						playerController.cash -= 25 * level;
				}
	}

	void OnMouseDown()
	{
		if ((Time.time - doubleClickStart) < 0.3f)
		{
			this.OnDoubleClick();
			doubleClickStart = -1;
		}
		else
		{
			doubleClickStart = Time.time;
		}
		if(currentState > 0 && (playerController.cash > 2+(level)))
		{
			gameObject.GetComponent<MeshFilter>().mesh = meshToUse;
			gameObject.renderer.material.color = Color.white;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			currentState = 0;
			playerController.cash -= 2+(level);	
			if(CR_running)
			{
				StopCoroutine("initRegerate");
				CR_running = false;
				StartCoroutine(initRegerate(Random.Range(2.5F,10F)));
			}
			else
			    StartCoroutine(initRegerate(Random.Range(1F,5F)));
		}
	}

	IEnumerator initRegerate (float waitTime)
	{
				if (currentState < 3) {
						CR_running = true;
						yield return new WaitForSeconds (waitTime);

						currentState ++;
						switch (currentState) {
						case 1:
		
								gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().constructionMesh;
								gameObject.renderer.material.color = Color.white;
								gameObject.transform.localScale = new Vector3(1f, 1f+(level/2), 1f);
								break;

						case 2:
	
								gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().completeMesh;
								gameObject.transform.localScale = new Vector3(1f, 1f+(level/2), 1f);
								gameObject.renderer.material.color = Color.white;
								break;

						case 3:
								gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().completeMesh;
								gameObject.renderer.material.color = Color.green;
								gameObject.transform.localScale = new Vector3(1f, 1f+(level/2), 1f);
								break;
						}

						Debug.Log ("State" + currentState);
			
						CR_running = false;
						if(currentState == 3)
							StartCoroutine(initRegerate(Random.Range(15F,50F)));
						else
							StartCoroutine(initRegerate (Random.Range(1F,3f)));

				} else if (currentState == 3 ) {

					CR_running = true;
					yield return new WaitForSeconds (waitTime);
			
					currentState = Random.Range (0, 100);
			
					Debug.Log ("State" + currentState);
					if(currentState <= 20)
					{		
						gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().completeMesh;
						currentState = 3;
						gameObject.transform.localScale = new Vector3(1f, 1f+(level/2), 1f);
						gameObject.renderer.material.color = Color.green;
					}
					else if(currentState > 20 && currentState <= 60)
					{
						gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().abandonedMesh;
						currentState = 4;
						gameObject.transform.localScale = new Vector3(0.5f, 0.5f+(level/2), 0.5f);
						gameObject.renderer.material.color = Color.red;
					}
					else if(currentState > 60)
					{
						gameObject.GetComponent<MeshFilter> ().mesh = mainController.GetComponent<DataController> ().rubbleMesh;
						currentState = 5;
						gameObject.transform.localScale = new Vector3(0.5f, 0.5f+(level/2), 0.5f);
						gameObject.renderer.material.color = Color.black;
					}	
					
					
					Debug.Log ("State" + currentState);
					
					CR_running = false;
					if(currentState == 3)
						StartCoroutine(initRegerate(Random.Range(20f,75F)));
				}


	}

}
