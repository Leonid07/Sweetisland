using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Button policyButton;
    public Button termsButton;
    public Button shareApp;


    [Header("Настройки для звуков и музыки")]
    public Button optionSound;
    public Text textSound;
    public Button optionMusic;
    public Text textMusic;

    [SerializeField] string _policyString = "https://www.termsfeed.com/live/397e77d8-4b88-43cf-a43f-64a3784fa74d";
    [SerializeField] string _termsString = "https://www.termsfeed.com/live/277c7336-26a6-435e-850e-feb0ee10f3ef";

    private UniWebView webView;

    private void Start()
    {
        policyButton.onClick.AddListener(PolicyView);
        termsButton.onClick.AddListener(TermsView);

        optionSound.onClick.AddListener(ToggleSound);
        optionMusic.onClick.AddListener(ToggleMusic);
        shareApp.onClick.AddListener(ShareApp);

        // Инициализация начального текста кнопок
        textSound.text = "Sound ON";
        textMusic.text = "Music ON";
    }

    void ToggleSound()
    {
        if (textSound.text == "Sound OFF")
        {
            textSound.text = "Sound ON";
            SoundManager.InstanceSound.soundLevelUnlock.volume = 1f; 
            SoundManager.InstanceSound.soundDamage.volume = 1f; 

        }
        else
        {
            textSound.text = "Sound OFF";
            SoundManager.InstanceSound.soundLevelUnlock.volume = 0f;
            SoundManager.InstanceSound.soundDamage.volume = 0f;
        }
    }

    void ToggleMusic()
    {
        if (textMusic.text == "Music OFF")
        {
            textMusic.text = "Music ON";
            SoundManager.InstanceSound.musicLevel.volume = 1f;
            SoundManager.InstanceSound.musicFon.volume = 1f;
        }
        else
        {
            textMusic.text = "Music OFF";
            SoundManager.InstanceSound.musicLevel.volume = 0f;
            SoundManager.InstanceSound.musicFon.volume = 0f;
        }
    }

    void ShareApp()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    void PolicyView()
    {
        OpenWebView(_policyString);
    }
    void TermsView()
    {
        OpenWebView(_termsString);
    }
    void OpenWebView(string url)
    {
        webView = gameObject.AddComponent<UniWebView>();

        webView.EmbeddedToolbar.Show();
        webView.EmbeddedToolbar.SetPosition(UniWebViewToolbarPosition.Top);
        webView.EmbeddedToolbar.SetDoneButtonText("Close");
        webView.EmbeddedToolbar.SetButtonTextColor(Color.white);
        webView.EmbeddedToolbar.SetBackgroundColor(Color.red);
        webView.EmbeddedToolbar.HideNavigationButtons();
        webView.OnShouldClose += (view) => {
            webView = null;
            return true;
        };

        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            if (statusCode == 200)
            {
                Debug.Log("WebView loaded successfully");
            }
            else
            {
                Debug.LogError("Failed to load WebView with status code: " + statusCode);
            }
        };

        webView.OnShouldClose += (view) =>
        {
            return true;
        };

        webView.Load(url);
        webView.Show();
        webView.EmbeddedToolbar.Show();
    }

    void OnDestroy()
    {
        if (webView != null)
        {
            webView.CleanCache();
            webView = null;
        }
    }
}
