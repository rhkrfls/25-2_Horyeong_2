using UnityEngine;

public enum InteractionType
{
    Dialogue,
    SavePoint,
}

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]
public class InteractionData : ScriptableObject
{
    public InteractionType interactionType; //상호작용 타입
    public bool isInteracted = false;               //상호작용 여부
    public bool isTriggered = false;               //플레이어가 상호작용 범위 안에 있는지 여부
    public float interactedCooldown = 0.5f;
}
