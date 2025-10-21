using UnityEngine;
using System.Collections.Generic;

// Inspector â���� ���� ���� �����ϵ��� ����
[System.Serializable]
public class DialogueLine
{
    public string characterName; // ���ϴ� ĳ���� �̸�
    [TextArea(3, 10)]
    public string text;          // ���� ��� ����
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> lines;
}