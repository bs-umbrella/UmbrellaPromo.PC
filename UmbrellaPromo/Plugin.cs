﻿using IPA;
using SiraUtil.Zenject;
using UmbrellaPromo.Installers;
using IPALogger = IPA.Logging.Logger;

namespace UmbrellaPromo
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        internal static IPALogger Logger { get; private set; }

        [Init]
        public void Init(IPALogger logger, IPA.Config.Config conf, Zenjector zenjector)
        {
            Logger = logger;
            zenjector.UseLogger(logger);
            zenjector.UseHttpService();
            zenjector.Install<MenuInstaller>(Location.Menu);
        }
    }
}
