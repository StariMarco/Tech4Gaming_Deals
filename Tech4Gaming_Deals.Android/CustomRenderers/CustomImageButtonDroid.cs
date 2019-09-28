using System;
using Tech4Gaming_Deals.CustomControls;
using Tech4Gaming_Deals.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomImageButton), typeof(CustomImageButtonDroid))]
namespace Tech4Gaming_Deals.Droid.CustomRenderers
{
    public class CustomImageButtonDroid : ImageButtonRenderer
    {
        public CustomImageButtonDroid(Context context) : base(context)
        {
             
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
        {
            base.OnElementChanged(e);

            
        }
    }
}
