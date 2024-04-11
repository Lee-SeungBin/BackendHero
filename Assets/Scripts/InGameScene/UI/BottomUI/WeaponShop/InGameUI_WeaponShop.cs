// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackEnd;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BackendManager;

namespace InGameScene.UI
{
    //===========================================================
    // 무기 상점 UI
    //===========================================================
    public class InGameUI_WeaponShop : InGameUI_BottomUIBase
    {
        //[SerializeField] private GameObject _weaponShopParentObject;
        //[SerializeField] private GameObject _weaponShopItemPrefab;
        [SerializeField] private Button _oneGacha;
        [SerializeField] private Button _tenGacha;
        [SerializeField] private Button _thirtyGacha;
        [SerializeField] private GameObject ResultPopup;
        [SerializeField] private InGameUI_WeaponShopResultItem Resultprefab;

        private List<InGameUI_WeaponShopResultItem> ActiveResulttItems = new List<InGameUI_WeaponShopResultItem>();
        private List<InGameUI_WeaponShopResultItem> DeActiveResulttItems = new List<InGameUI_WeaponShopResultItem>();

        // 무기 상점 아이템 생성
        public override void Init()
        {
            //foreach (var weapon in StaticManager.Backend.Chart.Weapon.Dictionary)
            //{
            //    Sprite sprite = weapon.Value.WeaponSprite;

            //    var newWeapon = Instantiate(_weaponShopItemPrefab, _weaponShopParentObject.transform, true);
            //    newWeapon.transform.localPosition = new Vector3(0, 0, 0);
            //    newWeapon.transform.localScale = new Vector3(1, 1, 1);

            //    newWeapon.GetComponent<InGameUI_WeaponShopItem>().Init(sprite, weapon.Value);
            //}
            _oneGacha.onClick.AddListener(() => Gacha(1));
            _tenGacha.onClick.AddListener(() => Gacha(10));
            _thirtyGacha.onClick.AddListener(() => Gacha(30));
        }

        private void Gacha(int count)
        {
            int Price = 0;
            switch (count)
            {
                case 1:
                    Price = 1000;
                    break;
                case 10:
                    Price = 9000;
                    break;
                case 30:
                    Price = 25000;
                    break;
                default:
                    break;
            }

            if (StaticManager.Backend.GameData.UserData.Jewel < Price)
            {
                StaticManager.UI.AlertUI.OpenWarningUI("구매 불가", "보석이 부족합니다.");
                return;
            }


            string selectedProbabilityFileId = "10714";

            var bro = Backend.Probability.GetProbabilitys(selectedProbabilityFileId, count);

            if (!bro.IsSuccess())
            {
                Debug.LogError(bro.ToString());
                return;
            }

            ResultPopup.gameObject.SetActive(true);

            LitJson.JsonData json = bro.GetFlattenJSON()["elements"];

            for (int i = 0; i < json.Count; i++)
            {
                InGameUI_WeaponShopResultItem resultitem;
                if (DeActiveResulttItems.Count > 0)
                {
                    resultitem = DeActiveResulttItems[0];
                    ActiveResulttItems.Add(resultitem);
                    DeActiveResulttItems.RemoveAt(0);
                    resultitem.gameObject.SetActive(true);
                }
                else
                {
                    resultitem = Instantiate(Resultprefab, ResultPopup.transform);
                    ActiveResulttItems.Add(resultitem);
                }

                ProbabilityItem item = new ProbabilityItem();

                item.WeaponID = json[i]["WeaponID"].ToString();

                Managers.Game.UpdateWeaponInventory(int.Parse(item.WeaponID));
                resultitem.Init(int.Parse(item.WeaponID), StaticManager.Backend.GameData.WeaponInventory.dup ? true : false);
            }
            Managers.Game.UpdateUserData(0, 0, -Price);
        }

        public void OnClickCloseResultPopup()
        {
            ResultPopup.SetActive(false);

            foreach (var item in ActiveResulttItems)
            {
                item.gameObject.SetActive(false);
            }
            DeActiveResulttItems.AddRange(ActiveResulttItems);
            ActiveResulttItems.Clear();
        }
    }
}