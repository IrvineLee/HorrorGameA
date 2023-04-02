using System;
using UnityEngine;

namespace Helper
{
	public class FlashColor : MonoBehaviour
	{
		[SerializeField] Material flashMaterial = null;
		[SerializeField] float duration = 0.2f;

		SpriteRenderer spriteRenderer;
		Material defaultMaterial;

		CoroutineRun cr = new CoroutineRun();

		void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			defaultMaterial = spriteRenderer.material;
		}

		public void Flash()
		{
			if (cr != null && !cr.IsDone)
				return;

			spriteRenderer.material = flashMaterial;
			CoroutineHelper.WaitFor(duration, () => spriteRenderer.material = defaultMaterial);
		}

		void OnDisable()
		{
			cr?.Abort();
			spriteRenderer.material = defaultMaterial;
		}
	}
}