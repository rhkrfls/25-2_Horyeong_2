using Unity.Cinemachine;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("시네머신 카메라")]
    // Inspector에서 Virtual Camera를 드래그하여 연결할 변수
    public CinemachineCamera virtualCamera;

    [Header("새로운 트래킹 타겟")]
    // 코드로 설정할 새로운 타겟 오브젝트의 Transform
    public Transform newTarget;

    void Start()
    {
        // 씬 시작 시 트래킹 타겟을 newTarget으로 설정합니다.
        GameObject target = GameObject.Find("Player");
        newTarget = target.transform;
        SetTrackingTarget(newTarget);
    }

    /// <summary>
    /// Cinemachine Virtual Camera의 Follow 타겟을 설정하는 함수
    /// </summary>
    /// <param name="targetTransform">새로운 타겟 오브젝트의 Transform</param>
    public void SetTrackingTarget(Transform targetTransform)
    {
        // 1. Virtual Camera 컴포넌트가 제대로 연결되었는지 확인합니다.
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera가 연결되지 않았습니다! Inspector를 확인해주세요.");
            return;
        }

        // 2. Virtual Camera의 Follow 속성에 새로운 Transform을 할당합니다.
        virtualCamera.Follow = targetTransform;

        // 타겟이 Look At도 사용하는 경우, LookAt 속성도 필요에 따라 설정할 수 있습니다.
        // virtualCamera.LookAt = targetTransform;

        Debug.Log($"Cinemachine 카메라의 트래킹 타겟이 '{targetTransform.name}'(으)로 설정되었습니다.");
    }

    // 예시: 게임 도중 타겟을 동적으로 변경하는 함수
    public void ChangeTargetDynamically(GameObject newGameObject)
    {
        if (newGameObject != null)
        {
            SetTrackingTarget(newGameObject.transform);
        }
    }
}
