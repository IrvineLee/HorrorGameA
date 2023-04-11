using UnityEngine;

namespace Personal.Pool
{
	public class SpawnableObject : MonoBehaviour
	{
		public bool IsReturnOnDisable { get => isReturnOnDisable; }

		SpawnablePoolBase ownPool;
		bool isReturnOnDisable = true;

		/// <summary>
		/// This is set by the pool when instantiating it. No other reason to call this.
		/// </summary>
		/// <param name="pool"></param>
		public void SetPool(SpawnablePoolBase pool) { ownPool = pool; }

		/// <summary>
		/// Set this object to not return to pool OnDisable.
		/// It will return when Return() function is called, which will then resets it back to returning to pool OnDisable.
		/// </summary>
		/// <param name="flag"></param>
		public void SetNotToReturnOnDisable() { isReturnOnDisable = false; }

		/// <summary>
		/// Returns this object back to its pool.
		/// </summary>
		public void Return()
		{
			BackToPool();
		}

		void BackToPool()
		{
			isReturnOnDisable = true;
			ownPool.Return(gameObject);
		}

		void OnDisable()
		{
			if (!isReturnOnDisable) return;

			BackToPool();
		}
	}
}