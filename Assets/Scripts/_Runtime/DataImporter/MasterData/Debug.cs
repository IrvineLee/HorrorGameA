using UnityEngine;

public class Debug : MonoBehaviour
{
	public static bool IsDebugBuild { get => UnityEngine.Debug.isDebugBuild; }

	public static void Log(string str)
	{
		if (IsDebugBuild) UnityEngine.Debug.Log(str);
	}

	public static void Log(object obj)
	{
		if (IsDebugBuild) UnityEngine.Debug.Log(obj);
	}

	public static void Log(object obj, Object context)
	{
		if (IsDebugBuild) UnityEngine.Debug.Log(obj, context);
	}

	public static void LogWarning(string str)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogWarning(str);
	}

	public static void LogWarning(object obj)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogWarning(obj);
	}

	public static void LogError(string str)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogError(str);
	}

	public static void LogError(object obj)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogError(obj);
	}

	public static void LogError(object obj, Object context)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogError(obj, context);
	}

	public static void LogAssertion(string str)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogAssertion(str);
	}

	public static void LogException(System.Exception exception)
	{
		if (IsDebugBuild) UnityEngine.Debug.LogException(exception);
	}

	public static void Break()
	{
		if (IsDebugBuild) UnityEngine.Debug.Break();
	}

	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		if (IsDebugBuild) UnityEngine.Debug.DrawLine(start, end, color);
	}
}