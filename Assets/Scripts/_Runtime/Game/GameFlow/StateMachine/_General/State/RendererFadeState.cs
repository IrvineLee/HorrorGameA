using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Object;

namespace Personal.FSM.Character
{
	public class RendererFadeState : StateBase
	{
		[SerializeField] bool isFadeActor = true;
		[SerializeField] float fadeActorDuration = 0.5f;

		ActorStateMachine actorStateMachine;

		public override async UniTask OnEnter()
		{
			await base.OnEnter();

			actorStateMachine = GetComponentInParent<InteractableObject>().ActorStateMachine;
			if (!actorStateMachine) return;

			if (isFadeActor) HandleFadeInActor(actorStateMachine, false);
			else HandleFadeInActor(actorStateMachine, true);
		}

		void HandleFadeInActor(ActorStateMachine actorStateMachine, bool isFlag)
		{
			actorStateMachine.GetComponentInChildren<IRendererDissolve>().FadeInRenderer(isFlag, fadeActorDuration);
		}
	}
}