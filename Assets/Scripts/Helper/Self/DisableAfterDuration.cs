using UnityEngine;

namespace Helper
{
	public class DisableAfterDuration : MonoBehaviour
	{
		[SerializeField] protected float duration = 1f;
		[SerializeField] protected float defaultDuration = 1f;

		public float Duration { get => duration; }
		public float DefaultDuration { get => defaultDuration; }

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
			cr = CoroutineHelper.WaitFor(duration, RunDisable);
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
