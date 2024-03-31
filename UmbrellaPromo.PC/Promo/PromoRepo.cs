using Newtonsoft.Json;
using SiraUtil.Logging;
using SiraUtil.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace UmbrellaPromo.PC.Promo
{
    internal class PromoRepo
    {
        [Inject] internal IHttpService _httpService;
        [Inject] internal SiraLog _log;

        internal static readonly List<Promo> promoRepo = new List<Promo>();
        internal static bool hasRegistered = false;

        public async Task RegisterCustomPromos()
        {
            _log.Info("Registering Custom Promos...");

            string baseurl = "https://raw.githubusercontent.com/bs-umbrella/UmbrellaRepo/main/Promo/";
            IHttpResponse indexJsonTask = await _httpService.GetAsync(baseurl + "index.json");

            string indexJson = await indexJsonTask.ReadAsStringAsync();

            if (indexJson == null)
            {
                _log.Info("Failed to download index.json");
                return;
            }

            PromoIndexResponse index = JsonConvert.DeserializeObject<PromoIndexResponse>(indexJson);

            string featuredUrl = baseurl + index.featured.ToString();

            IHttpResponse featuredPromosTask = await _httpService.GetAsync(featuredUrl);

            string featuredPromos = await featuredPromosTask.ReadAsStringAsync();

            if (featuredPromos == null)
            {
                _log.Info("Failed to download featured.json");
                return;
            }

            FeaturedPromoResponse featuredPromoResponse = JsonConvert.DeserializeObject<FeaturedPromoResponse>(featuredPromos);

            List<Pack> packs = featuredPromoResponse.promos;

            foreach (Pack pack in packs)
            {
                string name = pack.name.ToString();
                string url = pack.url.ToString();
                bool quest = pack.supportedOnQuest;
                string image = pack.image.ToString();
                _log.Info($"Pack: {name} | URL: {url} | Quest: {quest} | Image: {image}");
            }
        }

        public void RegisterAdditionalPromos(DlcPromoPanelModel promoModel)
        {
            _log.Info("Registering Promos...");

            PromoInfoSO promoInfoSO = new PromoInfoSO
            {
                _bannerPromoTextPosition = 37.0f,
                _bannerPromoText = "<color=#bdacd1><size=150%>Custom Levels",
                _bannerImage = BeatSaberMarkupLanguage.Utilities.LoadSpriteFromAssemblyAsync("UmbrellaPromo.PC.Images.Customs-Banner-Notepad.png").Result
            };

            DlcPromoPanelModel.PromoInfo promoInfo = new DlcPromoPanelModel.PromoInfo("custom_levelpack_CustomLevels", promoInfoSO, PlayerSensitivityFlag.Safe);
            Promo customLevelsRepo = new Promo("custom_levels", PromoType.CustomLevels, true, 15, promoInfo);
            RegisterPromo(customLevelsRepo);

            foreach (DlcPromoPanelModel.PromoInfo DLCPromoInfo in promoModel._promoInfos)
            {
                Plugin.Logger.Info($"Registering Promo: {DLCPromoInfo}");
                Promo promo = new Promo();
                string id = DLCPromoInfo.id;
                promo.promoId = id;
                promo.promoType = PromoType.OST_DLC;
                promo.supportedOnQuest = true;
                promo.promoInfo = DLCPromoInfo;
                RegisterPromo(promo);
            }

            // TODO: Implement featured playlist loading when json is available
            hasRegistered = true;
        }

        public static void RegisterPromo(Promo promo)
        {
            promoRepo.Add(promo);
        }

        public static List<Promo> GetPromos()
        {
            return promoRepo;
        }

        public static bool HasRegistered()
        {
            return hasRegistered;
        }
    }

    [Serializable]
    internal class PromoIndexResponse
    {
        [JsonProperty("lastUpdatedEither")] public int lastUpdatedEither;
        [JsonProperty("featured")] public string featured;
        [JsonProperty("tournament")] public string tournament;
    }

    [Serializable]
    internal class FeaturedPromoResponse
    {
        [JsonProperty("packs")] public List<Pack> promos;
    }

    [Serializable]
    internal class Pack
    {
        [JsonProperty("name")] public string name;
        [JsonProperty("url")] public string url;
        [JsonProperty("quest")] public bool supportedOnQuest;
        [JsonProperty("image")] public string image;
    }

    internal class TournamentPromoResponse
    {
        //[JsonProperty("name")] public string name;
        //[JsonProperty("url")] public string url;
        //[JsonProperty("quest")] public bool supportedOnQuest;
        //[JsonProperty("image")] public string image;
    }
}
