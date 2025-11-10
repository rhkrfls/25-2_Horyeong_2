using UnityEngine;
using System.Collections.Generic;

// Inspector 창에서 쉽게 편집 가능하도록 설정
[System.Serializable]
public class DialogueLine
{
    public string characterName; // 말하는 캐릭터 이름
    [TextArea(3, 10)]
    public string text;          // 실제 대사 내용
    public string objectName;    // 어떤 오브젝트와 상호작용을 하였는지
    public bool selection;      // 선택지가 있는지
    public string selectionText1; // 선택지 1 텍스트
    public string selectionText2; // 선택지 2 텍스트
    public string type;        // 대화 타입 (예: 일반 대화, 퀘스트 수락 등)
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> lines;
}