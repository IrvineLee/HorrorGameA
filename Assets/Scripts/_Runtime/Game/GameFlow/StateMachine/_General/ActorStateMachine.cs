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

		protected override async UniTask Awake()
		{
			await base.Awake();

			NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();
			AnimatorController = GetComponentInChildren<AnimatorController>();
		}

		void OnDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}