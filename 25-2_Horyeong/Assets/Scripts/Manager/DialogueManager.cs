using UnityEngine;
using TMPro; // TextMeshPro 사용을 위해 필요
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    // Inspector에서 연결할 UI 컴포넌트
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    // Inspector에서 연결할 대화 데이터
    public Dialogue currentDialogue;

    private int currentLineIndex = 0;

    void Start()
    {
        // 시작 시 대화창 비활성화
        dialoguePanel.SetActive(false);
    }

    // 대화 시작 함수
    public void StartDialogue(Dialogue dialogueToStart)
    {
        currentDialogue = dialogueToStart;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true); // 대화창 활성화
        DisplayNextLine();
    }

    public void LoadAndStartDialogue(string csvFileName)
    {
        // 1. Assets/Resources 폴더에서 TextAsset 로드
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError($"CSV 파일 로드 실패: Resources/{csvFileName}을 찾을 수 없습니다.");
            return;
        }

        // 2. CSV 파서로 Dialogue 객체 생성
        Dialogue loadedDialogue = CSVParser.ParseCSV(csvFile);

        // 3. 대화 시작
        if (loadedDialogue != null && loadedDialogue.lines.Count > 0)
        {
            StartDialogue(loadedDialogue);
        }
        else
        {
            Debug.LogWarning("로드된 대화 파일에 유효한 대사 줄이 없습니다.");
        }
    }

    // 다음 대화줄 출력 함수 (사용자 입력 등으로 호출됨)
    public void DisplayNextLine()
    {
        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        // 현재 줄의 데이터 가져오기
        DialogueLine line = currentDialogue.lines[currentLineIndex];

        // 이름 출력
        nameText.text = line.characterName;

        // 텍스트 코루틴 시작 (타이핑 효과)
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.text));

        currentLineIndex++;
    }

    // 타이핑 효과 구현 (코루틴)
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // 0.05초 간격으로 글자 출력
        }
    }

    // 대화 종료 함수
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("대화가 종료되었습니다.");
    }
}
