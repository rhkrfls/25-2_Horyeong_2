using System;
using UnityEngine;

// 파일로 저장/불러오기 가능하도록 설정
[System.Serializable]
public class GameData
{
    // === 공용 체력 ===
    public float currentHealth;

    // === 캐릭터 정보 (현재 활성화된 캐릭터) ===
    public string activeCharacterName; // 예: "Yuseong", "PlayerB"
    public float playerPositionX;
    public float playerPositionY;

    // === 게임 진행 상황 ===
    public string lastSceneName;
    public int score;

    // 초기 데이터 설정용 생성자
    public GameData()
    {
        currentHealth = 100f; // 초기 체력
        activeCharacterName = "Yuseong";
        playerPositionX = 0f;
        playerPositionY = 0f;
        lastSceneName = "CharacterScene";
        score = 0;
    }
}