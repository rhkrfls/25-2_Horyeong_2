using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    // 싱글턴. singleton 1개... 싱글턴화를 시켜 씬 이동시에도 파괴가 안되도록한다.
    static public CanvasManager instance;

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
        Main_Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas.worldCamera = Main_Cam;
        Invoke("CanvasSortOrder", 0.35f);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Camera Main_Cam;


    void Start()
    {
        canvas = GetComponent<Canvas>();
        Main_Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {

    }

    private void CanvasSortOrder()
    {
        canvas.sortingOrder = 5;
    }
}
