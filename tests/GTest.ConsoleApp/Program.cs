using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using GServer.Services.Abstractions;

namespace GTest.ConsoleApp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            
        }
        
        public class SomePipeline : PipelineProcessor<int>
        {
            protected override ITargetBlock<int> DefineFlow()
            {
                var options = new DataflowLinkOptions {PropagateCompletion = true};
                
                var step1 = new TransformBlock<int, int>(Increase);
                var step2 = new TransformBlock<int, DateTime>(GenerateDate);
                var step3 = new ActionBlock<DateTime>(Write);

                step1.LinkTo(step2, options);
                step2.LinkTo(step3, options);

                return step1;
            }

            private static int Increase(int input)
            {
                return input * 2;
            }

            private static DateTime GenerateDate(int input)
            {
                return DateTime.Today.AddHours(input);
            }

            private static void Write(DateTime input)
            {
                Console.WriteLine(input);
            }
        }
    }
}