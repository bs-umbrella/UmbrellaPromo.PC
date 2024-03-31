using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace UmbrellaPromo.PC
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger, IPA.Config.Config conf, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.UseHttpService();
        }
    }
}
