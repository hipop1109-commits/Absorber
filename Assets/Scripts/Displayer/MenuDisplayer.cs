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

    public TextMeshProUGUI[] slotTexts; // 세이브 슬롯 상태를 표시할 TextMeshPro 배열 (3개)
    public GameObject saveMenuPanel;   // 세이브 메뉴 UI 패널


    void Start()
    {
        // 볼륨 슬라이더 초기화
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // 해상도 설정 초기화
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();


        // 해상도 옵션 추가
        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // 버튼 텍스트 초기화
        UpdateButtonText();

        UpdateSaveSlots();
    }

    private void Update()
    {   // Esc 버튼 시 메뉴 오픈
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    // 메뉴버튼 
    public void OpenMenu()
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
    // 슬롯 상태 갱신
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

    // 슬롯 클릭
    public void OnSlotClicked(int slot)
    {
        if (GameManager.IsSlotEmpty(slot))
        {
            GameManager.SaveGame(slot); // 새 데이터를 저장
            Debug.Log("Game Saved in Slot " + slot);
        }
        else
        {
            var data = GameManager.LoadGame(slot); // 데이터를 불러옴
            Debug.Log("Game Loaded from Slot " + slot + ": Level " + data.playerLife);
            // 여기서 데이터를 기반으로 게임 상태를 업데이트
        }

        UpdateSaveSlots(); // UI 갱신
    }

    // 세이브 메뉴 열기
    public void OpenSaveMenu()
    {
        soundTab.SetActive(false);
        resolutionTab.SetActive(false);
        saveMenuPanel.SetActive(true);
        UpdateSaveSlots();
    }
    // 사운드 탭 열기
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
