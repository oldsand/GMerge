using System.Windows;
using System.Windows.Markup;


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page,
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page,
    // app, or any theme specific resource dictionaries)
)]

//XAML/CLR Namespacing mapping 
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.AttachedProperties")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Behaviors")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Common")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Controls")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Converters")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Extensions")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Mvvm")]
[assembly: XmlnsDefinition("http://gmerge.com/ui/wpf", "GalaxyMerge.Client.Core.Themes")]

[assembly: XmlnsPrefix("http://gmerge.com/ui/wpf", "ui")]