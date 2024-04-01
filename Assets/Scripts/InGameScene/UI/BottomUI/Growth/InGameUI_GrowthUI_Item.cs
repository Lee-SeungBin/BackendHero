// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackendData.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameScene.UI
{
    //===========================================================
    // 장비 UI에 사용되는 아이템 클래스
    //===========================================================
    public class InGameUI_GrowthUI_Item : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text StatName;
        [SerializeField] private TMP_Text Stat;
        [SerializeField] private TMP_Text Stat_Level;
        [SerializeField] private Button StatUpgradeButton;

        [SerializeField] private TMP_Text StatUpgradePriceText;

        private UserData UserInfo;

        // 내 게임정보의 장비 목록에서 데이터를 불러와 적용
        public void Init(Sprite sprite, UserData userData, UserData.GrowthType growthType, int count)
        {
            UserInfo = userData;

            _image.sprite = sprite;
            StatName.text = growthType.ToString();

            string stat = string.Empty;
            string statLevel = string.Empty;
            int statPoint = 0;
            switch (growthType)
            {
                case UserData.GrowthType.Atk:
                    statPoint = userData.Atk;
                    stat = userData.Atk.ToString() + " > " + (userData.Atk + UserData.GrowthAtk * count).ToString();
                    statLevel = "Lv. " + userData.GrowthAtkCount;
                    break;
                case UserData.GrowthType.Hp:
                    stat = userData.Hp.ToString() + " > " + (userData.Hp + UserData.GrowthHp * count).ToString(); ;
                    statLevel = "Lv. " + userData.GrowthHpCount.ToString();
                    break;
                case UserData.GrowthType.HpRecorvery:
                    stat = userData.HpRecorvery.ToString() + " > " + (userData.HpRecorvery + UserData.GrowthHpRecorvery * count).ToString();
                    statLevel = "Lv. " + userData.GrowthHpRecorveryCount.ToString();
                    break;
                default:
                    break;
            }
            //stat += $"atk {weaponInfo.GetCurrentWeaponStat().Atk}\n";
            //stat += $"spd {weaponInfo.GetCurrentWeaponStat().Spd}\n";
            //stat += $"delay {weaponInfo.GetCurrentWeaponStat().Delay}\n";
            Stat.text = stat;
            Stat_Level.text = statLevel;
            StatUpgradePriceText.text = (100 + (userData.GrowthAtkCount * count)).ToString();

            // 업그레이드 버튼 연결
            StatUpgradeButton.onClick.AddListener(() => OnClickUpgradeButton(userData, growthType, count));
        }

        // 업그레이드 클릭시 호출되는 함수
        void OnClickUpgradeButton(BackendData.GameData.UserData userData, UserData.GrowthType growthType, int count)
        {
            // 장비 업그레이드 비용
            float upgradePrice = 0f;
            switch (growthType)
            {
                case UserData.GrowthType.Atk:
                    upgradePrice = 100 + (userData.GrowthAtkCount * count);
                    break;
                case UserData.GrowthType.Hp:
                    upgradePrice = 100 + (userData.GrowthHpCount * count);
                    break;
                case UserData.GrowthType.HpRecorvery:
                    upgradePrice = 100 + (userData.GrowthHpRecorveryCount * count);
                    break;
                default:
                    break;
            }

            // 업그레이드 비용이 더 높을 경우
            if (StaticManager.Backend.GameData.UserData.Money < upgradePrice)
            {
                StaticManager.UI.AlertUI.OpenWarningUI("업그레이드 불가", "보유중인 재화가 부족합니다.");
                return;
            }

            // money 데이터 감소
            Managers.Game.UpdateUserData(-upgradePrice, 0);
            Managers.Game.UpdateUserGrowthData(count, growthType);

            // 돈이 충분할 경우, 레벨업 + 레벨 갱신 + 스텟 갱신
            string stat = string.Empty;
            string statLevel = string.Empty;
            switch (growthType)
            {
                case UserData.GrowthType.Atk:
                    stat = userData.Atk.ToString() + " > " + (userData.Atk * count).ToString();
                    statLevel = "Lv. " + userData.GrowthAtkCount.ToString();
                    break;
                case UserData.GrowthType.Hp:
                    stat = userData.Hp.ToString() + " > " + (userData.Hp * count).ToString(); ;
                    statLevel = "Lv. " + userData.GrowthHpCount.ToString();
                    break;
                case UserData.GrowthType.HpRecorvery:
                    stat = userData.HpRecorvery.ToString() + " > " + (userData.HpRecorvery * count).ToString(); ;
                    statLevel = "Lv. " + userData.GrowthHpRecorveryCount.ToString();
                    break;
                default:
                    break;
            }
        }
    }
}