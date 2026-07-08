using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DeskForge.Controls;

/// <summary>
/// A ContentControl that fades and slides its content in whenever Content changes,
/// so switching sidebar pages reads as a smooth transition instead of an instant swap.
/// </summary>
public class AnimatedContentControl : ContentControl
{
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);
        if (newContent is null) return;

        var offset = new TranslateTransform(0, 16);
        RenderTransform = offset;
        BeginAnimation(OpacityProperty, null);
        Opacity = 0;

        var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(240))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        var slide = new DoubleAnimation(16, 0, TimeSpan.FromMilliseconds(280))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        BeginAnimation(OpacityProperty, fade);
        offset.BeginAnimation(TranslateTransform.YProperty, slide);
    }
}
