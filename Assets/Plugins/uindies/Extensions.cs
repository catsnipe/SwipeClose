using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// list extensions
/// </summary>
public static class ListExtensions
{
    public static bool IsNullOrZero<T>(this IList<T> self)
    {
        return self == null || self.Count == 0;
    }
}

/// <summary>
/// game object extensions
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// 親オブジェクトを設定
    /// </summary>
    public static void SetParent(this GameObject self, GameObject parent)
    {
        self.transform.SetParent(parent.transform);
    }

    /// <summary>
    /// 親オブジェクトを設定
    /// </summary>
    public static void SetParent(this GameObject self, Component parent)
    {
        self.transform.SetParent(parent.transform);
    }

    /// <summary>
    /// 子オブジェクト数を取得
    /// </summary>
    public static int GetChildCount(this GameObject self)
    {
        return self.transform.childCount;
    }

    /// <summary>
    /// 親オブジェクトを取得
    /// </summary>
    public static GameObject GetParent(this GameObject self)
    {
        var t = self.transform.parent;
        return t != null ? t.gameObject : null;
    }

    /// <summary>
    /// ルートオブジェクトを取得
    /// </summary>
    public static GameObject GetRoot(this GameObject self)
    {
        var t = self.transform.root;
        return t != null ? t.gameObject : null;
    }

    /// <summary>
    /// レイヤー設定
    /// </summary>
    public static void SetLayer(this GameObject self, string layerName)
    {
        self.layer = LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// 子要素も含めてレイヤー設定
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer">LayerMask.NameToLayer() で取得するレイヤー番号</param>
    public static void SetLayerRecursively(this GameObject self, int layer)
    {
        self.layer = layer;

        foreach (Transform n in self.transform)
        {
            SetLayerRecursively(n.gameObject, layer);
        }
    }

    /// <summary>
    /// 子要素も含めてレイヤー設定
    /// </summary>
    public static void SetLayerRecursively(this GameObject self, string layerName)
    {
        self.SetLayerRecursively(LayerMask.NameToLayer(layerName));
    }

}

/// <summary>
/// component extensions
/// </summary>
public static class ComponentExtensions
{
    /// <summary>
    /// 親オブジェクトを設定
    /// </summary>
    public static void SetParent(this Component self, GameObject parent)
    {
        self.transform.SetParent(parent.transform);
    }

    /// <summary>
    /// 親オブジェクトを設定
    /// </summary>
    public static void SetParent(this Component self, Component parent)
    {
        self.transform.SetParent(parent.transform);
    }

    /// <summary>
    /// 子オブジェクト数を取得
    /// </summary>
    public static int GetChildCount(this Component self)
    {
        return self.transform.childCount;
    }

    /// <summary>
    /// 親オブジェクトを取得
    /// </summary>
    public static GameObject GetParent(this Component self)
    {
        var t = self.transform.parent;
        return t != null ? t.gameObject : null;
    }

    /// <summary>
    /// ルートオブジェクトを取得
    /// </summary>
    public static GameObject GetRoot(this Component self)
    {
        var t = self.transform.root;
        return t != null ? t.gameObject : null;
    }

    /// <summary>
    /// レイヤー設定
    /// </summary>
    public static void SetLayer(this Component self, string layerName)
    {
        self.gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// 子要素も含めてレイヤー設定
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer">LayerMask.NameToLayer() で取得するレイヤー番号</param>
    public static void SetLayerRecursively(this Component self, int layer)
    {
        self.gameObject.layer = layer;

        foreach (Transform n in self.gameObject.transform)
        {
            SetLayerRecursively(n, layer);
        }
    }

    /// <summary>
    /// 子要素も含めてレイヤー設定
    /// </summary>
    public static void SetLayerRecursively(this Component self, string layerName)
    {
        self.SetLayerRecursively(LayerMask.NameToLayer(layerName));
    }

    /// <summary>
    /// コンポーネントを返す。なければ作る
    /// </summary>
    public static void SafeGetComponent<T>(this Component self, out T component) where T : Component
    {
        component = self.GetComponent<T>();
        if (component == null)
        {
            component = self.gameObject.AddComponent<T>();
        }
    }

    /// <summary>
    /// コンポーネントを取得
    /// </summary>
    public static bool CheckGetComponent<T>(this Component self, out T component) where T : Component
    {
        component = self.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Missing Component");
            return false;
        }
        return true;
    }

