using UnityEngine;

public class SelfRotate : MonoBehaviour
{
	[SerializeField] float speed = 10;
	[SerializeField] Vector3 rotation = Vector3.up;
	[SerializeField] bool isRealtime = true;

	float time;

	void Update()
	{
		time = isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
		transform.Rotate(rotation, speed * time);
	}
}
