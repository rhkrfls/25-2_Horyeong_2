using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    private float GroogyCount = 1f;

    public bool isGroggy = false;
    public bool shouldEvaluate = true;

    public static Gamemanager Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (shouldEvaluate && isGroggy)
        {
            StartCoroutine(GroggyPlayer());
            shouldEvaluate = false;
        }
    }

    public IEnumerator GroggyPlayer()
    {
        yield return new WaitForSeconds(GroogyCount);
        Debug.Log("그로기 풀림!");
        isGroggy = false;
        shouldEvaluate = true;
    }
}
