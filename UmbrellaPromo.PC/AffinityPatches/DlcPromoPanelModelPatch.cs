using SiraUtil.Affinity;
using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UmbrellaPromo.PC.Promo;
using Zenject;
using static DlcPromoPanelModel;

namespace UmbrellaPromo.PC.AffinityPatches
{
    internal class DlcPromoPanelModelPatch : IAffinity
    {
        [Inject] internal PromoRepo _promoRepo;
        [Inject] internal SiraLog _log;

        [AffinityPatch(typeof(DlcPromoPanelModel), nameof(DlcPromoPanelModel.GetPackDataForMainMenuPromoBanner))]
        [AffinityPrefix]
        private PromoInfo GetPackDataForMainMenuPromoBannerPrefix(DlcPromoPanelModel __instance, out bool owned)
        {
            if (!PromoRepo.hasRegistered)
            {
                _log.Info("PromoRepo has not registered promos, registering promos...");
                _promoRepo.RegisterAdditionalPromos(__instance);
            }

            List<Promo.Promo> promos = PromoRepo.GetPromos();

            // Filter by type, sensitivity
            PlayerSensitivityFlag sensitivityFlag = __instance._playerDataModel.playerData.desiredSensitivityFlag;
            if (sensitivityFlag == PlayerSensitivityFlag.Unknown)
            {
                sensitivityFlag = PlayerSensitivityFlag.Themes;
            }

            List<PromoType> preferredTypes = new List<PromoType> { PromoType.OST_DLC, PromoType.CustomLevels };

            List<Promo.Promo> filteredPromos = promos.Where(promotion =>
                preferredTypes.Contains(promotion.promoType) &&
                promotion.promoInfo.contentRating <= sensitivityFlag
            ).ToList();

            // Select a random promo
            if (filteredPromos.Count == 0)
            {
                _log.Info("No promos found, returning null");
                owned = false; // idfk
                return null;
            }

            int totalWeight = filteredPromos.Sum(promotion2 => promotion2.weight);

            Random rd = new Random();
            int goalWeight = rd.Next(0, totalWeight);

            UmbrellaPromo.PC.Promo.Promo promo = null;
            foreach (Promo.Promo toFilterPromo in filteredPromos)
            {
                _log.Info($"Promo: {toFilterPromo.promoInfo.id} Weight: {toFilterPromo.weight}");
                totalWeight -= toFilterPromo.weight;
                if (totalWeight <= goalWeight)
                {
                    promo = toFilterPromo;
                    break;
                }
            }

            if (promo == null || promo.promoInfo == null)
            {
                _log.Info("Selected Promo is null, returning null");
                owned = false; // idfk
                return __instance._defaultPromoInfo;
            }

            _log.Info($"Selected Promo: {promo.promoInfo.id}");
            owned = true; // idfk
            return promo.promoInfo;
        }
    }
}
