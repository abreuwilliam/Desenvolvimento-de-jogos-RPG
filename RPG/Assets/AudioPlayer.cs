using System;
using NAudio.Wave;

public class AudioPlayer : IDisposable
{
    private IWavePlayer? waveOut;
    private AudioFileReader? reader;
    private bool isStopping;
    private bool loop;

    
    private EventHandler<StoppedEventArgs>? _playbackStoppedHandler;

    public void PlayLoop(string filePath)
    {
        Stop(); 
        loop = true;
        isStopping = false;

        waveOut = new WaveOutEvent();
        reader  = new AudioFileReader(filePath);
        waveOut.Init(reader);

        
        _playbackStoppedHandler = new EventHandler<StoppedEventArgs>((s, e) =>
        {
            try
            {
                
                if (isStopping || reader == null || waveOut == null) return;

                if (loop)
                {
                   
                    try
                    {
                        reader.Position = 0;
                        waveOut.Play();
                    }
                    catch
                    {
                        
                        try
                        {
                            
                            waveOut.Init(reader);
                            waveOut.Play();
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            catch
            {
                
            }
        });

        waveOut.PlaybackStopped += _playbackStoppedHandler;

        waveOut.Play();
    }

    public void Stop()
    {
        isStopping = true;
        loop = false;

        try
        {
            if (waveOut != null && _playbackStoppedHandler != null)
            {
                
                try { waveOut.PlaybackStopped -= _playbackStoppedHandler; } catch { /* ok */ }
            }

            try { waveOut?.Stop(); } catch { /* ok */ }
        }
        catch
        {
            
        }

        try { reader?.Dispose(); } catch { /* ok */ }
        try { waveOut?.Dispose(); } catch { /* ok */ }

        reader = null;
        waveOut = null;
        _playbackStoppedHandler = null;
    }

    public void Dispose() => Stop();
}
