using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAi : MonoBehaviour
{
	// public GameObject Model;
	//public int CurrentPosition, NextPosition, CurrentRound;
	private float DistanceToMove;
	public bool IsHunter, IsHunted;
	public object RipMarkPrefab;
	public GameObject WorldGenScript;
	public int IsPaused;
	protected float time = 0;


	public int ObjTag = 0;
	//public GameObject[] NeighborToMove = new GameObject[8];
	public List<GameObject> NeighborToMove = new List<GameObject>();


	// Start is called before the first frame update

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
		Gizmos.DrawWireSphere(this.gameObject.transform.position, 2.5f);
	}
	public float DistancetoMoveFunc(Vector3 PosA, Vector3 PosB)
	{
		return DistanceToMove = Vector3.Distance(PosA, PosB);
	}
	public void Idle()
	{
		Vector3 PreyPosition = new Vector3(0, 0, 0), HunterPosition = new Vector3(0, 0, 0);

		if (this.gameObject.tag == "SmallPrey")
		{
			PreyPosition = this.GetComponent<HuntedAi>().gameObject.transform.position;
		}

		if (this.gameObject.tag == "Hunter")
		{
			HunterPosition = this.GetComponent<HunterAi>().gameObject.transform.position;
		}

		

		NeighborToMove.Clear();

		if (this.gameObject.tag == "Hunter")
		{
			Collider[] hitColliders = Physics.OverlapSphere(this.GetComponent<HunterAi>().transform.position, 2.5f);
			foreach (Collider hitCollider in hitColliders)
			{
				//Debug.Log("Hunter hit something!");
				if (hitCollider.gameObject.tag != "WaterBlock" & hitCollider.gameObject.tag != "Hunter" & DistancetoMoveFunc(hitCollider.gameObject.transform.position, HunterPosition) > 2)
				{
					NeighborToMove.Add(hitCollider.gameObject);
				}
			}
		}

		if (this.gameObject.tag == "SmallPrey")
		{
			Collider[] hitColliders = Physics.OverlapSphere(this.GetComponent<HuntedAi>().transform.position, 2.5f);
			foreach (Collider hitCollider in hitColliders)
			{
				//Debug.Log("Prey hit something!");
				if (hitCollider.gameObject.tag != "WaterBlock" & hitCollider.gameObject.tag != "SmallPrey" & hitCollider.gameObject.tag != "Hunter" & DistancetoMoveFunc(hitCollider.gameObject.transform.position, PreyPosition) > 2)
				{
					NeighborToMove.Add(hitCollider.gameObject);
				}
			}
		}
	}

	public void RandomPositionToGo(GameObject Entity)
	{		
		if(this.NeighborToMove.Count<=0)
		{
			Vector3 HunterPosition = this.GetComponent<HunterAi>().gameObject.transform.position;
			if (this.gameObject.tag == "Hunter")
			{
				Collider[] hitColliders = Physics.OverlapSphere(this.GetComponent<HunterAi>().transform.position, 2.5f);
				foreach (Collider hitCollider in hitColliders)
				{
					//Debug.Log("Hunter hit something!");
					if (hitCollider.gameObject.tag != "WaterBlock" & hitCollider.gameObject.tag != "Hunter" & DistancetoMoveFunc(hitCollider.gameObject.transform.position, HunterPosition) > 2)
					{
						NeighborToMove.Add(hitCollider.gameObject);
					}
				}
			}
		}
		this.gameObject.transform.position = this.NeighborToMove[Random.Range(0, NeighborToMove.Count)].transform.position + new Vector3(0, 1, 0);
		this.Idle();	
	}

	
	public virtual void SeekPrey()
	{
		Debug.Log("Test!");
	}

	public void RunFromHunter()
	{

	}

	void Start()
	{
		RipMarkPrefab = Resources.Load("RipMark") as GameObject;
		WorldGenScript = GameObject.FindGameObjectWithTag("MainScript");
		IsPaused = WorldGenScript.GetComponent<WorldGenBase>().Paused;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
