using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Personal.GameState;
using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase
	{
		public NavMeshAgent NavMeshAgent { get; protected set; }
		public TargetInfo TargetInfo { get; protected set; }
		public DialogueSystemTrigger DialogueSystemTrigger { get; protected set; }

		protected override async UniTask Awake()
		{
			NavMeshAgent = GetComponent<NavMeshAgent>();
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>();

			await base.Awake();
		}

		void OnDisable()
		{
			NavMeshAgent.enabled = false;
		}
	}
}