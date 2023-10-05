using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class StandbyState : StateBase
	{
		[SerializeField] StateBase state = null;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			await state.Standby();
		}
	}
}