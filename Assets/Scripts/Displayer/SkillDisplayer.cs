using TMPro;
using UnityEngine;

public class SkillDisplayer : MonoBehaviour
{
    // 에너지 코어 txt
    public TextMeshProUGUI energyCoreText;

    // 스킬 설정창
    public GameObject skillPanel;

    private static SkillDisplayer instance;
    public static SkillDisplayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SkillDisplayer>(); // 자동으로 찾기
            }
            return instance;
        }
    }

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
    }


    // 스킬창 열림
    public void OpenSkillMenu()
    {
        skillPanel.SetActive(true);
        Time.timeScale = skillPanel.activeSelf ? 0 : 1;
    }
    // 스킬창 닫음
    public void CloseSkillMenu()
    {
        skillPanel.SetActive(false);
    }
}
