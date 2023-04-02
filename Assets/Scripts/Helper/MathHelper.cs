using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public class MathHelper : MonoBehaviour
	{
		public static float Round(float value, int digits)
		{
			float mult = Mathf.Pow(10.0f, (float)digits);
			return Mathf.Round(value * mult) / mult;
		}

		/// <summary>
		/// Clamps any angle to a value between 0 and 360 : -90 = 270
		/// </summary>
		public static float ClampAngle(float _Angle)
		{
			float ReturnAngle = _Angle;

			if (_Angle < 0f)
				ReturnAngle = (_Angle + (360f * ((_Angle / 360f) + 1)));

			else if (_Angle > 360f)
				ReturnAngle = (_Angle - (360f * (_Angle / 360f)));

			else if (ReturnAngle == 360) //Never use 360, only go from 0 to 359
				ReturnAngle = 0;

			return ReturnAngle;
		}

		/// <summary>
		/// Gives a random vector between min and max.
		/// </summary>
		public static Vector3 RandomVector(Vector3 min, Vector3 max)
		{
			return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
		}

		/// <summary>
		/// Gives a random euler rotation.
		/// </summary>
		public static Vector3 RandomRotation()
		{
			return new Vector3(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
		}

		/// <summary>
		/// Gives a random color.
		/// </summary>
		public static Color RandomColor()
		{
			return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		}

		/// <summary>
		/// Gives the absolute value of vect.
		/// </summary>
		public static Vector3 Abs(Vector3 vect)
		{
			return new Vector3(Mathf.Abs(vect.x), Mathf.Abs(vect.y), Mathf.Abs(vect.z));
		}

		/// <summary>
		/// Takes in a value angle of more than 359 degrees and turn it into value below 359 degrees.
		/// Ex. angle = 555, returns 196 degrees. angle = -90, returns 269 degrees.
		/// </summary>
		public static float Get360Angle(float angle)
		{
			if (angle < 0)
			{
				angle %= 359;
				angle = 359 - Mathf.Abs(angle);
			}

			return angle % 359;
		}

		/// <summary>
		/// Gives you the rotated angle from 'prevDegree' to 'currentDegree'.
		/// This solves the angle calculation of going over a complete circle.
		/// Ex. prevDegree = 270, currentDegree = 20, will give you a value of 110.
		/// </summary>
		public static float RotatedAngle(bool isClockwise, float prevDegree, float currentDegree)
		{
			float val = currentDegree - prevDegree;

			if (isClockwise)
			{
				if (currentDegree < prevDegree)
					val = 359 + currentDegree - prevDegree;
			}
			else
			{
				val = prevDegree - currentDegree;

				if (currentDegree > prevDegree)
					val = prevDegree + 359 - currentDegree;
			}

			return val;
		}

		public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
		{
			Vector3 c = current.eulerAngles;
			Vector3 t = target.eulerAngles;
			return Quaternion.Euler(
			  Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
			  Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
			  Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
			);
		}

		public static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime)
		{
			return new Vector3(
			  Mathf.SmoothDampAngle(current.x, target.x, ref currentVelocity.x, smoothTime),
			  Mathf.SmoothDampAngle(current.y, target.y, ref currentVelocity.y, smoothTime),
			  Mathf.SmoothDampAngle(current.z, target.z, ref currentVelocity.z, smoothTime)
			);
		}
	}
}
