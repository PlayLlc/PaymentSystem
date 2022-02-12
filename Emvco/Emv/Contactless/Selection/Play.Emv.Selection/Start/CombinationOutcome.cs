using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Selection.Start;

public record CombinationOutcome(SelectApplicationDefinitionFileInfoResponse ApplicationFileInformationResponse, Combination Combination);