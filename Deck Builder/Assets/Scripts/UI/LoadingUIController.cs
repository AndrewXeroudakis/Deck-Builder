using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingUIController : MonoBehaviour
{
    #region Variables
    [Header("Loading UI Elements")]
    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    RectTransform rectTransformTopPanel;
    [SerializeField]
    RectTransform rectTransformBottomPanel;
    [SerializeField]
    RectTransform rectTransformCenterPanel;
    [SerializeField]
    RectTransform rectTransformPokeballImage;
    [SerializeField]
    CanvasGroup canvasGroup;

    static readonly float fadeDuration = 0.20f;
    static readonly float scaleUpDuration = 0.75f;
    static readonly float scaleDownDuration = 0.35f;
    static readonly float sizeDuration = 0.5f;
    static readonly float moveLeftRightDuration = 0.75f;

    static readonly float initialWidth = 1920;
    static readonly float initialHeight = 100f;
    static readonly float heightOffset = 200f;
    static readonly float moveOffsetX = 30f;
    static readonly float rotationOffestZ = 45;

    Tween moveLeftRight;
    Tween rotateLeftRight;

    bool isInitializing;
    public bool IsInitializing { get { return isInitializing; } private set { isInitializing = value; } }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        ResetValues();

        // Deactivate loadingScreen
        loadingScreen.SetActive(false);
    }
    #endregion

    #region Methods
    void ResetValues()
    {
        // Screen is initializing
        isInitializing = true;

        // CanvasGroup
        canvasGroup.alpha = 0f;

        // Panels
        rectTransformCenterPanel.localScale = new Vector3(0f, 0f, 0f);
        rectTransformTopPanel.sizeDelta = new Vector2(initialWidth, initialHeight);
        rectTransformBottomPanel.sizeDelta = new Vector2(initialWidth, initialHeight);

        // Pokeball Image
        rectTransformPokeballImage.anchoredPosition = new Vector2(moveOffsetX, 0f);
        rectTransformPokeballImage.rotation = Quaternion.Euler(0f, 0f, -rotationOffestZ);
    }

    public void DisplayLoadingScreen(bool _display)
    {
        if (_display)
            StartCoroutine(StartLoading());
        else
            StartCoroutine(EndLoading());
    }

    IEnumerator StartLoading()
    {
        ResetValues();

        // Activate loadingScreen
        loadingScreen.SetActive(true);

        // Fade in
        canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);

        // Scale up
        Tween scaleUp = rectTransformCenterPanel.DOScale(1, scaleUpDuration).SetEase(Ease.OutBack);

        // Height increase
        rectTransformTopPanel.DOSizeDelta(new Vector2(initialWidth, heightOffset), sizeDuration).SetEase(Ease.OutSine);
        rectTransformBottomPanel.DOSizeDelta(new Vector2(initialWidth, heightOffset), sizeDuration).SetEase(Ease.OutSine);

        // Move pokeball left-right
        moveLeftRight = rectTransformPokeballImage.DOLocalMove(new Vector3(-moveOffsetX, 0f, 0f), moveLeftRightDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        rotateLeftRight = rectTransformPokeballImage.DORotate(new Vector3(0, 0, rotationOffestZ), moveLeftRightDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        yield return scaleUp.WaitForCompletion();

        // Screen finished initializing
        isInitializing = false;
    }

    IEnumerator EndLoading()
    {
        // Height decrease
        rectTransformTopPanel.DOSizeDelta(new Vector2(initialWidth, initialHeight), sizeDuration).SetEase(Ease.OutSine);
        rectTransformBottomPanel.DOSizeDelta(new Vector2(initialWidth, initialHeight), sizeDuration).SetEase(Ease.OutSine);

        // Scale down
        rectTransformCenterPanel.DOScale(0, scaleDownDuration).SetEase(Ease.InOutSine);

        // Fade out
        Tween fadeOut = canvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);

        yield return fadeOut.WaitForCompletion();

        // Deactivate loadingScreen
        loadingScreen.SetActive(false);

        // Kill looping tweens
        moveLeftRight.Kill();
        rotateLeftRight.Kill();
    }
    #endregion
}
