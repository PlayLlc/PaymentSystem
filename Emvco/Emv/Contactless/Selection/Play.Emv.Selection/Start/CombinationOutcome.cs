using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Start;

public record CombinationOutcome(SelectApplicationDefinitionFileInfoResponse ApplicationFileInformationResponse, Combination Combination);