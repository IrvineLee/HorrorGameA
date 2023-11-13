using UnityEngine;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class AddressableHelper : MonoBehaviour
{
	/// <summary>
	/// Handle the spawning of objects.
	/// </summary>
	public static async UniTask<GameObject> Spawn(string path, Vector3 position = default, Transform parent = default, bool isWorldPositionStay = false)
	{
		await LoadAssetAsync(path);
		return InstantiateAsync(path, position, parent, isWorldPositionStay);
	}

	/// <summary>
	/// Spawn multiple objects.
	/// </summary>
	public static async UniTask<List<GameObject>> SpawnMultiple(string path, int amount, Vector3 position = default, Transform parent = default, bool isWorldPositionStay = false)
	{
		await LoadAssetAsync(path);

		List<GameObject> goList = new();
		for (int i = 0; i < amount; i++)
		{
			goList.Add(InstantiateAsync(path, position, parent, isWorldPositionStay));
		}

		return goList;
	}

	/// <summary>
	/// Get the result of the path. Does NOT instantiate the object!
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static async UniTask<GameObject> GetResult(string path)
	{
		return await LoadAssetAsync(path);
	}

	static async UniTask<GameObject> LoadAssetAsync(string path)
	{
		AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
		await UniTask.WaitUntil(() => handle.Status != AsyncOperationStatus.None);

		return handle.Result;
	}

	static GameObject InstantiateAsync(string path, Vector3 position, Transform parent, bool isWorldPositionStay)
	{
		GameObject go = Addressables.InstantiateAsync(path, position, Quaternion.identity).Result;
		go.name = path;
		go.transform.SetParent(parent, isWorldPositionStay);
		return go;
	}
}
