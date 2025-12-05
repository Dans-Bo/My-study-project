using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManage : MonoBehaviour
{
    //[SerializeField] private List<AudioConfig> audioConfigs = new List<AudioConfig>();
    //
    [SerializeField]private AudioConfigSO audioConfigSO;
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource voiceSource;
    private Dictionary<AudioType,AudioConfig> audioDict = new Dictionary<AudioType, AudioConfig>();
    private float bgmVolumeScale = 1f;
    private float sfxVolumeScale = 1f;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup voiceGroup;


    void Awake()
    {
       bgmSource = CreatAudioSource("BGMSource", loop:true,bgmGroup);
       sfxSource = CreatAudioSource("SFXSource",loop:false,sfxGroup);
       voiceSource = CreatAudioSource("VoiceSource",loop:false,voiceGroup);

       InitAudioDict();
    }

    /// <summary>
    /// 创建Audiosource
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    private AudioSource CreatAudioSource(string name, bool loop, AudioMixerGroup group)
    {
        GameObject sourceObj = new GameObject(name);
        sourceObj.transform.parent = transform;
        AudioSource source = sourceObj.AddComponent<AudioSource>();
        source.loop = loop;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        return source;
    }
/// <summary>
/// 初始化字典
/// </summary>
    private void InitAudioDict()
    {
        if(audioConfigSO == null || audioConfigSO.audioConfigs == null)
        {
            Debug.Log($"未配置音频文件");
            return;
        }

        audioDict.Clear();
        foreach (var config in audioConfigSO.audioConfigs)
        {
            if(config.audioClip == null)
            {
                Debug.Log($"没有绑定音频资源");
                continue;
            }
            if(!audioDict.ContainsKey(config.audioType))
            {
                audioDict.Add(config.audioType,config);
            }else Debug.LogError($"音频类型{config.audioType}重复");
        }
    }

    public void PlayBGM(AudioType type)
    {
        if(!audioDict.TryGetValue(type,out var config) || !config.isBGM)
        {
            Debug.LogError($"{type}不是BGM");
            return;
        }

        bgmSource.clip = config.audioClip;
        bgmSource.volume = config.defaulVolume*bgmVolumeScale;
        bgmSource.Play();
        /* var clip = GetAudio(type);
        if(clip != null)
        {
            //var audio = audios[sourceIndex];
            //audio.clip = clip;
           // audio.loop = isLoop;
           // audio.Play();
        } */
    }

    public void PlaySFX(AudioType type ,bool loop = false)
    {
        if(!audioDict.TryGetValue(type,out var config)|| config.isBGM)
        {
            Debug.LogError($"{type}不是音效文件");
            return;
        }

        sfxSource.clip = config.audioClip;
        sfxSource.volume = config.defaulVolume;
        sfxSource.loop = loop;
        sfxSource.Play();
    }

    public void PlayVoice(AudioType type)
    {
        if(!audioDict.TryGetValue(type,out var config)|| config.isBGM)
        {
            Debug.LogError($"{type}不是音效文件");
            return;
        }

        voiceSource.clip = config.audioClip;
        voiceSource.volume = config.defaulVolume;
        voiceSource.Play();
    }

    public void SetBgmVolume(float value)
    {
        float clampedValue = Mathf.Clamp01(value);
        float volumeDb = clampedValue > 0 ? Mathf.Log10(clampedValue) * 20 : -80; // 音量为0时设为-80dB（静音）
        audioMixer.SetFloat("BGMVolume",volumeDb);
        bgmVolumeScale = Mathf.Clamp01(value);//将值限制在0~1之间，为负返回0，大于1，返回1
       
    }  

    public void SetSfxVolume(float value) 
    {
        float clampedValue = Mathf.Clamp01(value);
        float volumeDb = clampedValue > 0 ? Mathf.Log10(clampedValue) * 20 : -80; // 音量为0时设为-80dB（静音）
        audioMixer.SetFloat("SFXVolume",volumeDb);
        audioMixer.SetFloat("VoiceVolume",volumeDb);
        sfxVolumeScale = Mathf.Clamp01(value); //将值限制在0~1之间，为负返回0，大于1，返回1

    }

    public float GetBgmVolume() => bgmVolumeScale;
    public float GetSfxVolume() => sfxVolumeScale;
    
    public void StopBGM() =>bgmSource.Stop();

    public void StopSFX() =>sfxSource.Stop();

    public void StopVoice()=>voiceSource.Stop();

}
