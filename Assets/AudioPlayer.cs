using System;
using System.Collections.Generic;
using NAudio.Wave;

public class AudioPlayer : IDisposable
{
    private IWavePlayer? waveOut;
    private AudioFileReader? reader;
    private bool isStopping;
    private bool loop;

    // Pilha de músicas anteriores
    private Stack<string> musicStack = new Stack<string>();

    private EventHandler<StoppedEventArgs>? _playbackStoppedHandler;

    public void PlayLoop(string fileName)
{
    Stop();
    loop = true;
    isStopping = false;

    // 👉 Cria o caminho completo baseado na pasta /Assets
    string fullPath = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);

    if (!File.Exists(fullPath))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERRO] Arquivo não encontrado: {fullPath}");
        Console.ResetColor();
        return;
    }

    waveOut = new WaveOutEvent();
    reader = new AudioFileReader(fullPath);
    waveOut.Init(reader);

    _playbackStoppedHandler = (s, e) =>
    {
        if (isStopping || reader == null || waveOut == null)
            return;

        if (loop)
        {
            reader.Position = 0;
            waveOut.Play();
        }
    };

    waveOut.PlaybackStopped += _playbackStoppedHandler;
    waveOut.Play();
}


    // Salva música atual, troca por nova e retorna um objeto disposable
    public IDisposable Push(string newMusic)
    {
        string oldMusic = reader != null ? reader.FileName : "";

        if (!string.IsNullOrEmpty(oldMusic))
            musicStack.Push(oldMusic);

        PlayLoop(newMusic);

        return new PopMusic(this);
    }

    // Volta para a música anterior
    public void Pop()
    {
        if (musicStack.Count > 0)
        {
            string previous = musicStack.Pop();
            PlayLoop(previous);
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        isStopping = true;
        loop = false;

        try
        {
            if (waveOut != null && _playbackStoppedHandler != null)
                waveOut.PlaybackStopped -= _playbackStoppedHandler;

            waveOut?.Stop();
        }
        catch { }

        reader?.Dispose();
        waveOut?.Dispose();

        reader = null;
        waveOut = null;
        _playbackStoppedHandler = null;
    }

    public void Dispose() => Stop();

    private class PopMusic : IDisposable
    {
        private readonly AudioPlayer _player;

        public PopMusic(AudioPlayer player)
        {
            _player = player;
        }

        public void Dispose()
        {
            _player.Pop();
        }
    }
}
