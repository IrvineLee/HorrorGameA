using Cysharp.Threading.Tasks;

namespace Personal.FSM.Character
{
	public class ObjectEndState : StateBase
	{
		public override async UniTask OnEnter()
		{
			await base.OnEnter();
			await UniTask.DelayFrame(1);
		}
	}
}