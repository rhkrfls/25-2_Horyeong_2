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
                    Debug.LogWarning("Player ������Ʈ�� PlayerStatus ������Ʈ�� �����ϴ�!");
            }
            else
            {
                Debug.LogWarning("'Player' �±װ� ������ ������Ʈ�� ã�� �� �����ϴ�!");
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
