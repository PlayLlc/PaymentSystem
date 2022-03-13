using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.DataElements;
using Play.Emv.Terminal.Contracts;
using Play.Randoms;

namespace Play.Emv.Terminal.Common.Services.UnpredictableNumbers
{
    internal class UnpredictableNumberGenerator : IGenerateUnpredictableNumber
    {
        // WARNING: This should be generated using a device that can ensure total uniqueness, like a DUKPT compliant machine
        public UnpredictableNumber GenerateUnpredictableNumber() => new(Randomize.Numeric.UInt());
    }
}