using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Telapote : MonoBehaviour
{
    public bool isPortal;
    public GameObject thePlayer;
    public GameObject teleportPos;

    public Image PadeScreen;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float fadeTime;

    [SerializeField]
    private string Zoom;

    private void Update()
    {
        if (!isPortal)
        {
            TelapotePlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.teleport();
    }

    private void TelapotePlayer()
    {
        if (GameManager.isTelapote)
        {
            thePlayer.transform.position = this.teleportPos.transform.position;
            thePlayer.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void teleport()
    {
        StartCoroutine(ShowEndGame(0, 1));
        StartCoroutine(Wait());
        //SoundManager.instance.PlaySoundEffect(Zoom);
    }

    private IEnumerator ShowEndGame(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            //f fadeTime으로 나누어서 fadeTime 시간 동안 percent 값이 0 에서 1로 증가하도록 함
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 알파값을 start부터 end까지 fadeTime 시간 동안 변화시킨다.
            Color color = PadeScreen.color;
            color.a = Mathf.Lerp(start, end, percent);
            PadeScreen.color = color;

            yield return null;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        thePlayer.transform.position = this.teleportPos.transform.position;
        thePlayer.transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ShowEndGame(1, 0));
    }
}
