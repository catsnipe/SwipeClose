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
        // �X���C�h�o���A�j���[�V����
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

        // Display2 �̎q�I�u�W�F�N�g�ɂ���ꍇ�͌Ă�
        //swclose.Reset();
    }

    void onButtonClosing()
    {
        // �X���C�v�����͎~�߂�
        swclose.ProcessEnd();

        // �X���C�h�����A�j���[�V����
        ease.Hide(hideEnd);
    }

    void onClosing()
    {
        // X �ړ��� SwipeClose �ɔC����
        foreach (var effect in ease.GetEffect())
        {
            if (effect.Type == SimpleUIEase.eType.MoveX)
            {
                effect.Ease = EaseValue.eEase.None;
            }
        }

        // ���A�j���[�V�����̂�
        ease.Hide(hideEnd);
    }
}
