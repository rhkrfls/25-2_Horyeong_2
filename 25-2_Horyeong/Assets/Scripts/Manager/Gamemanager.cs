using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    private float GroogyCount = 1f;

    public bool isGroggy = false;
    public bool shouldEvaluate = true;


    void Start()
    {
        
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
        Debug.Log("�׷α� Ǯ��!");
        isGroggy = false;
        shouldEvaluate = true;
    }
}
