using UnityEngine;

using Helper;
using Cysharp.Threading.Tasks;

namespace Personal.Manager
{
	public class CursorManager : MonoBehaviourSingleton<CursorManager>
	{
		[SerializeField] Texture2D defaultCursor = null;
		[SerializeField] Vector2 hotspotOffset = Vector2.zero;

		protected override async UniTask Awake()
		{
			await base.Awake();

			SetDefault();
		}

		public void SetDefault()
		{
			SetToTexture(defaultCursor);
		}

		public void SetToTexture(Texture2D texture2D)
		{
			Cursor.SetCursor(texture2D, hotspotOffset, CursorMode.Auto);
		}
	}
}