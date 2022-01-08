using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Padd : MonoBehaviour
{
    static PadInput padd = new PadInput();

    /// <summary>
    /// update
    /// </summary>
    void Update()
    {
        padd?.Update();
    }
    
    /// <summary>
    /// set pad config
    /// </summary>
    public static void SetPadConfig(PadInput.PadConfig config)
    {
        padd.SetPadConfig(config);
    }

    /// <summary>
    /// get pad config
    /// </summary>
    public static PadInput.PadConfig GetPadConfig()
    {
        return padd.GetPadConfig();
    }
    
    /// <summary>
    /// set key config
    /// </summary>
    public static void SetKeyConfig(PadInput.KeyConfig config)
    {
        padd.SetKeyConfig(config);
    }

    /// <summary>
    /// get key config
    /// </summary>
    public static PadInput.KeyConfig GetKeyConfig()
    {
        return padd.GetKeyConfig();
    }

    /// <summary>
    /// set enabled of input
    /// </summary>
    public static void SetEnabled(bool enabled)
    {
        padd.SetEnabled(enabled);
    }

    /// <summary>
    /// get enabled of input
    /// </summary>
    public static bool GetEnabled()
    {
        return padd.GetEnabled();
    }

    /// <summary>
    /// アナログスティック（左）を取得
    /// </summary>
    /// <returns>-1（左上）～ 1（右下）、変化あったフレームは IsMoved == true</returns>
    public static PadInput.PadVector GetAxisL()
    {
        return padd.GetAxisL();
    }

    /// <summary>
    /// アナログスティック（右）を取得
    /// </summary>
    /// <returns>-1（左上）～ 1（右下）、変化あったフレームは IsMoved == true</returns>
    public static PadInput.PadVector GetAxisR()
    {
        return padd.GetAxisR();
    }

    /// <summary>
    /// トリガーのアナログ値（L2, R2）を取得。L2 が X、R2 が Y
    /// </summary>
    /// <returns>-1（押していない）～ 1（深く押している）、変化あったフレームは IsMoved == true</returns>
    public static PadInput.PadVector GetTrigger()
    {
        return padd.GetTrigger();
    }

    /// <summary>
    /// マウスの座標（画面サイズによって変動）を取得します
    /// </summary>
    /// <returns>左0～右画面最大サイズ（横）、下0～上画面最大サイズ（縦）、変化あったフレームは IsMoved == true</returns>
    public static PadInput.TouchVector GetMouse()
    {
        return padd.GetMouse();
    }

    /// <summary>
    /// タッチスクリーンの座標（画面サイズによって変動）を取得します
    /// </summary>
    /// <returns>左0～右画面最大サイズ（横）、下0～上画面最大サイズ（縦）、変化あったフレームは IsMoved == true</returns>
    public static PadInput.TouchVector GetTouchPos(int no)
    {
        return padd.GetTouchPos(no);
    }

    /// <summary>
    /// マウスホイールのスクロール値を取得
    /// </summary>
    public static float GetMouseWheel()
    {
        return padd.GetMouseWheel();
    }

    /// <summary>
    /// 該当のボタンが押されているかチェック
    /// </summary>
    /// <param name="button">確認するボタン</param>
    /// <returns>押されていれば true</returns>
    public static bool GetKey(ePad button)
    {
        return padd.GetKey(button);
    }

    /// <summary>
    /// 該当のボタンが押されているかチェック
    /// </summary>
    /// <param name="buttonBit">PadInput.PAD_XXXXX 指定。OR で複数指定可能</param>
    /// <returns>押されていれば true</returns>
    public static bool GetKey(ulong buttonBit)
    {
        return padd.GetKey(buttonBit);
    }

    /// <summary>
    /// 該当のボタンが押した瞬間かチェック
    /// </summary>
    /// <param name="button">確認するボタン</param>
    /// <returns>押された瞬間なら true</returns>
    public static bool GetKeyDown(ePad button)
    {
        return padd.GetKeyDown(button);
    }

    /// <summary>
    /// 該当のボタンが押した瞬間かチェック
    /// </summary>
    /// <param name="buttonBit">PadInput.PAD_XXXXX 指定。OR で複数指定可能</param>
    /// <returns>押された瞬間なら true</returns>
    public static bool GetKeyDown(ulong buttonBit)
    {
        return padd.GetKeyDown(buttonBit);
    }

    /// <summary>
    /// 該当のボタンが離した瞬間かチェック
    /// </summary>
    /// <param name="button">確認するボタン</param>
    /// <returns>離した瞬間なら true</returns>
    public static bool GetKeyUp(ePad button)
    {
        return padd.GetKeyUp(button);
    }

    /// <summary>
    /// 該当のボタンが離した瞬間かチェック
    /// </summary>
    /// <param name="buttonBit">PadInput.PAD_XXXXX 指定。OR で複数指定可能</param>
    /// <returns>離した瞬間なら true</returns>
    public static bool GetKeyUp(ulong buttonBit)
    {
        return padd.GetKeyUp(buttonBit);
    }

    /// <summary>
    /// 該当のボタンのリピートチェック。一定間隔でオン
    /// </summary>
    /// <param name="button">確認するボタン</param>
    /// <returns>リピートオンされた瞬間なら true</returns>
    public static bool GetKeyDelay(ePad button)
    {
        return padd.GetKeyDelay(button);
    }

    /// <summary>
    /// 該当のボタンのリピートチェック。一定間隔でオン
    /// </summary>
    /// <param name="buttonBit">PadInput.PAD_XXXXX 指定。OR で複数指定可能</param>
    /// <returns>リピートオンされた瞬間なら true</returns>
    public static bool GetKeyDelay(ulong buttonBit)
    {
        return padd.GetKeyDelay(buttonBit);
    }
    
    /// <summary>
    /// Input 直接コール
    /// </summary>
    public static bool NativeKey(Key key)
    {
        return padd.NativeKey(key);
    }

    /// <summary>
    /// Input 直接コール
    /// </summary>
    public static bool NativeKeyDown(Key key)
    {
        return padd.NativeKeyDown(key);
    }

}
