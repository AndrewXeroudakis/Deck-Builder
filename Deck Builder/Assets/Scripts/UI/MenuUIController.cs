using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    #region Variables
    [Header("Menu UI Elements")]
    public GameObject menuContainer;
    public GameObject menuOptionsContainer;
    public GameObject popUpContainer;
    [Space]
    [Header("Deck Builder")]
    public Button deckBuilderButton;
    [Space]
    [Header("Play")]
    public Button playButton;
    public GameObject playContainer;
    public Button playOkButton;
    [Space]
    [Header("Options")]
    public Button optionsButton;
    public GameObject optionsContainer;
    public Slider soundEffectsVolumeSlider;
    public Slider musicVolumeSlider;
    public Button resetButton;
    public Button yesButton;
    [Space]
    [Header("About")]
    public Button aboutButton;
    public GameObject aboutContainer;
    public Button aboutOkButton;
    [Space]
    [Header("Quit")]
    public GameObject quitContainer;
    public Button quitButton;
    public Button quitYesButton;
    public Button quitNoButton;
    [Space]
    [Header("Other")]
    [SerializeField]
    CanvasGroup canvasGroup;

    // Music and Sound
    static readonly string SOUNDEFFECTS_VOLUME_KEY = "soundEffectsVolume";
    static readonly string MUSIC_VOLUME_KEY = "musicVolume";
    static readonly float defaultSoundEffectsVolume = 0.5f;
    static readonly float defaultMusicVolume = 0.5f;
    AudioSource menuMusic;

    // Pop Up Values
    static readonly float moveDuration = 0.3f;
    static readonly float moveOffsetY = -1000f;
    bool popUpIsMoving;
    public bool PopUpIsMoving { get { return popUpIsMoving; } private set { popUpIsMoving = value; } }
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        SubscribeButtons();
        SubscribeEvents();
    }
    #endregion

    #region Methods
    void SubscribeButtons()
    {
        // Deck Builder
        deckBuilderButton.onClick.AddListener(DeckBuilderButtonPressed);

        // Play Game
        playButton.onClick.AddListener(PlayButtonPressed);
        playOkButton.onClick.AddListener(PlayOKButtonPressed);

        // Options
        optionsButton.onClick.AddListener(OptionsButtonPressed);
        resetButton.onClick.AddListener(ResetButtonPressed);
        yesButton.onClick.AddListener(YesButtonPressed);

        // About
        aboutButton.onClick.AddListener(AboutButtonPressed);
        aboutOkButton.onClick.AddListener(AboutOKButtonPressed);

        // Quit
        quitButton.onClick.AddListener(QuitButtonPressed);
        quitYesButton.onClick.AddListener(QuitYesButtonPressed);
        quitNoButton.onClick.AddListener(QuitNoButtonPressed);
    }

    void SubscribeEvents()
    {
        soundEffectsVolumeSlider.onValueChanged.AddListener(delegate { SetVolume(SOUNDEFFECTS_VOLUME_KEY, soundEffectsVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { SetVolume(MUSIC_VOLUME_KEY, musicVolumeSlider.value); });
    }

    public void OpenMainMenu()
    {
        // Unause
        Time.timeScale = 1;

        // Disable popUpContainer
        popUpContainer.SetActive(false);

        // Enable menu options container
        menuOptionsContainer.SetActive(true);

        // Enable menu container
        menuContainer.SetActive(true);

        // Play Music
        /*if (menuMusic == null)
            menuMusic = AudioManager.Instance.PlayMusic("MenuMusic");
        else
        {
            menuMusic.Stop();
            menuMusic.Play();
        }*/
    }

    #region Display Scripts
    void ResetPopUpValues(RectTransform _popUpRectTransform)
    {
        // Position
        _popUpRectTransform.anchoredPosition = new Vector2(0f, moveOffsetY);
    }

    void DisplayPopUp(GameObject _popUp, bool _display)
    {
        if (_display)
            StartCoroutine(StartPopUp(_popUp));
        else
            StartCoroutine(EndPopUp(_popUp));
    }

    IEnumerator StartPopUp(GameObject _popUp)
    {
        RectTransform rectTransform = _popUp.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // PopUp started moving
            popUpIsMoving = true;

            ResetPopUpValues(rectTransform);

            // Deactivate canvas
            canvasGroup.interactable = false;

            // Enable popUpContainer
            popUpContainer.SetActive(true);

            // Activate popUp
            _popUp.SetActive(true);

            // Move popUp up
            Tween moveUp = rectTransform.DOLocalMove(new Vector3(0f, 0f, 0f), moveDuration).SetEase(Ease.OutBack);

            yield return moveUp.WaitForCompletion();

            // Enable canvas
            canvasGroup.interactable = true;

            // PopUp finished moving
            popUpIsMoving = false;
        }
    }

    IEnumerator EndPopUp(GameObject _popUp)
    {
        RectTransform rectTransform = _popUp.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // PopUp started moving
            popUpIsMoving = true;

            // Deactivate canvas
            canvasGroup.interactable = false;

            // Move popUp down
            Tween moveDown = rectTransform.DOLocalMove(new Vector3(0f, moveOffsetY, 0f), moveDuration).SetEase(Ease.InBack);

            yield return moveDown.WaitForCompletion();

            // Disable popUpContainer
            popUpContainer.SetActive(false);

            // Deactivate popUp
            _popUp.SetActive(false);

            // Enable canvas
            canvasGroup.interactable = true;

            // PopUp finished moving
            popUpIsMoving = false;
        }
    }
    #endregion

    #region Deck Builder
    void DeckBuilderButtonPressed()
    {
        // Stop music
        /*if (menuMusic != null)
            menuMusic.Stop();*/

        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Disable Menu
        menuContainer.SetActive(false);

        // Open Deck Builder UI
        UIManager.Instance.deckBuilderUIController.Open();
    }
    #endregion

    #region Play
    void PlayButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Display popUp
        DisplayPopUp(playContainer, true);
    }

    void PlayOKButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Hide popUp
        DisplayPopUp(playContainer, false);
    }
    #endregion

    #region Options
    void OptionsButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Load volumes
        LoadVolume();

        // Display popUp
        DisplayPopUp(optionsContainer, true);
    }

    void SetVolume(string _key, float _value)
    {
        AudioManager.Instance.audioMixer.SetFloat(_key, Mathf.Log10(_value) * 20);
    }

    void SaveVolume()
    {
        PlayerPrefs.SetFloat(SOUNDEFFECTS_VOLUME_KEY, soundEffectsVolumeSlider.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolumeSlider.value);
    }

    void LoadVolume()
    {
        if (PlayerPrefs.HasKey(SOUNDEFFECTS_VOLUME_KEY))
            soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat(SOUNDEFFECTS_VOLUME_KEY);
        else
            soundEffectsVolumeSlider.value = defaultSoundEffectsVolume;

        if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        else
            musicVolumeSlider.value = defaultMusicVolume;
    }

    void ResetButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Reset values
        soundEffectsVolumeSlider.value = defaultSoundEffectsVolume;
        musicVolumeSlider.value = defaultMusicVolume;
    }

    void YesButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        //Save to player prefs
        SaveVolume();

        // Hide popUp
        DisplayPopUp(optionsContainer, false);
    }
    #endregion

    #region About
    void AboutButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Display popUp
        DisplayPopUp(aboutContainer, true);
    }

    void AboutOKButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Hide popUp
        DisplayPopUp(aboutContainer, false);
    }
    #endregion

    #region Quit
    void QuitButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Display popUp
        DisplayPopUp(quitContainer, true);
    }

    void QuitYesButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        Quit();
    }

    void QuitNoButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Display popUp
        DisplayPopUp(quitContainer, false);
    }

    void Quit()
    {
        Application.Quit();
    }
    #endregion

    #endregion
}
