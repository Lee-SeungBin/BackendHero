// Copyright 2013-2022 AFI, INC. All rights reserved.

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace InGameScene
{
    //===============================================================
    // 게임에서 보여주는 적 관련 클래스
    //===============================================================
    public class EnemyObject : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Vector3 _stayPosition;

        [SerializeField] private Slider HpBar;
        [SerializeField] private TMP_Text DamgeText;

        public string Name { get; private set; }
        public float MaxHp { get; private set; }

        public float Hp { get; private set; }
        public float Atk { get; private set; }
        public float Money { get; private set; }
        public float Exp { get; private set; }

        private const float _moveSpeed = 0.5f;
        private const float AttackDelay = 3f;
        private float CurrentTime = 0f;

        private Coroutine DamageTextCoroutine;

        // 적의 상태
        public enum EnemyState
        {
            Trace,
            Attack,
            Dead
        }

        public EnemyState CurrentEnemyState { get; private set; }
        private BackendData.Chart.Enemy.Item _currentEnemyChartItem; // 적의 차트 정보

        void Update()
        {
            switch (CurrentEnemyState)
            {
                case EnemyState.Trace:
                    TraceUpdate();
                    break;
                case EnemyState.Attack:
                    AttackUpdate();
                    break;
                case EnemyState.Dead:
                    DeadUpdate();
                    break;
            }
        }

        //지정된 좌표로 오고 죽을때까지 호출되는 함수.
        void TraceUpdate()
        {
            HpBar.gameObject.SetActive(true);
            transform.localPosition =
                Vector3.MoveTowards(transform.localPosition, _stayPosition, _moveSpeed * Time.deltaTime);

            if (transform.localPosition.Equals(_stayPosition))
            {
                Debug.Log("적 공격 시작");
                CurrentEnemyState = EnemyState.Attack;
            }
        }

        void AttackUpdate()
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime > AttackDelay)
            {
                //데미지를 입는 함수 추가
                Managers.Process.UpdatePlayerHP(Atk, StaticManager.Backend.GameData.UserData.Hp);
                Debug.Log("데미지" + Atk);
                CurrentTime = 0;
            }
        }

        //죽고나서 날라가는 함수
        void DeadUpdate()
        {
            HpBar.gameObject.SetActive(false);
            transform.Rotate(Vector3.back * (100f * Time.deltaTime));
        }

        //적의 정보로 초기화하는 함수
        public void Init(BackendData.Chart.Enemy.Item enemyInfo, float multiStat, Vector3 stayPosition)
        {
            _currentEnemyChartItem = enemyInfo;

            Name = enemyInfo.EnemyName;
            MaxHp = enemyInfo.Hp * multiStat;
            Hp = MaxHp;
            Atk = enemyInfo.Atk * multiStat;
            Money = enemyInfo.Money * multiStat;
            Exp = enemyInfo.Exp * multiStat;

            gameObject.GetComponent<SpriteRenderer>().sprite = _currentEnemyChartItem.EnemySprite;

            _stayPosition = stayPosition;
            CurrentEnemyState = EnemyState.Trace;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            HpBar.maxValue = MaxHp;
            SetCurrentHp(Hp);
            DamgeText.gameObject.SetActive(false);
        }

        //// 적 정보 갱신
        //public void SetEnemyInfo(string enemyName, float maxHp)
        //{
        //    _enemyNameText.text = enemyName;

        //    _maxHp = maxHp;
        //    _currentHp = maxHp;
        //    _enemyHpSlider.maxValue = _maxHp;

        //    SetCurrentHp(_currentHp);
        //}

        // 적 HP 갱신
        public void SetCurrentHp(float currentHp)
        {
            HpBar.value = currentHp;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (CurrentEnemyState != EnemyState.Trace)
            //{
            //    return;
            //}

            // 만약 화면 밖 회색 박스와 부딪히면 소멸
            if (collision.transform.CompareTag("BulletDestroyer"))
            {
                Dead();
                //Destroy(gameObject);
            }

            // 총알에 맞을 경우
            if (collision.transform.CompareTag("Bullet"))
            {
                float damage = collision.gameObject.GetComponent<BulletObject>().GetDamage();
                Hp -= damage;
                // Hp가 0이 될 경우
                if (Hp <= 0)
                {
                    Dead();
                    Managers.Process.UpdateEnemyStatus(this);
                }
                else
                {
                    if (DamageTextCoroutine == null)
                    {
                        DamageTextCoroutine = StartCoroutine(ActiveDamageText(1f, damage));
                    }
                    else
                    {
                        StopCoroutine(DamageTextCoroutine);
                        DamageTextCoroutine = StartCoroutine(ActiveDamageText(1f, damage));
                    }
                    //StartCoroutine(ActiveDamageText(2f, damage));
                }
                // 맞을 때마다 현재 자신의 hp 정보를 업데이트
                Managers.Process.UpdateNewEnemy();
                SetCurrentHp(Hp);
            }
        }
        private void OnCollisionEnter2D(Collision2D col)
        {

        }

        // 죽었을때 호출되는 함수
        private void Dead()
        {
            CurrentEnemyState = EnemyState.Dead;

            SetDropItem();

            // 오른쪽 위로 날라감
            _rigidbody2D.AddForce(new Vector2(200, 200), ForceMode2D.Force);
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

            //Destroy(gameObject, 5);

            StartCoroutine(ActiveFalse(2f));
        }

        private IEnumerator ActiveFalse(float sec)
        {
            yield return new WaitForSeconds(sec);

            _rigidbody2D.velocity = Vector2.zero;
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.SetActive(false);
            gameObject.transform.rotation = Quaternion.identity;
        }

        private IEnumerator ActiveDamageText(float sec, float damage)
        {
            DamgeText.text = damage.ToString();
            DamgeText.gameObject.SetActive(true);

            yield return new WaitForSeconds(sec);
            DamageTextCoroutine = null;
            DamgeText.gameObject.SetActive(false);
        }

        // 죽을 경우 일정 확률로 아이템을 떨어트리는 함수
        private void SetDropItem()
        {
            foreach (var dropItem in _currentEnemyChartItem.DropItemList)
            {
                double dropPercent = Math.Round((double)Random.Range(0, 100), 2);
                // 확률이 50%일 경우, 100중에 50미만만 나오면 된다
                // 확률이 10%일 경우, 100중에 1,2,3,4,5,6,7,8,9만 나와야한다
                // 확률이 1%일 경우, 100중에, 0.9 미만의 수만 나와야한다.
                if (dropItem.DropPercent > dropPercent)
                {
                    Managers.Game.UpdateItemInventory(dropItem.ItemID, 1);
                    Managers.Process.DropItem(transform.position, dropItem.ItemID);
                }
            }
        }
    }
}