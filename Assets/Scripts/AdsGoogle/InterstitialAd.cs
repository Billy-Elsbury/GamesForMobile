using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAd : MonoBehaviour
{

    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
    #elif UNITY_IPHONE
      private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
      private string _adUnitId = "unused";
    #endif

    private InterstitialAd _interstitialAd;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    private string GetResponseInfo()
    {
        throw new NotImplementedException();
    }

    private static void Load(string adUnitId, AdRequest adRequest, Action<InterstitialAd, LoadAdError> value)
    {
        throw new NotImplementedException();
    }

    private void Destroy()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void Show()
    {
        throw new NotImplementedException();
    }

    private bool CanShowAd()
    {
        throw new NotImplementedException();
    }
}
