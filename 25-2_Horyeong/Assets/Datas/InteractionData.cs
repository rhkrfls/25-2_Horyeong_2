using UnityEngine;

public enum InteractionType
{
    Dialogue,
    SavePoint,
}

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]
public class InteractionData : ScriptableObject
{
    public GameObject interactionPrompt;    //상호작용 가능 오브젝트임을 표시
    public InteractionType interactionType; //상호작용 타입
    public bool isInteracted;               //상호작용 여부
    public bool isTriggered;               //플레이어가 상호작용 범위 안에 있는지 여부

}
