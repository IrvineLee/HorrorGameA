using UnityEngine;

namespace Helper
{
	public class DisableAfterDuration : MonoBehaviour
	{
		[SerializeField] protected float duration = 1f;
		[SerializeField] protected bool isRealTime = false;

		public float Duration { get => duration; }

		protected CoroutineRun cr;

		protected virtual void Awake() { }

		protected virtual void OnEnable()
		{
			RunDisableAfterDuration();
		}

		public void SetDuration(float duration)
		{
			this.duration = duration;
			RunDisableAfterDuration();
		}

		protected virtual void RunDisableAfterDuration()
		{
			if (duration <= 0)
				return;

			cr?.StopCoroutine();
			cr = CoroutineHelper.WaitFor(duration, RunDisable, default, isRealTime);
		}

		protected virtual void RunDisable()
		{
			gameObject.SetActive(false);
		}

		protected virtual void OnDisable()
		{
			cr?.StopCoroutine();
		}
	}
}
