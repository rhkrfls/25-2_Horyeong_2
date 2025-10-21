using UnityEngine;
using TMPro; // TextMeshPro ����� ���� �ʿ�
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    // Inspector���� ������ UI ������Ʈ
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    // Inspector���� ������ ��ȭ ������
    public Dialogue currentDialogue;

    private int currentLineIndex = 0;

    void Start()
    {
        // ���� �� ��ȭâ ��Ȱ��ȭ
        dialoguePanel.SetActive(false);
    }

    // ��ȭ ���� �Լ�
    public void StartDialogue(Dialogue dialogueToStart)
    {
        currentDialogue = dialogueToStart;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true); // ��ȭâ Ȱ��ȭ
        DisplayNextLine();
    }

    public void LoadAndStartDialogue(string csvFileName)
    {
        // 1. Assets/Resources �������� TextAsset �ε�
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError($"CSV ���� �ε� ����: Resources/{csvFileName}�� ã�� �� �����ϴ�.");
            return;
        }

        // 2. CSV �ļ��� Dialogue ��ü ����
        Dialogue loadedDialogue = CSVParser.ParseCSV(csvFile);

        // 3. ��ȭ ����
        if (loadedDialogue != null && loadedDialogue.lines.Count > 0)
        {
            StartDialogue(loadedDialogue);
        }
        else
        {
            Debug.LogWarning("�ε�� ��ȭ ���Ͽ� ��ȿ�� ��� ���� �����ϴ�.");
        }
    }

    // ���� ��ȭ�� ��� �Լ� (����� �Է� ������ ȣ���)
    public void DisplayNextLine()
    {
        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        // ���� ���� ������ ��������
        DialogueLine line = currentDialogue.lines[currentLineIndex];

        // �̸� ���
        nameText.text = line.characterName;

        // �ؽ�Ʈ �ڷ�ƾ ���� (Ÿ���� ȿ��)
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.text));

        currentLineIndex++;
    }

    // Ÿ���� ȿ�� ���� (�ڷ�ƾ)
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // 0.05�� �������� ���� ���
        }
    }

    // ��ȭ ���� �Լ�
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("��ȭ�� ����Ǿ����ϴ�.");
    }
}
