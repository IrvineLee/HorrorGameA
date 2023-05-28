using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Interface;

namespace Personal.FSM.Character
{
	public class ScriptDisableState : StateBase
	{
		[SerializeField] Transform iProcessTrans = null;
		[SerializeField] List<MonoBehaviour> scriptList = new List<MonoBehaviour>();

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			IProcess iProcess = iProcessTrans.GetComponentInChildren<IProcess>(true);
			if (iProcess == null) return;
			if (!iProcess.IsCompleted()) return;

			foreach (var script in scriptList)
			{
				script.enabled = false;
			}
		}
	}
}