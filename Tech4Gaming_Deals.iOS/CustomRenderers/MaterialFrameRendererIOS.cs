using System;
using CoreGraphics;
using Tech4Gaming_Deals.CustomControls;
using Tech4Gaming_Deals.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MaterialFrame), typeof(MaterialFrameRendererIOS))]
namespace Tech4Gaming_Deals.iOS.CustomRenderers
{
    public class MaterialFrameRendererIOS : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            // Update shadow to match material design style
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(0, 0);
            Layer.ShadowOpacity = 0.8f;
            Layer.MasksToBounds = false;
        }
    }
}
