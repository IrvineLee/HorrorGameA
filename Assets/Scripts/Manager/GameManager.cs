using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helper;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		void Awake()
		{

		}

		void Start()
		{

		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				SceneManager.Instance.ChangeLevel(1);
			}
		}
	}
}