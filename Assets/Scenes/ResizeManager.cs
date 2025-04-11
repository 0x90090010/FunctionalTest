using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ResizeManager : MonoBehaviour
{
    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = GetComponent<RectTransform>();
    }

    public void ResizeCanvas()
    {
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

    /// <summary>
    /// ResizeFullScreenImageOverCanvas
    /// 主に全画面でCanvasより大きく表示したい画像に使用する
    /// 
    /// 仕様
    /// https://0x90090010.atlassian.net/wiki/pages/resumedraft.action?draftId=65701&draftShareId=754a3d08-0f33-434b-a9f4-289c5e82c1dd
    /// </summary>
    public void ResizeFullScreenImageOverCanvas(List<RectTransform> imageRectTransforms)
    {

        foreach (RectTransform imageRectTransform in imageRectTransforms)
        {

            if (imageRectTransform != null)
            {
                Image image = imageRectTransform.GetComponent<Image>();
                double ratio = 1;
                int imageWidth = (int)image.sprite.rect.width;
                int imageHeight = (int)image.sprite.rect.height;
                int resizeWidth = 1;
                int resizeHeight = 1;

                if (Screen.width <= Screen.height && image.sprite.rect.width <= image.sprite.rect.height)
                {
                    Debug.Log("Pattern A");
                    ratio = (double)Screen.width / (double)imageWidth;
                    resizeWidth = (int)(imageWidth * ratio);
                    resizeHeight = (int)(imageHeight * ratio);
                    if (resizeHeight < Screen.height)
                    {
                        ratio = (double)Screen.height / (double)resizeHeight;
                        resizeWidth = (int)(resizeWidth * ratio);
                        resizeHeight = (int)(resizeHeight * ratio);
                    }
                }
                else if (Screen.width > Screen.height && image.sprite.rect.width > image.sprite.rect.height)
                {
                    Debug.Log("Pattern B");
                    ratio = (double)Screen.height / (double)imageHeight;
                    resizeWidth = (int)(imageWidth * ratio);
                    resizeHeight = (int)(imageHeight * ratio);

                    if (resizeWidth < Screen.width)
                    {
                        ratio = (double)Screen.width / (double)resizeWidth;
                        resizeWidth = (int)(resizeWidth * ratio);
                        resizeHeight = (int)(resizeHeight * ratio);
                    }
                }
                else if (Screen.width > Screen.height && image.sprite.rect.width <= image.sprite.rect.height)
                {
                    Debug.Log("Pattern C");
                    ratio = (double)Screen.height / (double)imageHeight;
                    resizeWidth = (int)(imageWidth * ratio);
                    resizeHeight = (int)(imageHeight * ratio);

                    if (resizeWidth < Screen.width)
                    {
                        ratio = (double)Screen.width / (double)resizeWidth;
                        resizeWidth = (int)(resizeWidth * ratio);
                        resizeHeight = (int)(resizeHeight * ratio);
                    }
                }
                else // (Screen.width <= Screen.height && image.sprite.rect.width > image.sprite.rect.height)
                {
                    Debug.Log("Pattern D");
                    ratio = (double)Screen.width / (double)imageWidth;
                    resizeWidth = (int)(imageWidth * ratio);
                    resizeHeight = (int)(imageHeight * ratio);
                    if (resizeHeight < Screen.height)
                    {
                        ratio = (double)Screen.height / (double)resizeHeight;
                        resizeWidth = (int)(resizeWidth * ratio);
                        resizeHeight = (int)(resizeHeight * ratio);
                    }
                }


                imageRectTransform.sizeDelta = new Vector2(resizeWidth, resizeHeight);
                Debug.Log("Image resized!");
            }
            else
            {
                Debug.LogError("Image RectTransform is null.");
            }
        }
    }

    /// <summary>
    /// ResizeImageKeepingAspect
    /// 主に全画面に表示しない画像（ボタンなど）に使用する
    /// </summary>
    public void ResizeImageKeepingAspect(Vector2 screenSize, List<RectTransform> imageRectTransforms)
    {
        foreach (RectTransform imageRectTransform in imageRectTransforms)
        {
            if (imageRectTransform != null)
            {
                double widthRatio = Screen.width / screenSize.x;
                double heightRatio = Screen.height / screenSize.y;
                double ratio = Math.Min(widthRatio, heightRatio);
                Debug.Log("ResizeImageKeepingAspect: " + ratio);
                if (imageRectTransform != null)
                {
                    Image image = imageRectTransform.GetComponent<Image>();
                    int resizeWidth = (int)(image.sprite.rect.width * ratio);
                    int resizeHeight = (int)(image.sprite.rect.height * ratio);

                    imageRectTransform.sizeDelta = new Vector2(resizeWidth, resizeHeight);
                    Debug.Log("Image resized And Keep aspect!");
                }
            }
        }
    }
    
}
