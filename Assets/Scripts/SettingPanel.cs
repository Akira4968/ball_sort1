using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : ShowHidable
{
    [SerializeField] Animator _music;
    [SerializeField] Animator _sound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickRate()
    {

    }
    public void ONClickMusic()
    {
        _music.SetBool("IsOn", !_music.GetBool("IsOn"));
    }
    public void ONClickSound()
    {
        _sound.SetBool("IsOn", !_sound.GetBool("IsOn"));
    }
    public void OnClickCancel()
    {
        SharedUIManager.SettingPanel.Hide();
    }
}
