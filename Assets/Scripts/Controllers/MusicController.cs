using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CliffJump.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicController : MonoBehaviour
    {
        [Header("Clips")]
        [SerializeField] private AudioClip defaultLoop;
        [SerializeField] private AudioClip filterClip;
        [SerializeField] private AudioClip mutedLoop;
        [SerializeField] private AudioClip arpLoop;

        [Header("Fade")]
        [SerializeField] private float muteTime = 0.2f;
        [SerializeField] private float volumeDownTime = 1f;
        [SerializeField] private float volumeDownLevel = 0.6f;

        private AudioSource audioSource;
        private WaitForSeconds filterLength;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            filterLength = new WaitForSeconds(filterClip.length);
        }

        public void PlayRunLoop()
        {
            PlayLoop(defaultLoop);
        }

        public void PlaySlowdown()
        {
            audioSource.loop = false;
            audioSource.clip = filterClip;
            audioSource.volume = 1f;
            audioSource.Play();

            StartCoroutine(DoMutedAfterSlowdown());
        }

        private IEnumerator DoMutedAfterSlowdown()
        {
            yield return filterLength;
            PlayMuted();
        }

        public void PlayMuted()
        {
            PlayLoop(mutedLoop);
        }

        public void PlayArp()
        {
            PlayLoop(arpLoop);
        }

        private void PlayLoop(AudioClip clip)
        {
            StopAllCoroutines();

            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.volume = 1f;
            audioSource.Play();
        }

        public void FadeArp()
        {
            audioSource.DOFade(volumeDownLevel, volumeDownTime);
        }

        public void Stop()
        {
            StopAllCoroutines();

            audioSource.DOFade(0f, muteTime);
        }
    }
}