using System;
using Sampler.Contracts;

namespace Sampler.Processing
{
    public class ConsolePrinter : IPrinter
    {
        public void Print(string stringToPrint)
        {
            Console.WriteLine(stringToPrint);
        }
    }
}