using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EaseValue : MonoBehaviour
{
    delegate float ease(float time);
    
    /// <summary>
    /// 変化量パターン
    /// （平坦）Linear - Sin - Circ - Quad - Cubic - Quarantic - Quintic - Exp（急加速急減速）
    /// Back、Elastic、Bounce は遊びあり
    /// </summary>
    public enum eEase
    {
        /// <summary>指定なし</summary>
        None,
        /// <summary>線形補完</summary>
        Linear,
        /// <summary>3x 加速</summary>
        CubicIn,
        /// <summary>3x 減速</summary>
        CubicOut,
        /// <summary>3x 加速＆減速</summary>
        CubicInout,
        /// <summary>2x 加速</summary>
        QuadraticIn,
        /// <summary>2x 減速</summary>
        QuadraticOut,
        /// <summary>2x 加速＆減速</summary>
        QuadraticInout,
        /// <summary>4x 加速</summary>
        QuaranticIn,
        /// <summary>4x 減速</summary>
        QuaranticOut,
        /// <summary>4x 加速＆減速</summary>
        QuaranticInout,
        /// <summary>5x 加速</summary>
        QuinticIn,
        /// <summary>5x 減速</summary>
        QuinticOut,
        /// <summary>5x 加速＆減速</summary>
        QuinticInout,
        /// <summary>sin 加速</summary>
        SinusoidaiIn,
        /// <summary>sin 減速</summary>
        SinusoidaiOut,
        /// <summary>sin 加速＆減速</summary>
        SinusoidaiInout,
        /// <summary>2番目に変化量が大きい 加速</summary>
        ExponentialIn,
        /// <summary>2番目に変化量が大きい 減速</summary>
        ExponentialOut,
        /// <summary>2番目に変化量が大きい 加速＆減速</summary>
        ExponentialInout,
        /// <summary>初速はあるが、全てで一番遅い 加速</summary>
        CircularIn,
        /// <summary>初速はあるが、全てで一番遅い 減速</summary>
        CircularOut,
        /// <summary>初速はあるが、全てで一番遅い 加速＆減速</summary>
        CircularInout,
        /// <summary>開始時に遊び（範囲を超える）</summary>
        BackIn,
        /// <summary>終了時に遊び（範囲を超える）</summary>
        BackOut,
        /// <summary>開始終了時に遊び（範囲を超える）</summary>
        BackInout,
        /// <summary>開始時に大きく範囲を超えて跳ねる</summary>
        ElasticIn,
        /// <summary>終了時に大きく範囲を超えて跳ねる</summary>
        ElasticOut,
        /// <summary>開始終了時に大きく範囲を超えて跳ねる</summary>
        ElasticInout,
        /// <summary>開始時に範囲内で跳ねる</summary>
        BounceIn,
        /// <summary>終了時に範囲内で跳ねる</summary>
        BounceOut,
        /// <summary>開始終了時に範囲内で跳ねる</summary>
        BounceInout,
    }

    const float ELASTIC_AMPLITUDE = 1;
    const float ELASTIC_PERIOD = 0.4f;
    const float BOUNCE0 = 7.5625f;
    const float BOUNCE1 = 1f / 2.75f;
    const float BOUNCE2 = 2f / 2.75f;
    const float BOUNCE3 = 1.5f / 2.75f;
    const float BOUNCE4 = 2.5f / 2.75f;
    const float BOUNCE5 = 2.25f / 2.75f;
    const float BOUNCE6 = 2.625f / 2.75f;

    static ease[] methods = new ease[]
    {
        CubicInout,     // None(Default)
        Linear,
        CubicIn,
        CubicOut,
        CubicInout,
        QuadraticIn,
        QuadraticOut,
        QuadraticInout,
        QuaranticIn,
        QuaranticOut,
        QuaranticInout,
        QuinticIn,
        QuinticOut,
        QuinticInout,
        SinusoidaiIn,
        SinusoidaiOut,
        SinusoidaiInout,
        ExponentialIn,
        ExponentialOut,
        ExponentialInout,
        CircularIn,
        CircularOut,
        CircularInout,
        BackIn,
        BackOut,
        BackInout,
        ElasticIn,
        ElasticOut,
        ElasticInout,
        BounceIn,
        BounceOut,
        BounceInout,
    };
    
    /// <summary>
    /// 特定タイミングのEase Curve値を取得
    /// </summary>
    /// <param name="time">現在の時間</param>
    /// <param name="endtime">終了時間</param>
    /// <param name="type">Ease Curveの種類</param>
    /// <returns>Ease Curve値(0～1）</returns>
    public static float Get(float time, float endtime, eEase type = eEase.None)
    {
        float t = Mathf.Clamp01(time / endtime);
        return methods[(int)type](t);
    }

    /// <summary>
    /// Ease Curve に基づいた特定タイミングの値を取得
    /// </summary>
    /// <param name="time">現在の時間</param>
    /// <param name="endtime">終了時間</param>
    /// <param name="start">開始値</param>
    /// <param name="end">終了値</param>
    /// <param name="type">Ease Curveの種類</param>
    /// <returns>現在値</returns>
    public static float Get(float time, float endtime, float start, float end, eEase type = eEase.None)
    {
        float t = Mathf.Clamp01(time / endtime);
        float v = methods[(int)type](t);
        return start + (end - start) * v;
    }

    /// <summary>
    /// Ease Curve に基づいた特定タイミングのVector値を取得
    /// </summary>
    /// <param name="time">現在の時間</param>
    /// <param name="endtime">終了時間</param>
    /// <param name="start">開始値</param>
    /// <param name="end">終了値</param>
    /// <param name="type">Ease Curveの種類</param>
    /// <returns>現在値</returns>
    public static Vector2 GetVector2(float time, float endtime, Vector2 start, Vector2 end, eEase type = eEase.None)
    {
        float t = Mathf.Clamp01(time / endtime);
        float v = methods[(int)type](t);
        return new Vector2(start.x + (end.x - start.x) * v, start.y + (end.y - start.y) * v);
    }

    /// <summary>
    /// 線形補完
    /// </summary>
    /// <param name="t">time</param>
    /// <returns>result</returns>
    static float Linear(float t)
    {
        float v = t;
        return v;
    }

    /// <summary>
    /// 2x
    /// </summary>
    static float QuadraticIn(float t)
    {
        float v = t * t;
        return v;
    }

    static float QuadraticOut(float t)
    {
        float v = -t * (t - 2);
        return v;
    }

    static float QuadraticInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)1 / 2 * t * t;
        }
        else
        {
            t -= 1;
            v = (float)-1 / 2 * (t * (t - 2) - 1);
        }
        return v;
    }

    /// <summary>
    /// 3x
    /// </summary>
    static float CubicIn(float t)
    {
        float v = t * t * t;
        return v;
    }

    static float CubicOut(float t)
    {
        t -= 1;
        float v = t * t * t + 1;
        return v;
    }

    static float CubicInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)1 / 2 * t * t * t;
        }
        else
        {
            t -= 2;
            v = (float)1 / 2 * (t * t * t + 2);
        }
        return v;
    }

    /// <summary>
    /// 4x
    /// </summary>
    static float QuaranticIn(float t)
    {
        float v = t * t * t * t;
        return v;
    }

    static float QuaranticOut(float t)
    {
        t -= 1;
        float v = -(t * t * t * t - 1);
        return v;
    }

    static float QuaranticInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)1 / 2 * t * t * t * t;
        }
        else
        {
            t -= 2;
            v = (float)-1 / 2 * (t * t * t * t - 2);
        }
        return v;
    }

    /// <summary>
    /// 5x
    /// </summary>
    static float QuinticIn(float t)
    {
        float v = t * t * t * t * t;
        return v;
    }

    static float QuinticOut(float t)
    {
        t -= 1;
        float v = (t * t * t * t * t + 1);
        return v;
    }

    static float QuinticInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)1 / 2 * t * t * t * t * t;
        }
        else
        {
            t -= 2;
            v = (float)1 / 2 * (t * t * t * t * t + 2);
        }
        return v;
    }

    /// <summary>
    /// sin
    /// </summary>
    static float SinusoidaiIn(float t)
    {
        float v = 1 - Mathf.Cos(t * (Mathf.PI/2));
        return v;
    }

    static float SinusoidaiOut(float t)
    {
        float v = Mathf.Sin(t * (Mathf.PI/2));
        return v;
    }

    static float SinusoidaiInout(float t)
    {
        float v = (float)-1 / 2 * (Mathf.Cos(Mathf.PI*t) - 1);
        return v;
    }

    /// <summary>
    /// 
    /// </summary>
    static float ExponentialIn(float t)
    {
        float v = Mathf.Pow(2, 10 * (t - 1));
        return v;
    }

    static float ExponentialOut(float t)
    {
        float v = -Mathf.Pow( 2, -10 * t ) + 1;
        return v;
    }

    static float ExponentialInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)1 / 2 * Mathf.Pow(2, 10 * (t - 1));
        }
        else
        {
            t -= 1;
            v = (float)1 / 2 * (-Mathf.Pow(2, -10 * t) + 2);
        }
        return v;
    }

    /// <summary>
    /// 
    /// </summary>
    static float CircularIn(float t)
    {
        float v = -1 * (Mathf.Sqrt(1 - t*t) - 1);
        return v;
    }

    static float CircularOut(float t)
    {
        t -= 1;
        float v = Mathf.Sqrt(1 - t * t);
        return v;
    }

    static float CircularInout(float t)
    {
        t *= 2;

        float v;
        if (t < 1)
        {
            v = (float)-1 / 2 * (Mathf.Sqrt(1 - t*t) - 1);
        }
        else
        {
            t -= 2;
            v = (float)1 / 2 * (Mathf.Sqrt(1 - t*t) + 1);
        }
        return v;
    }

    /// <summary>
    /// 開始、終了時に遊び
    /// </summary>
    static float BackIn(float t)
    {
        float v = t * t * (2.70158f * t - 1.70158f);

        return v;
    }

    static float BackOut(float t)
    {
        float v = 1 - (t - 1) * (t-1) * (-2.70158f * (t-1) - 1.70158f);

        return v;
    }

    static float BackInout(float t)
    {
        float v;
        t *= 2;
        if (t < 1)
        {
            v = t * t * (2.70158f * t - 1.70158f) / 2;
        }
        else
        {
            t -= 1;
            v  = (1 - (t - 1) * (t - 1) * (-2.70158f * (t - 1) - 1.70158f)) / 2 + 0.5f;
        }

        return v;
    }
    
    /// <summary>
    /// 指定位置を超えて跳ねる
    /// </summary>
    static float ElasticIn(float t)
    {
        t -= 1;
        float v = -(ELASTIC_AMPLITUDE * Mathf.Pow(2, 10 * t) * Mathf.Sin( (t - (ELASTIC_PERIOD / (2 * Mathf.PI) * Mathf.Asin(1 / ELASTIC_AMPLITUDE))) * (2 * Mathf.PI) / ELASTIC_PERIOD));
        return v;
    }

    static float ElasticOut(float t)
    {
        float v = (ELASTIC_AMPLITUDE * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - (ELASTIC_PERIOD / (2 * Mathf.PI) * Mathf.Asin(1 / ELASTIC_AMPLITUDE))) * (2 * Mathf.PI) / ELASTIC_PERIOD) + 1);
        return v;
    }

    static float ElasticInout(float t)
    {
        float v;
        if (t < 0.5f)
        {
            t -= 0.5f;
            v = -0.5f * (Mathf.Pow(2, 10 * t) * Mathf.Sin((t - (ELASTIC_PERIOD / 4)) * (2 * Mathf.PI) / ELASTIC_PERIOD));
        }
        else
        {
            t -= 0.5f;
            v = Mathf.Pow(2, -10 * t) * Mathf.Sin((t - (ELASTIC_PERIOD / 4)) * (2 * Mathf.PI) / ELASTIC_PERIOD) * 0.5f + 1;
        }
        return v;
    }

    /// <summary>
    /// 指定範囲内で跳ねる
    /// </summary>
    static float BounceIn(float t)
    {
        t = 1 - t;

        float v;
        if (t < BOUNCE1)
        {
            v = 1 - BOUNCE0 * t * t;
        }
        else
        if (t < BOUNCE2)
        {
            v = 1 - (BOUNCE0 * (t - BOUNCE3) * (t - BOUNCE3) + 0.75f);
        }
        else
        if (t < BOUNCE4)
        {
            v = 1 - (BOUNCE0 * (t - BOUNCE5) * (t - BOUNCE5) + 0.9375f);
        }
        else
        {
            v = 1 - (BOUNCE0 * (t - BOUNCE6) * (t - BOUNCE6) + 0.984375f);
        }
        return v;
    }

    static float BounceOut(float t)
    {
        float v;
        if (t < BOUNCE1)
        {
            v = BOUNCE0 * t * t;
        }
        else
        if (t < BOUNCE2)
        {
            v = BOUNCE0 * (t - BOUNCE3) * (t - BOUNCE3) + 0.75f;
        }
        else
        if (t < BOUNCE4)
        {
            v = BOUNCE0 * (t - BOUNCE5) * (t - BOUNCE5) + 0.9375f;
        }
        else
        {
            v = BOUNCE0 * (t - BOUNCE6) * (t - BOUNCE6) + 0.984375f;
        }
        return v;
    }

    static float BounceInout(float t)
    {
        float v;
        if (t < 0.5f)
        {
            t = 1 - t * 2;
            if (t < BOUNCE1) v = (1 - BOUNCE0 * t * t) / 2;
            else
            if (t < BOUNCE2) v = (1 - (BOUNCE0 * (t - BOUNCE3) * (t - BOUNCE3) + 0.75f)) / 2;
            else
            if (t < BOUNCE4) v = (1 - (BOUNCE0 * (t - BOUNCE5) * (t - BOUNCE5) + 0.9375f)) / 2;
            else             v = (1 - (BOUNCE0 * (t - BOUNCE6) * (t - BOUNCE6) + 0.984375f)) / 2;
        }
        else
        {
            t = t * 2 - 1;
            if (t < BOUNCE1) v = (BOUNCE0 * t * t) / 2 + 0.5f;
            else
            if (t < BOUNCE2) v = (BOUNCE0 * (t - BOUNCE3) * (t - BOUNCE3) + 0.75f) / 2 + 0.5f;
            else
            if (t < BOUNCE4) v = (BOUNCE0 * (t - BOUNCE5) * (t - BOUNCE5) + 0.9375f) / 2 + 0.5f;
            else             v = (BOUNCE0 * (t - BOUNCE6) * (t - BOUNCE6) + 0.984375f) / 2 + 0.5f;
        }
        return v;
    }
}
