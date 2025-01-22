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
    // 사운드 탭 열기
    public void OpenSoundTab()
    {
        soundTab.SetActive(true);
        resolutionTab.SetActive(false);
    }
    // 해상도 탭 열기
    public void OpenResolutionTab()
    {
        soundTab.SetActive(false );
        resolutionTab.SetActive(true);
    }
}
