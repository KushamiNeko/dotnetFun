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

    private static readonly Random _rand = new();
    private static readonly string[] _musics = Directory.GetFiles(@"/home/neko/Music/playlist");

    private static readonly int _loopInterval = 60;
    private static readonly int _restInterval = 60 * 60;

    private static readonly List<int> _targets = new() { 25, 27, 30, 32, 35, 55, 57, 0, 2, 5 };

    static void Act()
    {
      string music = _musics[_rand.Next(0, _musics.Length)];

      // const string music = @"/home/neko/Music/playlist/We_Are_One_Vexento.wav";
      Pretty.ColorPrintln(Pretty.PaperAmber400, $"playing now: {music}");

      Process.Start("totem", $"\"{music}\"");
    }

    static void Main(string[] args)
    {

      int? cursor = null;

      int seconds = DateTime.Now.Second;
      int diff = 60 - seconds;

      Pretty.ColorPrintln(Pretty.PaperLime300, $"{diff} seconds to start monitoring...");
      Thread.Sleep(diff * 1000);

      Pretty.ColorPrintln(Pretty.PaperLime300, "start monitoring...");


      while (true)
      {

        DateTime now = DateTime.Now;

        if (now.Hour < 12 && now.Hour >= 3)
        {
          Pretty.ColorPrintln(Pretty.PaperLime300, $"sleep for {_restInterval} seconds");
          Thread.Sleep(_restInterval * 1000);
        }

        int min = now.Minute;

        if (cursor is int c)
        {
          if (min == _targets[c])
          {
            Act();
            cursor = (_targets.IndexOf(min) + 1) % _targets.Count;
          }

        }
        else
        {

          if (_targets.Contains(min))
          {
            Act();
            cursor = (_targets.IndexOf(min) + 1) % _targets.Count;
          }

        }

        Pretty.ColorPrintln(Pretty.PaperLime300, $"sleep for {_loopInterval} seconds");
        Thread.Sleep(_loopInterval * 1000);
      }

    }
  }
}
