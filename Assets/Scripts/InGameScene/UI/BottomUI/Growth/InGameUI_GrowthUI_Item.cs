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
        [HideInInspector] public int UpgradeCount;

        private UserData UserInfo;

        // 내 게임정보의 장비 목록에서 데이터를 불러와 적용
        public void Init(Sprite sprite, UserData userData, UserData.GrowthType growthType, int count)
        {
            UserInfo = userData;

            _image.sprite = sprite;
            StatName.text = growthType.ToString();
            UpgradeCount = count;
            //string stat = string.Empty;
            //string statLevel = string.Empty;

            //switch (growthType)
            //{
            //    case UserData.GrowthType.Atk:
            //        stat = userData.Atk.ToString() + " > " + (userData.Atk + UserData.GrowthAtk * count).ToString();
            //        statLevel = "Lv. " + userData.GrowthAtkCount;
            //        break;
            //    case UserData.GrowthType.Hp:
            //        stat = userData.Hp.ToString() + " > " + (userData.Hp + UserData.GrowthHp * count).ToString(); ;
            //        statLevel = "Lv. " + userData.GrowthHpCount.ToString();
            //        break;
            //    case UserData.GrowthType.HpRecorvery:
            //        stat = userData.HpRecorvery.ToString() + " > " + (userData.HpRecorvery + UserData.GrowthHpRecorvery * count).ToString();
            //        statLevel = "Lv. " + userData.GrowthHpRecorveryCount.ToString();
            //        break;
            //    default:
            //        break;
            //}
            //Stat.text = stat;
            //Stat_Level.text = statLevel;
            //StatUpgradePriceText.text = (100 + (userData.GrowthAtkCount * count)).ToString();
            StatTextInit(userData, growthType, count);


            // 업그레이드 버튼 연결
            StatUpgradeButton.onClick.AddListener(() => OnClickUpgradeButton(userData, growthType));
        }

        // 업그레이드 클릭시 호출되는 함수
        void OnClickUpgradeButton(UserData userData, UserData.GrowthType growthType)
        {
            // 장비 업그레이드 비용
            float upgradePrice = 0f;
            switch (growthType)
            {
                case UserData.GrowthType.Atk:
                    for (int level = userData.GrowthAtkCount; level < userData.GrowthAtkCount + UpgradeCount; level++)
                    {
                        upgradePrice += (level + 1) * 100;
                    }
                    break;
                case UserData.GrowthType.Hp:
                    for (int level = userData.GrowthHpCount; level < userData.GrowthHpCount + UpgradeCount; level++)
                    {
                        upgradePrice += (level + 1) * 100;
                    }
                    //upgradePrice = (userData.GrowthHpCount + 1) * 100 * count;
                    break;
                case UserData.GrowthType.HpRecorvery:
                    for (int level = userData.GrowthHpRecorveryCount; level < userData.GrowthHpRecorveryCount + UpgradeCount; level++)
                    {
                        upgradePrice += (level + 1) * 100;
                    }
                    //upgradePrice = (userData.GrowthHpRecorveryCount + 1) * 100 * count;
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
            Managers.Game.UpdateUserGrowthData(UpgradeCount, growthType);

            // 돈이 충분할 경우, 레벨업 + 레벨 갱신 + 스텟 갱신
            //string stat = string.Empty;
            //string statLevel = string.Empty;
            //switch (growthType)
            //{
            //    case UserData.GrowthType.Atk:
            //        stat = userData.Atk.ToString() + " > " + (userData.Atk * count).ToString();
            //        statLevel = "Lv. " + userData.GrowthAtkCount.ToString();
            //        break;
            //    case UserData.GrowthType.Hp:
            //        stat = userData.Hp.ToString() + " > " + (userData.Hp * count).ToString(); ;
            //        statLevel = "Lv. " + userData.GrowthHpCount.ToString();
            //        break;
            //    case UserData.GrowthType.HpRecorvery:
            //        stat = userData.HpRecorvery.ToString() + " > " + (userData.HpRecorvery * count).ToString(); ;
            //        statLevel = "Lv. " + userData.GrowthHpRecorveryCount.ToString();
            //        break;
            //    default:
            //        break;
            //}
            StatTextInit(userData, growthType, UpgradeCount);
        }

        public void StatTextInit(UserData userData, UserData.GrowthType growthType, int count)
        {
            string stat = string.Empty;
            string statLevel = string.Empty;
            string statPrice = string.Empty;
            int Price = 0;
            switch (growthType)
            {
                case UserData.GrowthType.Atk:
                    stat = userData.Atk.ToString() + " > " + (userData.Atk + UserData.GrowthAtk * count).ToString();
                    statLevel = "Lv. " + userData.GrowthAtkCount;
                    for (int level = userData.GrowthAtkCount; level < userData.GrowthAtkCount + count; level++)
                    {
                        Price += (level + 1) * 100;
                    }
                    statPrice = Price.ToString();
                    break;
                case UserData.GrowthType.Hp:
                    stat = userData.Hp.ToString() + " > " + (userData.Hp + UserData.GrowthHp * count).ToString(); ;
                    statLevel = "Lv. " + userData.GrowthHpCount.ToString();
                    for (int level = userData.GrowthHpCount; level < userData.GrowthHpCount + count; level++)
                    {
                        Price += (level + 1) * 100;
                    }
                    statPrice = Price.ToString();
                    break;
                case UserData.GrowthType.HpRecorvery:
                    stat = userData.HpRecorvery.ToString() + " > " + (userData.HpRecorvery + UserData.GrowthHpRecorvery * count).ToString();
                    statLevel = "Lv. " + userData.GrowthHpRecorveryCount.ToString();
                    for (int level = userData.GrowthHpRecorveryCount; level < userData.GrowthHpRecorveryCount + count; level++)
                    {
                        Price += (level + 1) * 100;
                    }
                    statPrice = Price.ToString();
                    break;
                default:
                    break;
            }

            Stat.text = stat;
            Stat_Level.text = statLevel;
            StatUpgradePriceText.text = statPrice;
        }
    }
}