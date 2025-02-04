using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 재생 가능한 오디오 타입 열거형 정의
    public enum AudioType
    {
        PlayerWalk, PlayerJump, PlayerDash, PlayerDie, PlayerHeal,
        PlayerWaterAttack, PlayerShootAttack, PlayerBombAttack, PlayerBomb, PlayerHealThrow,
        WeaponChange, WeaponAbsorb,
        Get,
        GameOver, GameClear,
        EnemyDie, EnemyHurt
    }

    // 버튼 클릭 및 마우스 오버 사운드
    public AudioSource clickSound;
    public AudioSource startButtonOverSound;
    public AudioSource buttonOverSound;
    public AudioSource sliderSound;


    [System.Serializable]
    // 오디오 데이터를 담는 구조체
    public struct Audio
    {
        public AudioType type; // 오디오 타입
        public AudioSource audioSource; // 오디오 소스
    }

    public Audio[] audios; // 여러 오디오 데이터를 저장
    private Dictionary<AudioType, AudioSource> audioDic; // 오디오 타입과 소스를 매핑하는 딕셔너리

    // 싱글톤 인스턴스
    private static AudioManager instance;
    public static AudioManager Instance => instance; // 외부에서 접근 가능한 정적 프로퍼티

    private void Awake()
    {
        // 싱글톤 패턴 구현: 중복 AudioManager 방지
        if (Instance != this && Instance != null)
        {
            Destroy(this); // 현재 인스턴스 파괴
            return;
        }
        else
        {
            instance = this; // 현재 인스턴스를 싱글톤으로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        InitializeAudioDictionary();
    }

    // 버튼 클릭 사운드 재생
    public void ButtonSound()
    {
        clickSound.Play();
    }

    // 버튼 마우스 오버 사운드 재생
    public void ButtonOverSound()
    {
        buttonOverSound.Play();
    }

    // 스타트 버튼 마우스 오버 사운드 재생
    public void StartButtonOverSound()
    {
        startButtonOverSound.Play();
    }

    // 슬라이드 사운드 재생
    public void SliderSound()
    {
        sliderSound.Play();
    }

    // 오디오 딕셔너리 초기화
    private void InitializeAudioDictionary()
    {
        audioDic = new Dictionary<AudioType, AudioSource>();
        foreach (var audio in audios)
        {
            audioDic[audio.type] = audio.audioSource; // 오디오 타입을 키로 설정
        }
    }

    // 특정 오디오 타입의 사운드 재생
    public void PlaySound(AudioType audioType)
    {
        if (audioDic.TryGetValue(audioType, out AudioSource audioSource))
        {
            audioSource.Play(); // 다시 재생
        }
        else
        {
            Debug.Log($"{audioType}가 없습니다."); // 없는 타입에 대한 경고
        }
    }

    // 특정 오디오 타입의 사운드 정지
    public void StopSound(AudioType audioType)
    {
        if (audioDic.TryGetValue(audioType, out AudioSource audioSource))
        {
            audioSource.Stop();
        }
        else
        {
            Debug.Log($"{audioType}가 없습니다."); // 없는 타입에 대한 경고
        }
    }

    //흡수처럼 꾹 눌러서 계속 나와야하는 애들
    public void PlayLoopSound(AudioType type)
    {
        if (audioDic.TryGetValue(type, out AudioSource source))
        {
            if (!source.isPlaying)
            {
                source.loop = true; // 루프 설정
                source.Play();
            }
        }
    }

    public void StopLoopSound(AudioType type)
    {
        if (audioDic.TryGetValue(type, out AudioSource source))
        {
            if (source.isPlaying)
            {
                source.loop = false; // 루프 해제
                source.Stop();
            }
        }
    }
}
