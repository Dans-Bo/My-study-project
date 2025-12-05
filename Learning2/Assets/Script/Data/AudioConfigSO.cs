using System;
using System.Collections.Generic;
using UnityEngine;


public enum AudioType
{
    //BGM
    BGM_Forest,BGM_InCombat,
    //音效
    SFX_RunSound,
    SFX_WeaponSwingLight1,SFX_WeaponSwingLight2,SFX_WeaponSwingLight3,
    SFX_PlayerJump,SFX_PlayerDown,
    SFX_WeaponHit,
    //声音
    Voice_PlayerAttack,Voic_PlayerHurt,
    Voice_MinorHurt,
    SFX_PlayerJump2,SFX_MouseSlide,SFX_MouseClick,SFX_MinoAttack


}

[Serializable]
public class AudioConfig
{
    public AudioType audioType;
    public AudioClip audioClip;
    public bool isBGM;
    [Range(0f,1f)]
    public float defaulVolume = 1f;
}
[CreateAssetMenu (fileName ="AudioConfig" , menuName ="Data/AudioConfig")]
public class AudioConfigSO:ScriptableObject
{
    public List <AudioConfig> audioConfigs = new List<AudioConfig>();
}
