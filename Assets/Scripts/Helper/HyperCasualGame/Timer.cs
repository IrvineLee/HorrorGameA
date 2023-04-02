using System;
using UnityEngine;

namespace Helper
{
	[Serializable]
	public class Timer
	{
		[SerializeField] int seconds;
		[SerializeField] int minutes;
		[SerializeField] int hours;

		public int Seconds { get => seconds; }
		public int Minutes { get => minutes; }
		public int Hours { get => hours; }

		public Timer(int seconds, int minutes, int hours)
		{
			this.seconds = seconds;
			if (seconds >= 60)
			{
				minutes += seconds / 60;
				this.seconds = seconds % 60;
			}

			this.minutes = minutes;
			if (minutes >= 60)
			{
				hours += minutes / 60;
				this.minutes = minutes % 60;
			}

			this.hours = hours;
		}

		public int GetSeconds()
		{
			return seconds + minutes * 60 + hours * 60 * 60;
		}
	}
}