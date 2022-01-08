using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// スワイプ画面遷移
/// </summary>
public class SwipeClose : MonoBehaviour
{
    /// <summary>
    /// 閉じる方向
    /// </summary>
    public enum eCloseVector
    {
        Left,
        Right,
    }

    [SerializeField]
    eCloseVector        CloseVector = eCloseVector.Right;

    /// <summary>
    /// スワイプで画面が閉じ始める時に呼ばれるイベント
    /// </summary>
    public Action       OnClosing;
    /// <summary>
    /// スワイプで画面が閉じ終わった時に呼ばれるイベント
    /// </summary>
    public Action       OnClosed;
    /// <summary>
    /// スワイプ開始の瞬間 true、スワイプをやめた瞬間 false を返すイベント
    /// （スワイプ中、しきい値以下の操作で一瞬やめた判定になる事もある）
    /// </summary>
    public Action<bool> SwipeChanged;

    /// <summary>
    /// スワイプを司るコルーチン
    /// </summary>
    Coroutine           coroutine;

    /// <summary>
    /// 実行（開始）
    /// </summary>
    public void ProcessStart()
    {
        ProcessEnd();
        coroutine = StartCoroutine(co_exec());
    }

    /// <summary>
    /// 実行終了
    /// </summary>
    public void ProcessEnd()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    /// <summary>
    /// SwapUI で動かしたポジションをリセット
    /// </summary>
    public void Reset()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 画面横移動
    /// </summary>
    IEnumerator co_exec()
    {
        // ドラッグにより移動したポジション
        float   pos               = 0;
        // 画面移動したポジション。pos を少し遅れてなめらかに追従する
        float   displayPos        = 0;
        // ドラッグ時間
        float   dragTime          = 0;
        // 前回スワイプ操作していたら true
        bool    preSwipeThresfold = false;

        while (true)
        {
            PadInput.TouchVector touch = Padd.GetTouchPos(0);

            if (Padd.GetKey(ePad.Touch1) == true)
            {
                dragTime += Time.deltaTime;
            }
            else
            if (Padd.GetKey(ePad.Touch1) == false)
            {
                var p = Mathf.Abs(pos);

                // シャッ、と素早くスワイプした場合は「明示的な切り替え指示」と判断して画面遷移する
                if (dragTime < 0.15f && p >= Screen.width * 0.15f)
                {
                    break;
                }
                // スワイプを離した時、一定量を超えていたらスワイプ画面遷移
                if (p >= Screen.width * 0.3f)
                {
                    break;
                }

                pos = 0;
                dragTime = 0;
            }

            // 縦の動きが少なく、かつ横の動きが縦の10倍以上ある時にスワイプ
            bool swipeThresfold = 
                (pos > 0) ||
                (
                    Mathf.Abs(touch.TouchMove.y) < 20 &&
                    Mathf.Abs(touch.TouchMove.x) > Mathf.Abs(touch.TouchMove.y) * 10
                );

            if (swipeThresfold == true)
            {
                pos += touch.TouchMove.x;
                if (CloseVector == eCloseVector.Right)
                {
                    if (pos < 0)
                    {
                        pos = 0;
                    }
                }
                else
                {
                    if (pos > 0)
                    {
                        pos = 0;
                    }
                }
            }

            if (preSwipeThresfold != swipeThresfold)
            {
                SwipeChanged?.Invoke(swipeThresfold);
            }

            // ディスプレイポジション
            if (Mathf.Abs(displayPos - pos) < 5)
            {
                displayPos = pos;
            }
            else
            {
                displayPos = displayPos + (pos - displayPos) * 0.35f;
            }

            // 横移動（スワイプ）
            this.transform.localPosition = new Vector3(displayPos, 0, 0);

            yield return null;
        }

        // スワイプの画面遷移開始
        OnClosing?.Invoke();

        var target = CloseVector == eCloseVector.Right ? Screen.width : -Screen.width;

        while (true)
        {
            if (Mathf.Abs(target - pos) < 5)
            {
                pos = target;
            }
            else
            {
                pos = pos + (target - pos) * 0.2f;
            }

            this.transform.localPosition = new Vector3(pos, 0, 0);

            if (pos == target)
            {
                break;
            }

            yield return null;
        }

        // スワイプの画面遷移終了
        OnClosed?.Invoke();
    }
}
