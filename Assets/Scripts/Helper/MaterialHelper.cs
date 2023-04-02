using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public class MaterialHelper : MonoBehaviour
	{
		public static void ChangeModeToFade(MeshRenderer meshRenderer)
		{
			meshRenderer.material.SetFloat("_Mode", 2);
			meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			meshRenderer.material.SetInt("_ZWrite", 0);
			meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
			meshRenderer.material.EnableKeyword("_ALPHABLEND_ON");
			meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			meshRenderer.material.renderQueue = 3000;
		}
	}
}
