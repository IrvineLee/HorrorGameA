using Cysharp.Threading.Tasks;
using Personal.Manager;
using UnityEngine;

namespace Personal.FSM.Character
{
	public class CinematicState : StateBase
	{
		[SerializeField] bool isOn = true;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			UIManager.Instance.ToolsUI.CinematicBars(isOn);
		}
	}
}