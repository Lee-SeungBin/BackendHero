﻿// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackendData.Chart.Quest;
using BackendData.GameData;
using InGameScene.UI;
using System;

namespace InGameScene
{
    //===========================================================
    // 아이템 사용, 무기 장착등의 UI와 데이터 변경을 한번에 제어하는 클래스
    //===========================================================
    public class GameManager
    {
        private UIManager _uiManager;
        private Player _player;

        public void Init(Player player, UIManager uiManager)
        {
            _uiManager = uiManager;
            _player = player;
        }

        // UserData와 관련된 UI와 서버에 저장될 데이터를 변경하는 함수
        // 해당 함수를 통해서만 UserData 변경 가능
        public void UpdateUserData(float money, float exp)
        {
            try
            {
                // 버프가 존재할 경우, 획득량 조정
                if (money > 0)
                {
                    money = Managers.Buff.GetBuffedStat(Buff.BuffStatType.Gold, money);
                }
                if (exp > 0)
                {
                    exp = Managers.Buff.GetBuffedStat(Buff.BuffStatType.Exp, exp);
                }

                // 조정된 획득량만큼 GameData의 UserData 업데이트
                StaticManager.Backend.GameData.UserData.UpdateUserData(money, exp);

                // 변경된 데이터에 맞게 UserUI 변경(우측 상단)
                _uiManager.UserUI.UpdateUI();

                // 퀘스트에 영향을 미칠 데이터가 존재한다면 QuestUI도 업데이트
                if (exp > 0)
                {
                    _uiManager.BottomUI.GetUI<InGameUI_Quest>().UpdateUI(QuestType.LevelUp);
                }
                if (money < 0)
                {
                    _uiManager.BottomUI.GetUI<InGameUI_Quest>().UpdateUI(QuestType.UseGold);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"UpdateUserData({money}, {exp}) 중 에러가 발생하였습니다\n{e}");
            }
        }

        public void UpdateUserGrowthData(int count, UserData.GrowthType growthType)
        {
            try
            {
                StaticManager.Backend.GameData.UserData.UpdateGrowthUserData(count, growthType);

                _uiManager.UserUI.UpdateUI();
            }
            catch (Exception e)
            {
                throw new Exception($"UpdateUserGrowthData({count}, {growthType}) 중 에러가 발생하였습니다\n{e}");
            }
        }

        public void UpdateUserStageData()
        {
            try
            {
                StaticManager.Backend.GameData.UserData.UpdateStageCount();

                _uiManager.StageUI.ShowTitleStage(StaticManager.Backend.GameData.UserData.StageCount.ToString());
            }
            catch (Exception e)
            {
                throw new Exception($"UpdateUserStageData 중 에러가 발생하였습니다\n{e}");
            }
        }

        // 아이템 인벤토리 관련 데이터와 UI를 업데이트하는 함수
        public void UpdateItemInventory(int itemID, int count)
        {
            try
            {
                StaticManager.Backend.GameData.ItemInventory.AddItem(itemID, count);
                _uiManager.BottomUI.GetUI<InGameUI_ItemInventory>().UpdateUI(itemID, count);
                _uiManager.UserUI.UpdateUI();

                if (count > 0)
                {
                    _uiManager.BottomUI.GetUI<InGameUI_Quest>()
                        .UpdateUIForGetItem(RequestItemType.Item, itemID);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"UpdateItemInventory({itemID}, {count}) 중 에러가 발생하였습니다\n{e}");
            }
        }

        // 무기 인벤토리 관련 데이터와 UI를 업데이트하는 함수
        public void UpdateWeaponInventory(int weaponId)
        {
            try
            {
                string myWeaponId = StaticManager.Backend.GameData.WeaponInventory.AddWeapon(weaponId);
                _uiManager.BottomUI.GetUI<InGameUI_Equip>().AddWeaponObjectInInventoryUI(myWeaponId);
                _uiManager.BottomUI.GetUI<InGameUI_Quest>()
                    .UpdateUIForGetItem(RequestItemType.Weapon, weaponId);
            }
            catch (Exception e)
            {
                throw new Exception($"UpdateWeaponInventory({weaponId}) 중 에러가 발생하였습니다\n{e}");
            }
        }

        // 무기 장착 관련 데이터와 UI를 업데이트하는 함수
        public void UpdateWeaponEquip(int position, string prevWeaponId, string myWeaponId)
        {
            try
            {
                // 기본 변경과 
                StaticManager.Backend.GameData.WeaponEquip.ChangeEquip(position, prevWeaponId, myWeaponId);
                _player.SetWeapon();
                _uiManager.BottomUI.GetUI<InGameUI_Equip>().UpdateUI(prevWeaponId, myWeaponId);
            }
            catch (Exception e)
            {
                StaticManager.UI.AlertUI.OpenErrorUI(GetType().Name, "UpdateWeaponEquip", $"UpdateWeaponEquip({position}, {prevWeaponId}, {myWeaponId}) 중 에러가 발생하였습니다\n" + e.ToString());
            }

        }
    }

}