
using System.Reflection;
using System.Windows;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Cradiator")]
[assembly: AssemblyDescription("Full-screen Cruise Control project status information radiator")]
[assembly: AssemblyConfiguration("")]
[assembly: log4net.Config.XmlConfigurator(Watch = true)] 
// Specifies the location in which theme dictionaries are stored for types in an assembly.
[assembly: ThemeInfo(
    // Specifies the location of system theme-specific resource dictionaries for this project.
    // The default setting in this project is "None" since this default project does not
    // include these user-defined theme files:
    //     Themes\Aero.NormalColor.xaml
    //     Themes\Classic.xaml
    //     Themes\Luna.Homestead.xaml
    //     Themes\Luna.Metallic.xaml
    //     Themes\Luna.NormalColor.xaml
    //     Themes\Royale.NormalColor.xaml
    ResourceDictionaryLocation.None,

    // Specifies the location of the system non-theme specific resource dictionary:
    //     Themes\generic.xaml
    ResourceDictionaryLocation.SourceAssembly)]
[assembly: AssemblyVersionAttribute("2.7.0.0")]
[assembly: AssemblyFileVersionAttribute("2.7.0.0")]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyCompanyAttribute("www.codeplex.com/Cradiator")]
[assembly: AssemblyProductAttribute("Cradiator")]
[assembly: AssemblyCopyrightAttribute("www.codeplex.com/Cradiator")]
