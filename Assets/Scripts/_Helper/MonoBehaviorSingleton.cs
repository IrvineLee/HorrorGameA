using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Helper
{
	public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
	{
		static TSelfType m_Instance = null;

		protected virtual async UniTask Awake()
		{
			DontDestroyOnLoad(Instance.gameObject);
			await UniTask.Delay(0);
		}

		public static TSelfType Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = (TSelfType)FindObjectOfType(typeof(TSelfType));
				}
				return m_Instance;
			}
		}
	}
}