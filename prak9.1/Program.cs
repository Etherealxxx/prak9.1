using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        MenuManager menu = new MenuManager();
        menu.RootPage();
    }
}


public class MenuManager
{
    List<Process> Processes = new List<Process>();
    enum Commands
    {
        D = -1,
        Delete = -2,
        Backspace = -3
    }

    [Obsolete]
    public void RootPage()
    {
        Console.Clear();
        Processes = ProcessManager.ShowProcesses();
        MainManager(startPos: 0, count: Processes.Count());
    }
    public void MainManager(int startPos = 0, int count = 3, string arrow = "->")
    {
        string empty = new string(' ', arrow.Length);
        int i = 0;
        Console.SetCursorPosition(0, 0);
        Console.Write(arrow);
        ConsoleKeyInfo key;

        for (; ; )
        {
            key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    if (i == count - 1)
                        continue;
                    Console.SetCursorPosition(0, i);
                    Console.Write(empty);
                    Console.SetCursorPosition(0, ++i);
                    Console.Write(arrow);
                    break;
                case ConsoleKey.UpArrow:
                    if (i == startPos)
                        continue;
                    Console.SetCursorPosition(0, i);
                    Console.Write(empty);
                    Console.SetCursorPosition(0, --i);
                    Console.Write(arrow);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    ProcManager(i);
                    break;
            }
        }
    
}

    [Obsolete]
    public void ProcManager(int numb)
    {
        Process proc = Processes[numb];
        ProcessManager.ShowProcessData(proc);
        int action = ProcMenu();
        switch (action)
        {
            case (int)Commands.D:
                DropProcess(proc);
                RootPage();
                break;
            case (int)Commands.Delete:
                DropAllProcessByName(proc);
                RootPage();
                break;
            case (int)Commands.Backspace:
                RootPage();
                break;
        }
    }

    private void DropProcess(Process process)
    {
        try
        {
            process.Kill();
        }
        catch { }
    }
    private void DropAllProcessByName(Process process)
    {
        foreach (Process proc in Processes)
        {
            try
            {
                if (proc.ProcessName == process.ProcessName)
                    proc.Kill();
            }
            catch { }
        }
    }

    private int ProcMenu()
    {
        ConsoleKeyInfo key;

        for (; ; )
        {
            key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.D:
                    return (int)Commands.D;
                case ConsoleKey.Delete:
                    return (int)Commands.Delete;
                case ConsoleKey.Backspace:
                    Console.Clear();
                    return (int)Commands.Backspace;
            }
        }
    }
}

static class ProcessManager
{

    [Obsolete]
    public static List<Process> ShowProcesses()
    {
        
        List<Process> processes = new List<Process>(Process.GetProcesses().ToList());
        foreach (Process process in processes)
        {
            Console.WriteLine("  " + process.ProcessName + "           " + process.PagedMemorySize + "           " + process.WorkingSet);
        }
        return processes;
    }

    [Obsolete]
    public static void ShowProcessData(Process process)
    {
        Console.Clear();
        Console.WriteLine(process.ProcessName);
        Console.WriteLine("----------------------------------------");
        try
        {
            Console.WriteLine("Использование диска: " + process.WorkingSet);

            Console.WriteLine("Приоритет: " + process.BasePriority);

            Console.WriteLine("Класс приоритета: " + process.PriorityClass);

            Console.WriteLine("Время использования процесса: " + process.UserProcessorTime);

            Console.WriteLine("Все время использования: " + process.TotalProcessorTime);

            Console.WriteLine("Использование оперативной памяти: " + process.PagedMemorySize);
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("D - завершить выбранный процесс");
            Console.WriteLine("Delete - завершить все процессы с таким именем");
        }
        catch
        {
            Console.Clear();
            Console.WriteLine("Отказано в доступе");
        }
        finally
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Backspace - вернуться в главное меню");
        }
    }
}