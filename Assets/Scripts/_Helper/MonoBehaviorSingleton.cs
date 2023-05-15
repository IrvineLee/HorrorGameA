using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Helper
{
	public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
	{
		static TSelfType m_Instance = null;

		UniTask uniTask = new UniTask();

		protected virtual UniTask Awake()
		{
			DontDestroyOnLoad(Instance.gameObject);
			return uniTask;
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