using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundsDictionary sounds = new SoundsDictionary();

   private void Start()
    {
       Init();
    }

    #region Init

    private void Init()
    {
        SubscribeOnEvents();
    }
    
    private void SubscribeOnEvents()
    {
        GameEvents.Instance.TileClicked += PlaySound;
        GameEvents.Instance.WrongTileClicked += PlaySound;
        GameEvents.Instance.TilesSwapped += PlaySound;
    }

    #endregion

    private void PlaySound(object sender, MyEventArgs args)
    {
        audioSource.clip = sounds[args.soundType];
        audioSource.Play();
    }

    public void PlaySound(SoundType soundType)
    {
        audioSource.clip = sounds[soundType];
        audioSource.Play();
    }

    private void OnDisable()
    {
        GameEvents.Instance.TileClicked -= PlaySound;
    }
}