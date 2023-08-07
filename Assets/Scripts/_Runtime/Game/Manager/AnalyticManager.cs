using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;

namespace Personal.Manager
{
	public class AnalyticManager : MonoBehaviourSingleton<AnalyticManager>
	{
		protected override UniTask Boot()
		{
			Debug.Log("AnalyticManager");
			GameAnalytics.Initialize();
			Debug.Log("AnalyticManager started");

			return base.Boot();
		}
	}
}