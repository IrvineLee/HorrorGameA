using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using StarterAssets;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine
	{
		[SerializeField] Transform stateParent = null;

		public FirstPersonController FirstPersonController { get; private set; }
		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }

		async void Start()
		{
			FirstPersonController = GetComponentInChildren<FirstPersonController>();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				stateList.Add(child.GetComponent<StateBase>());
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());

			StateDictionary.TryGetValue(typeof(PlayerFPSState), out StateBase currentState);
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