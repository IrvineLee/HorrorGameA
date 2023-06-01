using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.GameState
{
	public class AddToUIManager : GameInitialize
	{
		protected async override UniTask Awake()
		{
			await base.Awake();

			transform.SetParent(UIManager.Instance.transform);
		}
	}
}