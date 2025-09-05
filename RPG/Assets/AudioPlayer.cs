using System;
using NAudio.Wave;

public class AudioPlayer : IDisposable
{
    private IWavePlayer? waveOut;
    private AudioFileReader? reader;
    private bool isStopping;
    private bool loop;

    public void PlayLoop(string filePath)
    {
        Stop(); // garante estado limpo
        loop = true;
        isStopping = false;

        waveOut = new WaveOutEvent();
        reader  = new AudioFileReader(filePath);
        waveOut.Init(reader);

        // quando terminar, reinicia se for loop e não for um Stop() manual
        waveOut.PlaybackStopped += (s, e) =>
        {
            if (isStopping || reader == null || waveOut == null) return;
            if (loop)
            {
                reader.Position = 0;
                waveOut.Play();
            }
        };

        waveOut.Play();
    }

    public void Stop()
    {
        isStopping = true;
        loop = false;
        try { waveOut?.Stop(); } catch { /* ok */ }
        reader?.Dispose();
        waveOut?.Dispose();
        reader = null;
        waveOut = null;
    }

    public void Dispose() => Stop();
}
