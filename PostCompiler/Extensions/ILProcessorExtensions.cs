using System.Linq;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace PostCompiler.Extensions
{
    static class ILProcessorExtensions
    {
        public static void InsertAfterMethod(this ILProcessor processor, Collection<Instruction> instructions)
        {
            if (instructions.Count > 0)
            {
                var rets = processor.Body.Instructions
                    .Where(i => i.OpCode == OpCodes.Ret).OrderBy(i => i.Offset).Reverse().ToArray();

                foreach (var ret in rets)
                {
                    processor.Replace(ret, instructions[0]);
                    for (var i = 1; i < instructions.Count; i++)
                    {
                        processor.InsertAfter(instructions[i - 1], instructions[i]);
                    }
                    processor.InsertAfter(instructions[instructions.Count - 1], processor.Create(OpCodes.Ret));
                }

                processor.Body.Instructions.RecalculateOffset();
            }
        }

        public static void InsertBeforeMethod(this ILProcessor processor, Collection<Instruction> instructions)
        {
            if (instructions.Count > 0)
            {
                var first = processor.Body.Instructions[0];

                foreach (var i in instructions)
                {
                    processor.InsertBefore(first, i);
                }

                processor.Body.Instructions.RecalculateOffset();
            }
        }
    }
}
