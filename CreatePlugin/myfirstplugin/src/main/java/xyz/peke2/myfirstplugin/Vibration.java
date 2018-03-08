package xyz.peke2.myfirstplugin;

import android.content.Context;
import android.os.VibrationEffect;
import android.os.Vibrator;

/**
 * Created by peke2 on 2018/03/05.
 */

public class Vibration {
    Vibrator vibrator;

    public Vibration(Context context)
    {
        vibrator = (Vibrator)context.getSystemService(Context.VIBRATOR_SERVICE);
    }

    public boolean hasFunction()
    {
        if( vibrator == null )  return false;
        return vibrator.hasVibrator();
    }

    public boolean hasAmplitudeControl()
    {
        if( vibrator == null )  return false;
        return vibrator.hasAmplitudeControl();
    }

    public void oneShot(long milliseconds, int amplitude)
    {
        if( vibrator == null )  return;

        VibrationEffect effect = VibrationEffect.createOneShot(milliseconds, amplitude);
        vibrator.vibrate(effect);
    }

    public void vibrate(long[] timings, int[] amplitudes, int repeat)
    {
        if( vibrator == null )  return;

        VibrationEffect effect = VibrationEffect.createWaveform(timings, amplitudes, repeat);
        vibrator.vibrate(effect);
    }

    public void cancel()
    {
        vibrator.cancel();
    }
}
