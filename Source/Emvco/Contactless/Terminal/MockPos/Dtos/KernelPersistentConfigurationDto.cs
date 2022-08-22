using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel2.Databases;

namespace MockPos.Dtos;

public class KernelPersistentConfigurationDto
{
    #region Instance Values

    public byte KernelId { get; set; }
    public List<TagLengthValueDto> TagLengthValues { get; set; } = new();

    #endregion

    #region Serialization

    public KernelPersistentConfiguration Decode()
    {
        List<TagLengthValue> tagLengthValues = new();

        foreach (TagLengthValueDto? tlv in TagLengthValues)
            tagLengthValues.Add(tlv.Decode());

        return new KernelPersistentConfiguration(new KernelId(KernelId), tagLengthValues.ToArray());
    }

    #endregion
}