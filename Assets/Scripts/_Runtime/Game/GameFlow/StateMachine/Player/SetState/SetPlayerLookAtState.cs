using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.Character.Player;

namespace Personal.FSM.Character
{
	public class SetPlayerLookAtState : StateBase
	{
		[SerializeField] Transform lookAtTarget = null;

		[Tooltip("Does the player remain looking at target after state end?")]
		[SerializeField] bool isPersist = false;

		[Tooltip("Does it turn by animation or instantly?")]
		[SerializeField] bool isInstant = true;

		PlayerController playerController;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			playerController = StageManager.Instance.PlayerController;
			var lookAtInfo = new LookAtInfo(lookAtTarget, isPersist, isInstant);

			await playerController.LookAt(lookAtInfo);
		}
	}
}