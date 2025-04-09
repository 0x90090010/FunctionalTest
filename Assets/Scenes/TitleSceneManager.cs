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
    private GameObject titleBackgroundCanvasObject;
    public GameObject titleBackgroundImageObject;

    void Start()
    {
        SetTitleBackground();
        resizeCanvas = titleBackgroundCanvasObject.AddComponent<ResizeManager>();
        resizeImage = titleBackgroundImageObject.AddComponent<ResizeManager>();
        screenSize = new Vector2(Screen.width, Screen.height);
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
                resizeImage.ResizeImage();
                screenSize = new Vector2(Screen.width, Screen.height);
            }
            else
            {
                Debug.LogWarning("Image is null");
            }

            Debug.Log("ResizeEnd");
        }
    }

    void SetTitleBackground()
    {
        // Canvas用のゲームオブジェクトを作成
        titleBackgroundCanvasObject = new GameObject("TitleBackgroundCanvasObject");

        // Canvasコンポーネントを追加
        Canvas titleBackgroundCanvas = titleBackgroundCanvasObject.AddComponent<Canvas>();

        // レンダーモードをScreenSpaceCameraに設定
        titleBackgroundCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        titleBackgroundCanvasObject.AddComponent<GraphicRaycaster>();

        // EventSystemを生成(ボタンやイベントで遷移などをする際には必ず必要)
        GameObject eventSystemObject = new GameObject("TitleEventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();

        // image用のゲームオブジェクトを作成
        titleBackgroundImageObject = new GameObject("TitleBackgroundImage");

        // imageコンポーネントを追加
        Image titleBackgroundImage = titleBackgroundImageObject.AddComponent<Image>();

        // canvasの子要素に設定
        titleBackgroundImageObject.transform.SetParent(titleBackgroundCanvas.transform, false);

        // RectTransformの設定
        RectTransform rectTransform = titleBackgroundImageObject.GetComponent<RectTransform>();

        // 仮の背景
        Texture2D titleBackgroundTexture = Resources.Load<Texture2D>("DefaultTitleBackground");
        Sprite sprite = Sprite.Create(titleBackgroundTexture, new Rect(0, 0, titleBackgroundTexture.width, titleBackgroundTexture.height), new Vector2(0.5f, 0.5f));

        // 画像のサイズをRectTransformに設定
        rectTransform.sizeDelta = new Vector2(titleBackgroundTexture.width, titleBackgroundTexture.height);

        // 画像を表示
        titleBackgroundImage.sprite = sprite;
    }

}
