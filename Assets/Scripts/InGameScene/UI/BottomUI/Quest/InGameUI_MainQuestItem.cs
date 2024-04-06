// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackendData.Chart.Quest;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameScene.UI
{
    //===========================================================
    // 퀘스트 아이템 UI
    //===========================================================
    public class InGameUI_MainQuestItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text QuestNumberText;
        [SerializeField] private TMP_Text QuestDetailText;
        [SerializeField] private TMP_Text QuestProgressText;
        [SerializeField] private TMP_Text RewardText;
        [SerializeField] private Image RewardIcon;
        [SerializeField] private Sprite[] RewardIconSprites;
        [SerializeField] private Button _requestAchieveButton;

        private Item _questItemInfo;

        // 퀘스트 타입 리턴하는 함수
        public QuestType GetQuestType()
        {
            return _questItemInfo.QuestType;
        }

        public void Init(Item questItemInfo)
        {
            _questItemInfo = questItemInfo;

            UpdateMainQuestUI();

            _requestAchieveButton.onClick.RemoveAllListeners();
            _requestAchieveButton.onClick.AddListener(Reward);
        }


        public void UpdateMainQuestUI()
        {
            QuestNumberText.text = $"메인 퀘스트 - {StaticManager.Backend.GameData.UserData.MainQuestCount}";
            QuestDetailText.text = _questItemInfo.QuestContent;

            float RewardCount = 0f;
            float CurrentCount = 0f;
            float RequestCount = 0f;

            foreach (var item in _questItemInfo.RewardStat)
            {
                if (item.Money > 0)
                {
                    RewardIcon.sprite = RewardIconSprites[0];
                    RewardText.text = $"{item.Money}";
                    RewardCount = item.Money;
                }
                else
                {
                    RewardIcon.sprite = RewardIconSprites[1];
                    RewardText.text = $"{item.Jewel}";
                    RewardCount = item.Jewel;
                }
            }

            switch (_questItemInfo.QuestType)
            {
                case QuestType.DefeatEnemy_Main:
                    CurrentCount = StaticManager.Backend.GameData.UserData.MainDefeatEnemyCount;
                    RequestCount = _questItemInfo.RequestCount;
                    if (CurrentCount >= RequestCount)
                    {
                        CurrentCount = RequestCount;
                    }
                    break;
                case QuestType.AtkUp:
                    CurrentCount = StaticManager.Backend.GameData.UserData.GrowthAtkCount;
                    RequestCount = _questItemInfo.RequestCount * StaticManager.Backend.GameData.UserData.MainGrowthAtkCount;
                    break;
                case QuestType.HpUp:
                    CurrentCount = StaticManager.Backend.GameData.UserData.GrowthHpCount;
                    RequestCount = _questItemInfo.RequestCount * StaticManager.Backend.GameData.UserData.MainGrowthHpCount;
                    break;
                case QuestType.HpRecorveryUp:
                    CurrentCount = StaticManager.Backend.GameData.UserData.GrowthHpRecorveryCount;
                    RequestCount = _questItemInfo.RequestCount * StaticManager.Backend.GameData.UserData.MainGrowthHpRecorveryCount;
                    break;
                case QuestType.StageClear:
                    CurrentCount = StaticManager.Backend.GameData.UserData.StageCount;
                    RequestCount = _questItemInfo.RequestCount * StaticManager.Backend.GameData.UserData.MainStageClearCount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_questItemInfo.QuestType), _questItemInfo.QuestType, null);
            }

            QuestProgressText.text = $"{CurrentCount} / {RequestCount}";

            bool isAchieve = CurrentCount >= RequestCount;
            if (isAchieve)
            {
                _requestAchieveButton.interactable = true;
                _requestAchieveButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                _requestAchieveButton.interactable = false;
                _requestAchieveButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }

        // 각 보상 아이템 별로 보상을 지급하는 함수
        private void Reward()
        {
            if (_questItemInfo.RewardStat != null)
            {
                // 차트 정보에  money, exp를 주는 rewardStat이 존재한다면
                foreach (var item in _questItemInfo.RewardStat)
                {
                    InGameScene.Managers.Game.UpdateUserData(item.Money, item.Exp, item.Jewel);
                }
            }

            // 차트 정보에 아이템, 무기를 주는 RewardItem이 존재한다면
            if (_questItemInfo.RewardItem != null)
            {
                foreach (var item in _questItemInfo.RewardItem)
                {
                    switch (item.RewardItemType)
                    {
                        case RewardItemType.Item: // 아이템일 경우 아이템의 id를 가져와 업데이트
                            InGameScene.Managers.Game.UpdateItemInventory(item.Id, (int)item.Count);

                            break;
                        case RewardItemType.Weapon: // 무기일 경우, 무기의 id를 가져와 업데이트
                            InGameScene.Managers.Game.UpdateWeaponInventory(item.Id);
                            break;
                    }
                }
            }

            // 받은 보상을 UI로 표현
            StringBuilder rewardString = new StringBuilder();
            rewardString.Append("보상을 획득했습니다.\n");

            StaticManager.UI.AlertUI.OpenAlertUI("퀘스트 완료", rewardString.ToString());

            Managers.Game.UpdateUserMainQuestData();
            QuestType questType = GetQuestType();
            switch (questType)
            {
                case QuestType.DefeatEnemy_Main:
                    Managers.Game.UpdateUserMainDefeatEnemyData(true);
                    break;
                default:
                    Managers.Game.UpdateUserMainQuestData(questType);
                    break;
            }

            Init(StaticManager.Backend.Chart.Quest.Dictionary[(StaticManager.Backend.GameData.UserData.MainQuestCount % 5 + 4) % 5 + 1]);
            UpdateMainQuestUI();
        }
    }
}