using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        //Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float differantX = Mathf.Abs(playerPos.x - myPos.x);
        float differantY = Mathf.Abs(playerPos.y - myPos.y);

        //Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (differantX > differantY)
                {
                    transform.Translate(Vector3.right * dirX * 100);
                }
                else if (differantX < differantY)
                {
                    transform.Translate(Vector3.up * dirY * 100);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir * 50 + new Vector3(Random.Range(-3f, 3f), (Random.Range(-3f, 3f)), 0f));
                }
                break;
        }
    }
}
