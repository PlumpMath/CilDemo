using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace PostCompiler.Extensions
{
    static class InstructionCollectionExtensions
    {
        public static Collection<Instruction> RecalculateOffset(this Collection<Instruction> instructions)
        {
            var offset = 0;

            foreach (var i in instructions)
            {
                i.Offset = offset;
                offset += i.GetSize();
            }

            return instructions;
        }
    }
}
