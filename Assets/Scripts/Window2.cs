using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window2 : MonoBehaviour
{
    SimpleUIEase ease;
    SwipeClose   swclose;
    Button       back;

    void Awake()
    {
        ease    = GetComponent<SimpleUIEase>();

        swclose = GetComponent<SwipeClose>();
        swclose.OnClosing = onClosing;

        back    = GetComponentInChildren<Button>();
        back.onClick.AddListener(onButtonClosing);
    }

    void OnEnable()
    {
        // スライド出現アニメーション
        foreach (var effect in ease.GetEffect())
        {
            if (effect.Type == SimpleUIEase.eType.MoveX)
            {
                effect.Ease = EaseValue.eEase.CircularOut;
            }
        }
        ease.SetValue(0);
        ease.Show(showEnd);
    }

    void showEnd()
    {
        swclose.ProcessStart();
    }

    void hideEnd()
    {
        this.SetActive(false);

        // Display2 の子オブジェクトにつける場合は呼ぶ
        //swclose.Reset();
    }

    void onButtonClosing()
    {
        // スワイプ処理は止める
        swclose.ProcessEnd();

        // スライド消失アニメーション
        ease.Hide(hideEnd);
    }

    void onClosing()
    {
        // X 移動は SwipeClose に任せる
        foreach (var effect in ease.GetEffect())
        {
            if (effect.Type == SimpleUIEase.eType.MoveX)
            {
                effect.Ease = EaseValue.eEase.None;
            }
        }

        // αアニメーションのみ
        ease.Hide(hideEnd);
    }
}
