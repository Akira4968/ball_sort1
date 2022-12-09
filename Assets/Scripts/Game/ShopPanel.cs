using dotmob;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : ShowHidable
{
    [SerializeField] GameObject _holderPanel;
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] GameObject _ballPanel;
    [SerializeField] Image holderBtn;
    [SerializeField] Image backgroundBtn;
    [SerializeField] Image ballBtn;
    [SerializeField]List<GameObject> _selectList;
    [SerializeField] Sprite[] selected;
    [SerializeField] Sprite[] notSelected;
    // Start is called before the first frame update
    void Start()
    {
        _holderPanel.SetActive(true);
        _backgroundPanel.SetActive(false);
        _ballPanel.SetActive(false);
        holderBtn.sprite = selected[0];
        backgroundBtn.sprite = notSelected[1];
        ballBtn.sprite = notSelected[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickHolder()
    {
        _holderPanel.SetActive(true);
        _backgroundPanel.SetActive(false);
        _ballPanel.SetActive(false);
        holderBtn.sprite = selected[0];
        backgroundBtn.sprite = notSelected[1];
        ballBtn.sprite = notSelected[2];
    }
    public void OnClickBackground()
    {
        _holderPanel.SetActive(false);
        _backgroundPanel.SetActive(true);
        _ballPanel.SetActive(false);
        holderBtn.sprite = notSelected[0];
        backgroundBtn.sprite = selected[1];
        ballBtn.sprite = notSelected[2];
    }
    public void OnClickBall()
    {
        _holderPanel.SetActive(false);
        _backgroundPanel.SetActive(false);
        _ballPanel.SetActive(true);
        holderBtn.sprite = notSelected[0];
        backgroundBtn.sprite = notSelected[1];
        ballBtn.sprite = selected[2];
    }
    public void OnClickCancel()
    {
        UIManager.Instance._shopPanel.Hide();
        GameManager.LoadGame(new LoadGameData
        {
            Level = LevelManager.Instance.Level,
            GameMode = LevelManager.Instance.GameMode,
        }, false);
    }
}
