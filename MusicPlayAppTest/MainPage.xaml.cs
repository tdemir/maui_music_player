using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;

namespace MusicPlayAppTest;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnPlayMusicClicked(object sender, EventArgs e)
	{
        StopMusic();

        var pickedFiles = await FilePicker.Default.PickMultipleAsync(new PickOptions() { PickerTitle = "select mp3 files (max 5)" });
        List<FileResult> allMp3Files = null;
        if (pickedFiles != null)
        {
            allMp3Files = pickedFiles.Where(x => x.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)).Take(5).ToList();
                     
        }

        if (allMp3Files != null && allMp3Files.Any())
        {
            foreach (var item in allMp3Files)
            {
                await PlayMusic(item);
            }
        }
        else
        {
            await DisplayAlert("Alert", "no file found", "OK");
        }
    }

    private void OnStopMusicClicked(object sender, EventArgs e)
    {
        StopMusic();
    }

    List<IAudioPlayer> currentAudioPlayers = new List<IAudioPlayer>();

    private async Task PlayMusic(FileResult file)
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(file.FullPath));
        currentAudioPlayers.Add(audioPlayer);
        audioPlayer.Play();
    }

    private async Task StopMusic()
    {
        for (int i = 0; i < currentAudioPlayers.Count; i++)
        {
            currentAudioPlayers[i].Stop();
            currentAudioPlayers.RemoveAt(i);
            i--;
        }
    }
}


