using System;
using Mono.Cecil.Cil;
using PostCompiler.Factories;

namespace PostCompiler.InstructionBuilders
{
    abstract class InstructionBuilderBase
    {
        protected ILProcessor Processor { get; private set; }



        protected InstructionBuilderBase(ILProcessor processor)
        {
            if (processor == null)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[InstructionBuilderBase] is throwing exception ([processor] == [null]).");
                throw new ArgumentNullException("processor");
            }

            Processor = processor;
        }
    }
}
