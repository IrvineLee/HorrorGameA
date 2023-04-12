using Cysharp.Threading.Tasks;

namespace Personal.FSM
{
	public abstract class State
	{
		/// <summary>
		/// Called when the state begins
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnEnter() { await UniTask.Delay(0); }

		/// <summary>
		/// Called to request updating
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnUpdate() { await UniTask.Delay(0); }

		/// <summary>
		/// Called when the state is ended
		/// </summary>
		/// <returns></returns>
		public virtual async UniTask OnExit() { await UniTask.Delay(0); }
	}
}