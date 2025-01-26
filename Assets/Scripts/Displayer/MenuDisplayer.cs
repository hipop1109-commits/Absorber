using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor.Overlays;

public class MenuDisplayer : MonoBehaviour
{
    // Menu Panel
    [SerializeField] private GameObject menuPanel;

    private bool isGamePaused = false;

    // 메뉴 내부 패널
    [SerializeField]private GameObject savePanel;  
    [SerializeField]private GameObject videoPanel;
    [SerializeField] private GameObject soundPanel;
    
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
    
    // 사운드 볼륨슬라이더
    [SerializeField] private Slider MasterVolumeSlider;

    // 세이브 슬롯 텍스트
    [SerializeField] private TextMeshProUGUI saveSlotText;
    private SaveManager saveManager;

    private void OnEnable()
    {
        // 밝기 설정 초기화
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0;
            brightnessSlider.value = 0;
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }
        // 메뉴 활성화시 초기화
        brightnessSlider.value = colorAdjustments.postExposure.value;
        MasterVolumeSlider.value = AudioListener.volume;

        UpdateOnOffText();
    }

    void Start()
    {
        // 슬라이더 초기화
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        MasterVolumeSlider.onValueChanged.AddListener(SetVolume);

        // 해상도 리스트 추가
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }

        saveManager = SaveManager.Instance;
        UpdateSaveSlotUI();
        
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
        Time.timeScale = menuPanel.activeSelf ? 0 : 1;

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
    }
    
    // 볼륨 설정
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
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
        Screen.fullScreen = !Screen.fullScreen;
        UpdateOnOffText();
        
    }
    // 풀스크린 온오프
    private void UpdateOnOffText()
    {
        if(Screen.fullScreen)
        {
            OnOffButtonText.text = "On";
        }
        else
        {
            OnOffButtonText.text = "Off";
        }
    }

    public void UpdateSaveSlotUI()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();

        if (saveData != null)
        {
            saveSlotText.text = $"HP: {saveData.playerHP}, Energy: {saveData.energyCore}, " +
                                $"({saveData.lastSavedTime})";
        }
        else
        {
            saveSlotText.text = "Empty Slot";
        }
    }

    public void OnLoadClicked()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();
        SaveManager.Instance.LoadGameData(saveData);
    }
  
    // 세이브 메뉴 오픈
    public void OpenSaveMenu()
    {
        soundPanel.SetActive(false);
        videoPanel.SetActive(false);
        savePanel.SetActive(true);
    }
    // 사운드 메뉴 오픈
    public void OpenSoundMenu()
    {
        soundPanel.SetActive(true);
        videoPanel.SetActive(false);
        savePanel.SetActive(false);
    }
    // 비디오 메뉴 오픈 
    public void OpenVideoMenu()
    {
        soundPanel.SetActive(false);
        videoPanel.SetActive(true);
        savePanel.SetActive(false);
    }
}
