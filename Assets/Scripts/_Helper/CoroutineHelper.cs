using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Helper
{
	public class CoroutineHelper : CoroutineFundamental
	{
		#region 1. Wait Before Action

		/// <summary>
		/// Wait for seconds before doing action.
		/// </summary>
		public static CoroutineRun WaitFor(float seconds, Action doLast = default, Action<float> callbackMethod = default, bool isRealSeconds = false)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEWaitFor(cr, seconds, callbackMethod, isRealSeconds), doLast);
		}

		/// <summary>
		/// Wait next frame before doing action. 'isRealTime' deals with whether the timescale is 0 or 1.
		/// </summary>
		public static CoroutineRun WaitNextFrame(Action doLast, bool isRealTime = true, bool isWaitNextEndOfFrame = false)
		{
			return new CoroutineRun().Initialize(IEWaitNextFrame(isRealTime, isWaitNextEndOfFrame), doLast);
		}

		/// <summary>
		/// Wait next frame before doing action.
		/// </summary>
		public static CoroutineRun WaitNextFixedFrame(Action doLast)
		{
			return new CoroutineRun().Initialize(IEWaitNextFixedFrame(), doLast);
		}

		/// <summary>
		/// Wait for end of frame before doing action.
		/// </summary>
		public static CoroutineRun WaitEndOfFrame(Action doLast)
		{
			return new CoroutineRun().Initialize(IEWaitEndOfFrame(), doLast);
		}

		/// <summary>
		/// Wait until current animation ends.
		/// </summary>
		public static CoroutineRun WaitUntilCurrentAnimationEnds(Animator animator, Action doLast, bool isWaitEndOfFrame = false)
		{
			CoroutineRun cr = new CoroutineRun();
			if (isWaitEndOfFrame)
			{
				WaitEndOfFrame(() => cr.Initialize(IEWaitUntilCurrentAnimationEnds(cr, animator), doLast));
				return cr;
			}

			return cr.Initialize(IEWaitUntilCurrentAnimationEnds(cr, animator), doLast);
		}

		/// <summary>
		/// Wait until func returns true.
		/// 'timeOutDuration' automatically changes it to true after it expires, hence running the 'doLast' action.
		/// </summary>
		public static CoroutineRun WaitUntilFuncReturnsTrue(Func<bool> checkMethod, Action doLast, float timeOutDuration = Mathf.Infinity)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEWaitUntilFuncReturnsTrue(cr, checkMethod, timeOutDuration), doLast);
		}

		/// <summary>
		/// Wait till the reference bool is true.
		/// </summary>
		//public static CoroutineRun WaitTillTrue(Ref<bool> reference, Action doLast)
		//{
		//	return new CoroutineRun().Initialize(IEWaitTillTrue(reference), doLast);
		//}
		#endregion

		#region 2. Rigidbody Move

		/// <summary>
		/// Uses rigidbody 2D to move.
		/// </summary>
		public static CoroutineRun MoveTo(Rigidbody2D rgbd2D, Vector3 goToPosition, float moveSpeed,
										  Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			Vector3 direction = GetDirection(rgbd2D.transform.position, goToNewPosition);
			Action moveAct = () => RigidbodyMove(rgbd2D.transform.transform, rgbd2D, direction, moveSpeed, isDeltaTime);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, rgbd2D.transform, moveAct, goToNewPosition, () => { }, true), doLast);
		}

		/// <summary>
		/// Uses rigidbody 2D to move. Has a lookat vector.
		/// </summary>
		public static CoroutineRun MoveTo(Rigidbody2D rgbd2D, Vector3 goToPosition, float moveSpeed, Vector3 lookAt,
										  Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			Vector3 direction = GetDirection(rgbd2D.transform.position, goToNewPosition);
			Action moveAct = () => RigidbodyMove(rgbd2D.transform, rgbd2D, direction, moveSpeed, isDeltaTime);
			Action lookAct = () => LookAt2D(rgbd2D.transform, lookAt);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, rgbd2D.transform, moveAct, goToNewPosition, lookAct, true), doLast);
		}

		/// <summary>
		/// Uses rigidbody 3D to move.
		/// </summary>
		public static CoroutineRun MoveTo(Rigidbody rgbd, Vector3 goToPosition, float moveSpeed, Action doLast = default,
										  Vector3 disregardAxis = default, bool isDeltaTime = true)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			Vector3 direction = GetDirection(rgbd.transform.position, goToNewPosition);
			Action moveAct = () => RigidbodyMove(rgbd.transform, rgbd, direction, moveSpeed, isDeltaTime);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, rgbd.transform, moveAct, goToNewPosition, () => { }, true), doLast);
		}

		#endregion

		#region 3. Transform Move

		/// <summary>
		/// Uses transform.Translate to move.
		/// </summary>
		public static CoroutineRun MoveTo(Transform instance, Vector3 goToPosition, float moveSpeed,
									   Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			Vector3 direction = GetDirection(instance.position, goToNewPosition);
			Action moveAct = () => TransformMove(instance, direction, moveSpeed, isDeltaTime, space);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, instance, moveAct, goToNewPosition), doLast);
		}

		/// <summary>
		/// Move a transform to 'goToPosition' within 'duration'.
		/// </summary>
		public static CoroutineRun MoveToWithin(Transform instance, Vector3 goToPosition, float duration, Action doLast = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			float moveSpeed = Vector3.Distance(instance.position, goToPosition) / duration;

			Vector3 direction = GetDirection(instance.position, goToPosition);
			Action moveAct = () => TransformMove(instance, direction, moveSpeed, isDeltaTime, space);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, moveAct, duration, isDeltaTime), doLast);
		}

		/// <summary>
		/// Move a transform in a 'direction' of 'movespeed' for 'duration' seconds.
		/// </summary>
		public static CoroutineRun MoveFor(Transform instance, Vector3 direction, float moveSpeed, float duration, Action doLast = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			Action moveAct = () => TransformMove(instance, direction, moveSpeed, isDeltaTime, space);

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEMove(cr, moveAct, duration, isDeltaTime), doLast);
		}

		#endregion

		#region 4. Fade

		/// <summary>
		/// This changes 'fromAlpha' to 'toAlpha'.
		/// T includes SpriteRenderer, Image, Renderer, TextMeshProUGUI and Text.
		/// </summary>
		public static CoroutineRun FadeFromTo<T>(T component, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			return Fade(component, fromAlpha, toAlpha, duration, doLast, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's current material alpha to 'toAlpha' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun FadeToUsingShader(Renderer rend, float toAlpha, float duration, Action doLast = default, string propertyName = "_Color", bool isDeltaTime = true)
		{
			return FadeUsingShader(rend, propertyName, rend.material.color.a, toAlpha, duration, doLast, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's alpha from 'fromAlpha' to 'toAlpha' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun FadeFromToUsingShader(Renderer rend, float fromAlpha, float toAlpha, float duration, Action doLast = default, string propertyName = "_Color", bool isDeltaTime = true)
		{
			return FadeUsingShader(rend, propertyName, fromAlpha, toAlpha, duration, doLast, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's current material alpha to 'toAlpha' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun FadeToUsingURP(Renderer rend, float toAlpha, float duration, Action doLast = default, string propertyName = "_BaseColor", bool isDeltaTime = true)
		{
			return FadeUsingURP(rend, propertyName, rend.material.color.a, toAlpha, duration, doLast, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's alpha from 'fromAlpha' to 'toAlpha' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun FadeFromToUsingURP(Renderer rend, float fromAlpha, float toAlpha, float duration, Action doLast = default, string propertyName = "_BaseColor", bool isDeltaTime = true)
		{
			return FadeUsingURP(rend, propertyName, fromAlpha, toAlpha, duration, doLast, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's color to 'toColor' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun ColorToUsingURP(Renderer rend, Color toColor, float duration, Action doLast = default,
												   int materialIndex = 0, string propertyName = "_BaseColor", bool isDeltaTime = true)
		{
			return ColorChangeUsingURP(rend, propertyName, rend.material.color, toColor, duration, doLast, materialIndex, isDeltaTime);
		}

		/// <summary>
		/// This changes the Renderer's color from 'fromColor' to 'toColor' using the 'propertyName' in the shader.
		/// </summary>
		public static CoroutineRun ColorToUsingURP(Renderer rend, Color fromColor, Color toColor, float duration, Action doLast = default, string propertyName = "_BaseColor", bool isDeltaTime = true)
		{
			return ColorChangeUsingURP(rend, propertyName, fromColor, toColor, duration, doLast, 0, isDeltaTime);
		}

		#endregion

		#region 5. Pop Up

		/// <summary>
		/// Pop up a TextMeshProUGUI in the specified 'direction' for 'distance' within 'duration'.
		/// Wait for 'waitBeforeFadeOut' before fading within 'fadeOutTime'.
		/// </summary>
		public static CoroutineRun PopUPDisappear<T>(T ui, Vector3 direction, float distance,
										  float duration, float waitBeforeFadeOut, float fadeOutTime, float scaleMultiplier = 1, bool isDeltaTime = true) where T : Component
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEPopUPDisappear<T>(cr, ui.transform, direction, distance, duration, scaleMultiplier, waitBeforeFadeOut, fadeOutTime, isDeltaTime));
		}

		/// <summary>
		/// Pop up a TextMeshProUGUI in the specified 'direction' within 'duration' with a specified 'speed'.
		/// Fade ratio goes between 0~1 where 0.25f means the fade in/fade out will last for 0.25f * duration each.
		/// This means the TMP is solid alpha 1 for 0.5f * duration in-between.
		/// </summary>
		public static CoroutineRun PopUPDisappear_KeepMoving<T>(T ui, Vector3 direction, float duration, float speed, float fadeRatio,
																float scaleMultiplier = 1, bool isDeltaTime = true) where T : Component
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEPopUPDisappear_KeepMoving<T>(cr, ui.transform, direction, duration, speed, fadeRatio, scaleMultiplier, isDeltaTime));
		}

		#endregion

		#region 6. Rotate

		/// <summary>
		/// Perform rotation of target by rotateAmount within the specified time.
		/// </summary>
		public static CoroutineRun RotateWithinSeconds(Transform target, Vector3 rotateAmount, float duration, Action doLast = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IERotateWithinSeconds(cr, target, rotateAmount, duration, isDeltaTime, space), doLast);
		}

		#endregion

		#region 7. Lerp

		/// <summary>
		/// Lerp a transform from 'fromPosition' to 'goToPosition' in 'duration' seconds.
		/// </summary>
		public static CoroutineRun LerpFromTo(Transform instance, Vector3 fromPosition, Vector3 goToPosition, float duration,
										Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			Vector3 fromNewPosition = disregardAxis == default ? fromPosition : DisregardAxisNewPosition(fromPosition, disregardAxis);
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			if (space == Space.World) instance.position = fromNewPosition;
			else instance.localPosition = fromNewPosition;

			return LerpTo(instance, goToNewPosition, duration, doLast, default, isDeltaTime, space);
		}

		/// <summary>
		/// Lerp a transform to 'goToPosition' in 'duration' seconds.
		/// </summary>
		public static CoroutineRun LerpTo(Transform instance, Vector3 goToPosition, float duration,
										Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToPosition : DisregardAxisNewPosition(goToPosition, disregardAxis);

			Action<Vector3> callbackMethod = (result) =>
			{
				if (space == Space.World) instance.position = result;
				else instance.localPosition = result;
			};

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, space == Space.World ? instance.position : instance.localPosition, goToNewPosition,
												duration, callbackMethod, default, isDeltaTime), doLast);
		}

		/// <summary>
		/// Lerp a transform to 'goToPosition' in 'duration' seconds.
		/// </summary>
		public static CoroutineRun LerpTo(Transform instance, Transform goToInstance, float duration,
										Action doLast = default, Vector3 disregardAxis = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			Vector3 goToNewPosition = disregardAxis == default ? goToInstance.position : DisregardAxisNewPosition(goToInstance.position, disregardAxis);

			Action<Vector3> callbackMethod = (result) =>
			{
				if (space == Space.World) instance.position = result;
				else instance.localPosition = result;
			};

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, instance, goToInstance, duration, callbackMethod, default, isDeltaTime), doLast);
		}

		/// <summary>
		/// Lerp from startValue to endValue within the specified time. You will need to use the callback method to set the current lerp value back to the intended object.
		/// Ex. Action<float> callbackMethod = (result) => { cam.fieldOfView = result; };
		/// </summary>
		public static CoroutineRun LerpWithinSeconds<T>(T startValue, T endValue, float duration, Action<T> callbackMethod,
												  Action doLast = default, Func<bool> breakMethod = default, bool isDeltaTime = true)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, startValue, endValue, duration, callbackMethod, breakMethod, isDeltaTime), doLast);
		}

		/// <summary>
		/// Lerp from startValue to endValue within the specified speed. You will need to use the callback method to set the current lerp value back to the intended object.
		/// Ex. Action<float> callbackMethod = (result) => { cam.fieldOfView = result; };
		/// </summary>
		public static CoroutineRun LerpWithSpeed(float startValue, float endValue, float speed, Action<float> callbackMethod,
												  Action doLast = default, Func<bool> breakMethod = default, bool isDeltaTime = true)
		{
			float duration = endValue - startValue;
			if (startValue > endValue) duration = startValue - endValue;
			duration /= speed;

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, startValue, endValue, duration, callbackMethod, breakMethod, isDeltaTime), doLast);
		}

		/// <summary>
		/// Lerp from startValue to endValue within the specified speed. You will need to use the callback method to set the current lerp value back to the intended object.
		/// Ex. Action<float> callbackMethod = (result) => { cam.fieldOfView = result; };
		/// </summary>
		public static CoroutineRun LerpWithSpeed(int startValue, int endValue, float speed, Action<int> callbackMethod,
												  Action doLast = default, Func<bool> breakMethod = default, bool isDeltaTime = true)
		{
			float duration = endValue - startValue;
			if (startValue > endValue) duration = startValue - endValue;
			duration /= speed;

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, startValue, endValue, duration, callbackMethod, breakMethod, isDeltaTime), doLast);
		}

		/// <summary>
		/// Quaternion Lerp from startValue to endValue within the specified time. You will need to use the callback method to set the current lerp value back to the intended object.
		/// Ex. Action<Quaternion> callbackMethod = (result) => { transform.rotation = result; };
		/// </summary>
		public static CoroutineRun QuaternionLerpWithinSeconds(Transform instance, Quaternion startValue, Quaternion endValue, float duration,
												  Action doLast = default, Func<bool> breakMethod = default, bool isDeltaTime = true, Space space = Space.Self)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEQuaternionLerpWithinSeconds(cr, instance, startValue, endValue, duration, breakMethod, isDeltaTime, space), doLast);
		}
		#endregion

		#region 8. Run

		/// <summary>
		/// Run 'action' every 'runEvery' until the 'breakMethod' returns true.
		/// Putting in 0 for 'runEvery' will run every frame.
		/// Break metbod gets checked every frame.
		/// </summary>
		public static CoroutineRun RunActionUntilBreak(float runEvery, Action action, Func<bool> breakMethod, bool isRunAtStart = false,
													   bool isRealSeconds = false, bool isFixedUpdate = false, float timeOutDuration = Mathf.Infinity, Action doLast = null)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IERunActionUntilBreak(cr, runEvery, action, breakMethod, isRunAtStart, isRealSeconds, isFixedUpdate, timeOutDuration), doLast);
		}

		/// <summary>
		/// Run 'action' 'totalAction' times with 'delay' seconds.
		/// </summary>
		public static CoroutineRun RunActionAndDelayLoop(Action action, int totalAction, float delay, Action doLast = null)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEDelayActionFrequency(cr, action, totalAction, delay), doLast);
		}

		/// <summary>
		/// Run 'action' continuously with 'delay' seconds in between, breaking out after 'timeOutDuration'.
		/// </summary>
		public static CoroutineRun RunActionAndDelayLoop(Action action, float delay, float timeOutDuration, Action doLast = null)
		{
			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEDelayActionTimeOut(cr, action, delay, timeOutDuration), doLast);
		}
		#endregion

		public static void StopAllCoroutine()
		{
			HelperObj.Instance?.StopAllCoroutines();
		}
	}
}