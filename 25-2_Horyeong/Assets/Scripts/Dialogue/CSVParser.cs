using UnityEngine;
using System.Collections.Generic;

public static class CSVParser
{
    private const char LineSeparator = '\n'; // �� ������
    private const char FieldSeparator = '\t'; // �ʵ�(ĭ) ������

    // CSV ������ �о� Dialogue ��ü�� �����Ͽ� ��ȯ�ϴ� ���� �Լ�
    public static Dialogue ParseCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
            return null;
        }

        Dialogue dialogue = new Dialogue { lines = new List<DialogueLine>() };

        // 1. ��ü �ؽ�Ʈ�� �� ������ �и�
        string[] lines = csvFile.text.Split(LineSeparator);

        // ù ��(���)�� �ǳʶٱ� ���� 1���� ����
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim(); // ���� ����
            if (string.IsNullOrEmpty(line)) continue;

            // 2. �� ���� �ʵ�(ĭ) ������ �и�
            string[] fields = line.Split(FieldSeparator);

            // CSV ������ �°� (Index, Name, Dialogue) 3�� �ʵ尡 �ִ��� Ȯ��
            if (fields.Length >= 3)
            {
                DialogueLine dialogueLine = new DialogueLine
                {
                    // 2��(�ε��� 1) = Name
                    characterName = fields[1].Trim(),

                    // 3��(�ε��� 2) = Dialogue
                    text = fields[2].Trim().Replace("\"", "") // ����ǥ ����
                };

                dialogue.lines.Add(dialogueLine);
            }
        }

        return dialogue;
    }
}