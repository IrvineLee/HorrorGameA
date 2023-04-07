using UnityEngine;
using System.Collections;
using Helper;

[RequireComponent(typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
	[SerializeField] float fov = 60f;
	[SerializeField] float near = 0.3f;
	[SerializeField] float far = 1000f;
	[SerializeField] float orthographicSize = 50f;
	[SerializeField] float changeDuration = 1f;

	private Matrix4x4 ortho, perspective;

	private float aspect;
	private MatrixBlender blender;
	private bool orthoOn;

	Camera cam;

	void Start()
	{
		aspect = (float)Screen.width / (float)Screen.height;
		ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
		perspective = Matrix4x4.Perspective(fov, aspect, near, far);

		cam = GetComponent<Camera>();
		//cam.projectionMatrix = ortho;
		//orthoOn = true;
		blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
	}

	public void ChangeProjection()
	{
		orthoOn = !orthoOn;
		if (orthoOn)
		{
			blender.BlendToMatrix(ortho, changeDuration);
			CoroutineHelper.WaitFor(changeDuration, () =>
			{
				cam.orthographic = true;
				cam.orthographicSize = orthographicSize;
			});
		}
		else
			blender.BlendToMatrix(perspective, changeDuration);
	}
}
