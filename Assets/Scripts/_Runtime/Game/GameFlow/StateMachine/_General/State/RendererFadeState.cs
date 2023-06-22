using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class RendererFadeState : StateBase
	{
		[SerializeField] bool isFadeActor = true;
		[SerializeField] float fadeActorDuration = 0.5f;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			if (!stateMachine.InitiatorStateMachine) return;

			if (isFadeActor) HandleFadeInActor(stateMachine.InitiatorStateMachine, false);
			else HandleFadeInActor(stateMachine.InitiatorStateMachine, true);
		}

		void HandleFadeInActor(StateMachineBase initiatorStateMachine, bool isFlag)
		{
			initiatorStateMachine.GetComponentInChildren<IRendererDissolve>().FadeInRenderer(isFlag, fadeActorDuration);
		}
	}
}