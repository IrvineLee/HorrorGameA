using UnityEngine;

namespace Personal.Pool
{
	public class SpawnableObject : MonoBehaviour
	{
		[SerializeField] bool isReturnOnDisable = true;

		SpawnablePoolBase ownPool;

		/// <summary>
		/// This is set by the pool when instantiating it. No other reason to call this.
		/// </summary>
		/// <param name="pool"></param>
		public void SetPool(SpawnablePoolBase pool) { ownPool = pool; }

		/// <summary>
		/// Returns this object back to its pool. The gameobject will no longer be active.
		/// </summary>
		public void Return()
		{
			BackToPool();
		}

		void BackToPool()
		{
			ownPool.Return(gameObject);
		}

		void OnDisable()
		{
			if (!isReturnOnDisable) return;

			BackToPool();
		}
	}
}