using System.Collections.Generic;
using UnityEngine;

namespace Personal.Character.Player
{
	public class FlashlightFlicker : MonoBehaviour
	{
		[Tooltip("Minimum random light intensity")]
		[SerializeField] float minIntensity = 0f;
		[Tooltip("Maximum random light intensity")]
		[SerializeField] float maxIntensity = 1f;
		[Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
		[Range(1, 50)]
		[SerializeField] int smoothing = 5;

		// Continuous average calculation via FIFO queue
		// Saves us iterating every time we update, we just change by the delta
		Queue<float> smoothQueue;
		float lastSum = 0;

		new Light light;

		int defaultSmoothing;
		float defaultIntensity;

		void Start()
		{
			smoothQueue = new Queue<float>(smoothing);
			light = GetComponent<Light>();

			defaultSmoothing = smoothing;
			defaultIntensity = light.intensity;
		}

		void Update()
		{
			if (!light) return;

			// Pop off an item if too big
			while (smoothQueue.Count >= smoothing)
			{
				lastSum -= smoothQueue.Dequeue();
			}

			// Generate random new item, calculate new average
			float newVal = Random.Range(minIntensity, maxIntensity);
			smoothQueue.Enqueue(newVal);
			lastSum += newVal;

			// Calculate new smoothed average
			light.intensity = lastSum / smoothQueue.Count;
		}

		public void SetSmoothing(int smoothing) { this.smoothing = smoothing; }

		void OnDisable()
		{
			smoothing = defaultSmoothing;
			light.intensity = defaultIntensity;
		}

		/// <summary>
		/// Reset the randomness and start again. You usually don't need to call
		/// this, deactivating/reactivating is usually fine but if you want a strict
		/// restart you can do.
		/// </summary>
		void Reset()
		{
			smoothQueue.Clear();
			lastSum = 0;
		}
	}
}