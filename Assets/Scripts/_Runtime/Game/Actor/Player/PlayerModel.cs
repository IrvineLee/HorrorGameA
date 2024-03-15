using UnityEngine;

using Personal.Character.Animation;
using Personal.GameState;

namespace Personal.Character.Player
{
	public class PlayerModel : GameInitialize
	{
		[SerializeField] PlayerAnimatorController animatorController = null;
		[SerializeField] PlayerAnimatorController mirrorAnimatorController = null;

		public PlayerAnimatorController AnimatorController { get => animatorController; }
		public PlayerAnimatorController MirrorAnimatorController { get => mirrorAnimatorController; }
	}
}