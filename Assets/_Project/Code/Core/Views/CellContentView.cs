using System;
using System.Collections;
using _Project.Code.Core.Models.BoardLogic.Cells;
using _Project.Code.Core.Views.Helpers;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    [SelectionBase]
    public class CellContentView : View , IAnimatorEvent
    {
        //TODO: REMOVE THIS FOR DEBUG
        public CellContent CellContent;

        [SerializeField] private ParticleSystem _matchVFX;

        [SerializeField] protected Animator Animator;
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        
        private void Destroy()
        {
            Instantiate(_matchVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void Match() => Animator.SetTrigger(FadeIn);
        public void OnEventExecute() => Destroy();
    }
}