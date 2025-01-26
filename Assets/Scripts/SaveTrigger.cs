using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private SaveManager saveManager;

    private void Start()
    {
        saveManager = SaveManager.Instance;

        if (saveManager == null)
        {
            Debug.LogError("SaveManager instance is null!");
        }
    }

    // ´êÀ¸¸é ÀúÀå 
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            saveManager.SaveGame();
        }
    }
}
