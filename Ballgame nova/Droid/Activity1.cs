using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace MojehraDroid
{
    [Activity(//Label = "Mighty Square",
         MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        Hlavni game;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            game = new Hlavni();
             
            //disable UI od 4.4
            var view = (Android.Views.View)game.Services.GetService(typeof(Android.Views.View));
            view.SystemUiVisibility = (StatusBarVisibility)
                (SystemUiFlags.LayoutStable
                | SystemUiFlags.LayoutHideNavigation
                | SystemUiFlags.LayoutFullscreen
                | SystemUiFlags.HideNavigation
                | SystemUiFlags.Fullscreen
                //| SystemUiFlags.ImmersiveSticky
                );
            SetContentView(view);
            //SetContentView((View)g.Services.GetService(typeof(View)));
            game.Run();
            //if (g.vypnout)
            //{
            //    FinishAffinity();
            //    Java.Lang.JavaSystem.Exit(0);
            // tutovka Process.KillProcess(Process.MyPid());
            //}
        }

        //protected override void OnRestart()
        //{
        //    g.vypnout = false;
        //    g.Run();
        //}

        // nezabira mi
        private class MyUiVisibilityChangeListener : Java.Lang.Object, View.IOnSystemUiVisibilityChangeListener
        {
            View targetView;
            public MyUiVisibilityChangeListener(View v)
            {
                targetView = v;
            }
            public void OnSystemUiVisibilityChange(StatusBarVisibility v)
            {
                if (targetView.SystemUiVisibility != ((StatusBarVisibility)SystemUiFlags.HideNavigation
                    //| (StatusBarVisibility)SystemUiFlags.Immersive
                    ))
                {
                    targetView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation 
                        //| (StatusBarVisibility)SystemUiFlags.ImmersiveSticky
                        ;
                }
            }
        }
    }
}