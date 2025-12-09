using UnityEditor.PackageManager;
using UnityEngine;

public enum PLAYERNAME
{
    YUSEONG, SEOLHAN
}

public class Player : MonoBehaviour
{
    static public Player instance;

    public PLAYERNAME PN;

    public CharacterData dataYuseong;
    public CharacterData dataSeolhan;

    private PlayerController activeController;
    private float swapCooldown = 1.0f;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); 로 파괴 안되도록 막음
        }
        else
            Destroy(this.gameObject);

        activeController = FindAnyObjectByType<PlayerController>();

        // 초기 설정: A 캐릭터 활성화 및 데이터 로드
        PN = PLAYERNAME.YUSEONG;
        activeController.LoadCharacter(dataYuseong);
    }

    private void Update()
    {
        // 스왑 쿨타임 처리
        if (swapCooldown > 0)
        {
            swapCooldown -= Time.deltaTime;
        }
    }

    // F 키 등 스왑 입력 시 호출될 함수
    public void SwapCharacter()
    {
        if (swapCooldown > 0) return; // 쿨타임 중이면 스왑 불가

        // 1. 활성화/비활성화
        if (activeController.currentData.currentPlayerCharachter == PLAYERNAME.YUSEONG)
        {
            // A -> B 스왑
            CharacterSwapLogic(dataSeolhan);
        }
        else
        {
            // B -> A 스왑
            CharacterSwapLogic(dataYuseong);
        }

        swapCooldown = 1.0f; // 쿨타임 초기화
    }

    private void CharacterSwapLogic(CharacterData nextData)
    {
        activeController.LoadCharacter(nextData);
    }
}

