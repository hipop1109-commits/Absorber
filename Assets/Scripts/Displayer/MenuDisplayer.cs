using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuDisplayer : MonoBehaviour
{
    // ë©”ë‰´ ì°½
    [SerializeField] private GameObject menuPanel;

    // íƒ­
    [SerializeField]private GameObject resolutionTab;
    [SerializeField]private GameObject soundTab;

    // ë³¼ë¥¨ ìŠ¬ë¼ì´ë”
    [SerializeField]private Slider volumeSlider;

    //  í•´ìƒë„ ë“œë¡­ë‹¤ìš´
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    // í•´ìƒë„ ëª©ë¡
    private Resolution[] resolutions;

    // ë²„íŠ¼ í…ìŠ¤íŠ¸
    [SerializeField] private TextMeshProUGUI ButtonText;

    public TextMeshProUGUI[] slotTexts; // ¼¼ÀÌºê ½½·Ô »óÅÂ¸¦ Ç¥½ÃÇÒ TextMeshPro ¹è¿­ (3°³)
    public GameObject saveMenuPanel;   // ¼¼ÀÌºê ¸Ş´º UI ÆĞ³Î


    void Start()
    {
        // ë³¼ë¥¨ ìŠ¬ë¼ì´ë” ì´ˆê¸°í™”
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // í•´ìƒë„ ì„¤ì • ì´ˆê¸°í™”
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();


        // í•´ìƒë„ ì˜µì…˜ ì¶”ê°€
        foreach (Resolution res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + " x " + res.height));
        }
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // ë²„íŠ¼ í…ìŠ¤íŠ¸ ì´ˆê¸°í™”
        UpdateButtonText();

        UpdateSaveSlots();
    }

    private void Update()
    {   // Esc ë²„íŠ¼ ì‹œ ë©”ë‰´ ì˜¤í”ˆ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    // ë©”ë‰´ë²„íŠ¼ 
    public void OpenMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        Time.timeScale = menuPanel.activeSelf ? 0 : 1;
    }
    // ë©”ë‰´ ë‹«ìŒ 
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
    
    // ì „ì²´ ë³¼ë¥¨ ì¡°ì ˆ
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; 
    }

    // í•´ìƒë„ ì„¤ì •
    public void SetResolution(int index)
    {
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen );
    }

    // í’€ìŠ¤í¬ë¦° ì˜¨ì˜¤í”„
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        UpdateButtonText();
        
    }
    // í’€ìŠ¤í¬ë¦° ì˜¨ì˜¤í”„ í…ìŠ¤íŠ¸ ì—…ë°ì´íŠ¸
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
    // ½½·Ô »óÅÂ °»½Å
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

    // ½½·Ô Å¬¸¯
    public void OnSlotClicked(int slot)
    {
        if (GameManager.IsSlotEmpty(slot))
        {
            GameManager.SaveGame(slot); // »õ µ¥ÀÌÅÍ¸¦ ÀúÀå
            Debug.Log("Game Saved in Slot " + slot);
        }
        else
        {
            var data = GameManager.LoadGame(slot); // µ¥ÀÌÅÍ¸¦ ºÒ·¯¿È
            Debug.Log("Game Loaded from Slot " + slot + ": Level " + data.playerLife);
            // ¿©±â¼­ µ¥ÀÌÅÍ¸¦ ±â¹İÀ¸·Î °ÔÀÓ »óÅÂ¸¦ ¾÷µ¥ÀÌÆ®
        }

        UpdateSaveSlots(); // UI °»½Å
    }

    // ¼¼ÀÌºê ¸Ş´º ¿­±â
    public void OpenSaveMenu()
    {
        soundTab.SetActive(false);
        resolutionTab.SetActive(false);
        saveMenuPanel.SetActive(true);
        UpdateSaveSlots();
    }
    // »ç¿îµå ÅÇ ¿­±â
    public void OpenSoundTab()
    {
        soundTab.SetActive(true);
        resolutionTab.SetActive(false);
        saveMenuPanel.SetActive(false);
    }
    // í•´ìƒë„ íƒ­ ì—´ê¸°
    public void OpenResolutionTab()
    {
        soundTab.SetActive(false);
        resolutionTab.SetActive(true);
        saveMenuPanel.SetActive(false);
    }
}
