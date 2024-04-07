using GoogleMobileAds.Api;
using InGameScene;
using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-5923819461869029/7315634573";
#elif UNITY_IPHONE
  private string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string adUnitId = "ca-app-pub-5923819461869029/7315634573";
#endif

    private static AdManager _instance;
    public static AdManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        LoadInterstitialAd();
        LoadRewardedAd();

    }

    private RewardedAd _rewardedAd;

    private void LoadRewardedAd()
    {
        // Initialize an InterstitialAd.
        _rewardedAd = new RewardedAd(adUnitId);
        // Called when an ad request has successfully loaded.
        _rewardedAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request has failed to load.
        _rewardedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.IsLoaded())
        {
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            _rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded ad loaded.");
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        if (args != null)
        {
            Debug.Log("Rewarded ad failed to load with error: " +
                       args.LoadAdError.GetMessage());
        }
    }
    //public void ShowRewardedAd()
    //{
    //    const string rewardMsg =
    //        "Rewarded ad rewarded the user. Type: {0}, amount: {0}.";

    //    if (rewardedAd != null && rewardedAd.CanShowAd())
    //    {
    //        rewardedAd.Show((Reward reward) =>
    //        {
    //            // TODO: Reward the user.
    //            Debug.Log(reward.Type + reward.Amount);
    //            reward.Type = "보석";
    //            reward.Amount = 1000;
    //            Managers.Game.UpdateUserData(0, 0, (float)reward.Amount);
    //            StaticManager.UI.AlertUI.OpenAlertUI("광고 보상", $"{reward.Type} {reward.Amount}개 가 지급 완료되었습니다.");
    //            InitAd();
    //        });
    //    }
    //}

    private InterstitialAd _interstitalAd;

    private void LoadInterstitialAd()
    {
        // Initialize an InterstitialAd.
        _interstitalAd = new InterstitialAd(adUnitId);
        // Called when an ad request has successfully loaded.
        _interstitalAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request has failed to load.
        _interstitalAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _interstitalAd.LoadAd(request);
    }
    public void ShowInterstitialAd()
    {
        if (_interstitalAd != null && _interstitalAd.IsLoaded())
        {
            _interstitalAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    public void HandleUserEarnedReward(object sender, Reward reward)
    {
        Debug.Log("Rewarded ad granted a reward: " +
                   reward.Amount);

        reward.Type = "보석";
        reward.Amount = 1000;
        Managers.Game.UpdateUserData(0, 0, (float)reward.Amount);
        StaticManager.UI.AlertUI.OpenAlertUI("광고 보상", $"{reward.Type} {reward.Amount}개 가 지급 완료되었습니다.");
        LoadRewardedAd();
    }
}