using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM
{
	public abstract class StateMachineBase : MonoBehaviour
	{
		protected StateBase state;

		protected virtual async void Awake()
		{
			enabled = false;
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
			enabled = true;
		}

		public async UniTask SetState(StateBase state)
		{
			this.state = state;
			await state.OnEnter();
		}
	}
}