    /// <summary>
    /// コンポーネントを取得
    /// </summary>
    public static bool CheckGetComponentInChildren<T>(this Component self, out T component, bool includeInactive = true) where T : Component
    {
        component = self.GetComponentInChildren<T>(includeInactive);
        if (component == null)
        {
            Debug.LogError("Missing Component");
            return false;
        }
        return true;
    }

    /// <summary>
    /// コンポーネントに紐づく GameObject を破棄
    /// </summary>
    public static void Destroy(this Component self)
    {
        Object.Destroy(self.gameObject);
    }

    /// <summary>
    /// コンポーネントに紐づく GameObject を即時破棄
    /// </summary>
    public static void DestroyImmediate(this Component self)
    {
        Object.DestroyImmediate(self.gameObject);
    }

    /// <summary>
    /// コンポーネントに紐づく GameObject のアクティベート
    /// </summary>
    public static void SetActive(this Component self, bool value)
    {
        self.gameObject.SetActive(value);
    }

    /// <summary>
    /// 座標を取得します
    /// </summary>
    public static float GetX(this Component self)
    {
        return self.gameObject.transform.localPosition.x;
    }

    /// <summary>
    /// 座標を取得します
    /// </summary>
    public static float GetY(this Component self)
    {
        return self.gameObject.transform.localPosition.y;
    }

    /// <summary>
    /// 座標を設定します
    /// </summary>
    public static void SetX(this Component self, float x)
    {
        Vector3 trans = self.gameObject.transform.localPosition;
        trans.x = x;
        self.gameObject.transform.localPosition = trans;
    }

    /// <summary>
    /// 座標を設定します
    /// </summary>
    public static void SetY(this Component self, float y)
    {
        Vector3 trans = self.gameObject.transform.localPosition;
        trans.y = y;
        self.gameObject.transform.localPosition = trans;
    }

    /// <summary>
    /// 座標を設定します
    /// </summary>
    public static void SetXY(this Component self, float x, float y)
    {
        Vector3 trans = self.gameObject.transform.localPosition;
        trans.x = x;
        trans.y = y;
        self.gameObject.transform.localPosition = trans;
    }

    /// <summary>
    /// 回転を取得します
    /// </summary>
    public static float GetRotateX(this Component self)
    {
        return self.gameObject.transform.localEulerAngles.x;
    }

    /// <summary>
    /// 回転を取得します
    /// </summary>
    public static float GetRotateY(this Component self)
    {
        return self.gameObject.transform.localEulerAngles.y;
    }

    /// <summary>
    /// 回転を取得します
    /// </summary>
    public static float GetRotateZ(this Component self)
    {
        return self.gameObject.transform.localEulerAngles.z;
    }

    /// <summary>
    /// 回転を設定します
    /// </summary>
    public static void SetRotateXY(this Component self, float x, float y)
    {
        Vector3 trans = self.gameObject.transform.localEulerAngles;
        trans.x = x;
        trans.y = y;
        self.gameObject.transform.localEulerAngles = trans;
    }

    /// <summary>
    /// 回転を設定します
    /// </summary>
    public static void SetRotateX(this Component self, float x)
    {
        Vector3 trans = self.gameObject.transform.localEulerAngles;
        trans.x = x;
        self.gameObject.transform.localEulerAngles = trans;
    }

    /// <summary>
    /// 回転を設定します
    /// </summary>
    public static void SetRotateY(this Component self, float y)
    {
        Vector3 trans = self.gameObject.transform.localEulerAngles;
        trans.y = y;
        self.gameObject.transform.localEulerAngles = trans;
    }

    /// <summary>
    /// 回転を設定します
    /// </summary>
    public static void SetRotateZ(this Component self, float z)
    {
        Vector3 trans = self.gameObject.transform.localEulerAngles;
        trans.z = z;
        self.gameObject.transform.localEulerAngles = trans;
    }

    /// <summary>
    /// スケールを取得します
    /// </summary>
    public static float GetScaleX(this Component self)
    {
        return self.gameObject.transform.localScale.x;
    }

    /// <summary>
    /// スケールを取得します
    /// </summary>
    public static float GetScaleY(this Component self)
    {
        return self.gameObject.transform.localScale.y;
    }

