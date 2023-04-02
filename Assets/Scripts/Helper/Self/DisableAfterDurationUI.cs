using UnityEngine;

namespace Helper
{
	public class DisableAfterDurationUI : DisableAfterDuration
	{
		[SerializeField] protected float fadeDuration = 1f;

		SpriteRenderer spriteRenderer;

		protected override void Awake()
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		protected override void RunDisable()
		{
			if (spriteRenderer)
				cr = CoroutineHelper.FadeFromTo(spriteRenderer, 1, 0, fadeDuration, () => gameObject.SetActive(false));
			else
				gameObject.SetActive(false);
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (spriteRenderer)
				spriteRenderer.color = spriteRenderer.color.With(a: 1);
		}
	}
}
