using UmbrellaPromo.AffinityPatches;
using UmbrellaPromo.Promo;
using Zenject;

namespace UmbrellaPromo.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PromoRepo>().AsSingle();
            Container.BindInterfacesAndSelfTo<DlcPromoPanelModelPatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<MusicPackPromoBannerSetupPatch>().AsSingle();
        }
    }
}
