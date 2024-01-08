using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class PlayerStateMachine : ActorStateMachine, IFSMHandler
	{
		[SerializeField] Transform stateParent = null;

		public IReadOnlyDictionary<Type, StateBase> StateDictionary { get; private set; }
		public IFSMHandler IFSMHandler { get => this; }

		protected override void EarlyInitialize()
		{
			base.EarlyInitialize();
			Init().Forget();
		}

		public override Type GetStateType<T>(T type)
		{
			switch (type)
			{
				case PlayerStateType.Idle: return typeof(PlayerIdleState);
				case PlayerStateType.Standard: return typeof(PlayerStandardState);
				case PlayerStateType.LookAt: return typeof(PlayerLookAtState);
				case PlayerStateType.POVControl: return typeof(PlayerPOVControlState);
				case PlayerStateType.Cashier: return typeof(PlayerCashierState);
				default: return null;
			}
		}

		public bool IsPlayerThisState(Type type)
		{
			if (CurrentState && CurrentState.GetType() == type) return true;
			return false;
		}

		protected override void OnRendererDissolveBegin()
		{
			StageManager.Instance.PlayerController.PlayerAnimatorController.ResetAnimationBlend(0.25f);
		}

		async UniTask Init()
		{
			// Wait for the state base to call their Awake first.
			await UniTask.Yield();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				var stateBase = child.GetComponent<StateBase>();

				stateList.Add(stateBase);
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());
			((IFSMHandler)this).OnBegin(typeof(PlayerStandardState));
		}

		void IFSMHandler.OnBegin(Type type)
		{
			if (type == null) type = typeof(PlayerIdleState);

			SwitchToState(type).Forget();
		}

		void IFSMHandler.OnExit()
		{
			// You need to set it to the next state first so the previous OnExit gets called.
			SwitchToState(typeof(PlayerStandardState)).Forget();
			SetLookAtInfo(null);
		}

		async UniTask SwitchToState(Type type)
		{
			StateDictionary.TryGetValue(type, out StateBase state);

			if (state == null)
			{
				Debug.Log("Couldn't find state of type " + type.Name);
				return;
			}
			await SetState(state);
		}
	}
}