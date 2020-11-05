using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HunterAi : BaseAi
{
	public float DistanceToPrey = 0;
	public int ViewRange;
	//public int Target;
	public GameObject TargetObj;
	public List<GameObject> PreysNear = new List<GameObject>();
	public bool IsFollowingPrey;

	private int PreysKilled = 0, SetOtherPreysKilledScript = 0;
	// Start is called before the first frame update

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		//Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
		Gizmos.DrawWireSphere(this.gameObject.transform.position, 10f);
	}
	void Start()
	{
		WorldGenScript = GameObject.FindGameObjectWithTag("MainScript");
		

		RipMarkPrefab = null;
		PreysNear.Clear();
		Idle();
	}


	override public void SeekPrey()
	{
		Vector3 HunterPosition = this.GetComponent<HunterAi>().gameObject.transform.position;

		Collider[] hitColliders = Physics.OverlapSphere(this.GetComponent<HunterAi>().transform.position, 10f);

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "SmallPrey" & !PreysNear.Contains(hitCollider.gameObject))
			{
				Debug.Log("Theres a prey here!");
				PreysNear.Add(hitCollider.gameObject);

				IsFollowingPrey = true;
				if (PreysNear.Count == 1)
				{
					TargetObj = hitCollider.gameObject;
				}
				else if (PreysNear.Count >= 2)
				{
					TargetObj = PreysNear[Random.Range(0, PreysNear.Count)];
				}
			}
			if (TargetObj != null)
			{
				DistanceToPrey = DistancetoMoveFunc(this.gameObject.transform.position, TargetObj.transform.position);
			}


			if (IsFollowingPrey)
			{
				if (TargetObj == null)
				{
					IsFollowingPrey = false;
				}

			}
			else { PreysNear.Clear(); TargetObj = null; }


			if (PreysNear.Count > 0)
			{
				if (hitCollider.gameObject.tag == "SmallPrey")
				{
					int ObjIndex;
					ObjIndex = PreysNear.IndexOf(hitCollider.gameObject);

					if (DistancetoMoveFunc(this.gameObject.transform.position, PreysNear[ObjIndex].gameObject.transform.position) > 10)
					{
						PreysNear.RemoveAt(ObjIndex);
					}
				}
			}

			if (PreysNear.Count <= 0)
			{
				IsFollowingPrey = false;
			}

		}

		Collider[] killRange = Physics.OverlapSphere(this.GetComponent<HunterAi>().transform.position, 2.5f);
		foreach (Collider target in killRange)
		{
			if (target.gameObject.tag == "SmallPrey")
			{
				int ObjIndex;
				Debug.Log("Killed!");

				WorldGenScript.GetComponent<WorldGenBase>().NumberOfPreysKilled = WorldGenScript.GetComponent<WorldGenBase>().NumberOfPreysKilled + 1;
				if (PreysNear != null)
				{
					ObjIndex = PreysNear.IndexOf(target.gameObject);
					PreysNear.RemoveAt(ObjIndex);
				}
				Instantiate(Resources.Load("Prefabs/RipMark/RipMark", typeof(GameObject)) as GameObject, new Vector3(target.transform.position.x, 0, target.transform.position.z), Quaternion.identity);
				Destroy(target.gameObject);

			}
		}
		Collider[] hitColliderA = Physics.OverlapSphere(this.GetComponent<HunterAi>().transform.position, 2.5f);
		if (IsFollowingPrey == true)
		{
			this.NeighborToMove.Clear();
			foreach (Collider hitCollider in hitColliderA)
			{
				if (hitCollider.gameObject.tag != "WaterBlock" & hitCollider.gameObject.tag != "Hunter" & DistancetoMoveFunc(hitCollider.gameObject.transform.position, HunterPosition) > 2)
				{
					this.NeighborToMove.Add(hitCollider.gameObject);
				}
				if (this.NeighborToMove.Count > 0)
				{
					this.gameObject.transform.position = this.NeighborToMove[Random.Range(0, this.NeighborToMove.Count)].transform.position + new Vector3(0, 1, 0);
				}


			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		IsPaused = WorldGenScript.GetComponent<WorldGenBase>().Paused;
		if (IsPaused == 0)
		{
			time += Time.deltaTime;
			if (time > 0.1)
			{
				if (IsFollowingPrey == false)
				{
					RandomPositionToGo(this.gameObject);
				}

				SeekPrey();


				time = 0;
			}
		}
		/*if (Input.GetKeyDown(KeyCode.Space))
		{

			if (IsFollowingPrey == false)
			{
				RandomPositionToGo(this.gameObject);
			}

			SeekPrey();
		}*/


	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "SmallPrey")
		{
			Debug.Log("I killed a prey!");
			WorldGenScript.GetComponent<WorldGenBase>().NumberOfPreysKilled = WorldGenScript.GetComponent<WorldGenBase>().NumberOfPreysKilled + 1;
			Instantiate(Resources.Load("Prefabs/RipMark/RipMark", typeof(GameObject)) as GameObject, new Vector3(collision.transform.position.x, 0, collision.transform.position.z), Quaternion.identity);
			Destroy(collision.gameObject);
			//WorldGenScript.GetComponent<WorldGenBase>().SetNumberOfPreysKilled(1);

		}
	}




}
