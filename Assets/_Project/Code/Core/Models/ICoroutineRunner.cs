using System.Collections;
using UnityEngine;

namespace _Project.Code.Core.Models
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}