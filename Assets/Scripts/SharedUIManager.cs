using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedUIManager : Singleton<SharedUIManager>
{

    [SerializeField] private LoadingPanel _loadingPanel;
    [SerializeField] private RatingPopUp _ratingPopUp;
    [SerializeField] private PopUpPanel _popUpPanel;
    [SerializeField] private ConsentPanel _consentPanel;
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] ShopPanel _shopPanel;

    public static ConsentPanel ConsentPanel => Instance._consentPanel;
    public static PopUpPanel PopUpPanel => Instance._popUpPanel;
    public static LoadingPanel LoadingPanel => Instance?._loadingPanel;
    public static RatingPopUp RatingPopUp => Instance?._ratingPopUp;
    public static SettingPanel SettingPanel => Instance?._settingPanel;
    public static ShopPanel ShopPanel=> Instance?._shopPanel;
}


