using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class RewardedAdmob : MonoBehaviour
{
    public ZDGPlayer player;
    public ZDGGameController gameController;

    public string rewardedID = "ca-app-pub-3940256099942544/5224354917";
    public bool isAdOpened;
    public bool isRewardGiven;
    public bool isAdClosed;

    private RewardedAd _rewardedAd;

    [SerializeField]
    private GameObject timerForResume;

    private void Awake()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
    }

    public void Start()
    {
        isAdOpened = false;
        isRewardGiven = false;
        isAdClosed = false;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardedID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                if (isAdOpened == true && isRewardGiven == true && gameController.isGameOver == true)
                {
                    player.fuel += 2000000000;
                    player.health += 75;
                }
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                //RegisterReloadHandler(_rewardedAd);
            });
        }
    }

    /*private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }*/

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
            isAdOpened = true;
            isRewardGiven = true;
            Time.timeScale = 0f;
            Debug.Log("Rewarded.");
            // here goes the reward
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            isAdClosed = true;
            timerForResume.gameObject.SetActive(true);
            Debug.Log("Rewarded ad full screen content closed.");
            if (isAdOpened == true)
            {
                gameController.gameOverCanvas.gameObject.SetActive(false);
                gameController.gameCanvas.gameObject.SetActive(true);
                StartCoroutine(RewardAdInformationWindow(0));
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedAd();
            };
        };

    }

    IEnumerator RewardAdInformationWindow(float delay)
    {
        if (timerForResume)
        {
            yield return new WaitForSeconds(3f);

            if (isAdClosed == true)
            {
                isAdOpened = false;
                isRewardGiven = true;
                gameController.isGameOver = false;
                timerForResume.SetActive(false);
            }
        }

        yield return null;
    }
}
