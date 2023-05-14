using UnityEngine;

using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class AddressableHelper : MonoBehaviour
{
	/// <summary>
	/// Handle the spawning of objects.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static async UniTask<GameObject> Spawn(string path, Vector3 position = default, Transform parent = default)
	{
		AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
		await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None);

		GameObject go = Addressables.InstantiateAsync(path, position, Quaternion.identity).Result;
		go.name = path;
		go.transform.SetParent(parent);

		return go;
	}
}
