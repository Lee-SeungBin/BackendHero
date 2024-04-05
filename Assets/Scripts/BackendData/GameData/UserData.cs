// Copyright 2013-2022 AFI, INC. All rights reserved.

using BackEnd;
using BackendData.Chart.Quest;
using InGameScene;
using LitJson;
using System;
using System.Globalization;

namespace BackendData.GameData
{
    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(변수)
    //===============================================================
    public partial class UserData
    {
        public const int GrowthAtk = 3;
        public const int GrowthHp = 10;
        public const int GrowthHpRecorvery = 1;
        public enum GrowthType
        {
            None = -1,
            Atk,
            Hp,
            HpRecorvery,
            Min = Atk,
            Max = HpRecorvery,
        }
        public int Level { get; private set; }
        public int Atk { get; private set; }
        public int Hp { get; private set; }
        public int HpRecorvery { get; private set; }
        public float Money { get; private set; }
        public string LastLoginTime { get; private set; }
        public int MainQuestCount { get; private set; }
        public int MainDefeatEnemyCount { get; private set; }
        public int MainGrowthAtkCount { get; private set; }
        public int MainGrowthHpCount { get; private set; }
        public int MainGrowthHpRecorveryCount { get; private set; }
        public int MainStageClearCount { get; private set; }
        public int StageCount { get; private set; }
        public int GrowthAtkCount { get; private set; }
        public int GrowthHpCount { get; private set; }
        public int GrowthHpRecorveryCount { get; private set; }


        public float Exp { get; private set; }

        public float MaxExp { get; private set; }
        public float Jewel { get; private set; }

        public float DayUsingGold { get; set; }
        public float WeekUsingGold { get; set; }
        public float MonthUsingGold { get; set; }

        public int DayDefeatEnemyCount { get; private set; }
        public int WeekDefeatEnemyCount { get; private set; }
        public int MonthDefeatEnemyCount { get; private set; }
    }

    //===============================================================
    // UserData 테이블의 데이터를 담당하는 클래스(함수)
    //===============================================================
    public partial class UserData : Base.GameData
    {

        // 데이터가 존재하지 않을 경우, 초기값 설정
        protected override void InitializeData()
        {
            Level = 1;
            Atk = 30;
            Hp = 100;
            HpRecorvery = 10;
            MaxExp = 100;
            StageCount = 1;
            MainQuestCount = 1;
            MainGrowthHpCount = 1;
            MainGrowthAtkCount = 1;
            MainGrowthHpRecorveryCount = 1;
            MainStageClearCount = 1;
        }

        // Backend.GameData.GetMyData 호출 이후 리턴된 값을 파싱하여 캐싱하는 함수
        // 서버에서 데이터를 불러오늖 함수는 BackendData.Base.GameData의 BackendGameDataLoad() 함수를 참고해주세요
        protected override void SetServerDataToLocal(JsonData gameDataJson)
        {
            Level = int.Parse(gameDataJson["Level"].ToString());
            Atk = int.Parse(gameDataJson["Atk"].ToString());
            Hp = int.Parse(gameDataJson["Hp"].ToString());
            HpRecorvery = int.Parse(gameDataJson["HpRecorvery"].ToString());
            GrowthAtkCount = int.Parse(gameDataJson["GrowthAtkCount"].ToString());
            GrowthHpCount = int.Parse(gameDataJson["GrowthHpCount"].ToString());
            GrowthHpRecorveryCount = int.Parse(gameDataJson["GrowthHpRecorveryCount"].ToString());
            MainQuestCount = int.Parse(gameDataJson["MainQuestCount"].ToString());
            MainDefeatEnemyCount = int.Parse(gameDataJson["MainDefeatEnemyCount"].ToString());
            MainGrowthAtkCount = int.Parse(gameDataJson["MainGrowthAtkCount"].ToString());
            MainGrowthHpCount = int.Parse(gameDataJson["MainGrowthHpCount"].ToString());
            MainGrowthHpRecorveryCount = int.Parse(gameDataJson["MainGrowthHpRecorveryCount"].ToString());
            MainStageClearCount = int.Parse(gameDataJson["MainStageClearCount"].ToString());

            StageCount = int.Parse(gameDataJson["StageCount"].ToString());
            Exp = float.Parse(gameDataJson["Exp"].ToString());
            MaxExp = float.Parse(gameDataJson["MaxExp"].ToString());
            Money = float.Parse(gameDataJson["Money"].ToString());
            LastLoginTime = gameDataJson["LastLoginTime"].ToString();

            DayUsingGold = float.Parse(gameDataJson["DayUsingGold"].ToString());
            WeekUsingGold = float.Parse(gameDataJson["WeekUsingGold"].ToString());
            MonthUsingGold = float.Parse(gameDataJson["MonthUsingGold"].ToString());

            DayDefeatEnemyCount = int.Parse(gameDataJson["DayDefeatEnemyCount"].ToString());
            WeekDefeatEnemyCount = int.Parse(gameDataJson["WeekDefeatEnemyCount"].ToString());
            MonthDefeatEnemyCount = int.Parse(gameDataJson["MonthDefeatEnemyCount"].ToString());

            Jewel = gameDataJson.ContainsKey("Jewel") ? float.Parse(gameDataJson["Jewel"].ToString()) : 0;
        }

