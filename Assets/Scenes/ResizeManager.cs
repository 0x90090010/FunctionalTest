using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeManager : MonoBehaviour
{
    private RectTransform canvasRectTransform;
    private RectTransform imageRectTransform;
    public TitleSceneManager fuga;

    void Start()
    {
        canvasRectTransform = GetComponent<RectTransform>();
        imageRectTransform = GetComponent<RectTransform>();
        Debug.Log(fuga);
    }

    public void ResizeCanvas()
    {
        // null�`�F�b�N
        if (canvasRectTransform != null)
        {
            // canvas�̃T�C�Y���X�N���[���T�C�Y�ɍ��킹��
            canvasRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            Debug.Log("Canvas resized!");
        }
        else
        {
            Debug.LogError("Canvas RectTransform is null.");
        }
    }

    public void ResizeImage()
    {
        // null�`�F�b�N
        if (imageRectTransform != null)
        {
            float ratio = -1;

            // image�̃T�C�Y���X�N���[���T�C�Y�ɍ��킹��
            if (Screen.width <= Screen.height)
            {
                ratio = (float)Screen.width / (float)imageRectTransform.sizeDelta.x;
            }
            else
            {
                ratio = (float)Screen.height / (float)imageRectTransform.sizeDelta.y;
            }

            // �摜�̃T�C�Y�𒲐����邽�߂̌W��
            float hoge = 1.01f;

            // �摜�̃T�C�Y�𒲐�
            int width = (int)(imageRectTransform.sizeDelta.x * ratio * hoge);
            int height = (int)(imageRectTransform.sizeDelta.y * ratio * hoge);

            imageRectTransform.sizeDelta = new Vector2(width, height);
            Debug.Log("Image resized!");
        }
        else
        {
            Debug.LogError("Image RectTransform is null.");
        }
    }
}
