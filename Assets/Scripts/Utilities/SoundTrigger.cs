using UnityEngine;

namespace CliffJump.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void TriggerSound() => audioSource.PlayOneShot(clip);
    }
}