using UnityEngine;
using UnityEditor;

namespace EasyTransition
{
	[CustomEditor(typeof(TransitionManager))]
	public class TransitionManagerEditor : Editor
	{
		public Texture transitionManagerLogo;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var bgTexture = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
			var style = new GUIStyle(GUI.skin.box);
			style.normal.background = bgTexture;

			GUILayout.Box(transitionManagerLogo, style, GUILayout.Width(Screen.width - 20), GUILayout.Height(Screen.height / 15));

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionManagerSettings"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("canvasScaler"));
			serializedObject.ApplyModifiedProperties();
		}
	}
}

