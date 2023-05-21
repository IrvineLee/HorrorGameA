using UnityEngine;

using Personal.Manager;
using Personal.GameState;
using Cysharp.Threading.Tasks;

namespace Personal.InputProcessing
{
	public class InputControllerBase : GameInitialize
	{
		protected InputReader inputReader;

		public Vector2 Move { get; protected set; }
		public Vector2 Look { get; protected set; }

		public bool IsInteract { get; protected set; }
		public bool IsCancel { get; protected set; }

		protected override async UniTask Awake()
		{
			await UniTask.WaitUntil(() => GameManager.Instance && GameManager.Instance.IsLoadingOver);

			inputReader = InputManager.Instance.InputReader;
			isAwakeCompleted = true;
		}
	}
}