using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Fun.Utilities;

namespace MusicAlarm
{
    class Program
    {

        // private static readonly Random _rand = new();
        private static int _musicCursor = 0;
        private static readonly string[] _musics = Directory.GetFiles(Path.Join(Environment.GetEnvironmentVariable("HOME"), "Music", "playlist"));

        private static readonly int _loopInterval = 60;
        private static readonly int _restInterval = 60 * 60;

        private static readonly List<int> _targets = new() { 23, 25, 28, 30, 53, 55, 58, 0 };
        // private static readonly List<int> _targets = new() { 27, 28, 29, 30, 57, 58, 59, 0 };

        // private static readonly List<int> _30minTargets = new() { 25, 27, 30, 32, 35 };
        // private static readonly List<int> _60minTargets = new() { 55, 57, 0, 2, 5 };

        // private static readonly string _30minMusic = @"/home/neko/Music/playlist/Wonki - Sunset Paradise.mp3";
        // private static readonly string _60minMusic = @"/home/neko/Music/playlist/We Are One - Vexento [Vlog No Copyright Music].wav";

        // private static readonly string _30minMusic = Path.Join(Environment.GetEnvironmentVariable("HOME"), "Music", "playlist", @"Wonki - Sunset Paradise.mp3");
        // private static readonly string _60minMusic = Path.Join(Environment.GetEnvironmentVariable("HOME"), "Music", "playlist", @"We Are One - Vexento [Vlog No Copyright Music].wav");

        private static int? played = null;

        static void Main(string[] args)
        {


            int seconds = DateTime.Now.Second;
            int diff = 60 - seconds;

            Pretty.ColorPrintln(Pretty.PaperLime300, $"{diff} seconds to start monitoring...");
            Thread.Sleep(diff * 1000);

            Pretty.ColorPrintln(Pretty.PaperLime300, "start monitoring...");


            while (true)
            {

                DateTime now = DateTime.Now;

                if (now.Hour < 12 && now.Hour >= 4)
                {
                    Pretty.ColorPrintln(Pretty.PaperLime300, $"sleep for {_restInterval} seconds");
                    Thread.Sleep(_restInterval * 1000);
                }

                int min = now.Minute;

                string music;

                // if (_30minTargets.Contains(min))
                // {
                //     music = _30minMusic;
                //     goto playing;
                // }
                // else if (_60minTargets.Contains(min))
                // {
                //     music = _60minMusic;
                //     goto playing;
                // }
                if (_targets.Contains(min))
                {
                    music = _musics[_musicCursor];
                    _musicCursor = (_musicCursor + 1) % _musics.Length;
                    goto playing;
                }
                else
                {
                    goto monitoring;
                }

            playing:
                if ((played is int && played != min) || played == null)
                {
                    played = min;
                    Pretty.ColorPrintln(Pretty.PaperAmber400, $"playing now: {music}");
                    Process.Start("totem", $"\"{music}\"");
                }

            monitoring:
                Pretty.ColorPrintln(Pretty.PaperLime300, $"sleep for {_loopInterval} seconds");
                Thread.Sleep(_loopInterval * 1000);
                continue;
            }


        }
    }
}
