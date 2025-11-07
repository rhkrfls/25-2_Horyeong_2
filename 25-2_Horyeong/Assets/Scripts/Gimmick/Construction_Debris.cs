using UnityEngine;

public class Construction_Debris : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public int damage;


    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        playerStatus.TakeDamage(damage, this.transform);
    }
}
