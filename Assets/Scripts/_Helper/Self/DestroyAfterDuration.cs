using UnityEngine;

namespace Helper
{
	public class DestroyAfterDuration : MonoBehaviour
	{
		[SerializeField] float duration = 1f;

		void Start()
		{
			Destroy(gameObject, duration);
		}
	}
}
