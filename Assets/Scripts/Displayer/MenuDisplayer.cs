using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuDisplayer : MonoBehaviour
{
    // 메뉴 창
    public GameObject menuPanel;

    // 탭
    public GameObject resolutionTab;
    public GameObject soundTab;

    // 볼륨 슬라이더
    public Slider volumeSlider;

    //  해상도 드롭다운
    public TMP_Dropdown resolutionDropdown;

    // 해상도 목록
    private Resolution[] resolutions;

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
    }
    // 메뉴버튼 
    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
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
