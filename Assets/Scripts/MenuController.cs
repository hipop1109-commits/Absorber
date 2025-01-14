using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // 메뉴 창
    public GameObject menuPanel;
    // 스킬 설정 창
    public GameObject skillPanel;
    // 볼륨 슬라이더
    public Slider volumeSlider; 

    void Start()
    {
        // 슬라이더 초기화
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
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
    // 스킬창 열림
    public void OpenSkillMenu()
    {
        skillPanel.SetActive(true);
    }
    // 스킬창 닫음
    public void CloseSkillMenu()
    {
        skillPanel.SetActive(false);
    }
    // 전체 볼륨 조절
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
    }
}
