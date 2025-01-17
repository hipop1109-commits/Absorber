using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayer : MonoBehaviour
{
    // 플레이어 생명 이미지
    public List<GameObject> lifeImages;

   
    // 생명 이미지 활성화, 비활성화
    public void SetLives(int life)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < life)
            {
                lifeImages[i].SetActive(true);
            }
            else
            {
                lifeImages[i].SetActive(false);
            }
        }
    }
    
}
