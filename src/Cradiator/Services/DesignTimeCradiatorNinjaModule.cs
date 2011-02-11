using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Cradiator.Converters;
using Cradiator.Audio;
using System.Speech.Synthesis;
using Cradiator.Model;

namespace Cradiator.Services
{

    /// <summary>
    /// Loads the dependend items for the converters, so you can use the designer again in Visual Studio
    /// I use a MVVM like approach, setting a design object with some data so we have a designer with data
    /// Makes making screens a lot easier.
    /// This works great in VS2010
    /// having ninject in the converters makes things a lot more complicated than it needs be.
    /// </summary>
    public class DesignTimeCradiatorNinjaModule : NinjectModule
    {
        /// <summary>
        /// probably hacky code, but the designer shows again, feel free to clean this up
        /// got no experience with ninject
        /// </summary>
        public override void Load()
        {
            //for the speech converter
            Bind<ISpeechSynthesizer>().ToConstant(new CradiatorSpeechSynthesizer(new SpeechSynthesizer()));

            var ss = new SpeechSynthesizer();

            var css = new CradiatorSpeechSynthesizer(ss);
            var vs = new VoiceSelector(css);

            Bind<InstalledVoiceConverter>().ToConstant(new InstalledVoiceConverter( vs ));


            //for the build breakers
            var fs = new FixerStrategy();
            var bs = new BuildBuster( new Config.ConfigSettings(), fs, new GuiltFactory());
            Bind<IBuildBuster>().ToConstant(bs);

        }

    }
}
