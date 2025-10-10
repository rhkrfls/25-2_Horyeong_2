using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]   // ������ ����ȭ
public class Sound
{
    public string name; // Bgm �̸�
    public AudioClip clip;  // Bgm
}

public class SoundManager : MonoBehaviour
{
    // �̱���. singleton 1��... �̱���ȭ�� ���� �� �̵��ÿ��� �ı��� �ȵǵ����Ѵ�.
    static public SoundManager instance;
    private void Awake()    // ��ü ������ ���� ����
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); �� �ı� �ȵǵ��� ����
        }
        else
            Destroy(this.gameObject);
    }

    // ����� �ͼ�
    public AudioMixer audioMixer;

    // �����̴�
    public Slider BgmSlider;
    public Slider SfxSlider;

    public AudioSource[] audioSourceEffects;    // ���� ����θ� ���� ����ϱ� ���� �迭�� ����
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];  // ����� �ҽ� ������ �÷��� ���� ������ ������ ��ġ��Ų��
    }

    // ���� ����
    public void SetBgmVolme()
    {
        // �α� ���� �� ����
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void SetSfxVolme()
    {
        // �α� ���� �� ����
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }



    public void PlaySoundEffect(string _name)   // _name�� �Ѿ���� _name�� ��ġ�ϴ� Sound Class �ȿ��ִ� name�� ã�� Sound Class�ȿ� ������ clip�� ����� �ҽ��ȿ� �־ ����
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� AudioSource�� ������Դϴ�.");
                return;
            }
        }
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�");
    }

    public void StopAllSoundEffect()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSoundEffect(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�.");
    }
}
