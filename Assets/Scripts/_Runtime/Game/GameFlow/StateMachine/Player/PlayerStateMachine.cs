using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine
	{
		[SerializeField] Transform stateParent = null;

		public FPSController FPSController { get; private set; }
		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }

		protected override async UniTask Awake()
		{
			await base.Awake();

			FPSController = GetComponentInChildren<FPSController>();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				var stateBase = child.GetComponent<StateBase>();

				stateList.Add(stateBase);
				stateBase.SetFSM(this);
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());
			await SwitchToState(typeof(PlayerStandardState));
		}

		public async UniTask SwitchToState(Type type)
		{
			StateDictionary.TryGetValue(type, out StateBase currentState);

			if (currentState == null)
			{
				Debug.Log("Couldn't find state of type " + type.Name);
				return;
			}
			await SetState(currentState);
		}
	}
}