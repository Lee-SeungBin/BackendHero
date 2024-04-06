// Copyright 2013-2022 AFI, INC. All rights reserved.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameScene.UI
{
    //===========================================================
    // 무기 상점 UI의 아이템 클래스
    //===========================================================
    public class InGameUI_WeaponShopResultItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _weaponName;
        [SerializeField] private TMP_Text _weaponPrice;

        private BackendData.Chart.Weapon.Item _weaponInfo;

        // 무기 차트의 정보에 따라 초기화
        public void Init(int weaponid, bool dup)
        {
            _weaponInfo = StaticManager.Backend.Chart.Weapon.Dictionary[weaponid];
            _image.sprite = _weaponInfo.WeaponSprite;
            _weaponName.text = _weaponInfo.WeaponName;
            if (dup)
            {
                _weaponName.text = _weaponInfo.WeaponName + "중복 보상";
                Managers.Game.UpdateUserData(0, 0, 100);
            }
            _weaponPrice.gameObject.SetActive(dup);
            _weaponPrice.text = "100";
            StaticManager.Backend.GameData.WeaponInventory.dup = false;
        }
    }
}