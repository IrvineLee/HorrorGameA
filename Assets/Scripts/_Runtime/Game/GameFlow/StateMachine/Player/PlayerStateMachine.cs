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

		protected override void Initialize()
		{
			base.Initialize();

			List<StateBase> stateList = new();
			foreach (Transform child in stateParent)
			{
				var stateBase = child.GetComponent<StateBase>();

				stateList.Add(stateBase);
				stateBase.SetMyFSM(this);
			}

			StateDictionary = stateList.ToDictionary((state) => state.GetType());
			SwitchToState(typeof(PlayerStandardState)).Forget();
		}

		public override async UniTask SwitchToState(Type type)
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

		protected override void OnRendererDissolveBegin()
		{
			StageManager.Instance.PlayerController.FPSController.ResetAnimationBlend(0.25f);
		}

		void IFSMHandler.OnBegin(Type type)
		{
			if (type == null) type = typeof(PlayerIdleState);
			SwitchToState(type).Forget();
		}

		void IFSMHandler.OnExit()
		{
			SwitchToState(typeof(PlayerStandardState)).Forget();
		}
	}
}