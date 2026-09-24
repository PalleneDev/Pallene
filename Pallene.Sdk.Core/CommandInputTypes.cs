using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Core
{
    public static class CommandInputTypes
    {
        public struct StartStopUnitArgs
        {
            public required bool Immed { get; init; }
            public required byte FormatLayerNumber { get; init; }
            public required byte PowerConditions { get; init; }
            public required bool FL { get; init; }
            public required bool LoEj { get; init; }
            public required bool Start { get; init; }
        }

        public struct PreventAllowMediumRemovalArgs
        {
            public required bool Persistent { get; init; }
            public required bool Prevent { get; init; }
        }
    }
}
