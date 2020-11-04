using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEditor;

public class WorldGenBase : MonoBehaviour
{
	public GameObject HunterPrefab, PreyPrefab;
	public int MaxRow, MaxColumn;

	protected float time = 0;

	[SerializeField]
	protected int RoundNumber = 0;

	[SerializeField]
	protected GameObject[] PreyObjects = null;

	[SerializeField]
	protected GameObject[] BlockArray = null;

	[SerializeField]
	protected GameObject[,] BlockPositions = new GameObject[30,30];

	[SerializeField]
	private int NumberOfPreysGenerated = 0;
		
	public int NumberOfPreysKilled = 0;

	public Button NextRoundButton, ExitGameButton;

	public Text RoundNumberText;


	// Start is called before the first frame update
	void Start()
	{
		SetButton();

		GenerateWorld();

		SpawnPrey();

		SpawnHunter();
	}
	void RestartLevel()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	void ExitGame()
	{
		Debug.Log("Exit!");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
	}
	public void SetRoundText()
	{
		RoundNumberText = GameObject.FindGameObjectWithTag("RoundNumberText").GetComponent<Text>();
		RoundNumberText.text = RoundNumber.ToString();
	}
	public void SetButton()
	{
		ExitGameButton = GameObject.FindGameObjectWithTag("ExitGameButton").GetComponent<Button>();
		ExitGameButton.onClick.AddListener(ExitGame);
		NextRoundButton = GameObject.FindGameObjectWithTag("RestartButton").GetComponent<Button>();
		NextRoundButton.onClick.AddListener(RestartLevel);
	}
	public void SetNumberOfPreysKilled(int Number)
	{
		NumberOfPreysKilled = NumberOfPreysKilled + Number;
	}

	void SpawnHunter()
	{
		Vector3 RandomPosHunter = new Vector3(0, 0, 0);
		int RandomVarHunterX = Random.Range(0, 30);
		int RandomVarHunterY = Random.Range(0, 30);
		RandomPosHunter = BlockPositions[RandomVarHunterX, RandomVarHunterY].gameObject.transform.position;
		
			if (BlockPositions[RandomVarHunterX, RandomVarHunterY].gameObject.tag != "WaterBlock" & RandomPosHunter !=GameObject.FindGameObjectWithTag("SmallPrey").transform.position)
			{
				Instantiate(HunterPrefab, new Vector3(RandomPosHunter.x, 0, RandomPosHunter.z), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
			}
		
	}

	void SpawnPrey()
	{
		NumberOfPreysGenerated = Random.Range(5, 10);
		PreyObjects = new GameObject[NumberOfPreysGenerated];

		for (int i = 0; i < NumberOfPreysGenerated; i++)
		{
			while (PreyObjects[i] == null)
			{
				Vector3 RandomPos = new Vector3(0, 0, 0);
				int RandomVarPreyX = Random.Range(0, 30);
				int RandomVarPreyY = Random.Range(0, 30);
				RandomPos = BlockPositions[RandomVarPreyX, RandomVarPreyY].gameObject.transform.position;

				if (PreyObjects[i] == null)
				{
					if (BlockPositions[RandomVarPreyX, RandomVarPreyY].gameObject.tag != "WaterBlock")
					{
						PreyObjects[i] = Instantiate(PreyPrefab, new Vector3(RandomPos.x, 0, RandomPos.z), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0)) as GameObject;
					}
				}
			};
		}
	}

	void GenerateWorld()
	{
		int k = -1;

		for (int i = 0; i < MaxRow; i++)
		{
			for (int j = 0; j < MaxColumn; j++)
			{

				int RandomNumber = 0;
				int BlockSelector = 0;

				RandomNumber = Random.Range(0, 20);
				if (RandomNumber !=10 & RandomNumber!=5)
				{
					BlockSelector = 0;
				}
				if (RandomNumber == 10)
				{
					BlockSelector = 1;
				}
				if(RandomNumber == 5)
				{
					BlockSelector = 2;
				}

				k = k + 1;
				BlockPositions[i,j] = Instantiate(BlockArray[BlockSelector], new Vector3(j * 2.0f, -1, -i * 2.0f), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));

			}

		}
	}
	// Update is called once per frame
	void Update()
	{

		time += Time.deltaTime;
		if (time > 0.1)
		{
			RoundNumber = RoundNumber + 1;
			time = 0;
		}
		
		/*	if (Input.GetKeyDown(KeyCode.Space))
		{
			RoundNumber = RoundNumber + 1;
		}
		*/
		if(NumberOfPreysKilled >= NumberOfPreysGenerated)
		{
			Time.timeScale = 0;
			//Debug.Log("End Game");
		}
		else { Time.timeScale = 1; }

		SetRoundText();
		
	}
}
