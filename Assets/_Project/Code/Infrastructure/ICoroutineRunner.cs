using System.Collections;
using UnityEngine;

namespace _Project.Code.Infrastructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}