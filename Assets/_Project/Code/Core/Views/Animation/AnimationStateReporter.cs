using UnityEngine;

namespace _Project.Code.Core.Views.Animation
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimatorStateReader _stateReader;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            FindStateReader(animator);
            
            _stateReader.EnteredState(stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindStateReader(animator);
            
            _stateReader.ExitedState(stateInfo.shortNameHash);
        }

        private void FindStateReader(Animator animator)
        {
            if(_stateReader != null)
                return;

            _stateReader = animator.gameObject.GetComponent<IAnimatorStateReader>();
        }
    }
}