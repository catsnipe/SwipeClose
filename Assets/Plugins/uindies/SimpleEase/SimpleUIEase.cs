using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Events;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SimpleUIEaseEffect
{
    /// <summary>
    /// アニメーションの種類
    /// </summary>
    public SimpleUIEase.eType Type = SimpleUIEase.eType.Fade;
    /// <summary>
    /// 原点となるポジション値。アニメーション終了時はこの値になる
    /// </summary>
    public float              Pos = 0;
    /// <summary>
    /// Fade 以外のタイプで、移動変化量。MoveX であれば -1 が左から、1 が右から
    /// </summary>
    public float              Ratio = -1;
    /// <summary>
    /// イージングの種類
    /// </summary>
    public EaseValue.eEase    Ease = EaseValue.eEase.CubicOut;
}

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]

[Serializable]
public class SimpleUIEase : MonoBehaviour
{
    /// <summary>
    /// アニメーションタイプ
    /// </summary>
    public enum eType
    {
        /// <summary>
        /// αフェード
        /// </summary>
        Fade,
        /// <summary>
        /// 横方向移動
        /// </summary>
        MoveX,
        /// <summary>
        /// 縦方向移動
        /// </summary>
        MoveY,
        /// <summary>
        /// 横スケール変更
        /// </summary>
        ScaleX,
        /// <summary>
        /// 縦スケール変更
        /// </summary>
        ScaleY,
        /// <summary>
        /// 回転
        /// </summary>
        RotateZ,
    }

    [SerializeField, Range(0.05f, 10f), Tooltip("IN、または OUT するまでの時間を設定します。")]
    public float     TotalTime = 0.3f;

    [SerializeField, Space(10)]
    List<SimpleUIEaseEffect> Effects = new List<SimpleUIEaseEffect>();

    [SerializeField, Space(10), Range(0, 10f), Tooltip("表示アニメーションが始まるまでのディレイタイムを指定します。")]
    float             DelayTimeBeforeShow = 0;
    [SerializeField, Range(0, 10f), Tooltip("非表示アニメーションが始まるまでのディレイタイムを指定します。")]
    float             DelayTimeBeforeHide = 0;
    [SerializeField, Space(10), Tooltip("Show / Hide に合わせて自動的に SetActive() を実行します。")]
    bool              AutoActivate = false;
    [SerializeField, Tooltip("Show / Hide に合わせて自動的に CanvasGroup の入力可否を設定します。")]
    bool              AutoBlockRaycasts = true;

    [SerializeField, Header("Debug"), Range(0, 1), Tooltip("アニメーションの確認を行います。0 が非表示、1 が表示。")]
    float             Value = 1;

    [SerializeField, Header("Event")]
    public UnityEvent OnFadein   = null;
    [SerializeField]
    public UnityEvent OnFadeout  = null;

    Action            OnFadein1  = null;
    Action            OnFadeout1 = null;

    RectTransform     rectTransform;
    CanvasGroup       canvasGroup;

    Coroutine         co_fadein;
    Coroutine         co_fadeout;

#if UNITY_EDITOR
    List<SimpleUIEaseEffect> compares;
#endif

    /// <summary>
    /// awake
    /// </summary>
    void Awake()
    {
        initCache();

        transitionUpdate(rectTransform, canvasGroup, Value);

    }
    
