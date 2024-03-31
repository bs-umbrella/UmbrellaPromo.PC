using UmbrellaPromo.PC.AffinityPatches;
using UmbrellaPromo.PC.Promo;
using Zenject;

namespace UmbrellaPromo.PC.Installers
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
