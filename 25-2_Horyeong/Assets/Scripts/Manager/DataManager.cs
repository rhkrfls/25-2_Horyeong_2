using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;
    public GameData gameData;

    private string dataPath;
    private string saveFileName = "savegame.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dataPath = Path.Combine(Application.persistentDataPath, saveFileName);

            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        // 1. 저장 데이터를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(gameData, true); // true는 가독성(Pretty Print)을 높여줍니다.

        try
        {
            // 2. 파일에 쓰기 (System.IO.File 사용)
            File.WriteAllText(dataPath, json);
            Debug.Log($"[Save] 게임 데이터가 저장되었습니다: {dataPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Save Error] 데이터 저장 실패: {e.Message}");
        }
    }

    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            try
            {
                // 1. 파일에서 JSON 문자열 읽어오기 (System.IO.File 사용)
                string json = File.ReadAllText(dataPath);

                // 2. JSON 문자열을 GameData 객체로 변환
                gameData = JsonUtility.FromJson<GameData>(json);

                Debug.Log("[Load] 게임 데이터 로드 성공.");

            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Load Error] 데이터 로드 실패. 새 데이터를 생성합니다. 오류: {e.Message}");
                // 로드 실패 시 새 데이터 생성
                gameData = new GameData();
            }
        }
        else
        {
            // 저장된 파일이 없을 경우 새 데이터 생성
            Debug.Log("[Load] 저장 파일이 존재하지 않습니다. 새 데이터를 생성합니다.");
            gameData = new GameData();
        }
    }

    // ----------------------------------------------------
    // 데이터를 업데이트하고 저장하는 예시 함수
    // ----------------------------------------------------
    public void UpdateAndSavePlayerPosition(PlayerController player)
    {
        PlayerStatus playerHP = FindObjectOfType<PlayerStatus>();

        gameData.currentHealth = playerHP.GetmaxHp();
        gameData.activeCharacterName = player.currentData.currentPlayerCharachter.ToString();
        gameData.playerPositionX = player.transform.position.x;
        gameData.playerPositionY = player.transform.position.y;
        //gameData.lastSceneName = "CharacterScene";

        SaveData();
    }
}
