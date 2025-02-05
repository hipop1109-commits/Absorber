using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.Overlays;
using UnityEditor.ShaderGraph.Internal;
#endif

public class MenuDisplayer : MonoBehaviour
{
    // Menu Panel
    [SerializeField] private GameObject menuPanel;

    private bool isGamePaused = false;
    
    // 메뉴 내부 패널
    [SerializeField]private GameObject savePanel;  
    [SerializeField]private GameObject videoPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [SerializeField] private GameObject loadNoticePanel;

    // 해상도 드롭다운
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    // 해상도 리스트
    private Resolution[] resolutions;

    // 온오프 버튼
    [SerializeField] private TextMeshProUGUI OnOffButtonText;

    // 밝기 슬라이더
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    [SerializeField] private AudioMixer audioMixer;
    // 사운드 볼륨슬라이더
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    // 세이브 슬롯 텍스트
    [SerializeField] private TextMeshProUGUI saveSlotText;
    private SaveManager saveManager;

    private bool isFullScreen;

    public static MenuDisplayer instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bool isFullScreen = Screen.fullScreen;
    }
    private void OnEnable()
    {
        // 밝기 설정 초기화
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0;
            brightnessSlider.value = 0;
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }
        // 밝기 리스너 등록
        brightnessSlider.value = colorAdjustments.postExposure.value;

        UpdateOnOffText();
    }

    void Start()
    {
        // 오디오 슬라이더 저장 값 불러오기 , 없으면 기본 값 1
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // 오디오믹서의 현재 볼륨을 슬라이더 값으로 변환
        audioMixer.GetFloat("MasterVolume", out float masterDb);
        masterVolumeSlider.value = Mathf.Pow(10, masterDb / 20);
        audioMixer.GetFloat("MasterVolume", out float bgmDb);
        masterVolumeSlider.value = Mathf.Pow(10, bgmDb / 20);
        audioMixer.GetFloat("MasterVolume", out float sfxDb);
        masterVolumeSlider.value = Mathf.Pow(10, sfxDb / 20);


        // 리스너 등록
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        masterVolumeSlider.onValueChanged.AddListener((value) => {
            SetMasterVolume(value);
            SliderSoundPlay();
        });
        bgmVolumeSlider.onValueChanged.AddListener((value) => {
            SetBGMVolume(value);
            SliderSoundPlay();
        });
        sfxVolumeSlider.onValueChanged.AddListener((value) => {
            SetSFXVolume(value);
            SliderSoundPlay();
        });


        // 초기 오디오 볼륨 설정
        SetMasterVolume(masterVolumeSlider.value);
        SetBGMVolume(bgmVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);

        // 해상도 리스트 추가
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }

        saveManager = SaveManager.Instance;
        UpdateSaveSlotUI();
        // 초기 풀스크린 상태를 반영
        UpdateOnOffText();
    }

    private void Update()
    {   // Esc 로 메뉴 패널 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    // 메뉴 패널 열기
    public void OpenMenu()
    {
        isGamePaused = !isGamePaused;
        menuPanel.SetActive(isGamePaused);
        Time.timeScale = menuPanel.activeSelf ? 0 : 1; // 메뉴 활성화시 클릭 막기 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // 메뉴 패널 닫기
    public void CloseMenu()
    {
        isGamePaused  = false;
        menuPanel.SetActive(false);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 밝기 조절 값
    private void AdjustBrightness(float value)
    {
        colorAdjustments.postExposure.value = Mathf.Lerp(-1f, 1f, value);
        SliderSoundPlay();
    }

    private void SliderSoundPlay()
    {
        AudioManager.Instance.SliderSound();
    }
    
    // 전체 볼륨 설정
    public void SetMasterVolume(float volume)
    {
        float dbVolume = (volume > 0.0001f) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dbVolume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

        // 오디오 믹서 값 변경 후 슬라이더 반영
        audioMixer.GetFloat("MasterVolume", out float newDbVolume);
        masterVolumeSlider.value = Mathf.Pow(10, newDbVolume / 20);
    }
    // 배경음 설정
    public void SetBGMVolume(float volume)
    {
        float dbVolume = (volume > 0.0001f) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("BGMVolume", dbVolume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
        audioMixer.GetFloat("BGMVolume", out float newDbVolume);
        bgmVolumeSlider.value = Mathf.Pow(10, newDbVolume / 20);
    }
    // 효과음 설정
    public void SetSFXVolume(float volume)
    {
        float dbVolume = (volume > 0.0001f) ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("SFXVolume", dbVolume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        audioMixer.GetFloat("SFXVolume", out float newDbVolume);
        sfxVolumeSlider.value = Mathf.Pow(10, newDbVolume / 20);
    }
    // 해상도 설정
    public void SetResolution(int index)
    {
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen );
    }

    // 풀스크린 버튼
    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen; // 로컬 변수로 상태 관리
        Screen.fullScreen = isFullScreen;
        UpdateOnOffText();
    }

    // 풀스크린 온오프
    private void UpdateOnOffText()
    {
        OnOffButtonText.text = isFullScreen ? "On" : "Off";
    }

    // 세이브 정보 슬롯 텍스트 업데이트
    public void UpdateSaveSlotUI()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();
        
        if (saveData != null)
        {
            saveSlotText.text = $"({saveData.lastSavedTime})";
                                
            //$"HP: {saveData.playerHP}, Energy: {saveData.energyCore}, " + $
        }
        else
        {
            saveSlotText.text = "Empty Slot";
        }
    }
    // 클릭시 저장 게임 로드 
    public void OnLoadClicked()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();
        if (saveData != null)
        {
            SaveManager.Instance.LoadGameData(saveData);
            menuPanel.SetActive(false);
            menuPanel.SetActive(false);

            Time.timeScale = 1;
            PlayerController.instance.Restart();
            UpdateSaveSlotUI();
        }
        else
        {
            loadNoticePanel.SetActive(true);
        }
    }

    // 로딩 패널 버튼을 눌렀을 때 실행되는 함수
    public void OnClickYesLoadingPanel()
    {
        // 현재 활성화된 씬의 이름을 가져옴
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName != "StartScene")
        {
            // 현재 씬이 StartScene이 아니면 StartScene으로 이동
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            // 현재 씬이 StartScene이면 지정된 오브젝트 비활성화
            loadNoticePanel.SetActive(false);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");

    }

    // 세이브 메뉴 오픈
    public void OpenSaveMenu()
    {
        soundPanel.SetActive(false);
        videoPanel.SetActive(false);
        savePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    // 사운드 메뉴 오픈
    public void OpenSoundMenu()
    {
        soundPanel.SetActive(true);
        videoPanel.SetActive(false);
        savePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }
    // 비디오 메뉴 오픈 
    public void OpenVideoMenu()
    {
        soundPanel.SetActive(false);
        videoPanel.SetActive(true);
        savePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }
    // 메인 메뉴 오픈 
    public void OpenMainMenu()
    {
        soundPanel.SetActive(false);
        videoPanel.SetActive(false);
        savePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
