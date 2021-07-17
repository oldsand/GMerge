using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GServer.Services.Abstractions
{
    public abstract class PipelineProcessor<T> : IPipelineProcessor<T>
    {
        private readonly BufferBlock<T> _input;
        private readonly DataflowLinkOptions _inputLinkOptions;

        protected PipelineProcessor(int inputCapacity = DataflowBlockOptions.Unbounded, bool propagateCompletion = true)
        {
            var inputBlockOptions = new DataflowBlockOptions
            {
                BoundedCapacity = inputCapacity
            };
                
            _input = new BufferBlock<T>(inputBlockOptions);
                
            _inputLinkOptions = new DataflowLinkOptions {PropagateCompletion = propagateCompletion};
        }
        
        public Task Completion { get; set; }

        public void Complete()
        {
            _input.Complete();
        }

        protected abstract ITargetBlock<T> DefineFlow();

        protected void AssignCompletion(IDataflowBlock block)
        {
            Completion = block.Completion;
        }

        public void Enqueue(T input)
        {
            _input.SendAsync(input);
        }

        public void Start()
        {
            var flow = DefineFlow();
            _input.LinkTo(flow, _inputLinkOptions);
        }
    }
}