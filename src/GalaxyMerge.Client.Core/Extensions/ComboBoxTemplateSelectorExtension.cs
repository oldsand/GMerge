#nullable enable
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaxyMerge.Client.Core.Selectors;

namespace GalaxyMerge.Client.Core.Extensions
{
    public class ComboBoxTemplateSelectorExtension : MarkupExtension
    {
        public DataTemplate? SelectedItemTemplate { get; set; }
        public DataTemplateSelector? SelectedItemTemplateSelector { get; set; }
        public DataTemplate? DropdownItemsTemplate { get; set; }
        public DataTemplateSelector? DropdownItemsTemplateSelector { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => new ComboBoxTemplateSelector
            {
                SelectedItemTemplate = SelectedItemTemplate,
                SelectedItemTemplateSelector = SelectedItemTemplateSelector,
                DropdownItemsTemplate = DropdownItemsTemplate,
                DropdownItemsTemplateSelector = DropdownItemsTemplateSelector
            };
    }
}