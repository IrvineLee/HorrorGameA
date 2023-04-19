using Cysharp.Threading.Tasks;
using Personal.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM
{
	public abstract class StateMachine : MonoBehaviour
	{
		protected State state;

		protected virtual async void Awake()
		{
			enabled = false;
			await UniTask.WaitUntil(() => GameManager.Instance.IsLoadingOver);
			enabled = true;
		}

		public async void SetState(State state)
		{
			this.state = state;
			await state.OnEnter();
		}
	}
}