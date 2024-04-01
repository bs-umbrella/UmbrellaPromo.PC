namespace UmbrellaPromo.Promo
{
    public enum PromoType
    {
        OST_DLC,
        CustomLevels,
        FeaturedPlaylist
    }

    public class Promo
    {
        public string promoId;
        public PromoType promoType;
        public bool supportedOnQuest;
        public int weight = 1;
        public DlcPromoPanelModel.PromoInfo promoInfo;

        public Promo() { }

        public Promo(string promoId, PromoType promoType, bool supportedOnQuest, int weight, DlcPromoPanelModel.PromoInfo promoInfo)
        {
            this.promoId = promoId;
            this.promoType = promoType;
            this.supportedOnQuest = supportedOnQuest;
            this.weight = weight;
            this.promoInfo = promoInfo;
        }
    }
}
