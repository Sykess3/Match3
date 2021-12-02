using System;
using System.Collections;
using UnityEngine;

namespace _Project.Code.Core.Views
{
    public class View : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer SpriteRenderer;
        public event Action Destroyed;
        public event Action Created;


        private void Start()
        {
            Created?.Invoke();
            
            OnStart();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
            
            OnDestroyed();
        }

        public void Hide() => StartCoroutine(DoFadeIn());
        
        private IEnumerator DoFadeIn()
        {
            while (SpriteRenderer.color.a > 0)
            {
                Color color = SpriteRenderer.color;
                color = new Color(color.r, color.g, color.b, color.a - 0.06f);
                SpriteRenderer.color = color;
                yield return new WaitForSeconds(0.06f);
            }

            gameObject.SetActive(false);
        }
        
        

        protected virtual void OnDestroyed() { }
        protected virtual void OnStart() { }
    }
}