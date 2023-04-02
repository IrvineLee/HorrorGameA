using UnityEngine;

namespace HyperCasualGame.Helper
{
	public class SafeArea : MonoBehaviour
	{
		[SerializeField] bool isExecuteUpdate = false;

		void Start()
		{
			CalculateSafeArea();
		}

		void Update()
		{
			if (isExecuteUpdate) CalculateSafeArea();
		}

		void CalculateSafeArea()
		{
			RectTransform rectTransform = GetComponent<RectTransform>();
			Vector2 minAnchor = Screen.safeArea.position;
			Vector3 maxAnchor = minAnchor + Screen.safeArea.size;

			minAnchor.x /= Screen.width;
			minAnchor.y /= Screen.height;
			maxAnchor.x /= Screen.width;
			maxAnchor.y /= Screen.height;

			rectTransform.anchorMin = minAnchor;
			rectTransform.anchorMax = maxAnchor;
		}
	}
}