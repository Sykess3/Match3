using UnityEngine;

namespace _Project.Code.Meta.Views.Hud
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;

        public void Play(AudioClip audioClip) => _audio.PlayOneShot(audioClip);
    }
}