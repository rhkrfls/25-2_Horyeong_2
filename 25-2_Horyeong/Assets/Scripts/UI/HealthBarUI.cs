using UnityEngine;
using UnityEngine.UI; // Slider 사용을 위해 필요

public class HealthBarUI : MonoBehaviour
{
    // Inspector에 연결할 UI Slider 컴포넌트
    public Slider healthSlider;

    public PlayerStatus healthManager;

    void Start()
    {
        // 1. HealthManager 인스턴스 찾기 (씬에 하나만 있다고 가정)
        healthManager = FindAnyObjectByType<PlayerStatus>();

        if (healthManager != null)
        {
            // 2. 이벤트 구독: 체력 변경 이벤트가 발생하면 UpdateHealthBar 함수를 실행하도록 등록
            healthManager.OnHealthChanged += UpdateHealthBar;

            // 초기 체력 상태를 UI에 반영
            UpdateHealthBar(healthManager.GetCurrentHp(), healthManager.GetmaxHp());
        }
        else
        {
            Debug.LogError("HealthManager를 씬에서 찾을 수 없습니다! 연결 실패.");
        }
    }

    // HealthManager 이벤트가 호출될 때 실행되는 함수
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // 3. Slider의 최대값을 설정
        healthSlider.maxValue = maxHealth;

        // 4. Slider의 현재 값(체력)을 설정
        healthSlider.value = currentHealth;
    }

    // 씬에서 오브젝트가 파괴될 때 이벤트 구독 해제 (메모리 누수 방지)
    private void OnDestroy()
    {
        if (healthManager != null)
        {
            healthManager.OnHealthChanged -= UpdateHealthBar;
        }
    }
}