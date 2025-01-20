using UnityEngine;

public class SkillDisplayer : MonoBehaviour
{
    // 스킬 설정창
    public GameObject skillPanel;

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
