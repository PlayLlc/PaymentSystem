using MongoDB.Bson.Serialization.Attributes;

namespace MerchantPortal.Core.Entities.PointOfSale;

public class PosConfigurationHeader
{
    [BsonId]
    public long Id { get; set; }

    public long TerminalId { get; set; }

    public long StoreId { get; set; }

    public long MerchantId { get; set; }

    public long CompanyId { get; set; }
}
