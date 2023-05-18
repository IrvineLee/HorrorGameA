using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Personal.GameState;
using Personal.Character.Animation;
using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase
	{
		public TargetInfo TargetInfo { get; protected set; }
		public NavMeshAgent NavMeshAgent { get; protected set; }
		public DialogueSystemTrigger DialogueSystemTrigger { get; protected set; }
		public AnimatorController AnimatorController { get; protected set; }

		protected List<StateBase> orderedStateList = new List<StateBase>();

		protected override async UniTask Awake()
		{
			await base.Awake();

			NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			AnimatorController = GetComponentInChildren<AnimatorController>();
		}

		protected override void Update()
		{
			if (state == null) return;

			state.OnUpdate();
		}

		protected async UniTask PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				state.SetFSM(this);
				await SetState(state);
			}
		}

		void OnDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}