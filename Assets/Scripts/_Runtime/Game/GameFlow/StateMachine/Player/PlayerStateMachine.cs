using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine, IFSMHandler
	{
		[SerializeField] Transform stateParent = null;

		public FPSController FPSController { get; private set; }
		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			FPSController = GetComponentInChildren<FPSController>();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				var stateBase = child.GetComponent<StateBase>();

				stateList.Add(stateBase);
				stateBase.SetFSM(this);
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());
			SwitchToState(typeof(PlayerStandardState)).Forget();
		}

		public async UniTask SwitchToState(Type type)
		{
			StateDictionary.TryGetValue(type, out StateBase state);

			if (state == null)
			{
				Debug.Log("Couldn't find state of type " + type.Name);
				return;
			}
			await SetState(state);
		}

		public bool IsPlayerThisState(Type type)
		{
			if (CurrentState.GetType() == type) return true;
			return false;
		}

		protected override void OnRendererDissolving(bool isFlag)
		{
			if (isFlag) FPSController.ResetAnimationBlend();
		}

		void IFSMHandler.OnBegin()
		{
			SwitchToState(typeof(PlayerIdleState)).Forget();
		}

		void IFSMHandler.OnExit()
		{
			SwitchToState(typeof(PlayerStandardState)).Forget();
		}
	}
}