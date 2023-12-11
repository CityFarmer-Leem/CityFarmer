using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LandBuyPanel : MonoBehaviour
{
    private TextMeshProUGUI _landLevel;
    private TextMeshProUGUI _landValue;
    private TextMeshProUGUI _landText;
    private Button _landBuyButton;
    private ShopManager _shopManager;
    private ShopData _shopData;
    private Land_UI _land_UI;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        Transform parentTr = transform.parent;
        _shopManager = parentTr.GetComponent<ShopManager>();
        _shopData = parentTr.GetComponent<ShopData>();
        _landLevel = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _landValue = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _landText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _landBuyButton = transform.GetChild(3).GetComponent<Button>();
        _land_UI = transform.parent.parent.GetChild(0).GetComponent<Land_UI>();
    }
    private void ShowText()
    {
        Debug.Log(InfoManager.Instance.UserInfo.UserLandLevel);
        Shop shop = _shopData.LandShop.Find(x => x.ShopLevel == InfoManager.Instance.UserInfo.UserLandLevel);
        _landLevel.text = "���� ���� : " + shop.ShopLevel;
        _landValue.text = "���� ���� : " + shop.ShopPrice;
        _landText.text = "���� ������ " + (shop.ShopLevel + 1) + "�ܰ�� �ø��ϴ�. ������ Ȯ�� �˴ϴ�.";
        _landBuyButton.onClick.AddListener(() => LandLevelUp(shop));
    }
    private void OnEnable()
    {
        ShowText();
    }
    private void LandLevelUp(Shop shop)
    {
        _shopManager.ClickBuyButton(shop.ShopSeq);
        ShowText();
       
        _land_UI.CreateTimer();
    }
}
