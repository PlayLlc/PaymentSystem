using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Pcd.Contracts;

// HACK: This probably shouldn't live here. This was moved here pretty much for convenience to be able to use the interface in the Play.Emv.Security module. Need to look at the pattern and update it to make more sense
public interface IWriteIccSecuritySessionData
{
    public void EnqueueStaticDataToBeAuthenticated(EmvCodec codec, ReadRecordResponse rapdu);
    public void EnqueueStaticDataToBeAuthenticated(StaticDataAuthenticationTagList tagList, IReadTlvDatabase database);
    public void Update(GenerateApplicationCryptogramResponse value);
}