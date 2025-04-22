using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    private ResizeManager resizeCanvas;
    private ResizeManager resizeImage;
    private Vector2 screenSize;
    private Vector2 defaultScreenSize;
    private GameObject titleCanvasObject;
    private Canvas titleCanvas;
    private GameObject titleBackgroundImageObject;
    private TextMeshProUGUI startText;
    private float time;
    private GameObject titleSampleButtonImageObject;
    private List<RectTransform> imageRectTransforms;
    private List<RectTransform> buttonsRectTransforms;
    private List<RectTransform> buttonsTextRectTransforms;
    private List<int> buttonsTextDefaultSize;
    private List<string> buttonPositionList;

    void Start()
    {
        imageRectTransforms = new List<RectTransform>();
        buttonsRectTransforms = new List<RectTransform>();
        buttonsTextRectTransforms = new List<RectTransform>();
        buttonsTextDefaultSize = new List<int>();
        buttonPositionList = new List<string>();
        TitleMain();
        SetTitleBackground();
        SetButton();
        SetStartText();
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
                resizeImage.ResizeText(defaultScreenSize, buttonsTextRectTransforms, buttonsTextDefaultSize);
                //UpdatePositionList();
                resizeImage.UpdatePosition(buttonsRectTransforms, buttonPositionList);
                screenSize = new Vector2(Screen.width, Screen.height);
            }
            else
            {
                Debug.LogWarning("Image is null");
            }

            Debug.Log("ResizeEnd");
        }

        //Debug.Log(GetTextColorAlpha(startText.color));
        Color color = startText.color;
        color.a = GetTextColorAlpha(startText.color);
        startText.color = color;
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
        TitleUpLeftButton();
        TitleUpRightButton();
        TitleLowLeftButton();
        TitleLowRightButton();
    }

    void TitleUpRightButton()
    {
        // 座標の初期位置を設定
        int x = (int)(Screen.width) - 10;
        int y = -10;

        // 座標をテキストとしてリストに追加
        buttonPositionList.Add($"(int)(Screen.width) - 10,{y}");

        // image用のゲームオブジェクトを作成
        titleSampleButtonImageObject = new GameObject("TitleSampleButtonImage");

        // imageコンポーネントを追加
        Image titleSampleButtonImage = titleSampleButtonImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleSampleButtonImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform imageRectTransform = titleSampleButtonImageObject.GetComponent<RectTransform>();

        // 仮のボタン
        Texture2D titleSampleButtonTexture = Resources.Load<Texture2D>("SampleButton");
        Debug.Log("ButtonSize: " + titleSampleButtonTexture.width + ", " + titleSampleButtonTexture.height);
        Sprite sprite = Sprite.Create(titleSampleButtonTexture, new Rect(0, 0, titleSampleButtonTexture.width, titleSampleButtonTexture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform.sizeDelta = new Vector2(titleSampleButtonTexture.width, titleSampleButtonTexture.height);

        // ここでボタン位置を設定する
        imageRectTransform.anchorMin = new Vector2(0, 1);
        imageRectTransform.anchorMax = new Vector2(0, 1);
        imageRectTransform.pivot = new Vector2(1.0f, 1.0f); // 右上を中心
        imageRectTransform.anchoredPosition = new Vector2(x, y);

        // text用のゲームオブジェクトを作成
        GameObject titleSampleButtonTextObject = new GameObject("TitleSampleButtonText");

        // textコンポーネントを追加
        Text buttonText = titleSampleButtonTextObject.AddComponent<Text>();

        // canvasの子要素に設定
        titleSampleButtonTextObject.transform.SetParent(titleSampleButtonImageObject.transform, false);

        // textの内容を設定
        buttonText.text = "UPRIGHT";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.fontSize = 12;
        buttonsTextDefaultSize.Add(buttonText.fontSize);

        // RectTransformの設定
        RectTransform textRectTransform = buttonText.GetComponent<RectTransform>();

        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // ボタンの表示
        titleSampleButtonImage.sprite = sprite;
        buttonsRectTransforms.Add(imageRectTransform);
        buttonsTextRectTransforms.Add(textRectTransform);
    }

    void TitleUpLeftButton()
    {
        // 座標の初期位置を設定
        int x = 10;
        int y = -10;

        // 座標をテキストとしてリストに追加
        buttonPositionList.Add($"{x},{y}");

        // image用のゲームオブジェクトを作成
        titleSampleButtonImageObject = new GameObject("TitleSampleButtonImage");

        // imageコンポーネントを追加
        Image titleSampleButtonImage = titleSampleButtonImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleSampleButtonImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform imageRectTransform = titleSampleButtonImageObject.GetComponent<RectTransform>();

        // 仮のボタン
        Texture2D titleSampleButtonTexture = Resources.Load<Texture2D>("SampleButton");
        Debug.Log("ButtonSize: " + titleSampleButtonTexture.width + ", " + titleSampleButtonTexture.height);
        Sprite sprite = Sprite.Create(titleSampleButtonTexture, new Rect(0, 0, titleSampleButtonTexture.width, titleSampleButtonTexture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform.sizeDelta = new Vector2(titleSampleButtonTexture.width, titleSampleButtonTexture.height);

        // ここでボタン位置を設定する
        imageRectTransform.anchorMin = new Vector2(0, 1);
        imageRectTransform.anchorMax = new Vector2(0, 1);
        imageRectTransform.pivot = new Vector2(0.0f, 1.0f); // 左上を中心
        imageRectTransform.anchoredPosition = new Vector2(x, y);

        // text用のゲームオブジェクトを作成
        GameObject titleSampleButtonTextObject = new GameObject("TitleSampleButtonText");

        // textコンポーネントを追加
        Text buttonText = titleSampleButtonTextObject.AddComponent<Text>();

        // canvasの子要素に設定
        titleSampleButtonTextObject.transform.SetParent(titleSampleButtonImageObject.transform, false);

        // textの内容を設定
        buttonText.text = "UPLEFT";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.fontSize = 12;
        buttonsTextDefaultSize.Add(buttonText.fontSize);

        // RectTransformの設定
        RectTransform textRectTransform = buttonText.GetComponent<RectTransform>();

        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // ボタンの表示
        titleSampleButtonImage.sprite = sprite;
        buttonsRectTransforms.Add(imageRectTransform);
        buttonsTextRectTransforms.Add(textRectTransform);
    }

    void TitleLowLeftButton()
    {
        // 座標の初期位置を設定
        int x = 10;
        int y = -(int)(Screen.height) + 10;

        // 座標をテキストとしてリストに追加
        buttonPositionList.Add($"{x},-(int)(Screen.height) + 10");

        // image用のゲームオブジェクトを作成
        titleSampleButtonImageObject = new GameObject("TitleSampleButtonImage");

        // imageコンポーネントを追加
        Image titleSampleButtonImage = titleSampleButtonImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleSampleButtonImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform imageRectTransform = titleSampleButtonImageObject.GetComponent<RectTransform>();

        // 仮のボタン
        Texture2D titleSampleButtonTexture = Resources.Load<Texture2D>("SampleButton");
        Debug.Log("ButtonSize: " + titleSampleButtonTexture.width + ", " + titleSampleButtonTexture.height);
        Sprite sprite = Sprite.Create(titleSampleButtonTexture, new Rect(0, 0, titleSampleButtonTexture.width, titleSampleButtonTexture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform.sizeDelta = new Vector2(titleSampleButtonTexture.width, titleSampleButtonTexture.height);

        // ここでボタン位置を設定する
        imageRectTransform.anchorMin = new Vector2(0, 1);
        imageRectTransform.anchorMax = new Vector2(0, 1);
        imageRectTransform.pivot = new Vector2(0.0f, 0.0f); // 左下を中心
        imageRectTransform.anchoredPosition = new Vector2(x, y);

        // text用のゲームオブジェクトを作成
        GameObject titleSampleButtonTextObject = new GameObject("TitleSampleButtonText");

        // textコンポーネントを追加
        Text buttonText = titleSampleButtonTextObject.AddComponent<Text>();

        // canvasの子要素に設定
        titleSampleButtonTextObject.transform.SetParent(titleSampleButtonImageObject.transform, false);

        // textの内容を設定
        buttonText.text = "LOWLEFT";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.fontSize = 12;
        buttonsTextDefaultSize.Add(buttonText.fontSize);

        // RectTransformの設定
        RectTransform textRectTransform = buttonText.GetComponent<RectTransform>();

        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // ボタンの表示
        titleSampleButtonImage.sprite = sprite;
        buttonsRectTransforms.Add(imageRectTransform);
        buttonsTextRectTransforms.Add(textRectTransform);
    }

    void TitleLowRightButton()
    {
        // 座標の初期位置を設定
        int x = (int)(Screen.width) - 10;
        int y = -(int)(Screen.height) + 10;

        // 座標をテキストとしてリストに追加
        buttonPositionList.Add($"(int)(Screen.width) - 10,-(int)(Screen.height) + 10");

        // image用のゲームオブジェクトを作成
        titleSampleButtonImageObject = new GameObject("TitleSampleButtonImage");

        // imageコンポーネントを追加
        Image titleSampleButtonImage = titleSampleButtonImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleSampleButtonImageObject.transform.SetParent(titleCanvas.transform, false);

        // RectTransformの設定
        RectTransform imageRectTransform = titleSampleButtonImageObject.GetComponent<RectTransform>();

        // 仮のボタン
        Texture2D titleSampleButtonTexture = Resources.Load<Texture2D>("SampleButton");
        Debug.Log("ButtonSize: " + titleSampleButtonTexture.width + ", " + titleSampleButtonTexture.height);
        Sprite sprite = Sprite.Create(titleSampleButtonTexture, new Rect(0, 0, titleSampleButtonTexture.width, titleSampleButtonTexture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform.sizeDelta = new Vector2(titleSampleButtonTexture.width, titleSampleButtonTexture.height);

        // ここでボタン位置を設定する
        imageRectTransform.anchorMin = new Vector2(0, 1);
        imageRectTransform.anchorMax = new Vector2(0, 1);
        imageRectTransform.pivot = new Vector2(1.0f, 0.0f); // 右下を中心
        imageRectTransform.anchoredPosition = new Vector2(x, y);

        // text用のゲームオブジェクトを作成
        GameObject titleSampleButtonTextObject = new GameObject("TitleSampleButtonText");

        // textコンポーネントを追加
        Text buttonText = titleSampleButtonTextObject.AddComponent<Text>();

        // canvasの子要素に設定
        titleSampleButtonTextObject.transform.SetParent(titleSampleButtonImageObject.transform, false);

        // textの内容を設定
        buttonText.text = "UPLEFT";
        buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.fontSize = 12;
        buttonsTextDefaultSize.Add(buttonText.fontSize);

        // RectTransformの設定
        RectTransform textRectTransform = buttonText.GetComponent<RectTransform>();

        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // ボタンの表示
        titleSampleButtonImage.sprite = sprite;
        buttonsRectTransforms.Add(imageRectTransform);
        buttonsTextRectTransforms.Add(textRectTransform);
    }

    void SetStartText()
    {
        // text用のゲームオブジェクトを作成
        GameObject startTextObject = new GameObject("StartText");
        
        // textコンポーネントを追加
        //startText = startTextObject.AddComponent<Text>();
        startText = startTextObject.AddComponent<TextMeshProUGUI>();

        // canvasの子要素に設定
        startTextObject.transform.SetParent(titleCanvas.transform, false);

        // textの内容を設定
        startText.text = "ANIMATION TEXT";
        //startText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        startText.font = Resources.Load<TMP_FontAsset>("Fonts/LiberationSans SDF");
        //startText.alignment = TextAnchor.MiddleCenter;
        startText.alignment = TextAlignmentOptions.Center;
        startText.color = Color.white;
        startText.fontSize = 20;

        // マテリアルのインスタンスを取得（これで他のオブジェクトへの影響を防ぐ）
        startText.fontMaterial = new Material(startText.fontMaterial);

        // アウトラインを有効化するには、以下の2つのプロパティを設定する
        startText.fontMaterial.EnableKeyword("OUTLINE_ON");

        // アウトラインの色と幅を設定
        startText.outlineWidth = 0.4f; // アウトラインの幅
        startText.outlineColor = Color.green; // アウトラインの色

        // RectTransformの設定
        RectTransform textRectTransform = startText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.position = new Vector2(Screen.width / 2, Screen.height / 4);
    }

    float GetTextColorAlpha(Color color)
    {
        time += Time.deltaTime * 5f;
        float value = (Mathf.Sin(time) + 1f) / 2f;
        float alpha = Mathf.Lerp(0.5f, 1.0f, value);
        //Debug.Log(color.a);
        return alpha;
    }
}
