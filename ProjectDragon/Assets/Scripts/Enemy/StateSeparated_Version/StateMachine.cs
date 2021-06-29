using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T owner;
    private IState<T> previousState;
    private IState<T> currentState;
    public Dictionary<System.Enum, IState<T>> states = new Dictionary<System.Enum, IState<T>>();


    //변수 초기화
    public StateMachine(T _owner)
    {
        this.owner = _owner;

    }

    //상태 변경
    public void ChangeState(IState<T> newState)
    {
        //같은 상태를 변환하려 한다면 나감
        if(currentState == newState || newState ==null) return;

        previousState = currentState;

        //상태가 있다면 종료
        if(currentState != null) currentState.OnExit(owner);

        currentState = newState;

        //새로 적용된 상태가 null이 아니면 실행
        if (currentState != null) currentState.OnEnter(owner);
    }

    // 초기상태설정
    public void Initial_Setting(T _owner, IState<T> _InitialState)
    {
        owner = _owner;
        ChangeState(_InitialState);
    }

    //상태 업데이트
    public void Update()
    {
        if (currentState != null) currentState.OnExecute(owner);
    }

    //이전 상태로 돌아가기
    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }
}
