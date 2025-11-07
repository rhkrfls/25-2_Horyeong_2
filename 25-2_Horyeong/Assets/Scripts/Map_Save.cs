using UnityEngine;

public class Map_Save : MonoBehaviour
{
    public bool isChecking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChecking = true;
            Debug.Log("저장 지점 도착!");
        }
    }

}
