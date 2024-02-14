using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using TMPro;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.InputProcessing;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Personal.Puzzle.Pinwheel
{
	public class PinwheelPuzzle : PuzzleController, IPuzzle
	{
		[Space()]
		[ChildGameObjectsOnly]
		[SerializeField] Transform outerPinwheelTrans = null;
		[ChildGameObjectsOnly]
		[SerializeField] Transform centerPinwheelAt = null;

		[Space]
		[AssetsOnly]
		[SerializeField] Transform pinwheelPrefab = null;
		[AssetsOnly]
		[SerializeField] Transform centerPinwheelPrefab = null;
		[AssetsOnly]
		[SerializeField] Transform rotateOffsetPrefab = null;
		[ChildGameObjectsOnly]
		[SerializeField] CanvasGroupFade canvasGroupFade = null;
		[SerializeField] TextMeshProUGUI turnRemainTMP = null;

		[Space]
		[ChildGameObjectsOnly]
		[SerializeField] Transform debugDisplay = null;
		[AssetsOnly]
		[SerializeField] Transform debugDisplayTMP = null;

		[Space]
		[SerializeField] float outerPinwheelRotation = 0f;
		[SerializeField] PositiveNegative centerTurnType = PositiveNegative.Positive;
		[SerializeField] float rotateDuration = 0.5f;
		[SerializeField] int turnRemain = 3;

		[Space]
		[DetailedInfoBox("PinwheelInfo", "Active Color of Center Pinwheel always points towards the first pinwheel in the list")]
		[SerializeField] Pinwheel centerPinwheel = new();
		[SerializeField] List<Pinwheel> pinwheelList = new();

		[SerializeField] [HideInInspector] bool isDisplayDebug;

		public Pinwheel CenterPinwheel { get => centerPinwheel; }
		public List<Pinwheel> PinwheelList { get => pinwheelList; }

		int defaultTurnRemain;
		List<Collider> colliderList = new();


		protected override void Initialize()
		{
			base.Initialize();
			if (puzzleState == PuzzleState.Completed) return;

			InitialPinwheelSetup();
			EnableHitCollider(false);
		}

		public override Transform GetActiveSelectionForGamepad()
		{
			int index = ((ControlInput)ControlInputBase.ActiveControlInput).CurrentActiveIndex;
			return pinwheelList[index].PinwheelTrans;
		}

		public List<Transform> GetInteractableObjectList()
		{
			return PinwheelList.ConvertAll(x => x.PinwheelTrans);
		}

		/// <summary>
		/// Initialize the pinwheels.
		/// </summary>
		void InitialPinwheelSetup()
		{
			defaultTurnRemain = turnRemain;

			centerPinwheel.InitializeRotation();
			foreach (var pinwheel in pinwheelList)
			{
				pinwheel.InitializeRotation();
			}

			InitializeEndColor();
			SetTurnRemainTMP(turnRemain);

			foreach (Transform child in outerPinwheelTrans)
			{
				var collider = child.GetComponentInChildren<Collider>();
				if (collider) colliderList.Add(collider);
			}
		}

		/// <summary>
		/// Set the end color for pinwheel.
		/// </summary>
		void InitializeEndColor()
		{
			if (pinwheelList.Count <= 0) return;

			int count = centerTurnType == PositiveNegative.Positive ? pinwheelList.Count - turnRemain : pinwheelList.Count + turnRemain;
			int index = count.WithinCountLoopOver(pinwheelList.Count, true);

			for (int i = 0; i < centerPinwheel.BasicColorList.Count; i++)
			{
				int outerIndex = (index + i).WithinCountLoopOver(pinwheelList.Count, true);
				Pinwheel pinwheel = pinwheelList[outerIndex];

				pinwheel.SetEndColor(centerPinwheel.BasicColorList[i]);
				//Debug.Log("Pinwheel " + pinwheel.PinwheelTrans.name + " color : " + centerPinwheel.BasicColorList[i]);
			}
		}

		void SetTurnRemainTMP(int value)
		{
			turnRemainTMP.text = "Turns: " + (value).ToString();
		}

		/// <summary>
		/// Handle the clicked tile.
		/// </summary>
		/// <param name="trans"></param>
		void IPuzzle.ClickedInteractable(Transform trans)
		{
			foreach (var pinwheel in pinwheelList)
			{
				if (!pinwheel.PinwheelTrans.Equals(trans)) continue;
				if (pinwheel.IsCenterPinwheel) continue;

				slideCR = pinwheel.Turn(rotateDuration);
				centerPinwheel.Turn(rotateDuration);
				SetTurnRemainTMP(--turnRemain);
				break;
			}
		}

		/// <summary>
		/// Cancelled the puzzle.
		/// </summary>
		void IPuzzle.CancelSelected()
		{
			puzzleState = PuzzleState.Failed;
		}

		/// <summary>
		/// Check puzzle answer.
		/// </summary>
		async void IPuzzle.CheckPuzzleAnswer()
		{
			await UniTask.WaitUntil(() => slideCR.IsDone, cancellationToken: this.GetCancellationTokenOnDestroy());

			if (turnRemain > 0) return;

			foreach (var pinwheel in pinwheelList)
			{
				if (pinwheel.IsMatchingColor()) continue;

				puzzleState = PuzzleState.Failed;

				Debug.Log("You failed! Try again.");
				return;
			}

			EndAndGetReward();
		}

		/// <summary>
		/// Complete the puzzle.
		/// This assumes that pinwheel with only 1 endColor doesn't need to rotate again after reaching it's own endColor.
		/// In cases where turnRemain does not reach 0, it will try to rotate 1 pinwheel at a time to reach 0 and checks the color.
		/// This will not do all the possible pinwheel rotation to reach 0. If it does not reach, it will just choose the nearest color.
		/// </summary>
		void IPuzzle.AutoComplete()
		{
			for (int i = 0; i < turnRemain; i++)
			{
				centerPinwheel.Turn();
			}

			List<Pinwheel> similarColorPinwheelList = new();
			foreach (var pinwheel in pinwheelList)
			{
				if (pinwheel.HasDuplicateEndColor()) similarColorPinwheelList.Add(pinwheel);
				while (!pinwheel.IsMatchingColor())
				{
					pinwheel.Turn();
					turnRemain--;
				}
			}

			if (turnRemain <= 0 || TryRotateOnePinwheelAtATime(similarColorPinwheelList))
			{
				EndAndGetReward();
				return;
			}

			// You will need to do combination calculation for any other possible pinwheel rotation. (Order does not matter)
		}

		/// <summary>
		/// Rotate one pinwheel at a time and see whether it can reach turnRemain 0.
		/// </summary>
		/// <param name="similarColorPinwheelList"></param>
		/// <returns></returns>
		bool TryRotateOnePinwheelAtATime(List<Pinwheel> similarColorPinwheelList)
		{
			int turnRemainTemp = turnRemain;
			foreach (var pinwheel in similarColorPinwheelList)
			{
				int index = pinwheel.ActiveIndex;
				while (turnRemain > 0)
				{
					pinwheel.Turn();
					turnRemain--;
				}

				// If the pinwheel is not the end color, this pinwheel turn is wrong. Reset and proceed to the next one.
				if (!pinwheel.IsMatchingColor())
				{
					turnRemain = turnRemainTemp;
					pinwheel.RotateUntilIndex(index);

					continue;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Reset to default.
		/// </summary>
		void IPuzzle.ResetToDefault()
		{
			ResetPuzzle();
		}

		protected override void OnBegin(bool isFlag)
		{
			base.OnBegin(isFlag);
			EnableHitCollider(isFlag);

			if (isFlag)
			{
				puzzleState = PuzzleState.None;
				canvasGroupFade.BeginFadeIn();
				return;
			}

			canvasGroupFade.BeginFadeOut(() =>
			{
				if (puzzleState == PuzzleState.Completed) return;
				ResetPuzzle();
			});
		}

		/// <summary>
		/// Reset the puzzle to default.
		/// </summary>
		void ResetPuzzle()
		{
			centerPinwheel.Reset();
			foreach (var pinwheel in pinwheelList)
			{
				pinwheel.Reset();
			}

			turnRemain = defaultTurnRemain;
			SetTurnRemainTMP(turnRemain);
		}

		void EnableHitCollider(bool isFlag)
		{
			foreach (var collider in colliderList)
			{
				collider.enabled = isFlag;
			}
		}

		/// <summary>
		/// Initialize pinwheel with colors attached to it.
		/// </summary>
		[Button("Initialize Pinwheel", Icon = SdfIconType.Tools, IconAlignment = IconAlignment.RightEdge, ButtonAlignment = 1f)]
		void InitializePinwheel()
		{
			// Reset values.
			outerPinwheelTrans.rotation = Quaternion.identity;
			outerPinwheelTrans.DestroyAllChildren();

			for (int i = 0; i < pinwheelList.Count; i++)
			{
				PositiveNegative turnRotation = centerTurnType == PositiveNegative.Positive ? PositiveNegative.Negative : PositiveNegative.Positive;
				SpawnPinwheel(i, pinwheelPrefab.name + "_" + i.ToString(), pinwheelList[i], turnRotation, false);
			}

			// Center pinwheel.
			Transform instance = SpawnPinwheel(0, pinwheelPrefab.name + "_Center", centerPinwheel, centerTurnType, true);
			instance.position = centerPinwheelAt.position;

			outerPinwheelTrans.rotation = Quaternion.Euler(new Vector3(0, 0, -outerPinwheelRotation));

			SpawnDebugDisplay();
		}

		/// <summary>
		/// Spawn pinwheels.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="name"></param>
		/// <param name="pinwheel"></param>
		/// <param name="turnRotation"></param>
		/// <param name="isCenterPinwheel"></param>
		/// <returns></returns>
		Transform SpawnPinwheel(int index, string name, Pinwheel pinwheel, PositiveNegative turnRotation, bool isCenterPinwheel)
		{
			float angle = index * Mathf.PI * 2f / pinwheelList.Count;
			if (pinwheelList.Count <= 0) angle = 0;

			// Instantiate pinwheel.
			Transform instance = Instantiate(isCenterPinwheel ? centerPinwheelPrefab : pinwheelPrefab, outerPinwheelTrans);
			instance.name = name;
			instance.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle) + 1f, outerPinwheelTrans.position.z);
			instance.rotation = Quaternion.Euler(new Vector3(0, 90, 90));

			// Set instantiated transform.
			pinwheel.SetPinwheel(this, instance, isCenterPinwheel);
			pinwheel.SetRotationType(turnRotation);

			SpawnColors(pinwheel);
			return instance;
		}

		/// <summary>
		/// Spawn the color associated with pinwheel.
		/// </summary>
		/// <param name="pinwheel"></param>
		void SpawnColors(Pinwheel pinwheel)
		{
			// Instantiate the colors and rotate it always facing the center.
			for (int i = 0; i < pinwheel.BasicColorList.Count; i++)
			{
				BasicColor basicColor = pinwheel.BasicColorList[i];
				Transform instance = pinwheel.PinwheelTrans;

				// Instantiate the colors.
				var colorInstance = Instantiate(rotateOffsetPrefab, instance.position, rotateOffsetPrefab.rotation);
				colorInstance.name += "_" + basicColor.ToString();
				colorInstance.SetParent(instance);

				colorInstance.localPosition = colorInstance.localPosition.With(y: -1.01f);
				colorInstance.GetComponentInChildren<SpriteRenderer>().color = basicColor.GetColor();

				// Only face to rotate center if outer pinwheel.
				if (!pinwheel.IsCenterPinwheel)
					colorInstance.up = centerPinwheelAt.position - instance.position;

				float angle = i * 360 / pinwheel.BasicColorList.Count;

				// To make sure the rotation of the spawned colors are assigned to the correct order, 0~360, left to right.
				if (colorInstance.up.y >= 0 || colorInstance.up.y.FloatPointFix() == 0)
					angle = -angle;

				colorInstance.rotation = Quaternion.Euler(colorInstance.rotation.eulerAngles.With(z: angle + colorInstance.rotation.eulerAngles.z));
			}
			pinwheel.Setup();
		}

		/// <summary>
		/// Enable/Disable index display for pinwheel.
		/// </summary>
		[Button("Debug Display", Icon = SdfIconType.Tools, IconAlignment = IconAlignment.RightEdge, ButtonAlignment = 1f)]
		void DebugDisplay()
		{
			isDisplayDebug = !isDisplayDebug;
			debugDisplay.gameObject.SetActive(isDisplayDebug);
		}

		/// <summary>
		/// Spawn the debug display.
		/// </summary>
		void SpawnDebugDisplay()
		{
			debugDisplay.DestroyAllChildren();
			for (int i = 0; i < pinwheelList.Count; i++)
			{
				Pinwheel pinwheel = pinwheelList[i];

				var tmp = Instantiate(debugDisplayTMP, debugDisplay);
				tmp.position = pinwheel.PinwheelTrans.position;
				tmp.localPosition += debugDisplayTMP.localPosition;
				tmp.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
			}
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode) return;

			InitializeEndColor();

			// Swap the rotation type.
			centerPinwheel.SetRotationType(centerTurnType);
			foreach (var pinwheel in pinwheelList)
			{
				pinwheel.SetRotationType(centerTurnType == 0 ? PositiveNegative.Negative : PositiveNegative.Positive);
			}
		}
#endif
	}
}
