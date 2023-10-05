using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class QuestManager : MonoBehaviourSingleton<QuestManager>
	{

		protected override UniTask Boot()
		{
			return base.Boot();
		}
	}
}