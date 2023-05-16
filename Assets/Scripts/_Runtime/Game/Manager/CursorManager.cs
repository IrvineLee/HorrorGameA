using UnityEngine;
using UnityEngine.SceneManagement;

using Helper;
using Cysharp.Threading.Tasks;
using PixelCrushers;

namespace Personal.Manager
{
	public class CursorManager : MonoBehaviourSingleton<CursorManager>
	{
		[SerializeField] Transform crosshairUI = null;
		[SerializeField] Transform mouseCursorUI = null;

		protected override async UniTask Awake()
		{
			await base.Awake();

			if (SceneManager.GetActiveScene().name.Equals(SceneName.Title))
			{
				SetToMouseCursor(true);
				return;
			}

			SetToMouseCursor(false);
		}

		public void SetToMouseCursor(bool isFlag)
		{
			Cursor.visible = false;
			InputDeviceManager.instance.ForceCursorFalse();

			crosshairUI.gameObject.SetActive(!isFlag);
			mouseCursorUI.gameObject.SetActive(isFlag);
		}
	}
}