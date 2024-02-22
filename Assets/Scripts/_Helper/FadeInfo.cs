
using System;

namespace Helper
{
	public class FadeInfo
	{
		public CoroutineRun CR { get; set; }
		public float StartValue { get; }
		public float EndValue { get; }
		public float Duration { get; }
		public Action DoLast { get; }

		public FadeInfo(float startValue, float endValue, float duration, Action doLast = default)
		{
			//CR = cr;
			StartValue = startValue;
			EndValue = endValue;
			Duration = duration;
			DoLast = doLast;
		}
	}
}