using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM_Enemy : Enemy_Test
{
    #region Property
    protected StateMachine<BaseFSM_Enemy> stateMachine;
    [HideInInspector] public int curAttackCount = 0;
    [HideInInspector] public Tracking tracking;

    #endregion

    protected override void Awake()
    {
        //초기 설정
        ResetState();
        tracking = GetComponent<Tracking>();
        base.Awake();
    }

    //상태 업데이트 코루틴
    protected IEnumerator StateUpdate()
    {
        while (!isDead)
        {
            stateMachine.Update();
            yield return null;
        }
    }

    //상태 머신 작동
    public override IEnumerator Start_On()
    {
        yield return StartCoroutine(base.Start_On());

        //콜라이더 크기를 계산하며 그리드생성
        tracking.pathFinding.Create(col, transform.GetComponentInParent<t_Grid>());

        //상태머신 시작
        StartCoroutine(StateUpdate());
        yield return null;

        //1초 대기
        yield return new WaitForSeconds(1.0f);

        //공격감지 검사
        StartCoroutine(AttackRangeCheck());
    }

    ////상태 변경
    //public virtual void ChangeState<T>(T newState)
    //{
    //}

    //상태 데이터들을 초기화하는 곳
    protected virtual void InitStateDatas()
    {
    }

    //상태 리셋
    protected void ResetState()
    {
        stateMachine = new StateMachine<BaseFSM_Enemy>(gameObject.GetComponent<BaseFSM_Enemy>());
        
        //상태 데이터 초기화
        InitStateDatas();
    }

}
