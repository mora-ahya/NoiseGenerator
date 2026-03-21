using UnityEngine;

[CreateAssetMenu(fileName = "ValueNoise", menuName = "ScriptableObjects/Noise/ValueNoise")]
public class ValueNoise : NoiseBase
{
    public override float Noise(Vector2 uv)
    {
        uv *= 8.0f;
        Vector2 i_uv = Floor(uv);
        Vector2 f_uv = Fract(uv);
        float f1 = Random(i_uv + Vector2.zero);
        float f2 = Random(i_uv + Vector2.right);
        float f3 = Random(i_uv + Vector2.up);
        float f4 = Random(i_uv + Vector2.one);

        float interX = Interpolation(f_uv.x);
        float interY = Interpolation(f_uv.y);

        float o = Mix(Mix(f1, f2, interX), Mix(f3, f4, interX), interY);
        return o;
    }
}