    /// <summary>
    /// スケールを設定します
    /// </summary>
    public static void SetScaleX(this Component self, float x)
    {
        Vector3 trans = self.gameObject.transform.localScale;
        trans.x = x;
        self.gameObject.transform.localScale = trans;
    }

    /// <summary>
    /// スケールを設定します
    /// </summary>
    public static void SetScaleY(this Component self, float y)
    {
        Vector3 trans = self.gameObject.transform.localScale;
        trans.y = y;
        self.gameObject.transform.localScale = trans;
    }

    /// <summary>
    /// スケールを設定します
    /// </summary>
    public static void SetScaleXY(this Component self, float x, float y)
    {
        Vector3 trans = self.gameObject.transform.localScale;
        trans.x = x;
        trans.y = y;
        self.gameObject.transform.localScale = trans;
    }
}

/// <summary>
/// image extensions
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// サイズを設定します
    /// </summary>
    public static void SetNativeSize(this Image self)
    {
        if (self.sprite != null)
        {
            SetSize(self, self.sprite.rect.width, self.sprite.rect.height);
        }
        else
        {
            SetSize(self, 100, 100);
        }
    }

    /// <summary>
    /// サイズを設定します
    /// </summary>
    public static void SetSize(this Image self, float width, float height)
    {
        self.rectTransform.SetSize(width, height);
    }
    
    /// <summary>
    /// カラーのα値を変更します
    /// </summary>
    public static void SetAlpha(this Image self, float alpha)
    {
        self.color = new Color(self.color.r, self.color.g, self.color.b, alpha);
    }
}

/// <summary>
/// text mesh pro extensions
/// </summary>
public static class TextMeshProUGUIExtensions
{
    /// <summary>
    /// カラーのα値を変更します
    /// </summary>
    public static void SetAlpha(this TextMeshProUGUI self, float alpha)
    {
        self.color = new Color(self.color.r, self.color.g, self.color.b, alpha);
    }
}

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}

/// <summary>
/// RectTransform 型の拡張メソッドを管理するクラス
/// </summary>
public static class RectTransformExtensions
{
    /// <summary>
    /// 幅を返します
    /// </summary>
    public static float GetWidth(this RectTransform self)
    {
        return self.sizeDelta.x;
    }
    
    /// <summary>
    /// 高さを返します
    /// </summary>
    public static float GetHeight(this RectTransform self)
    {
        return self.sizeDelta.y;
    }

    /// <summary>
    /// 幅を設定します
    /// </summary>
    public static void SetWidth(this RectTransform self, float width)
    {
        var size = self.sizeDelta;
        size.x = width;
        self.sizeDelta = size;
    }
    
    /// <summary>
    /// 高さを設定します
    /// </summary>
    public static void SetHeight(this RectTransform self, float height)
    {
        var size = self.sizeDelta;
        size.y = height;
        self.sizeDelta = size;
    }
    
    /// <summary>
    /// サイズを取得します
    /// </summary>
    public static Vector2 GetSize(this RectTransform self)
    {
        return self.sizeDelta;
    }
    
    /// <summary>
    /// サイズを設定します
    /// </summary>
    public static void SetSize(this RectTransform self, float width, float height)
    {
        self.sizeDelta = new Vector2(width, height);
    }

    public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (allign)
        {
            case (AnchorPresets.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPresets.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPresets.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPresets.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPresets.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPresets.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }

    public static void SetPivot(this RectTransform source, PivotPresets preset)
    {

        switch (preset)
        {
            case (PivotPresets.TopLeft):
                {
                    source.pivot = new Vector2(0, 1);
                    break;
                }
            case (PivotPresets.TopCenter):
                {
                    source.pivot = new Vector2(0.5f, 1);
                    break;
                }
            case (PivotPresets.TopRight):
                {
                    source.pivot = new Vector2(1, 1);
                    break;
                }

            case (PivotPresets.MiddleLeft):
                {
                    source.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleCenter):
                {
                    source.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleRight):
                {
                    source.pivot = new Vector2(1, 0.5f);
                    break;
                }

            case (PivotPresets.BottomLeft):
                {
                    source.pivot = new Vector2(0, 0);
                    break;
                }
            case (PivotPresets.BottomCenter):
                {
                    source.pivot = new Vector2(0.5f, 0);
                    break;
                }
            case (PivotPresets.BottomRight):
                {
                    source.pivot = new Vector2(1, 0);
                    break;
                }
        }
    }
}

