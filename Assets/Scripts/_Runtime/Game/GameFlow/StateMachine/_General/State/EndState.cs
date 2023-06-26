using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class EndState : StateBase
	{
		[SerializeField] bool isReturnToPool = true;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			if (!isReturnToPool) return;
			PoolManager.Instance.ReturnSpawnedObject(stateMachine.gameObject);
		}
	}
}