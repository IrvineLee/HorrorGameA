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
			GameAnalytics.Initialize();

			return base.Boot();
		}
	}
}