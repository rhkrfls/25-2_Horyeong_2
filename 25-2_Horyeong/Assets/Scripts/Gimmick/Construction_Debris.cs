using UnityEngine;

public class Construction_Debris : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public int damage;


    void Start()
    {
        if (playerStatus == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerStatus = playerObj.GetComponent<PlayerStatus>();

                if (playerStatus == null)
                    Debug.LogWarning("Player 오브젝트에 PlayerStatus 컴포넌트가 없습니다!");
            }
            else
            {
                Debug.LogWarning("'Player' 태그가 지정된 오브젝트를 찾을 수 없습니다!");
            }
        }
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerStatus.TakeDamage(damage);
    }
}
