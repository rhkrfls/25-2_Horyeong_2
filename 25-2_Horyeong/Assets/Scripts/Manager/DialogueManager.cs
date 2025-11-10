using UnityEngine;
using TMPro; // TextMeshPro 사용을 위해 필요
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("스토리")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public GameObject talkPanel;
    public TextMeshProUGUI talkText;

    public GameObject selectionPanel;
    public TextMeshProUGUI selectionText1;
    public TextMeshProUGUI selectionText2;

    // Inspector에서 연결할 대화 데이터
    public Dialogue currentDialogue;
    public bool isTypewriting = false;
    private int currentLineIndex = 0;

    public static DialogueManager Instance;

    void Start()
    {
        Instance = this;
        // 시작 시 대화창 비활성화
        dialoguePanel.SetActive(false);
        talkPanel.SetActive(false);
        selectionPanel.SetActive(false);
    }

    // 대화 시작 함수
    public void StartDialogue(Dialogue dialogueToStart)
    {
        currentDialogue = dialogueToStart;

        CheckDialogueType();
    }

    public void LoadAndStartDialogue(string csvFileName, string objectName)
    {
        // 1. Assets/Resources 폴더에서 TextAsset 로드
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);
        currentLineIndex = 0;

        if (csvFile == null)
        {
            Debug.LogError($"CSV 파일 로드 실패: Resources/{csvFileName}을 찾을 수 없습니다.");
            return;
        }

        // 2. CSV 파서로 Dialogue 객체 생성
        Dialogue loadedDialogue = CSVParser.ParseCSV(csvFile);

        if (loadedDialogue != null && loadedDialogue.lines[currentLineIndex].type != "dialogue")
        {
            while (loadedDialogue.lines[currentLineIndex].objectName != objectName)
            {
                currentLineIndex++;
                if (currentLineIndex >= loadedDialogue.lines.Count)
                {
                    Debug.LogWarning($"대화 파일에 '{objectName}'에 해당하는 대화가 없습니다.");
                    return;
                }
            }
        }

        // 3. 대화 시작
        if (loadedDialogue.lines.Count > 0)
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
        if (currentDialogue.lines[currentLineIndex].type == "end")
        {
            EndDialogue();
            return;
        }

        // 현재 줄의 데이터 가져오기
        DialogueLine line = currentDialogue.lines[currentLineIndex];
        string currentType = line.type;

        // 텍스트 코루틴 시작 (타이핑 효과)
        StopAllCoroutines();

        if (isTypewriting)
            skipTypeSentence(line.text, currentType);

        else
            StartCoroutine(TypeSentence(line.text, currentType));
    }

    public void SkipText()
    {
        if (currentDialogue == null) return;

        StopAllCoroutines();

        while (currentDialogue.lines[currentLineIndex].type != "end")
        {
            currentLineIndex++;
        }

        EndDialogue();
    }

    public void skipTypeSentence(string sentence, string type)
    {
        StopAllCoroutines();

        if (type == "dialogue")
            dialogueText.text = sentence;

        if (type == "talk")
            talkText.text = sentence;

        isTypewriting = false;
        currentLineIndex++;
    }

    // 타이핑 효과 구현 (코루틴)
    IEnumerator TypeSentence(string sentence, string type)
    {
        isTypewriting = true;

        if (type == "dialogue")
            dialogueText.text = "";

        if (type == "talk")
            talkText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (type == "dialogue")
                dialogueText.text += letter;

            if (type == "talk")
                talkText.text += letter;

            yield return new WaitForSeconds(0.05f); // 0.05초 간격으로 글자 출력
        }
        isTypewriting = false;
        currentLineIndex++;
    }

    // 대화 종료 함수
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        talkPanel.SetActive(false);
        Debug.Log("대화가 종료되었습니다.");
    }

    private void SetTalkPanelPosition()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();

        talkPanel.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.4f, player.transform.position.z);
    }

    public void CheckDialogueType()
    {
        if (currentDialogue.lines[currentLineIndex].type == "dialogue")
        {
            dialoguePanel.SetActive(true);
            talkPanel.SetActive(false);

            DisplayNextLine();
        }

        else if (currentDialogue.lines[currentLineIndex].type == "talk")
        {
            talkPanel.SetActive(true);
            dialoguePanel.SetActive(false);

            SetTalkPanelPosition();
            DisplayNextLine();
        }

        else if (currentDialogue.lines[currentLineIndex].type == "select")
        {
            dialoguePanel.SetActive(true);
            talkPanel.SetActive(false);
            selectionPanel.SetActive(true);

            dialogueText.text = currentDialogue.lines[currentLineIndex].text;

            dialogueText.alignment = TextAlignmentOptions.Top;
            selectionText1.text = currentDialogue.lines[currentLineIndex].selectionText1;
            selectionText2.text = currentDialogue.lines[currentLineIndex].selectionText2;
        }
    }
}
