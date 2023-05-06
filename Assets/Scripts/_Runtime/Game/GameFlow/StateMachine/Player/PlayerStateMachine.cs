using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using StarterAssets;
using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine
	{
		[SerializeField] Transform stateParent = null;

		public FirstPersonController FirstPersonController { get; private set; }
		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			FirstPersonController = GetComponentInChildren<FirstPersonController>();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				stateList.Add(child.GetComponent<StateBase>());
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());

			StateDictionary.TryGetValue(typeof(PlayerDefaultState), out StateBase currentState);
			await SetState(currentState);
			await currentState.OnExit();
		}

		async void Update()
		{
			if (state == null) return;

			await state.OnUpdate();
		}
	}
}