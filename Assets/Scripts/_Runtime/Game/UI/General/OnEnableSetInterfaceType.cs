
using Cysharp.Threading.Tasks;
using Personal.GameState;
using Personal.Manager;
using UnityEngine;

namespace Personal.UI
{
	public class OnEnableSetInterfaceType : GameInitialize
	{
		[SerializeField] UIInterfaceType uiInterfaceType = UIInterfaceType.None;

		protected async override UniTask Awake()
		{
			await base.Awake();

			UIManager.Instance.RegisterInterfaceUI(uiInterfaceType, gameObject);
		}

		protected override async UniTask OnEnable()
		{
			await base.OnEnable();

			UIManager.Instance.SetActiveInterfaceType(uiInterfaceType);
		}

		void OnDisable()
		{
			UIManager.Instance.SetActiveInterfaceType(UIInterfaceType.None);
		}
	}
}
