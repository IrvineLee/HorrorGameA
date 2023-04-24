using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Personal.GameState;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase
	{
		public NavMeshAgent NavMeshAgent { get; protected set; }
		public TargetInfo TargetInfo { get; protected set; }

		protected override async UniTask Awake()
		{
			NavMeshAgent = GetComponent<NavMeshAgent>();
			await base.Awake();
		}

		void OnDisable()
		{
			NavMeshAgent.enabled = false;
		}
	}
}