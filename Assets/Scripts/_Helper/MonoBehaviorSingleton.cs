using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Helper
{
	public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
	{
		static TSelfType m_Instance = null;
		static bool isQuitting;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);
			Initialization().Forget();
		}

		protected virtual async UniTask Initialization() { await UniTask.Yield(); }

		public static TSelfType Instance
		{
			get
			{
				// To prevent creating a new instance in case of qutting.
				if (isQuitting) return null;

				if (m_Instance == null)
				{
					m_Instance = (TSelfType)FindObjectOfType(typeof(TSelfType));

					if (m_Instance == null)
						m_Instance = (new GameObject(typeof(TSelfType).Name)).AddComponent<TSelfType>();
				}
				return m_Instance;
			}
		}

		void OnApplicationQuit()
		{
			isQuitting = true;
		}
	}
}