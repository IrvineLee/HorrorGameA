using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class ScriptDisableState : StateBase
	{
		[SerializeField] List<MonoBehaviour> scriptList = new List<MonoBehaviour>();

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			foreach (var script in scriptList)
			{
				script.enabled = false;
			}
		}
	}
}