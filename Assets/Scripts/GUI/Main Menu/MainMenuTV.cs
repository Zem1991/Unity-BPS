using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenuTV : MonoBehaviour {
    public List<VideoClip> videosList = new List<VideoClip>();
    public VideoClip currentVideo;
    public bool lockFromMainMenu = false;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    private Coroutine coroutine;

    // Use this for initialization
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        Application.runInBackground = true;
        startNextVideo();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (videoPlayer.isPlaying)
        //    {
        //        pauseVideo(false);
        //    }
        //    else
        //    {
        //        unpauseVideo(false);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    if (!lockFromMainMenu)
        //        startNextVideo();
        //}
    }

    public void startNextVideo()
    {
        if (lockFromMainMenu)
            return;

        if (coroutine != null)
        {
            videoPlayer.Stop();
            audioSource.Stop();
            StopCoroutine(coroutine);
        }
        if (!currentVideo)
        {
            currentVideo = videosList[Random.Range(0, videosList.Count)];
        }
        else
        {
            int index = videosList.IndexOf(currentVideo) + 1;
            if (index >= videosList.Count)
                index = 0;
            currentVideo = videosList[index];
        }
        coroutine = StartCoroutine(playVideo());
    }

    public void pauseVideo()
    {
        if (videoPlayer.isPlaying)
        {
            pauseVideo(false);
        }
        else
        {
            unpauseVideo(false);
        }
    }

    public void pauseVideo(bool calledFromMainMenu)
    {
        if (calledFromMainMenu)
        {
            lockFromMainMenu = true;
        }
        videoPlayer.Pause();
        audioSource.Pause();
    }

    public void unpauseVideo(bool calledFromMainMenu)
    {
        if (calledFromMainMenu)
        {
            lockFromMainMenu = false;
        }
        if (!lockFromMainMenu)
        {
            videoPlayer.Play();
            audioSource.Play();
        }
    }

    IEnumerator playVideo()
    {
        //audioSource.Pause();

        //We want to play from video clip not from url
        videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        //videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = currentVideo;
        videoPlayer.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            //Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }

        //Debug.Log("Done Preparing Video");

        ////Assign the Texture from Video to RawImage to be displayed
        //image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        //Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }
        //Debug.Log("Done Playing Video");
    }
}
