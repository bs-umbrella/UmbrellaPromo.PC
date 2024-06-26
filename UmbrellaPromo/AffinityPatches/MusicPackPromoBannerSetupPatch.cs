﻿using JetBrains.Annotations;
using SiraUtil.Affinity;
using SiraUtil.Logging;
using Zenject;

namespace UmbrellaPromo.AffinityPatches
{
    internal class MusicPackPromoBannerSetupPatch : IAffinity
    {
        [Inject] internal SiraLog _log;

        [AffinityPatch(typeof(MusicPackPromoBanner), nameof(MusicPackPromoBanner.Setup))]
        [AffinityPrefix]
        private void SetupPrefix(MusicPackPromoBanner __instance, DlcPromoPanelModel.PromoInfo newPromoInfo, bool probablyOwned)
        {
            _log.Info("Setting up Promo [MusicPackPromoBanner.Setup]");
            if (newPromoInfo == null)
            {
                return;
            }
        }

        [AffinityPatch(typeof(MusicPackPromoBanner), nameof(MusicPackPromoBanner.Setup))]
        [AffinityPostfix]
        private void SetupPostfix(MusicPackPromoBanner __instance, DlcPromoPanelModel.PromoInfo newPromoInfo, bool probablyOwned)
        {
            _log.Info("Finished setting up Promo [MusicPackPromoBanner.Setup]");
            __instance._promoText.richText = true;
            __instance.gameObject.SetActive(true);
        }

        [AffinityPatch(typeof(MainMenuViewController), "DidActivate")]
        [AffinityPostfix]
        [AffinityAfter(new string[] { "com.BeatSaber.AdBlocker" })]
        internal void Postfix(ref MusicPackPromoBanner ____musicPackPromoBanner) => ____musicPackPromoBanner.gameObject.SetActive(true);
    }
}
