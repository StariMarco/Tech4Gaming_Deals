using System;
using Android.Content;
using Android.Views;
using Tech4Gaming_Deals.CustomControls;
using Tech4Gaming_Deals.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryDroid))]
namespace Tech4Gaming_Deals.Droid.CustomRenderers
{
    public class RoundedEntryDroid : EntryRenderer
    {
        public RoundedEntryDroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            if(e.OldElement == null)
            {
                Control.Background = Android.App.Application.Context.GetDrawable(Resource.Layout.rounded_entry);
                Control.Gravity = GravityFlags.CenterVertical;
                Control.SetPadding(10, 0, 0, 0);
            }
        }
    }
}
