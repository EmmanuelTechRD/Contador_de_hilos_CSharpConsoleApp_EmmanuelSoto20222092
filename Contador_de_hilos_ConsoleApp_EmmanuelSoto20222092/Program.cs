using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Dictionary<int, Counter> counters = new Dictionary<int, Counter>();
    static bool running = true;

    static void Main()
    {
        while (running)
        {
            Console.WriteLine("\nMenú:");
            Console.WriteLine("1. Iniciar un contador");
            Console.WriteLine("2. Detener un contador");
            Console.WriteLine("3. Mostrar estado de los contadores");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    StartCounter();
                    break;
                case "2":
                    StopCounter();
                    break;
                case "3":
                    ShowCounters();
                    break;
                case "4":
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine("Opción no válida. Presione una tecla para continuar.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void StartCounter()
    {
        Console.Write("Ingrese el ID del contador: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Ingrese el intervalo de tiempo en milisegundos: ");
            if (int.TryParse(Console.ReadLine(), out int interval))
            {
                if (!counters.ContainsKey(id))
                {
                    Counter counter = new Counter(id, interval);
                    counters[id] = counter;
                    counter.Start();
                    Console.WriteLine("Contador iniciado. Presione una tecla para continuar.");
                }
                else
                {
                    Console.WriteLine("El contador ya está en ejecución.");
                }
            }
        }
        Console.ReadKey();
    }

    static void StopCounter()
    {
        Console.Write("Ingrese el ID del contador a detener: ");
        if (int.TryParse(Console.ReadLine(), out int id) && counters.ContainsKey(id))
        {
            counters[id].Stop();
            counters.Remove(id);
            Console.WriteLine("Contador detenido. Presione una tecla para continuar.");
        }
        else
        {
            Console.WriteLine("El contador no existe.");
        }
        Console.ReadKey();
    }

    static void ShowCounters()
    {
        Console.WriteLine("\nEstado de los contadores:");
        foreach (var counter in counters.Values)
        {
            Console.WriteLine($"Contador {counter.Id}: {counter.Count}");
        }
        Console.WriteLine("Presione una tecla para continuar.");
        Console.ReadKey();
    }

    static void ExitProgram()
    {
        foreach (var counter in counters.Values)
        {
            counter.Stop();
        }
        running = false;
    }
}

class Counter
{
    public int Id { get; }
    public int Count { get; private set; }
    private int Interval;
    private Thread thread;
    private bool isRunning;

    public Counter(int id, int interval)
    {
        Id = id;
        Interval = interval;
        Count = 0;
        isRunning = false;
    }

    public void Start()
    {
        isRunning = true;
        thread = new Thread(Run) { IsBackground = true };
        thread.Start();
    }

    private void Run()
    {
        while (isRunning)
        {
            Thread.Sleep(Interval);
            Count++;
        }
    }

    public void Stop()
    {
        isRunning = false;
        thread.Join();
    }
}
