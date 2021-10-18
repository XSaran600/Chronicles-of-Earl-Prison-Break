using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

// https://answers.unity.com/questions/1479729/load-scene-after-video-ended.html

public class LoadSceneAfterCinematic : MonoBehaviour {

    public VideoPlayer VideoPlayer; // Drag & Drop the GameObject holding the VideoPlayer component
    void Start()
    {
        VideoPlayer.loopPointReached += LoadScene;
    }

    private void Update()
    {
        if(Input.anyKey)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }
}