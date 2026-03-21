using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBase : ScriptableObject
{
    public virtual float Noise(Vector2 uv)
    {
        return 1.0f;
    }

    protected static float Random(Vector2 v)
    {
        return HashWithoutSine.Hash12(v);
    }

    protected static Vector2 Random2(Vector2 v)
    {
        return HashWithoutSine.Hash22(v);
    }

    protected static float Dot(Vector2 v1, Vector2 v2)
    {
        return Vector2.Dot(v1, v2);
    }

    protected static float Dot(Vector3 v1, Vector3 v2)
    {
        return Vector3.Dot(v1, v2);
    }

    protected static float Sin(float f)
    {
        return Mathf.Sin(f);
    }

    protected static float Floor(float f)
    {
        return Mathf.Floor(f);
    }

    protected static Vector2 Floor(Vector2 p)
    {
        return new Vector2(Floor(p.x), Floor(p.y));
    }

    protected static Vector3 Floor(Vector3 p)
    {
        return new Vector3(Floor(p.x), Floor(p.y), Floor(p.z));
    }

    protected static float Fract(float f)
    {
        return f - Floor(f);
    }

    protected static Vector2 Fract(Vector2 p)
    {
        return p - Floor(p);
    }

    protected static Vector3 Fract(Vector3 p)
    {
        return p - Floor(p);
    }

    protected static Vector3 AddFToVec3(Vector3 v, float f)
    {
        Vector3 v1 = Vector3.one * f;
        return v + v1;
    }

    protected static Vector3 CrossVec3Vec3(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    protected static float Interpolation(float f)
    {
        return f * f * f * (f * (6.0f * f - 15.0f) + 10.0f);
    }

    protected static float Mix(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }


    protected class HashWithoutSine
    {
        // Based on "Hash without Sine" by David Hoskins
        // MIT License


        public static float Hash12(Vector2 p)
        {
            Vector3 p3 = Fract(new Vector3(p.x, p.y, p.x) * 0.1031f);
            float f = Dot(p3, AddFToVec3(new Vector3(p3.y, p3.z, p3.x), 33.33f));
            p3 = AddFToVec3(p3, f);
            return Fract((p3.x + p3.y) * p3.z);
        }

        public static Vector2 Hash22(Vector2 p)
        {
            Vector3 p3 = CrossVec3Vec3(new Vector3(p.x, p.y, p.x), new Vector3(0.1031f, 0.1030f, 0.0973f));
            p3 = Fract(p3);
            float f = Dot(p3, AddFToVec3(new Vector3(p3.y, p3.z, p3.x), 33.33f));
            p3 = AddFToVec3(p3, f);
            return Fract((new Vector2(p3.x, p3.x) + new Vector2(p3.y, p3.z)) * new Vector2(p3.z, p3.y));
        }
    }
}
