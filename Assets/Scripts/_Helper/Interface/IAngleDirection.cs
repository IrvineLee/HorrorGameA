
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
	public interface IAngleDirection
	{
		/// <summary>
		/// Get a list of directions based on the direction and shot angle. Ex: Upwards within 90 degree angle.
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="numberOfShots"></param>
		/// <param name="shotAngle"></param>
		/// <returns></returns>
		static List<Vector2> GetDirectionListBasedOn(Vector2 direction, int numberOfShots, float shotAngle)
		{
			float startAngle = GetStartEndAngle(direction, shotAngle)[0];

			float increment = (shotAngle * Mathf.Deg2Rad) / (numberOfShots - 1);
			List<Vector2> directionList = new List<Vector2>();

			for (int i = 0; i < numberOfShots; i++)
			{
				float newAngle = startAngle + (increment * i);
				float x = Mathf.Sin(newAngle);
				float y = Mathf.Cos(newAngle);

				directionList.Add(new Vector3(x, y));
			}
			return directionList;
		}

		/// <summary>
		/// Get a list of directions around 360 degress.
		/// </summary>
		/// <param name="numberOfShots"></param>
		/// <param name="startAngle"></param>
		/// <returns></returns>
		static List<Vector2> GetDirectionListFor360Degrees(int numberOfShots, float startAngle)
		{
			float increment = ((360 * Mathf.Deg2Rad) / numberOfShots) + startAngle * Mathf.Deg2Rad;
			List<Vector2> directionList = new List<Vector2>();

			for (int i = 0; i < numberOfShots; i++)
			{
				float newAngle = increment * i;
				float x = Mathf.Sin(newAngle);
				float y = Mathf.Cos(newAngle);

				directionList.Add(new Vector3(x, y));
			}
			return directionList;
		}

		/// <summary>
		/// Get a random direction based on inserted direction and the angle. Ex. Upwards within 90 degree angle.
		/// \      /
		///  \    /
		///   \90/
		/// </summary>
		static Vector2 GetRandomDirectionBasedOn(Vector2 direction, float shotAngle)
		{
			List<float> angleList = GetStartEndAngle(direction, shotAngle);

			float random1 = Random.Range(angleList[0], angleList[1]);
			float random2 = Random.Range(angleList[0], angleList[1]);
			return new Vector2(Mathf.Sin(random1), Mathf.Cos(random2));
		}

		/// <summary>
		/// Get the first and last direction.
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="shotAngle"></param>
		/// <returns></returns>
		static List<Vector2> GetFirstAndLastDirectionBasedOn(Vector2 direction, float shotAngle)
		{
			List<float> angleList = GetStartEndAngle(direction, shotAngle);
			List<Vector2> directionList = new List<Vector2>();

			directionList.Add(new Vector2(Mathf.Sin(angleList[0]), Mathf.Cos(angleList[0])));
			directionList.Add(new Vector2(Mathf.Sin(angleList[1]), Mathf.Cos(angleList[1])));

			return directionList;
		}

		/// <summary>
		/// Get the start and end angle. Index 0 is start angle. Index 1 is end angle.
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="shotAngle"></param>
		/// <returns></returns>
		static List<float> GetStartEndAngle(Vector2 direction, float shotAngle)
		{
			float angle = Vector2.Angle(Vector2.up, direction) * Mathf.Deg2Rad;
			if (direction.x < 0) angle = -angle;

			float halfViewAngle = (shotAngle * Mathf.Deg2Rad) / 2;
			float startAngle = angle - halfViewAngle;
			float endAngle = angle + halfViewAngle;

			List<float> angleList = new List<float>();
			angleList.Add(startAngle);
			angleList.Add(endAngle);

			return angleList;
		}
	}
}
