using System;

namespace GServer.Services.Abstractions
{
    public interface IPipelineProcessor<in T>
    {
        void Enqueue(T input);
        void Start();
    }
}