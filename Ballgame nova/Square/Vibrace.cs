using Android.OS;

namespace Square
{
    public static class Vibrace
    {
        private static Vibrator vibrator;
        private static VibrationEffect vibe;
        private static VibFunc vibrace = Null;
        private delegate void VibFunc(int ms);

        public static void Init(Vibrator vib)
        {
            vibrator = vib;
            if (vibrator != null && vibrator.HasVibrator)
            {
                if (Build.VERSION.SdkInt <= BuildVersionCodes.NMr1)
                    vibrace = VibrateOld;
                else
                    vibrace = Vibrate;
            }
            else
                vibrace = Null;
        }

        public static void Vibruj(int ms)
        {
            vibrace(ms);
        }

        private static void VibrateOld(int ms)
        {
            vibrator.Vibrate(ms);     
        }

        private static void Vibrate(int ms)
        {
            vibe = VibrationEffect.CreateOneShot(ms, VibrationEffect.DefaultAmplitude);
            vibrator.Vibrate(vibe, null);
        }

        private static void Null(int ms)
        {
            return;
        }
    }
}
