using UnityEngine;

public class ButtonSound : MonoBehaviour 
{ 
    public AudioClip hoverSound; // 버튼 위로 마우스가 올라갈 때 재생할 소리
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = hoverSound;
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            Debug.Log("play");
            audioSource.Play();
        }
    }
}

