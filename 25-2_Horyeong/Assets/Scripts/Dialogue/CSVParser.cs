using UnityEngine;
using System.Collections.Generic;

public static class CSVParser
{
    private const char LineSeparator = '\n'; // 줄 구분자
    private const char FieldSeparator = '\t'; // 필드(칸) 구분자

    // CSV 파일을 읽어 Dialogue 객체를 생성하여 반환하는 정적 함수
    public static Dialogue ParseCSV(TextAsset csvFile)
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
            return null;
        }

        Dialogue dialogue = new Dialogue { lines = new List<DialogueLine>() };

        // 1. 전체 텍스트를 줄 단위로 분리
        string[] lines = csvFile.text.Split(LineSeparator);

        // 첫 줄(헤더)은 건너뛰기 위해 1부터 시작
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim(); // 공백 제거
            if (string.IsNullOrEmpty(line)) continue;

            // 2. 각 줄을 필드(칸) 단위로 분리
            string[] fields = line.Split(FieldSeparator);

            DialogueLine dialogueLine = new DialogueLine
            {
                type = fields[1].Trim(), // 1열(인덱스 0) = Type
                selection = fields[2].Trim().ToLower() == "true", // 5열 = Selection (true/false)
                selectionText1 = fields[3].Trim(), // 6열 = SelectionText1
                selectionText2 = fields[4].Trim(),  // 7열 = SelectionText
                objectName = fields[5].Trim(), // 2열(인덱스 1) = ObjectName
                characterName = fields[6].Trim(),
                text = fields[7].Trim().Replace("\"", ""), // 따옴표 제거
            };

            dialogue.lines.Add(dialogueLine);
        }

        return dialogue;
    }
}