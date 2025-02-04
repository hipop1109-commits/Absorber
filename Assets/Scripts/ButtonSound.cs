using UnityEngine;

public class ButtonSound : MonoBehaviour 
{ 
    public AudioSource hoverSound; // 버튼 위로 마우스가 올라갈 때 재생할 소리

    public AudioSource clickSound; // 버튼 위로 마우스가 올라갈 때 재생할 소리
    private void Start()
    {

    }

    public void PlayHoverSound()
    {
        hoverSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }
}

