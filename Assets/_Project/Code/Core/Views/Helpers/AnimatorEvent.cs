using UnityEngine;

namespace _Project.Code.Core.Views.Helpers
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEvent : MonoBehaviour
    {
        private IAnimatorEvent _receiver;
        private void Start()
        {
            _receiver = transform.parent.GetComponent<IAnimatorEvent>();
            
        }

        public void OnFadeInEnded_Event() => _receiver.OnEventExecute();
    }
}