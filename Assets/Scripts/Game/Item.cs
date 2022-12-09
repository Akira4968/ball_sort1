using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private bool isHolder;
    [SerializeField] private bool isBackground;
    [SerializeField] private bool isBall;
    [SerializeField] int index;
    [SerializeField] GameObject _selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickItem()
    {
        switch(index)
        {
            case 0:
                {
                    PrefManager.SetInt("holder", 0);
                    
                    break;
                }
            case 1:
                {
                    PrefManager.SetInt("holder", 1);
                    break;
                }
            case 2:
                {
                    PrefManager.SetInt("holder", 2);
                    break;
                }
            case 3:
                {
                    PrefManager.SetInt("holder", 3);
                    break;
                }
            case 4:
                {
                    PrefManager.SetInt("holder", 4);
                    break;
                }
            case 5:
                {
                    PrefManager.SetInt("holder", 5);
                    break;
                }
        }
        //LevelManager.Instance.GetHolderPrefap()
    }
}
