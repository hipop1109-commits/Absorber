using UnityEngine;
using UnityEngine.UI;

public class GaugeDisplayer : MonoBehaviour
{
    // 게이지 이미지
    public Image rockGaugeImage; 
    public Image grassGaugeImage;
    public Image waterGaugeImage;

    private static GaugeDisplayer instance;
    public static GaugeDisplayer Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // 게이지 차오르는 이미지
        rockGaugeImage.fillAmount = WeaponController.Instance.RockGauge / WeaponController.Instance.MaxGauge;
        grassGaugeImage.fillAmount = WeaponController.Instance.GrassGauge / WeaponController.Instance.MaxGauge;
        waterGaugeImage.fillAmount = WeaponController.Instance.WaterGauge / WeaponController.Instance.MaxGauge;
    }
}
