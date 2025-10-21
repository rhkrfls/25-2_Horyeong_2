using UnityEngine;
using System.Collections.Generic;

// Inspector 창에서 쉽게 편집 가능하도록 설정
[System.Serializable]
public class DialogueLine
{
    public string characterName; // 말하는 캐릭터 이름
    [TextArea(3, 10)]
    public string text;          // 실제 대사 내용
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> lines;
}