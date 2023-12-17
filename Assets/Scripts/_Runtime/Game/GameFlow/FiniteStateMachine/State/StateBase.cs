using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Quest;
using Personal.InteractiveObject;

namespace Personal.FSM
{
	public abstract class StateBase : MonoBehaviour
	{
		[Tooltip("Should this state wait till OnExit finishes before proceeding to the next state?")]
		[SerializeField] bool isWaitForOnExit = false;

		public bool IsWaitForOnExit { get => isWaitForOnExit; }

		protected StateMachineBase stateMachine;
		protected bool isEntered;

		protected QuestTypeSet questTypeSet;
		protected List<InteractionEnd> interactionEndList = new();

		void Awake()
		{
			stateMachine = GetComponentInParent<StateMachineBase>(true);

			questTypeSet = GetComponentInChildren<QuestTypeSet>(true);
			interactionEndList = GetComponentsInChildren<InteractionEnd>(true).ToList();
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnEnter()
		{
			if (stateMachine.IsPauseStateMachine)
			{
				await UniTask.WaitUntil(() => !stateMachine.IsPauseStateMachine);
			}

			isEntered = true;
			questTypeSet?.TryUpdateData();
		}

		/// <summary>
		/// Called from the state machine.
		/// </summary>
		/// <returns></returns>
		public void OnUpdateRun()
		{
			if (!isEntered) return;

			OnUpdate();
		}

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public virtual void OnUpdate() { }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnExit()
		{
			if (stateMachine.IsPauseStateMachine)
			{
				await UniTask.WaitUntil(() => !stateMachine.IsPauseStateMachine);
			}

			isEntered = false;

			foreach (var interactionEnd in interactionEndList)
			{
				interactionEnd.EnableInteractables().Forget();
			}
		}

		/// <summary>
		/// This is typically used in CompareState script to halt the state before moving on.
		/// Can also be used to reset values, similar to OnExit but at different time.
		/// </summary>
		/// <returns></returns>
		public virtual UniTask Standby() { return UniTask.CompletedTask; }

		private void OnValidate()
		{
			transform.name = GetType().Name;
		}

		void Reset()
		{
			transform.name = GetType().Name;
		}
	}
}