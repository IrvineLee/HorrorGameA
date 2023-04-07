using UnityEngine;
using DG.Tweening;
using System.Collections;

using Helper;

namespace HyperCasualGame.Bullet
{
	public class OnEnableSpriteScale : MonoBehaviour
	{
		[SerializeField] float scaleSpeed = 0.2f;
		[SerializeField] Vector3 beforeScale = new Vector3(0.1f, 0.1f, 1f);
		[SerializeField] bool isDisableOnComplete = false;
		[SerializeField] SpriteRenderer spriteRenderer;
		[SerializeField] GameObject disableGameObject = null;

		Vector3 afterScale;

		void Awake()
		{
			afterScale = this.transform.localScale;
		}

		void OnEnable()
		{
			spriteRenderer.color = spriteRenderer.color.With(a: 1);
			spriteRenderer.enabled = true;

			spriteRenderer.transform.localScale = beforeScale;
			spriteRenderer.transform.DOScale(afterScale, scaleSpeed).SetEase(Ease.Linear).OnComplete(() =>
			{
				if (isDisableOnComplete == true)
				{
					disableGameObject?.gameObject.SetActive(false);
				}
			});
		}
	}
}