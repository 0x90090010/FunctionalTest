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
        // nullチェック
        if (canvasRectTransform != null)
        {
            // canvasのサイズをスクリーンサイズに合わせる
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
        // nullチェック
        if (imageRectTransform != null)
        {
            float ratio = -1;

            // imageのサイズをスクリーンサイズに合わせる
            if (Screen.width <= Screen.height)
            {
                ratio = (float)Screen.width / (float)imageRectTransform.sizeDelta.x;
            }
            else
            {
                ratio = (float)Screen.height / (float)imageRectTransform.sizeDelta.y;
            }

            // 画像のサイズを調整するための係数
            float hoge = 1.01f;

            // 画像のサイズを調整
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
