// ***********************************************************************
// Assembly         : NmtCustomControls
// Author           : Niel Morgan Thomas
// Created          : 12-19-2013
//
// Last Modified By : Niel Morgan Thomas
// Last Modified On : 12-21-2013
// ***********************************************************************
// <copyright file="WindowsProgressRing.cs" company="Niel Morgan thomas">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NMT.Wpf.Controls
{
  /// <summary>
  /// Class WindowsProgressRing.
  /// </summary>
  [TemplatePart(Name = "PART_body", Type = typeof(Grid))]
  public class WindowsProgressRing : Control
  {
    /// <summary>
    /// The part body
    /// </summary>
    private Grid partBody;

    #region -- Properties --

    public Grid Body { get { return partBody; } }

    #region Speed
    /// <summary>
    /// The speed property
    /// </summary>
    public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
      "Speed ", typeof(Duration), typeof(WindowsProgressRing), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2.5)),
        FrameworkPropertyMetadataOptions.AffectsRender, SpeedChanged, SpeedValueCallback));

    /// <summary>
    /// Gets or sets the speed.
    /// </summary>
    /// <value>The speed.</value>
    public Duration Speed
    {
      get { return (Duration)GetValue(SpeedProperty); }
      set { SetValue(SpeedProperty, value); }
    }
    /// <summary>
    /// Speed changed.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
    private static void SpeedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      var wpr = (WindowsProgressRing) dependencyObject;
      if (wpr.Body == null) return;
      var speed = (Duration)dependencyPropertyChangedEventArgs.NewValue;
      wpr.SetStoryBoard(speed);
    }

    /// <summary>
    /// Speed value callback.
    /// </summary>
    /// <param name="dependencyObject">The dependency object.</param>
    /// <param name="baseValue">The base value.</param>
    /// <returns>System.Object.</returns>
    private static object SpeedValueCallback(DependencyObject dependencyObject, object baseValue)
    {
      if (((Duration)baseValue).HasTimeSpan && ((Duration)baseValue).TimeSpan > TimeSpan.FromSeconds(5))
        return new Duration(TimeSpan.FromSeconds(5));
      return baseValue;
    }
    #endregion // Speed

    #region Items
    /// <summary>
    /// The items property
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(int),
      typeof(WindowsProgressRing), new FrameworkPropertyMetadata(6,
        FrameworkPropertyMetadataOptions.AffectsRender, ItemsChanged, ItemsValueCallback));

    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>The items.</value>
    public int Items
    {
      get { return (int)GetValue(ItemsProperty); }
      set { SetValue(ItemsProperty, value); }
    }

    /// <summary>
    /// Items changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
    private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wpr = (WindowsProgressRing)d;
      if (wpr.Body == null) return;
      wpr.Body.Children.Clear();
      var items = (int)e.NewValue;
      for (var i = 0; i < items; i++)
      {
        var ellipse = new Ellipse
                              {
                                VerticalAlignment = VerticalAlignment.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                ClipToBounds = false,
                                RenderTransformOrigin = new Point(0.5, 2.5)
                              };
        wpr.Body.Children.Add(ellipse);
        // Binding 
        var binding = new Binding(ForegroundProperty.Name)
                      {
                        RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (WindowsProgressRing), 1)
                      };
        BindingOperations.SetBinding(ellipse, Shape.FillProperty, binding);
        // Placement
        Grid.SetColumn(ellipse, 2);
        Grid.SetRow(ellipse, 0);
      }
      wpr.SetStoryBoard(wpr.Speed);
    }

    /// <summary>
    /// Items callback.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="basevalue">The base value.</param>
    /// <returns>System.Object.</returns>
    private static object ItemsValueCallback(DependencyObject d, object basevalue)
    {
      if ((int)basevalue > 20)
        return 20;
      if ((int)basevalue < 1)
        return 1;
      return basevalue;
    }
    #endregion

    #endregion

    /// <summary>
    /// Sets the story board.
    /// </summary>
    /// <param name="speed">The speed.</param>
    private void SetStoryBoard(Duration speed)
    {
      int delay = 0;
      foreach (Ellipse ellipse in partBody.Children)
      {
        ellipse.RenderTransform = new RotateTransform(0);
        var animation = new DoubleAnimation(0, -360, speed)
        {
          RepeatBehavior = RepeatBehavior.Forever,
          EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseInOut },
          BeginTime = TimeSpan.FromMilliseconds(delay += 100)
        };
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        Storyboard.SetTarget(animation, ellipse);
        Storyboard.SetTargetProperty(animation, new PropertyPath("(Rectangle.RenderTransform).(RotateTransform.Angle)"));
        storyboard.Begin();
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsProgressRing" /> class.
    /// </summary>
    public WindowsProgressRing()
    {
      var res = (ResourceDictionary)Application.LoadComponent(new Uri("/WindowsProgressRing;component/Themes/WindowsProgressRingStyle.xaml", UriKind.Relative));
      Style = (Style)res["WindowsProgressRingStyle"];
    }

    /// <summary>
    /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
    /// </summary>
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      partBody = Template.FindName("PART_body", this) as Grid;
      ItemsChanged(this, new DependencyPropertyChangedEventArgs(ItemsProperty, 0, Items));
      SpeedChanged(this, new DependencyPropertyChangedEventArgs(SpeedProperty, Duration.Forever, Speed));
    }
  }
}
