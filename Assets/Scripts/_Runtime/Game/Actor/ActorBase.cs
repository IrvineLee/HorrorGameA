using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.FSM;
using Personal.Manager;

namespace Personal.FSM.Character
{
	public class ActorBase : StateBase
	{
		protected Transform Actor { get; private set; }

		public void SetActor(Transform actor)
		{
			Actor = actor;
		}

		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnEnter()
		{
			await base.OnEnter();
		}

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnUpdate()
		{
			await base.OnUpdate();
		}

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public override async UniTask OnExit()
		{
			await base.OnExit();
		}

		protected virtual void HandleMovement() { }
	}
}