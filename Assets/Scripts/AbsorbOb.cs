using UnityEngine;

public class AbsorbOb : MonoBehaviour
{
    public float fadeSpeed = 0.5f; // 투명해지는 속도 (작을수록 느림)
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 자신의 SpriteRenderer 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 특정 태그를 가진 오브젝트만 작동
        if (other.CompareTag("AbsorbEffect"))
        {
            // 오브젝트의 SpriteRenderer 가져오기
            Color currentColor = spriteRenderer.color;

            if (spriteRenderer != null)
            {
                // 알파값(투명도)을 점점 감소
                float newAlpha = Mathf.Max(currentColor.a - (fadeSpeed * Time.deltaTime), 0);
                spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

                // 알파값이 0이 되면 오브젝트 비활성화
                if (newAlpha <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

