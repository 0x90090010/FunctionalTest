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
        // ResizeImageの仕様
        // https://0x90090010.atlassian.net/wiki/pages/resumedraft.action?draftId=65701&draftShareId=754a3d08-0f33-434b-a9f4-289c5e82c1dd



        if (imageRectTransform != null)
        {
            Image image = imageRectTransform.GetComponent<Image>();
            Debug.Log("size: " + image.sprite.rect.width + ", " + image.sprite.rect.height);
            double ratio = 1;
            int imageWidth = (int)image.sprite.rect.width;
            int imageHeight = (int)image.sprite.rect.height;
            int resizeWidth = 1;
            int resizeHeight = 1;

            if (Screen.width <= Screen.height && image.sprite.rect.width <= image.sprite.rect.height)             
            {
                Debug.Log("Pattern A");
                ratio = (double)Screen.width / (double)imageWidth;
                Debug.Log("ratio: " + ratio);
                resizeWidth = (int)(imageWidth * ratio);
                resizeHeight = (int)(imageHeight * ratio);
            }
            else if (Screen.width > Screen.height && image.sprite.rect.width > image.sprite.rect.height)
            {
                Debug.Log("Pattern B");
                ratio = (double)Screen.height / (double)imageHeight;
                Debug.Log("ratio: " + ratio);
                resizeWidth = (int)(imageWidth * ratio);
                resizeHeight = (int)(imageHeight * ratio);

                if (resizeWidth < Screen.width)
                {
                    Debug.Log("Pattern B - 1");
                    Debug.Log("Width: " + resizeWidth + "," +  Screen.width);
                    ratio = (double)Screen.width / (double)resizeWidth;
                    resizeWidth = (int)(resizeWidth * ratio);
                    resizeHeight = (int)(resizeHeight * ratio);
                }
            }
            else if (Screen.width > Screen.height && image.sprite.rect.width <= image.sprite.rect.height)
            {
                Debug.Log("Pattern C");
                ratio = (double)Screen.height / (double)imageHeight;
                Debug.Log("ratio: " + ratio);
                resizeWidth = (int)(imageWidth * ratio);
                resizeHeight = (int)(imageHeight * ratio);

                if (resizeWidth < Screen.width)
                {
                    Debug.Log("Pattern C - 1");
                    Debug.Log("Width: " + resizeWidth + "," + Screen.width);
                    ratio = (double)Screen.width / (double)resizeWidth;
                    resizeWidth = (int)(resizeWidth * ratio);
                    resizeHeight = (int)(resizeHeight * ratio);
                }
            }
            else // (Screen.width <= Screen.height && imageRectTransform.sizeDelta.x > imageRectTransform.sizeDelta.y)
            {
                Debug.Log("Pattern D");
                ratio = (double)Screen.width / (double)imageWidth;
                Debug.Log("ratio: " + ratio);
                resizeWidth = (int)(imageWidth * ratio);
                resizeHeight = (int)(imageHeight * ratio);
                Debug.Log("resize D: " + resizeWidth + ", " + resizeHeight);
                if (resizeHeight < Screen.height)
                {
                    Debug.Log("Pattern D-1");
                    Debug.Log("Height: " + resizeHeight + "," + Screen.height);
                    ratio = (double)Screen.height / (double)resizeHeight;
                    resizeWidth = (int)(resizeWidth * ratio);
                    resizeHeight = (int)(resizeHeight * ratio);
                    Debug.Log("resize D-1: " + resizeWidth + ", " + resizeHeight);
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
