using MP3Updater.Core;
using ShellProgressBar;
using System;
using System.IO;
using System.Text;
using System.Threading;



  
    public class Mp3UpdaterProgressBar 
    {
    internal  void drawTextProgressBar(int progress, int total)
    {
        //draw empty progress bar
        Console.CursorLeft = 0;
        Console.Write("["); 
        Console.CursorLeft = 32;
        Console.Write("]"); 
        Console.CursorLeft = 1;
        float onechunk = 30.0f / total;

        //draw filled part
        int position = 1;
        for (int i = 0; i < onechunk * progress; i++)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.CursorLeft = position++;
            Console.Write(" ");
        }

        //draw unfilled part
        for (int i = position; i <= 31; i++)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.CursorLeft = position++;
            Console.Write(" ");
        }

        //draw totals
        Console.CursorLeft = 35;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
    }

    public void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action childAction = null)
    {
        var initialMessage = pbar.Message;
        for (var i = 0; i < ticks; i++)
        {
            pbar.Message = $"Start {i + 1} of {ticks}: {initialMessage}";
            childAction?.Invoke();
            Thread.Sleep(sleep);
            pbar.Tick($"End {i + 1} of {ticks}: {initialMessage}");
        }
    }
}

