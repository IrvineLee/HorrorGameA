using System.Collections.Generic;
using UnityEngine;

using Personal.Interface;

namespace Personal.InteractiveObject
{
	public class InteractionEnd_IProcess : InteractionEnd
	{
		[SerializeField] Transform iProcessTrans = null;

		protected override bool IsEnded()
		{
			IProcess iProcess = iProcessTrans.GetComponentInChildren<IProcess>(true);
			if (iProcess == null) return false;
			if (!iProcess.IsCompleted()) return false;

			return true;
		}
	}
}

