using _Project.Code.Core.Views;
using _Project.Code.Meta.Views.Hud;
using UnityEngine;
using Zenject;

namespace _Project.Code.Meta.Views.Audio
{
    public class CellContentAudioEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip _audio;
        private AudioPlayer _player;
        private ICellContentView _view;


        public void Construct(AudioPlayer player) => _player = player;

        private void Start()
        {
            _view = transform.parent.GetComponent<ICellContentView>();
            _view.Matched += PlaySound;
        }

        private void OnDestroy() => _view.Matched -= PlaySound;

        private void PlaySound() => _player.Play(_audio);
    }
}