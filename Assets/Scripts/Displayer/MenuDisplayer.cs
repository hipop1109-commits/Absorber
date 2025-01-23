using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuDisplayer : MonoBehaviour
{
    // 메뉴 창
    [SerializeField] private GameObject menuPanel;

    // 탭
    [SerializeField]private GameObject resolutionTab;
    [SerializeField]private GameObject soundTab;

    // 볼륨 슬라이더
    [SerializeField]private Slider volumeSlider;

    //  해상도 드롭다운
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    // 해상도 목록
    private Resolution[] resolutions;

    // 버튼 텍스트
    [SerializeField] private TextMeshProUGUI ButtonText;

    public TextMeshProUGUI[] slotTexts; // ���̺� ���� ���¸� ǥ���� TextMeshPro �迭 (3��)
    public GameObject saveMenuPanel;   // ���̺� �޴� UI �г�


    private void OnEnable()
    {
        // 메뉴 활성화시 초기화
        brightnessSlider.value = colorAdjustments.postExposure.value;
        MasterVolumeSlider.value = AudioListener.volume;

        UpdateOnOffText();          UpdateSaveSlots();
    }      void Start()     {         // 밝기 설정 초기화         if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0;
            brightnessSlider.value = 0;
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        }         else
        {
            Debug.LogError("밝기 설정이 되지 않았어요");
        }
        // 슬라이더 초기화
        resolutionDropdown.onValueChanged.AddListener(SetResolution);         MasterVolumeSlider.onValueChanged.AddListener(SetVolume);          // 해상도 리스트 추가         resolutions = Screen.resolutions;         resolutionDropdown.ClearOptions();          foreach (Resolution res in resolutions)         {             resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));         }              }      private void Update()     {   // Esc 로 메뉴 패널 활성화         if (Input.GetKeyDown(KeyCode.Escape))         {             OpenMenu();         }     }      // 메뉴 패널 열기     public void OpenMenu()     {         isGamePaused = !isGamePaused;         menuPanel.SetActive(isGamePaused);         Time.timeScale = menuPanel.activeSelf ? 0 : 1;

        Cursor.lockState = CursorLockMode.None;         Cursor.visible = true;     }     // 메뉴 패널 닫기     public void CloseMenu()     {         isGamePaused  = false;         menuPanel.SetActive(false);          Time.timeScale = 1;          Cursor.lockState = CursorLockMode.None;         Cursor.visible = true;     }      // 밝기 조절 값     private void AdjustBrightness(float value)
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        Time.timeScale = menuPanel.activeSelf ? 0 : 1;
    }
    // 메뉴 닫음 
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
    
    // 전체 볼륨 조절
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

    // 풀스크린 온오프
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        UpdateButtonText();
        
    }
    // 풀스크린 온오프 텍스트 업데이트
    private void UpdateButtonText()
    {
        if(Screen.fullScreen)
        {
            ButtonText.text = "On";
        }
        else
        {
            ButtonText.text = "Off";
        }
    }
    // ���� ���� ����
    public void UpdateSaveSlots()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            int slot = i + 1;
            if (GameManager.IsSlotEmpty(slot))
            {
                slotTexts[i].text = "Slot " + slot + ": Empty";
            }
            else
            {
                var data = GameManager.LoadGame(slot);
                slotTexts[i].text = "Slot " + slot + ": " + data.playerPosition;
            }
        }
    }

    // ���� Ŭ��
    public void OnSlotClicked(int slot)
    {
        if (GameManager.IsSlotEmpty(slot))
        {
            GameManager.SaveGame(slot); // �� �����͸� ����
            Debug.Log("Game Saved in Slot " + slot);
        }
        else
        {
            var data = GameManager.LoadGame(slot); // �����͸� �ҷ���
            Debug.Log("Game Loaded from Slot " + slot + ": Level " + data.playerLife);
            // ���⼭ �����͸� ������� ���� ���¸� ������Ʈ
        }

        UpdateSaveSlots(); // UI ����
    }

    // ���̺� �޴� ����
    public void OpenSaveMenu()
    {
        soundTab.SetActive(false);
        resolutionTab.SetActive(false);
        saveMenuPanel.SetActive(true);
        UpdateSaveSlots();
    }
    // ���� �� ����
    public void OpenSoundTab()
    {
        soundTab.SetActive(true);
        resolutionTab.SetActive(false);
        saveMenuPanel.SetActive(false);
    }
    // 해상도 탭 열기
    public void OpenResolutionTab()
    {
        soundTab.SetActive(false);
        resolutionTab.SetActive(true);
        saveMenuPanel.SetActive(false);
    }
}
