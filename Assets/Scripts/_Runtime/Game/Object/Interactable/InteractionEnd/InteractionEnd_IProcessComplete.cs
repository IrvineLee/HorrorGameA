﻿using System.Collections.Generic;
using UnityEngine;

using Personal.Interface;
using Personal.FSM;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_IProcessComplete : InteractionEnd
	{
		[SerializeField] Transform iProcessTrans = null;

		protected override bool IsEnded()
		{
			IProcess iProcess = iProcessTrans.GetComponentInChildren<IProcess>(true);
			if (iProcess == null) return false;
			if (!iProcess.IsCompleted()) return false;

			return true;
		}

		protected override void HandleInteractable()
		{
			GetComponentInParent<InteractionAssign>()?.SetProcessComplete();
		}
	}
}