    /// <summary>
    /// start
    /// </summary>
    void Start()
    {
        if (Value > 0 || co_fadein != null)
        {
            if (AutoActivate == true)
            {
                this.SetActive(true);
            }
            if (AutoBlockRaycasts == true)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            if (AutoActivate == true)
            {
                this.SetActive(false);
            }
            if (AutoBlockRaycasts == true)
            {
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    /// <summary>
    /// アタッチする瞬間、RectTransform で設定された値を自動的に入れる
    /// </summary>
    void Reset()
    {
        initCache();
    }
    
    /// <summary>
    /// on validate
    /// </summary>
    void OnValidate()
    {
        initCache();

#if UNITY_EDITOR
        // OnValidate 前と今回の値を比較し、Used の変更やタイプ変更があった場合は rectTransform の値を取り直す
        if (compares == null)
        {
            compares = new List<SimpleUIEaseEffect>();
        }
        for (int i = compares.Count; i < Effects.Count; i++)
        {
            var compare = new SimpleUIEaseEffect();
            compare.Type = Effects[i].Type;
            compares.Add(compare);
        }

        for (int i = 0; i < Effects.Count; i++)
        {
            SimpleUIEaseEffect effect = Effects[i];

            if (effect.Type == compares[i].Type)
            {
                continue;
            }

            compares[i].Type = effect.Type;

            if (effect.Type == eType.MoveX)
            {
                effect.Pos = rectTransform.GetX();
                Debug.Log($"[MoveX] position reset.");
            }
            if (effect.Type == eType.MoveY)
            {
                effect.Pos = rectTransform.GetY();
                Debug.Log($"[MoveY] position reset.");
            }
            if (effect.Type == eType.ScaleX)
            {
                effect.Pos = rectTransform.GetScaleX();
                Debug.Log($"[ScaleX] position reset.");
            }
            if (effect.Type == eType.ScaleY)
            {
                effect.Pos = rectTransform.GetScaleY();
                Debug.Log($"[ScaleY] position reset.");
            }
            if (effect.Type == eType.RotateZ)
            {
                effect.Pos = rectTransform.GetRotateZ();
                Debug.Log($"[RotateZ] position reset.");
            }
        }
#endif
        transitionUpdate(rectTransform, canvasGroup, Value);
    }
    
    /// <summary>
    /// Value を強制的に変更
    /// </summary>
    /// <param name="value">0:hide～1:show</param>
    public void SetValue(float value)
    {
        if (Value == value)
        {
            return;
        }

        initCache();

        if (value == 1 || co_fadein  != null)
        {
            OnFadein?.Invoke();

            OnFadein1?.Invoke();
            OnFadein1 = null;

            if (AutoActivate == true)
            {
                this.SetActive(true);
            }
            if (AutoBlockRaycasts == true)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }
        else
        if (value == 0 || co_fadeout != null)
        {
            OnFadeout?.Invoke();

            OnFadeout1?.Invoke();
            OnFadeout1 = null;

            if (AutoActivate == true)
            {
                this.SetActive(false);
            }
            if (AutoBlockRaycasts == true)
            {
                canvasGroup.blocksRaycasts = false;
            }
        }
        stopCoroutine();

        Value = value;
        transitionUpdate(rectTransform, canvasGroup, Value);
    }

    /// <summary>
    /// 表示
    /// </summary>
    public void Show(Action fadeinEndFunc = null)
    {
        if (co_fadeout != null)
        {
            StopCoroutine(co_fadeout);
            co_fadeout = null;
        }

        if (Value == 1 || co_fadein != null)
        {
            return;
        }

        initCache();

        if (AutoActivate == true)
        {
            this.SetActive(true);
        }
        if (AutoBlockRaycasts == true)
        {
            // まだ許可は出さない
            canvasGroup.blocksRaycasts = false;
        }

        if (gameObject.activeInHierarchy == false)
        {
            SetValue(1);
            return;
        }

        OnFadein1 = fadeinEndFunc;
        stopCoroutine();
        co_fadein = StartCoroutine(fadein());
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void Hide(Action fadeoutEndFunc = null)
    {
        if (co_fadein != null)
        {
            StopCoroutine(co_fadein);
            co_fadein = null;
        }

        if (Value == 0 || co_fadeout != null)
        {
            return;
        }

        initCache();

        if (AutoBlockRaycasts == true)
        {
            canvasGroup.blocksRaycasts = false;
        }

        if (gameObject.activeInHierarchy == false)
        {
            SetValue(0);
            return;
        }

        OnFadeout1 = fadeoutEndFunc;
        stopCoroutine();
        co_fadeout = StartCoroutine(fadeout());
    }
    
    /// <summary>
    /// Effect 取得
    /// </summary>
    public List<SimpleUIEaseEffect> GetEffect()
    {
        return Effects;
    }

    /// <summary>
    /// キャッシュ登録
    /// </summary>
    void initCache()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        if (canvasGroup == null)
        {
            canvasGroup   = GetComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// in out 両方のコルーチン停止
    /// </summary>
    void stopCoroutine()
    {
        if (co_fadein != null)
        {
            StopCoroutine(co_fadein);
            co_fadein = null;
        }
        if (co_fadeout != null)
        {
            StopCoroutine(co_fadeout);
            co_fadeout = null;
        }
    }

    /// <summary>
    /// fadein
    /// </summary>
    IEnumerator fadein()
    {
        yield return new WaitForSeconds(DelayTimeBeforeShow);

        float time     = Time.time;
        float startVal = Value;

        while (true)
        {
            float value = Mathf.Clamp01((Time.time - time) / TotalTime);
            Value = Mathf.Clamp01(startVal + (1 - startVal) * value);

            transitionUpdate(rectTransform, canvasGroup, Value);
            
            if (AutoBlockRaycasts == true)
            {
                // 完全表示より少し前にレイキャストはONにしておく（ユーザビリティを考えて）
                if (value >= 0.75f)
                {
                    canvasGroup.blocksRaycasts = true;
                }
            }

            if (value >= 1)
            {
                break;
            }
            yield return null;
        }

        OnFadein?.Invoke();

        OnFadein1?.Invoke();
        OnFadein1 = null;

        co_fadein = null;
    }

    /// <summary>
    /// fadeout
    /// </summary>
    IEnumerator fadeout()
    {
        yield return new WaitForSeconds(DelayTimeBeforeHide);

        float time     = Time.time;
        float startVal = Value;

        while (true)
        {
            float value = Mathf.Clamp01((Time.time - time) / TotalTime);
            Value = Mathf.Clamp01(startVal + (0 - startVal) * value);

            transitionUpdate(rectTransform, canvasGroup, Value);
            
            if (value >= 1)
            {
                break;
            }
            yield return null;
        }

        if (AutoActivate == true)
        {
            this.SetActive(false);
        }

        OnFadeout?.Invoke();

        OnFadeout1?.Invoke();
        OnFadeout1 = null;

        co_fadeout = null;
    }

    /// <summary>
    /// 状態更新
    /// </summary>
    void transitionUpdate(RectTransform rectTrans, CanvasGroup group, float value)
    {
        foreach (SimpleUIEaseEffect effect in Effects)
        {
            if (effect.Type == eType.Fade)
            {
                group.alpha = EaseValue.Get(value, 1);
            }
            if (effect.Ease != EaseValue.eEase.None)
            {
                if (effect.Type == eType.MoveX)
                {
                    rectSetX(rectTrans, EaseValue.Get(value, 1, effect.Pos + rectGetWidth(rectTrans) * effect.Ratio, effect.Pos, effect.Ease));
                }
                if (effect.Type == eType.MoveY)
                {
                    rectSetY(rectTrans, EaseValue.Get(value, 1, effect.Pos + rectGetHeight(rectTrans) * effect.Ratio, effect.Pos, effect.Ease));
                }
                if (effect.Type == eType.ScaleX)
                {
                    rectSetScaleX(rectTrans, EaseValue.Get(value, 1, effect.Pos + effect.Ratio, effect.Pos, effect.Ease));
                }
                if (effect.Type == eType.ScaleY)
                {
                    rectSetScaleY(rectTrans, EaseValue.Get(value, 1, effect.Pos + effect.Ratio, effect.Pos, effect.Ease));
                }
                if (effect.Type == eType.RotateZ)
                {
                    rectSetRotateZ(rectTrans, EaseValue.Get(value, 1, effect.Pos + effect.Ratio, effect.Pos, effect.Ease));
                }
            }
        }
    }

    /// <summary>
    /// 幅を返します
    /// </summary>
    public static float rectGetWidth(RectTransform self)
    {
        return self.rect.size.x;
    }
    
    /// <summary>
    /// 高さを返します
    /// </summary>
    public static float rectGetHeight(RectTransform self)
    {
        return self.rect.size.y;
    }

    /// <summary>
    /// 座標を設定します
    /// </summary>
    static void rectSetX(RectTransform self, float x)
    {
        Vector3 trans = self.gameObject.transform.localPosition;
        trans.x = x;
        self.gameObject.transform.localPosition = trans;
    }

    /// <summary>
    /// 座標を設定します
    /// </summary>
    static void rectSetY(RectTransform self, float y)
    {
        Vector3 trans = self.gameObject.transform.localPosition;
        trans.y = y;
        self.gameObject.transform.localPosition = trans;
    }

    /// <summary>
    /// スケールを設定します
    /// </summary>
    static void rectSetScaleX(RectTransform self, float x)
    {
        Vector3 trans = self.gameObject.transform.localScale;
        trans.x = x;
        self.gameObject.transform.localScale = trans;
    }

    /// <summary>
    /// スケールを設定します
    /// </summary>
    static void rectSetScaleY(RectTransform self, float y)
    {
        Vector3 trans = self.gameObject.transform.localScale;
        trans.y = y;
        self.gameObject.transform.localScale = trans;
    }

    /// <summary>
    /// 回転を設定します
    /// </summary>
    static void rectSetRotateZ(RectTransform self, float z)
    {
        Vector3 trans = self.gameObject.transform.localEulerAngles;
        trans.z = z;
        self.gameObject.transform.localEulerAngles = trans;
    }
}
