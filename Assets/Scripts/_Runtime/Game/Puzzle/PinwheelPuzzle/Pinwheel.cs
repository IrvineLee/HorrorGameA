using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

using Helper;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Puzzle.Pinwheel
{
	[Serializable]
	public class Pinwheel
	{
		[SerializeField] [HideInInspector] Transform pinwheelTrans = null;
		[SerializeField] [HideInInspector] Collider collider = null;
		[SerializeField] [HideInInspector] int activeIndex = -1;
		[SerializeField] [HideInInspector] PositiveNegative rotationType;
		[SerializeField] [HideInInspector] bool isCenterPinwheel;

		// Odin variables.
		[SerializeField] [HideInInspector] int colorListCount;

		[OnCollectionChanged("ListCountChanged")]
		[SerializeField] List<BasicColor> basicColorList = new();

		[Tooltip("The index from colorList facing towards the center of the pinwheel")]
		[PropertyRange(0, "colorListCount")]
		[HideIf("isCenterPinwheel")]
		[SerializeField] int faceCenterIndex;

		[HorizontalGroup("Active Color")]
		[ShowInInspector]
		[ReadOnly]
		public BasicColor ActiveColor { get => activeIndex < 0 || activeIndex > basicColorList.Count - 1 ? BasicColor.Blue : basicColorList[activeIndex]; }

		[HorizontalGroup("Active Color")]
		[ShowInInspector]
		[ReadOnly]
		[LabelText("")]
		[LabelWidth(10)]
		public Color ActiveColorReal { get => ActiveColor.GetColor(); }

		[HorizontalGroup("End Color")]
		[ShowInInspector]
		[ReadOnly]
		[HideIf("isCenterPinwheel")]
		public BasicColor EndColor { get; private set; }

		[HorizontalGroup("End Color")]
		[ShowInInspector]
		[ReadOnly]
		[LabelText("")]
		[LabelWidth(10)]
		[HideIf("isCenterPinwheel")]
		public Color EndColorReal { get => EndColor.GetColor(); }
		public Collider Collider { get => collider; }
		public Transform PinwheelTrans { get => pinwheelTrans; }
		public List<BasicColor> BasicColorList { get => basicColorList; }
		public int FaceCenterIndex { get => faceCenterIndex; }
		public bool IsCenterPinwheel { get => isCenterPinwheel; }

		PinwheelPuzzle pinwheelPuzzle;
		Quaternion initialRotation;

		public void SetPinwheel(PinwheelPuzzle pinwheelPuzzle, Transform pinwheelTrans, bool isCenterPinwheel)
		{
			this.pinwheelPuzzle = pinwheelPuzzle;
			this.pinwheelTrans = pinwheelTrans;
			this.isCenterPinwheel = isCenterPinwheel;
		}

		public void SetRotationType(PositiveNegative rotationType) { this.rotationType = rotationType; }
		public void SetEndColor(BasicColor endColor) { EndColor = endColor; }

		/// <summary>
		/// After spawning, call this to rotate it to the correct direction.
		/// </summary>
		public void Setup()
		{
			for (int i = 0; i < FaceCenterIndex; i++)
			{
				Turn(0, true);
			}

			activeIndex = faceCenterIndex;
			colorListCount = basicColorList.Count - 1;
		}

		/// <summary>
		/// Initialization.
		/// </summary>
		public void InitializeRotation()
		{
			initialRotation = pinwheelTrans.rotation;
		}

		/// <summary>
		/// Reset to default.
		/// </summary>
		public void Reset()
		{
			activeIndex = faceCenterIndex;
			pinwheelTrans.rotation = initialRotation;
		}

		/// <summary>
		/// Turn the pinwheel.
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="isSetup"></param>
		/// <returns></returns>
		public CoroutineRun Turn(float duration = 0, bool isSetup = false)
		{
			activeIndex = rotationType == PositiveNegative.Positive ? activeIndex + 1 : activeIndex - 1;
			activeIndex = activeIndex.WithinCount(basicColorList.Count);

			float angle = 360 / basicColorList.Count;

			if (!isSetup)
			{
				angle = rotationType == PositiveNegative.Positive ? angle : -angle;
			}

			Vector3 rotateAngle = new Vector3(0, angle, 0);

			if (duration > 0)
			{
				return CoroutineHelper.RotateWithinSeconds(pinwheelTrans, rotateAngle, duration, null, true);
			}
			else
			{
				pinwheelTrans.Rotate(rotateAngle);
				return null;
			}
		}

		/// <summary>
		/// Is the active and end color matching?
		/// </summary>
		/// <returns></returns>
		public bool IsMatchingColor()
		{
			if (ActiveColor == EndColor) return true;
			return false;
		}

#if UNITY_EDITOR
		/// <summary>
		/// Make all the pinwheel colors the same amount.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="value"></param>
		void ListCountChanged(CollectionChangeInfo info, object value)
		{
			var changedBasicColorList = ((List<BasicColor>)value);
			int count = changedBasicColorList.Count;

			foreach (Pinwheel pinwheel in pinwheelPuzzle.PinwheelList)
			{
				if (pinwheel == null) continue;

				var basicColorList = pinwheel.BasicColorList;
				MakeToSameCount(basicColorList, count);
			}

			MakeToSameCount(pinwheelPuzzle.CenterPinwheel.basicColorList, count);
			colorListCount = count - 1;
		}
#endif

		/// <summary>
		/// Update colors to same count.
		/// </summary>
		/// <param name="basicColorList"></param>
		/// <param name="count"></param>
		void MakeToSameCount(List<BasicColor> basicColorList, int count)
		{
			while (basicColorList.Count < count)
			{
				basicColorList.Add(new BasicColor());
			}

			while (basicColorList.Count > count)
			{
				basicColorList.RemoveAt(basicColorList.Count - 1);
			}
		}
	}
}
