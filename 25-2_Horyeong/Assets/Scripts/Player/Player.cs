using UnityEngine;

public enum PLAYERNAME
{
    YUSEONG, SEOLHAN
}

public class Player : MonoBehaviour
{
    public PLAYERNAME PN;

    public GameObject yuseong;
    public GameObject seolhan;

    public CharacterData dataYuseong;
    public CharacterData dataSeolhan;

    private PlayerController activeController;
    private float swapCooldown = 1.0f;

    private void Start()
    {
        // 초기 설정: A 캐릭터 활성화 및 데이터 로드
        PN = PLAYERNAME.YUSEONG;
        seolhan.SetActive(false);
        activeController = yuseong.GetComponent<PlayerController>();
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
        if (yuseong.activeSelf)
        {
            // A -> B 스왑
            CharacterSwapLogic(yuseong, seolhan, dataSeolhan);
        }
        else
        {
            // B -> A 스왑
            CharacterSwapLogic(seolhan, yuseong, dataYuseong);
        }

        swapCooldown = 1.0f; // 쿨타임 초기화
    }

    private void CharacterSwapLogic(GameObject prevObj, GameObject nextObj, CharacterData nextData)
    {
        // 이전 캐릭터 위치를 새 캐릭터에게 전달
        nextObj.transform.position = prevObj.transform.position;

        // 오브젝트 활성화/비활성화
        prevObj.SetActive(false);
        nextObj.SetActive(true);

        // 새 캐릭터 컨트롤러에게 데이터 로드
        activeController = nextObj.GetComponent<PlayerController>();
        activeController.LoadCharacter(nextData);

        // 카메라 타겟 변경 (필요하다면)
        // CameraManager.Instance.SetTarget(nextObj.transform);
    }
}

