using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Helper
{
	public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
	{
		static TSelfType m_Instance = null;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);
			Boot().Forget();
		}

		protected virtual async UniTask Boot() { await UniTask.CompletedTask; }

		public static TSelfType Instance
		{
			get
			{
				// To prevent creating a new instance in case of qutting.
				if (App.IsQuitting) return null;

				if (m_Instance == null)
				{
					m_Instance = (TSelfType)FindObjectOfType(typeof(TSelfType));

					if (m_Instance == null)
						m_Instance = (new GameObject(typeof(TSelfType).Name)).AddComponent<TSelfType>();
				}
				return m_Instance;
			}
		}
	}
}