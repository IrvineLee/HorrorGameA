using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM
{
	public abstract class StateMachine<T> : MonoBehaviour where T : State
	{
		protected T state;

		public async void SetState(T state)
		{
			this.state = state;
			await state.OnEnter();
		}
	}
}