using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyTransition
{
	[CreateAssetMenu(fileName = "TransitionSettings", menuName = "Florian Butz/New Transition Settings", order = 0)]
	public class TransitionManagerSettings : ScriptableObject
	{
		[SerializeField] TransitionSettings[] transitions;

		[SerializeField] Material multiplyColorMaterial;
		[SerializeField] Material addColorMaterial;

		public IReadOnlyDictionary<TransitionType, TransitionSettings> TransitionSettingDictionary { get; private set; }
		public Material MultiplyColorMaterial { get => multiplyColorMaterial; }
		public Material AddColorMaterial { get => addColorMaterial; }

		public void Initialize()
		{
			TransitionSettingDictionary = transitions.ToDictionary(i => i.TransitionType);
		}

		public TransitionSettings GetTransitionSetting(TransitionType transitionType)
		{
			if (!TransitionSettingDictionary.TryGetValue(transitionType, out TransitionSettings transitionSetting))
			{
				Debug.LogError("The transitionType: " + transitionType + " is not a valid id.");
				return null;
			}
			return transitionSetting;
		}
	}

	public enum TransitionType
	{
		[StringValue("TransitionFade")]
		Fade = 0,

		[StringValue("TransitionCircleWipe")]
		CircleWipe,

		[StringValue("TransitionLinearWipe")]
		LinearWipe,

		[StringValue("TransitionRectangleGrid")]
		RectangleGrid,

		[StringValue("TransitionDoubleWipe")]
		DoubleWipe,

		[StringValue("TransitionDiagonalRectangleGrid")]
		DiagonalRectangleGrid,

		[StringValue("TransitionRectangleWipe")]
		RectangleWipe,

		[StringValue("TransitionVerticalCurtain")]
		VerticalCurtain,

		[StringValue("TransitionHorizontalCurtain")]
		HorizontalCurtain,

		[StringValue("TransitionBrush")]
		Brush,

		[StringValue("TransitionPaintSplash")]
		PaintSplash,

		[StringValue("TransitionNoise")]
		Noise,
	}

	[Serializable]
	public class TransitionSettings
	{
		[Header("Transition Settings")]
		[Tooltip("The name when you transition is called, to seperate it from others. Example: 'CircleFade'")]
		[SerializeField] TransitionType transitionType = TransitionType.Fade;

		[Tooltip("The resolution of the canvas the transition was made in. For some transitions this might change.")]
		[SerializeField] Vector2 referenceResolution = new Vector2(1920, 1080);

		[Tooltip("If set to true you can't interact with any UI until the transition is over.")]
		[SerializeField] bool blockRaycasts = false;

		[Space(10)]
		[Tooltip("Changes the color tint mode. Multiply just tints the color and Add adds the color to the transition.")]
		[SerializeField] ColorTintMode colorTintMode = ColorTintMode.Multiply;

		[Tooltip("Changes the color of the transition based on the color tint mode.")]
		[SerializeField] Color colorTint = Color.white;

		[Tooltip("If the transition uses the UICutoutMask component.")]
		[SerializeField] bool isCutoutTransition = false;

		[Space(10)]
		[Tooltip("Changes the animation speed of the transition. Only works when theres 1 Animator component somewhere on the transition prefab.")]
		[Range(0.5f, 2f)]
		[SerializeField] float transitionSpeed = 1;

		[Space(10)]
		[Tooltip("Sets the size of the transition on the x axis to -1.")]
		[SerializeField] bool flipX;
		[Tooltip("Sets the size of the transition on the y axis to -1.")]
		[SerializeField] bool flipY;

		[Header("Transition Prefabs")]
		[Space(10)]
		[SerializeField] GameObject transitionIn;
		[SerializeField] GameObject transitionOut;

		public TransitionType TransitionType { get => transitionType; }
		public Vector2 ReferenceResolution { get => referenceResolution; }
		public bool BlockRaycasts { get => blockRaycasts; }
		public ColorTintMode ColorTintMode { get => colorTintMode; }
		public Color ColorTint { get => colorTint; }
		public bool IsCutoutTransition { get => isCutoutTransition; }
		public float TransitionSpeed { get => transitionSpeed; }
		public bool FlipX { get => flipX; }
		public bool FlipY { get => flipY; }
		public GameObject TransitionIn { get => transitionIn; }
		public GameObject TransitionOut { get => transitionOut; }
	}

	public enum ColorTintMode { Multiply, Add }
}
