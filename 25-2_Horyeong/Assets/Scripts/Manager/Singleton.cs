using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    // 싱글턴. singleton 1개... 싱글턴화를 시켜 씬 이동시에도 파괴가 안되도록한다.
    static public Singleton instance;

    private void Awake()    // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); 로 파괴 안되도록 막음
        }
        else
            Destroy(this.gameObject);
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
}
