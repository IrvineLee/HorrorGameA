using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine
	{
		[SerializeField] Transform stateParent = null;

		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }

		async void Start()
		{
			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				stateList.Add(child.GetComponent<StateBase>());
			}

			StateDictionary = (IReadOnlyDictionary<Type, StateBase>)stateList.ToDictionary((state) => state.GetType());

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