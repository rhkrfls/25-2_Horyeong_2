using UnityEngine;

public enum PLAYERNAME
{
    YUSEONG, SEOLHAN
}

public class Player : MonoBehaviour
{
    public PLAYERNAME PN;

    private void Awake()
    {
        PN = PLAYERNAME.YUSEONG;
    }

    public void SwapPlayer()
    {
        
    }
}
