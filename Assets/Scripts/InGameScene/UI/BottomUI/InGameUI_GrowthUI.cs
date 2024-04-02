// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackendData.GameData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGameScene.UI
{
    public class InGameUI_GrowthUI : MonoBehaviour
    {
        private InGameUI_BottomUIBase[] _bottomUIs;
        private Button[] XUIButtons;
        private int Count = 1;
        private List<GameObject> GrowthObj = new List<GameObject>();
        private UserData userData;
        [SerializeField] private GameObject _UIChangeButtonParentObject;
        [SerializeField] private GameObject _GrowthStatPrefab;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            userData = StaticManager.Backend?.GameData.UserData;
            _bottomUIs = transform.GetComponentsInChildren<InGameUI_BottomUIBase>();
            int GrowthMin = (int)UserData.GrowthType.Min;
            int GrowthMax = (int)UserData.GrowthType.Max;

            // BottomUI 정보 불러와 초기화
            foreach (var ui in _bottomUIs)
            {
                ui.Init();
            }

            //바텀UI의 버튼 배열 
            XUIButtons = _UIChangeButtonParentObject.GetComponentsInChildren<Button>();

            // 각 버튼별 클릭시 활성화되는 UI 배정
            for (int i = 0; i < XUIButtons.Length; i++)
            {
                int index = i;
                XUIButtons[index].onClick.AddListener(() => ChangeUI(index));
            }

            for (int i = GrowthMin; i <= GrowthMax; i++)
            {
                GameObject Temp;
                if (GrowthObj.Count <= GrowthMax)
                {
                    Temp = Instantiate(_GrowthStatPrefab, transform);
                    GrowthObj.Add(Temp);
                }
                else
                {
                    Temp = GrowthObj[i];
                    Temp.SetActive(true);
                }

                Temp.GetComponent<InGameUI_GrowthUI_Item>().Init(null, userData, (UserData.GrowthType)i, Count);
            }

            // 2번 UI 현재 장비로 초기 설정
            ChangeUI(2);
        }

        // 바텀 내 각 BottomUIBase를 가지고 있는 UI 클래스에 접근
        public T GetUI<T>() where T : InGameUI_BottomUIBase
        {
            for (int i = 0; i < _bottomUIs.Length; i++)
            {
                if (typeof(T) == _bottomUIs[i].GetType())
                {
                    return (T)_bottomUIs[i];
                }
            }

            throw new Exception($"{typeof(T)}가 존재하지 않습니다.");
        }

        // 버튼을 누를경우 해당 UI로 변경
        void ChangeUI(int index)
        {
            try
            {
                for (int i = 0; i < XUIButtons.Length; i++)
                {
                    XUIButtons[i].image.color = Color.white;
                }

                XUIButtons[index].image.color = Color.gray;
                switch (index)
                {
                    case 0:
                        Count = 100;
                        break;
                    case 1:
                        Count = 10;
                        break;
                    case 2:
                        Count = 1;
                        break;
                    default:
                        break;
                }

                int GrowthMin = (int)UserData.GrowthType.Min;
                int GrowthMax = (int)UserData.GrowthType.Max;
                for (int i = GrowthMin; i <= GrowthMax; i++)
                {
                    GrowthObj[i].GetComponent<InGameUI_GrowthUI_Item>().UpgradeCount = Count;
                    GrowthObj[i].GetComponent<InGameUI_GrowthUI_Item>().StatTextInit(userData, (UserData.GrowthType)i, Count);
                }

            }
            catch (Exception e)
            {
                throw new Exception(
                    $"활성되지 않은 Bottom UI가 존재합니다.\n시도된 UI : {index}번\n전체 Bottom UI 개수 : {_bottomUIs.Length}\n\n{e}");
            }
        }
    }
}