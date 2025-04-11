using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    private ResizeManager resizeCanvas;
    private ResizeManager resizeImage;
    private Vector2 screenSize;
    private Vector2 defaultScreenSize;
    private GameObject titleCanvasObject;
    private Canvas titleCanvas;
    private GameObject titleBackgroundImageObject;
    private GameObject titleSampleButtonImageObject;
    private List<RectTransform> imageRectTransforms;
    private List<RectTransform> buttonsRectTransforms;

    void Start()
    {
        imageRectTransforms = new List<RectTransform>();
        buttonsRectTransforms = new List<RectTransform>();

        TitleMain();
        SetTitleBackground();
        SetButton();
        resizeCanvas = titleCanvasObject.AddComponent<ResizeManager>();
        resizeImage = titleBackgroundImageObject.AddComponent<ResizeManager>();
        screenSize = new Vector2(Screen.width, Screen.height);
        defaultScreenSize = new Vector2(Screen.width, Screen.height); 
    }

    void Update()
    {
        // 現在の画面サイズを取得
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

        // 画面サイズが変更されたかチェック
        if (currentScreenSize != screenSize)
        {
            if (resizeCanvas != null)
            {

                Debug.Log("canvas");
                resizeCanvas.ResizeCanvas();
                screenSize = new Vector2(Screen.width, Screen.height);
            }
            else
            {
                Debug.LogWarning("Canvas is null");
            }

            if (resizeImage != null)
            {
                Debug.Log("image");
                resizeImage.ResizeFullScreenImageOverCanvas(imageRectTransforms);
                resizeImage.ResizeImageKeepingAspect(defaultScreenSize, buttonsRectTransforms);
                screenSize = new Vector2(Screen.width, Screen.height);
            }
            else
            {
                Debug.LogWarning("Image is null");
            }

            Debug.Log("ResizeEnd");
        }
    }

    void TitleMain()
    {
        // Canvas用のゲームオブジェクトを作成
        titleCanvasObject = new GameObject("TitleCanvasObject");
        Debug.Log(titleCanvasObject);

        // Canvasコンポーネントを追加
        titleCanvas = titleCanvasObject.AddComponent<Canvas>();
        Debug.Log(titleCanvas);

        titleCanvasObject.AddComponent<GraphicRaycaster>();

        // レンダーモードをScreenSpaceCameraに設定
        titleCanvas.renderMode = RenderMode.ScreenSpaceCamera;

        // EventSystemを生成(ボタンやイベントで遷移などをする際には必ず必要)
        GameObject eventSystemObject = new GameObject("TitleEventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }

    void SetTitleBackground()
    {
        // image用のゲームオブジェクトを作成
        titleBackgroundImageObject = new GameObject("TitleBackgroundImage");

        // imageコンポーネントを追加
        Image titleBackgroundImage = titleBackgroundImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleBackgroundImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform rectTransform = titleBackgroundImageObject.GetComponent<RectTransform>();

        // 仮の背景
        Texture2D titleBackgroundTexture = Resources.Load<Texture2D>("DefaultTitleBackground");
        Sprite sprite = Sprite.Create(titleBackgroundTexture, new Rect(0, 0, titleBackgroundTexture.width, titleBackgroundTexture.height), new Vector2(0.5f, 0.5f));

        double ratio = 1;
        int resizeWidth = 1;
        int resizeHeight = 1;

        if (Screen.width <= Screen.height && titleBackgroundTexture.width <= titleBackgroundTexture.height)
        {
            ratio = (double)Screen.width / (double)titleBackgroundTexture.width;
            resizeWidth = (int)(titleBackgroundTexture.width * ratio);
            resizeHeight = (int)(titleBackgroundTexture.height * ratio);

            if (resizeHeight < Screen.height)
            {
                ratio = (double)Screen.height / (double)resizeHeight;
                resizeWidth = (int)(resizeWidth * ratio);
                resizeHeight = (int)(resizeHeight * ratio);
            }
        }
        else if (Screen.width > Screen.height && titleBackgroundTexture.width > titleBackgroundTexture.height)
        {
            ratio = (double)Screen.height / (double)titleBackgroundTexture.height;
            resizeWidth = (int)(titleBackgroundTexture.width * ratio);
            resizeHeight = (int)(titleBackgroundTexture.height * ratio);

            if (resizeWidth < Screen.width)
            {
                ratio = (double)Screen.width / (double)resizeWidth;
                resizeWidth = (int)(resizeWidth * ratio);
                resizeHeight = (int)(resizeHeight * ratio);
            }
        }
        else if (Screen.width > Screen.height && titleBackgroundTexture.width <= titleBackgroundTexture.height)
        {
            ratio = (double)Screen.height / (double)titleBackgroundTexture.height;
            resizeWidth = (int)(titleBackgroundTexture.width * ratio);
            resizeHeight = (int)(titleBackgroundTexture.height * ratio);

            if (resizeWidth < Screen.width)
            {
                ratio = (double)Screen.width / (double)resizeWidth;
                resizeWidth = (int)(resizeWidth * ratio);
                resizeHeight = (int)(resizeHeight * ratio);
            }
        }
        else // (Screen.width <= Screen.height && titleBackgroundTexture.width > titleBackgroundTexture.height)
        {
            ratio = (double)Screen.width / (double)titleBackgroundTexture.width;
            resizeWidth = (int)(titleBackgroundTexture.width * ratio);
            resizeHeight = (int)(titleBackgroundTexture.height * ratio);

            if (resizeHeight < Screen.height)
            {
                ratio = (double)Screen.height / (double)resizeHeight;
                resizeWidth = (int)(resizeWidth * ratio);
                resizeHeight = (int)(resizeHeight * ratio);
            }
        }

        rectTransform.sizeDelta = new Vector2(resizeWidth, resizeHeight);

        // 画像を表示
        titleBackgroundImage.sprite = sprite;
        imageRectTransforms.Add(rectTransform);

    }

    void SetButton()
    {
        // image用のゲームオブジェクトを作成
        titleSampleButtonImageObject = new GameObject("TitleSampleButtonImage");

        // imageコンポーネントを追加
        Image titleSampleButtonImage = titleSampleButtonImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleSampleButtonImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform rectTransform = titleSampleButtonImageObject.GetComponent<RectTransform>();

        // 仮のボタン
        Texture2D titleSampleButtonTexture = Resources.Load<Texture2D>("SampleButton");
        Sprite sprite = Sprite.Create(titleSampleButtonTexture, new Rect(0, 0, titleSampleButtonTexture.width, titleSampleButtonTexture.height), new Vector2(0.5f, 0.5f));
        rectTransform.sizeDelta = new Vector2(titleSampleButtonTexture.width, titleSampleButtonTexture.height);
        titleSampleButtonImage.sprite = sprite;
        buttonsRectTransforms.Add(rectTransform);
}
}
