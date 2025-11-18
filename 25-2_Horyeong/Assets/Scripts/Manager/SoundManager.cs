using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]   // 데이터 증명화
public class Sound
{
    public string name; // Bgm 이름
    public AudioClip clip;  // Bgm
}

public class SoundManager : MonoBehaviour
{
    // 싱글턴. singleton 1개... 싱글턴화를 시켜 씬 이동시에도 파괴가 안되도록한다.
    static public SoundManager instance;

    private void Awake()    // 객체 생성시 최초 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // DontDestroyOnLoad(); 로 파괴 안되도록 막음
        }
        else
            Destroy(this.gameObject);
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playSoundName = new string[audioSourceEffects.Length];  // 오디오 소스 갯수와 플래이 사운드 네임의 갯수를 일치시킨다
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 오디오 믹서
    public AudioMixer audioMixer;

    // 슬라이더
    public Slider MasterSlider;
    public Slider BgmSlider;
    public Slider SfxSlider;

    public TMP_Text MasterVolume;
    public TMP_Text BGMVolume;
    public TMP_Text SFXVolume;
    public TMP_Text MasterVolume1;
    public TMP_Text BGMVolume1;
    public TMP_Text SFXVolume1;

    public AudioSource[] audioSourceEffects;    // 여러 오디로를 동시 재생하기 위해 배열로 선언
    public AudioSource audioSourceBgm;

    public string[] playSoundName;
    public string playBGMSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Start()
    {
        //playSoundName = new string[audioSourceEffects.Length];  // 오디오 소스 갯수와 플래이 사운드 네임의 갯수를 일치시킨다
    }

    private void Update()
    {
        MasterVolumeText();
        BGMVolumeText();
        SFXVolumeText();
    }

    // 볼륨 조절
    public void SetMasterVolume()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("BGM", (Mathf.Log10(MasterSlider.value) * 20));
        audioMixer.SetFloat("SFX", (Mathf.Log10(MasterSlider.value) * 20));

        BgmSlider.value = MasterSlider.value;
        SfxSlider.value = MasterSlider.value;
    }

    public void SetBgmVolume()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("BGM", (Mathf.Log10(BgmSlider.value) * 20));
    }

    public void SetSfxVolume()
    {
        // 로그 연산 값 전달
        audioMixer.SetFloat("SFX", (Mathf.Log10(SfxSlider.value) * 20));
    }

    private void MasterVolumeText()
    {
        if (MasterSlider.value < 0.01)
        {
            MasterVolume.text = "0";
            MasterVolume1.text = "0";
        }
        float a = MasterSlider.value * 50;
        MasterVolume.text = Mathf.FloorToInt(a) + "%";
        MasterVolume1.text = Mathf.FloorToInt(a) + "%";
    }

    private void BGMVolumeText()
    {
        if (BgmSlider.value < 0.01)
        {
            BGMVolume.text = "0";
            BGMVolume1.text = "0";
        }
        float a = BgmSlider.value * 50;
        BGMVolume.text = Mathf.FloorToInt(a) + "%";
        BGMVolume1.text = Mathf.FloorToInt(a) + "%";
    }
    private void SFXVolumeText()
    {
        if (SfxSlider.value < 0.01)
        {
            SFXVolume.text = "0";
            SFXVolume1.text = "0";
        }
        float a = SfxSlider.value * 50;
        SFXVolume.text = Mathf.FloorToInt(a) + "%";
        SFXVolume1.text = Mathf.FloorToInt(a) + "%";
    }


    public void PlaySoundBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                playBGMSoundName = bgmSounds[i].name;
                audioSourceBgm.clip = bgmSounds[i].clip;
                audioSourceBgm.Play();
                return;
            }
        }
    }


    public void PlaySoundEffect(string _name)   // _name이 넘어오면 _name과 일치하는 Sound Class 안에있는 name을 찾고 Sound Class안에 있으면 clip을 오디오 소스안에 넣어서 실행
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
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다");
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
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }
}
