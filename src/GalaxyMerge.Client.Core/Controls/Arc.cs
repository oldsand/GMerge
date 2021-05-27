using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GalaxyMerge.Client.Core.Controls
{
    public sealed class Arc : Shape
    {
        public Point Center
        {
            get => (Point) GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point), typeof(Arc),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

        public double StartAngle
        {
            get => (double) GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }
        
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double EndAngle
        {
            get => (double) GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }
        
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(Math.PI / 2.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get => (double) GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }
        
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool SmallAngle
        {
            get => (bool) GetValue(SmallAngleProperty);
            set => SetValue(SmallAngleProperty, value);
        }
        
        public static readonly DependencyProperty SmallAngleProperty =
            DependencyProperty.Register(nameof(SmallAngle), typeof(bool), typeof(Arc),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static Arc() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));

        protected override Geometry DefiningGeometry
        {
            get
            {
                var a0 = StartAngle < 0 ? StartAngle + 2 * Math.PI : StartAngle;
                var a1 = EndAngle < 0 ? EndAngle + 2 * Math.PI : EndAngle;

                if (a1 < a0)
                    a1 += Math.PI * 2;

                var sweepDirection = SweepDirection.Counterclockwise;
                bool large;

                if (SmallAngle)
                {
                    large = false;
                    sweepDirection = (a1 - a0) > Math.PI ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                }
                else
                    large = Math.Abs(a1 - a0) < Math.PI;

                var p0 = Center + new Vector(Math.Cos(a0), Math.Sin(a0)) * Radius;
                var p1 = Center + new Vector(Math.Cos(a1), Math.Sin(a1)) * Radius;

                var segments = new List<PathSegment>
                {
                    new ArcSegment(p1, new Size(Radius, Radius), 0.0, large, sweepDirection, true)
                };

                var figures = new List<PathFigure>
                {
                    new PathFigure(p0, segments, true)
                    {
                        IsClosed = false
                    }
                };

                return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }
    }
}