        // 테이블 이름 설정 함수
        public override string GetTableName()
        {
            return "UserData";
        }

        // 컬럼 이름 설정 함수
        public override string GetColumnName()
        {
            return null;
        }

        // 데이터 저장 시 저장할 데이터를 뒤끝에 맞게 파싱하는 함수
        public override Param GetParam()
        {
            Param param = new Param();

            param.Add("Level", Level);
            param.Add("Atk", Atk);
            param.Add("Hp", Hp);
            param.Add("HpRecorvery", HpRecorvery);
            param.Add("GrowthAtkCount", GrowthAtkCount);
            param.Add("GrowthHpCount", GrowthHpCount);
            param.Add("GrowthHpRecorveryCount", GrowthHpRecorveryCount);
            param.Add("MainQuestCount", MainQuestCount);
            param.Add("MainDefeatEnemyCount", MainDefeatEnemyCount);
            param.Add("MainGrowthAtkCount", MainGrowthAtkCount);
            param.Add("MainGrowthHpCount", MainGrowthHpCount);
            param.Add("MainGrowthHpRecorveryCount", MainGrowthHpRecorveryCount);
            param.Add("MainStageClearCount", MainStageClearCount);

            param.Add("StageCount", StageCount);
            param.Add("Money", Money);
            param.Add("Jewel", Jewel);
            param.Add("Exp", Exp);
            param.Add("MaxExp", MaxExp);
            param.Add("LastLoginTime", string.Format("{0:MM-DD:HH:mm:ss.fffZ}", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            param.Add("DayUsingGold", DayUsingGold);
            param.Add("WeekUsingGold", WeekUsingGold);
            param.Add("MonthUsingGold", MonthUsingGold);

            param.Add("DayDefeatEnemyCount", DayDefeatEnemyCount);
            param.Add("WeekDefeatEnemyCount", WeekDefeatEnemyCount);
            param.Add("MonthDefeatEnemyCount", MonthDefeatEnemyCount);

            return param;
        }

        // 적 처치 횟수를 갱신하는 함수
        public void CountDefeatEnemy()
        {
            DayDefeatEnemyCount++;
            WeekDefeatEnemyCount++;
            MonthDefeatEnemyCount++;
        }

        // 유저의 정보를 변경하는 함수
        public void UpdateUserData(float money, float exp, float jewel)
        {
            IsChangedData = true;

            Exp += exp;
            Money += money;
            Jewel += jewel;

            if (money < 0)
            {
                float tempMoney = Math.Abs(money);
                DayUsingGold += tempMoney;
                WeekUsingGold += tempMoney;
                MonthUsingGold += tempMoney;
            }

            if (Exp > MaxExp)
            {
                while (Exp > MaxExp)
                {
                    LevelUp();
                }
            }
        }

        // 유저의 성장 정보를 변경하는 함수 ( Atk - 0, Hp - 1, HpRecovery - 2)
        public void UpdateGrowthUserData(int count, GrowthType growthType)
        {
            IsChangedData = true;

            switch (growthType)
            {
                case GrowthType.Atk:
                    Atk += GrowthAtk * count;
                    GrowthAtkCount += count;
                    break;
                case GrowthType.Hp:
                    Hp += GrowthHp * count;
                    GrowthHpCount += count;
                    Managers.Process.GrowthPlayerHP(GrowthHp * count);
                    break;
                case GrowthType.HpRecorvery:
                    HpRecorvery += GrowthHpRecorvery * count;
                    GrowthHpRecorveryCount += count;
                    break;
                default:
                    break;
            }
        }

        public void UpdateStageCount()
        {
            if (StageCount == StaticManager.Backend.Chart.Stage.List.Count)
            {
                return;
            }
            IsChangedData = true;
            StageCount++;
        }

        public void UpdateMainQuestCount()
        {
            IsChangedData = true;
            MainQuestCount++;
        }

        public void UpdateMainDefeatEnemyCount(bool reset)
        {
            IsChangedData = true;
            MainDefeatEnemyCount = reset ? 0 : MainDefeatEnemyCount + 1;
        }

        public void UpdateMainQuestCount(QuestType questType)
        {
            IsChangedData = true;
            switch (questType)
            {
                case QuestType.AtkUp:
                    MainGrowthAtkCount++;
                    break;
                case QuestType.HpUp:
                    MainGrowthHpCount++;
                    break;
                case QuestType.HpRecorveryUp:
                    MainGrowthHpRecorveryCount++;
                    break;
                case QuestType.StageClear:
                    MainStageClearCount++;
                    break;
                default:
                    break;

            }
        }

        // 레벨업하는 함수
        private void LevelUp()
        {
            //Exp가 MaxExp를 초과했을 경우를 대비하여 빼기
            Exp -= MaxExp;

            //기존 경험치에서 1.1배
            MaxExp = (float)Math.Truncate(MaxExp * 1.1);

            Level++;
        }
    }
}