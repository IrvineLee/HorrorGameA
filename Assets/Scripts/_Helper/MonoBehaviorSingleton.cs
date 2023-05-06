using UnityEngine;

namespace Helper
{
	public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
	{
		static TSelfType m_Instance = null;

		void Awake()
		{
			if (Application.isPlaying)
				DontDestroyOnLoad(Instance.gameObject);
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