using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public RuntimeAnimatorController animatorController;

    public PLAYERNAME currentPlayerCharachter;

    public float maxMoveSpeed = 7f;
    public float accelerationRate = 10f;
    public float jumpForce = 8f;

    public float mass = 2.5f;
    public float gravityScale = 3f;
}
