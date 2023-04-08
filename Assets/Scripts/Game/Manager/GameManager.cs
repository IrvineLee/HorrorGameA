using System.Collections;
using UnityEngine;

using Helper;

namespace Personal.Manager
{
	public class GameManager : MonoBehaviourSingleton<GameManager>
	{
		public MasterDataManager MasterData { get => MasterDataManager.Instance; }

		IEnumerator Start()
		{
			MasterDataManager.CreateInstance();
			yield return new WaitUntil(() => IsInitialized());
		}

		bool IsInitialized()
		{
			if (MasterDataManager.Instance == null)
				return false;

			return true;
		}
	}
}