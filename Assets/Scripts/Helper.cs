using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float RoundToNearest(float value, float rounding)
    {
        return Mathf.Round(value / rounding) * rounding;
    }

    public static Vector3 RoundToNearest(Vector3 value, float rounding)
    {
        return new Vector3(RoundToNearest(value.x, rounding), RoundToNearest(value.y, rounding), RoundToNearest(value.z, rounding));
    }

    public static float RoundOut(float value)
    {
        if(value <= 0f)
        {
            return Mathf.Floor(value);
        } else
        {
            return Mathf.Ceil(value);
        }
    }

    public static Vector3 RoundOut(Vector3 value)
    {
        return new Vector3(RoundOut(value.x), RoundOut(value.y), RoundOut(value.z));
    }

    //https://stackoverflow.com/questions/51905268/how-to-find-closest-point-on-line
    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;

        float dotP = Vector3.Dot(lhs, direction);
        return origin + direction * dotP;
    }

    public static Vector3 FindNearestPointOnLine(Ray ray, Vector3 point) => FindNearestPointOnLine(ray.origin, ray.direction, point);

    public static Vector2 Vector2FromPolarCoordinates(float angle, float distance)
    {
        float rads = Mathf.Deg2Rad * angle;

        return new Vector2(Mathf.Cos(rads) * distance, Mathf.Sin(rads) * distance);
    }
}
