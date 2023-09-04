using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;

namespace LaunchpadSamplingApp.Components
{
    public class Launchpad : TemplatedControl
    {
        private double _size = 100;
        public double Size { get { return _size; } set {  _size = value; } }

        private Grid _grid;
        private Border _innerBorder, _outerBorder;


        public Launchpad() 
        {
            
        }

        protected override Control GetTemplateFocusTarget()
        {
            return base.GetTemplateFocusTarget();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _grid = e.NameScope.Find<Grid>("PART_Grid");
            _innerBorder = e.NameScope.Find<Border>("PART_InnerBorder");
            _outerBorder = e.NameScope.Find<Border>("PART_OuterBorder");
        }

        protected override void OnTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            base.OnTemplateChanged(e);
        }
    }
}
