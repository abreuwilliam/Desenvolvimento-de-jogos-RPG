using System;
using NAudio.Wave;

public class AudioPlayer : IDisposable
{
    private IWavePlayer? waveOut;
    private AudioFileReader? reader;
    private bool isStopping;
    private bool loop;

    // guardamos a referência ao handler para podermos removê-lo ao Stop()
    private EventHandler<StoppedEventArgs>? _playbackStoppedHandler;

    public void PlayLoop(string filePath)
    {
        Stop(); // garante estado limpo
        loop = true;
        isStopping = false;

        waveOut = new WaveOutEvent();
        reader  = new AudioFileReader(filePath);
        waveOut.Init(reader);

        // cria o handler e guarda a referência
        _playbackStoppedHandler = new EventHandler<StoppedEventArgs>((s, e) =>
        {
            try
            {
                // proteção extra: se estamos em processo de Stop, sai
                if (isStopping || reader == null || waveOut == null) return;

                if (loop)
                {
                    // tenta reiniciar a reprodução do início
                    try
                    {
                        reader.Position = 0;
                        waveOut.Play();
                    }
                    catch
                    {
                        // se Play falhar por qualquer razão, tenta reinicializar e reproduzir
                        try
                        {
                            // Re-Init só se for necessário/possível
                            waveOut.Init(reader);
                            waveOut.Play();
                        }
                        catch
                        {
                            // se mesmo assim falhar, suprime a exceção (não queremos crashar)
                        }
                    }
                }
            }
            catch
            {
                // suprime erros inesperados dentro do handler para evitar crash
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
                // remove o handler antes de parar/dispose — evita que o handler execute em um objeto descartado
                try { waveOut.PlaybackStopped -= _playbackStoppedHandler; } catch { /* ok */ }
            }

            try { waveOut?.Stop(); } catch { /* ok */ }
        }
        catch
        {
            // ignora falhas ao parar
        }

        try { reader?.Dispose(); } catch { /* ok */ }
        try { waveOut?.Dispose(); } catch { /* ok */ }

        reader = null;
        waveOut = null;
        _playbackStoppedHandler = null;
    }

    public void Dispose() => Stop();
}
