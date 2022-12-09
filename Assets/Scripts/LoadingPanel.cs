using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : ShowHidable {
    [SerializeField] Image _loaddingfill;
    public float Speed
    {
        get { return anim.speed; }
        set { anim.speed = value; }
    }
    public float Fill
    {
        get { return _loaddingfill.fillAmount; }
        set { _loaddingfill.fillAmount = value; }
    }
}
