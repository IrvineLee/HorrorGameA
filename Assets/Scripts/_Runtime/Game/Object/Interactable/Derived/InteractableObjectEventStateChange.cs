﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;
using Personal.InputProcessing;

namespace Personal.InteractiveObject
{
	public class InteractableObjectEventStateChange : InteractableObject
	{
		[SerializeField] protected ActionMapType actionMapType = ActionMapType.Player;

		protected OrderedStateMachine orderedStateMachine;
		protected InteractionAssign interactionAssign;

		protected override void Initialize()
		{
			base.Initialize();

			orderedStateMachine = GetComponentInChildren<OrderedStateMachine>();
			interactionAssign = GetComponentInChildren<InteractionAssign>();
		}

		protected override async UniTask HandleInteraction()
		{
			// When events happened, hide the items.
			StageManager.Instance.PlayerController.Inventory.FPS_ShowItem(false);

			var ifsmHandler = InitiatorStateMachine.GetComponentInChildren<IFSMHandler>();

			ifsmHandler?.OnBegin(null);
			InputManager.Instance.EnableActionMap(actionMapType);

			await orderedStateMachine.Begin(interactionAssign, InitiatorStateMachine);

			InputManager.Instance.SetToDefaultActionMap();
			ifsmHandler?.OnExit();
		}
	}
}

