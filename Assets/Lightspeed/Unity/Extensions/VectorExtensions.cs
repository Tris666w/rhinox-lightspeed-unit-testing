﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Rhinox.Lightspeed
{
    public static class VectorExtensions
    {
        public static Vector2 With(this Vector2 vec, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vec.x, y ?? vec.y);
        }

        public static Vector3 With(this Vector3 vec, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vec.x, y ?? vec.y, z ?? vec.z);
        }
        
        public static Vector3 With(this Vector3 vec, float n, Axis axis)
        {
            return new Vector3(
                axis.HasFlag(Axis.X) ? n : vec.x,
                axis.HasFlag(Axis.Y) ? n : vec.y,
                axis.HasFlag(Axis.Z) ? n : vec.z
            );
        }
        
        public static Vector3 With(this Vector3 vec, Vector3 v, Axis axis)
        {
            return new Vector3(
                axis.HasFlag(Axis.X) ? v.x : vec.x,
                axis.HasFlag(Axis.Y) ? v.y : vec.y,
                axis.HasFlag(Axis.Z) ? v.z : vec.z
            );
        }
        
        public static float Get(this Vector3 vec, Axis axis)
        {
            if (axis.HasFlag(Axis.X)) return vec.x;
            if (axis.HasFlag(Axis.Y)) return vec.y;
            if (axis.HasFlag(Axis.Z)) return vec.z;
            throw new InvalidEnumArgumentException($"Invalid Get Value with Axis '{axis}'.");
        }

        public static Vector3 Add(this Vector3 vec, float x = 0f, float y = 0f, float z = 0f)
        {
            return new Vector3(vec.x + x, vec.y + y, vec.z + z);
        }

        public static Vector3 DivideBy(this Vector3 vec, Vector3 other)
        {
            return new Vector3(
                vec.x / other.x,
                vec.y / other.y,
                vec.z / other.z
                );
        }
        
        public static Vector3 MultiplyWith(this Vector3 vec, Vector3 other)
        {
            return new Vector3(
                vec.x * other.x,
                vec.y * other.y,
                vec.z * other.z
            );
        }

        public static Vector3 Flat(this Vector3 vec)
        {
            return vec.With(y: 0);
        }

        public static Vector3 FlatUI(this Vector3 vec)
        {
            return vec.With(z: 0);
        }

        public static Vector3 DirectionTo(this Vector3 p, Vector3 other, bool normalized = true)
        {
            var dir = other - p;
            return normalized ? dir.normalized : dir;
        }

        public static float DistanceTo(this Vector3 p, Vector3 other)
        {
            if (other == p) return 0;
            return (other - p).magnitude;
        }

        public static float SqrDistanceTo(this Vector3 p, Vector3 other)
        {
            // NOTE: Do NOT use when doing math on these distances

            // sqrMagnitude has better performance than magnitude
            // works better for comparing distances
            return (other - p).sqrMagnitude;
        }

        public static bool IsZero(this Vector3 v)
        {
            return v == Vector3.zero;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static Vector3 RotateAround(this Vector3 v, Vector3 pivot, Quaternion rotation)
        {
            var dir = v - pivot; // get point direction relative to pivot
            dir = rotation * dir;
            v = dir + pivot;
            return v;
        }

        public static Vector3 GetClosestTo(this IEnumerable<Vector3> points, Vector3 position,
            ref float sqrSmallestDistance)
        {
            Vector3 closest = Vector3.zero;
            if (points == null) return closest;

            foreach (var o in points)
            {
                var sqrDistance = (position - o).sqrMagnitude;

                if (sqrDistance >= sqrSmallestDistance)
                    continue;

                sqrSmallestDistance = sqrDistance;
                closest = o;
            }

            return closest;
        }

        public static Vector3 GetClosestTo<T>(this IEnumerable<T> pointContainers, Vector3 position,
            ref float sqrSmallestDistance, Func<T, Vector3> pointConverter)
        {
            Vector3 closest = Vector3.zero;
            if (pointContainers == null) return closest;

            foreach (var o in pointContainers)
            {
                Vector3 p = pointConverter(o);
                var sqrDistance = (position - p).sqrMagnitude;

                if (sqrDistance >= sqrSmallestDistance)
                    continue;

                sqrSmallestDistance = sqrDistance;
                closest = p;
            }

            return closest;
        }

        public static Vector3 GetClosestTo(this IEnumerable<Vector3> points, Vector3 position)
        {
            var sqrSmallestDistance = float.MaxValue;

            return points.GetClosestTo(position, ref sqrSmallestDistance);
        }

        public static Vector3 GetClosestTo<T>(this IEnumerable<T> pointContainers, Vector3 position,
            Func<T, Vector3> pointConverter)
        {
            var sqrSmallestDistance = float.MaxValue;

            return pointContainers.GetClosestTo(position, ref sqrSmallestDistance, pointConverter);
        }

        public static bool AnyIsNaN(this Vector3 vec)
        {
            return float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z);
        }
        
        public static bool AnyIsNegativeInfinity(this Vector3 vec)
        {
            return float.IsNegativeInfinity(vec.x) || float.IsNegativeInfinity(vec.y) || float.IsNegativeInfinity(vec.z);
        }
        
        public static bool AnyIsPositiveInfinity(this Vector3 vec)
        {
            return float.IsPositiveInfinity(vec.x) || float.IsPositiveInfinity(vec.y) || float.IsPositiveInfinity(vec.z);
        }
        
        public static bool AnyIsInfinity(this Vector3 vec)
        {
            return float.IsInfinity(vec.x) || float.IsInfinity(vec.y) || float.IsInfinity(vec.z);
        }

        public static string Print(this Vector3 vec)
        {
            return ("(" + vec.x.ToString("0.0#######") + ", " + vec.y.ToString("0.0#######") + ", " +
                    vec.z.ToString("0.0#######") + ")");
        }
        
        public static Vector3 RoundAllValues(this Vector3 vec)
        {
            vec.x = Mathf.Round(vec.x);
            vec.y = Mathf.Round(vec.y);
            vec.z = Mathf.Round(vec.z);

            return vec;
        }
        
        public static Vector3 Clamp(this Vector3 vec, float min, float max)
        {
            vec.x = Mathf.Clamp(vec.x, min, max);
            vec.y = Mathf.Clamp(vec.y, min, max);
            vec.z = Mathf.Clamp(vec.z, min, max);

            return vec;
        }
        
        /// <summary>
        /// Returns the average of the 3 vector components.
        /// </summary>
        public static float Average(this Vector3 vec)
        {
            var total = (vec.x + vec.y + vec.z);
            return total / 3;
        }
        
        public static float Max(this Vector3 vec)
        {
            return Mathf.Max(vec.x, vec.y, vec.z);
        }
        
        public static float Min(this Vector3 vec)
        {
            return Mathf.Min(vec.x, vec.y, vec.z);
        }
        
        /// <summary>
        /// Gets the center of a vector3 collection
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Vector3 GetAverage(this Vector3[] vectors)    
        {
            Vector3 sum = Vector3.zero;
            foreach (var v in vectors)
                sum += v;
            sum = sum / vectors.Length;
            return sum;
        }
        
        /// <summary>
        /// Gets the center of a vector3 collection
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Vector2 GetAverage(this Vector2[] vectors)    
        {
            Vector2 sum = Vector2.zero;
            foreach (var v in vectors)
                sum += v;
            sum = sum / vectors.Length;
            return sum;
        }

        /// <summary>
        /// Checks if two vectors are at (nearly) the same position.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="maxOffset"></param>
        /// <returns></returns>
        public static bool IsNearlySamePoint(this Vector3 v1, Vector3 v2, float maxOffset)
        {
            var maxOffsetSqr = maxOffset * maxOffset;
            var sqrOffset = (v1 - v2).sqrMagnitude;
            return sqrOffset <= maxOffsetSqr;
        }

        /// <summary>
        /// Checks if a collection of Vectors contains a different point at (nearly) the same position.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="check"></param>
        /// <param name="maxOffset"></param>
        /// <returns></returns>
        public static bool ContainsWithOffset(this List<Vector3> collection, Vector3 check, float maxOffset)
        {
            foreach (var vector in collection)
            {
                if(vector.IsNearlySamePoint(check, maxOffset))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes (near-) duplicates of vector3's from a collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="maxOffset"></param>
        /// <returns></returns>
        public static List<Vector3> RemoveDuplicates(this List<Vector3> collection, float maxOffset)
        {
            List<Vector3> newCollection = new List<Vector3>();
            for (int i = 0; i < collection.Count; i++)
            {
                var v = collection[i];
                if(i==0 || !collection.ContainsWithOffset(v, maxOffset))
                {
                    newCollection.Add(v);
                }
            }
            return newCollection;
        }
        
        /// <summary>
        /// Turns an yAngle into a x, z direction, y is always 0.
        /// </summary>
        public static Vector3 ToDirection(this float yAngle)
        {
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * (yAngle)),
                0,
                Mathf.Cos(Mathf.Deg2Rad * (yAngle)));
        }

        /// <summary>
        /// Turns an angle into a x, y direction
        /// </summary>
        public static Vector2 To2DDirection(this float angle)
        {
            return new Vector2(Mathf.Sin(Mathf.Deg2Rad * (angle)),
                Mathf.Cos(Mathf.Deg2Rad * (angle)));
        }

        /// <summary>
        /// returns a clockwise angle, 0 = (0, -1)
        /// </summary>
        public static float ToAngle(this Vector2 direction)
        {
            float angle = 0;
            if (direction.x >= 0)
                angle = Vector2.Angle(Vector2.up, direction);
            else 
                angle = 180 + Vector2.Angle(Vector2.down, direction);
            return angle;
        }

        /// <summary>
        /// returns a clockwise angle, 0 = (0, -1)
        /// Only x and y are used.
        /// </summary>
        public static float ToAngle(this Vector3 direction)
        {
            return new Vector2(direction.x, direction.y).ToAngle();
        }

        /// <summary>
        /// Returns a vec with all positive values
        /// </summary>
        public static Vector3 Abs(this Vector3 vec)
        {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }
        
        public static System.Numerics.Vector2 ToNumericVector(this Vector2 vec)
        {
            return new System.Numerics.Vector2(vec.x, vec.y);
        }

        public static System.Numerics.Vector3 ToNumericVector(this Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.x, vec.y, vec.z);
        }

        public static System.Numerics.Vector4 ToNumericVector(this Vector4 vec)
        {
            return new System.Numerics.Vector4(vec.x, vec.y, vec.z, vec.w);
        }
        
        public static Vector3 Lerp(this Vector3 vec, Vector3 target, float lerpPct)
        {
            Vector3 v = new Vector3();
            v.x = Mathf.Lerp(vec.x, target.x, lerpPct);
            v.y = Mathf.Lerp(vec.y, target.y, lerpPct);
            v.z = Mathf.Lerp(vec.z, target.z, lerpPct);
            return v;
        }
        
        public static float Map(this float value, Vector2 from, Vector2 to)
        {
            return (value - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;
        }
        
        public static bool LossyEquals(this Vector2 val1, Vector2 val2) => LossyEquals(val1, val2, float.Epsilon);
        
        public static bool LossyEquals(this Vector2 val1, Vector2 val2, float epsilon)
        {
            return val1.x.LossyEquals(val2.x, epsilon) &&
                   val1.y.LossyEquals(val2.y, epsilon);

        }
        
        public static bool LossyEquals(this Vector3 val1, Vector3 val2) => LossyEquals(val1, val2, float.Epsilon);
        
        public static bool LossyEquals(this Vector3 val1, Vector3 val2, float epsilon)
        {
            return val1.x.LossyEquals(val2.x, epsilon) &&
                   val1.y.LossyEquals(val2.y, epsilon) &&
                   val1.z.LossyEquals(val2.z, epsilon);

        }
        
        public static bool LossyEquals(this Vector4 val1, Vector4 val2) => LossyEquals(val1, val2, float.Epsilon);
        
        public static bool LossyEquals(this Vector4 val1, Vector4 val2, float epsilon)
        {
            return val1.x.LossyEquals(val2.x, epsilon) &&
                   val1.y.LossyEquals(val2.y, epsilon) &&
                   val1.z.LossyEquals(val2.z, epsilon) &&
                   val1.w.LossyEquals(val2.w, epsilon);

        }
        
        public static bool IsCardinal(this Vector3 normal)
        {
            return TryGetCardinalAxis(normal, out Axis result);
        }

        public static bool TryGetCardinalAxis(this Vector3 axis, out Axis result)
        {
            axis.Normalize();

            if (Vector3.right.IsColinear(axis))
            {
                result = Axis.X;
                return true;
            }
            if (Vector3.up.IsColinear(axis))
            {
                result = Axis.Y;
                return true;
            }
            if (Vector3.forward.IsColinear(axis))
            {
                result = Axis.Z;
                return true;
            }

            result = Axis.None;
            return false;
        }

        public static bool IsColinear(this Vector3 direction, Vector3 otherDirection)
        {
            return (1.0f - Mathf.Abs(Vector3.Dot(direction.normalized, otherDirection.normalized))) < float.Epsilon;
        }

        public static Bounds GetBounds(this IList<Vector3> vertices)
        {
            if (vertices == null || vertices.Count == 0)
                return default(Bounds);
            var b = new Bounds(vertices[0], Vector3.zero);
            for (int i = 1; i < vertices.Count; ++i)
                b.Encapsulate(vertices[i]);
            return b;
        }
    }
}