using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void OnEnter(T obj);
    void OnExecute(T obj);
    void OnExit(T obj);
}