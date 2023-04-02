using Cysharp.Threading.Tasks;
using Helper;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzzle.Pinwheel
{
	public class PinwheelPuzzle : MonoBehaviour, IPuzzle
	{
		[ChildGameObjectsOnly]
		[SerializeField] Transform outerPinwheelTrans = null;
		[ChildGameObjectsOnly]
		[SerializeField] Transform centerPinwheelAt = null;

		[Space]
		[AssetsOnly]
		[SerializeField] Transform pinwheelPrefab = null;
		[AssetsOnly]
		[SerializeField] Transform rotateOffsetPrefab = null;
		[SceneObjectsOnly]
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
		CoroutineRun slideCR = new CoroutineRun();

		void Start()
		{
			defaultTurnRemain = turnRemain;

			InitializeEndColor();
			SetTurnRemainTMP(turnRemain);
		}

		void Update()
		{
			// Check puzzle click.
			if (Input.GetMouseButtonDown(0) && slideCR.IsDone)
			{
				RaycastHit hit;

				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);

				if (Physics.Raycast(ray, out hit))
				{
					((IPuzzle)this).ClickedInteractable(hit.transform);
					((IPuzzle)this).CheckPuzzleAnswer();
				}
			}
		}

		/// <summary>
		/// Set the end color for pinwheel.
		/// </summary>
		void InitializeEndColor()
		{
			if (pinwheelList.Count <= 0) return;

			int count = centerTurnType == PositiveNegative.Positive ? pinwheelList.Count - turnRemain : pinwheelList.Count + turnRemain;
			int index = count.WithinCount(pinwheelList.Count, false);

			centerPinwheel.Initialize();
			for (int i = 0; i < centerPinwheel.BasicColorList.Count; i++)
			{
				int outerIndex = (index + i).WithinCount(pinwheelList.Count, false);
				Pinwheel pinwheel = pinwheelList[outerIndex];

				pinwheel.Initialize();
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
				if (pinwheel.PinwheelTrans.Equals(trans))
				{
					if (pinwheel.IsCenterPinwheel)
						continue;

					slideCR = pinwheel.Turn(rotateDuration);
					centerPinwheel.Turn(rotateDuration);
					SetTurnRemainTMP(--turnRemain);
					break;
				}
			}
		}

		/// <summary>
		/// Check puzzle answer.
		/// </summary>
		async void IPuzzle.CheckPuzzleAnswer()
		{
			await UniTask.WaitUntil(() => slideCR.IsDone);

			if (turnRemain > 0) return;

			foreach (var pinwheel in pinwheelList)
			{
				if (!pinwheel.IsMatchingColor())
				{
					Debug.Log("You failed! Try again.");
					ResetPuzzle();
					return;
				}
			}

			Debug.Log("YOU WIN!");
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
			Transform instance = Instantiate(pinwheelPrefab, outerPinwheelTrans);
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

		void OnValidate()
		{
			InitializeEndColor();

			centerPinwheel.SetRotationType(centerTurnType);

			foreach (var pinwheel in pinwheelList)
			{
				pinwheel.SetPinwheel(this, pinwheel.PinwheelTrans, false);
				pinwheel.SetRotationType(centerTurnType == 0 ? PositiveNegative.Negative : PositiveNegative.Positive);
			}
		}
	}
}
