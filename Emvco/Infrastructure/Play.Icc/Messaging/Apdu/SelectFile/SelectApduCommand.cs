using System;

using Play.Icc.FileSystem;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Icc.Messaging.Apdu.SelectFile;

public class SelectApduCommand : ApduCommand
{
    #region Constructor

    private SelectApduCommand(Class @class, Instruction instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private SelectApduCommand(Class @class, Instruction instruction, byte parameter1, byte parameter2) : base(@class, instruction,
                                                                                                              parameter1, parameter2)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Select a Dedicated File that is a parent of the current Dedicated File
    /// </summary>
    public static SelectApduCommand DedicatedFile(DedicatedFileName applicationIdentifier) =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.SelectFile, SelectionMode.DedicatedFileName,
            FilePosition.FirstOrOnly, applicationIdentifier.AsByteArray());

    /// <summary>
    ///     Select a file by supplying the file path from the Master File
    /// </summary>
    public static SelectApduCommand File(byte[] filePathFromMasterFile)
    {
        FilePath? filePath = new(filePathFromMasterFile);

        if (filePath.DoesPathStartFromRootDirectory())
        {
            return new SelectApduCommand(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
                                         SelectionMode.ElementaryFileChild, FilePosition.FirstOrOnly, ((ReadOnlySpan<byte>) filePath)[2..]);
        }

        return new SelectApduCommand(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
                                     SelectionMode.ElementaryFileChild, FilePosition.FirstOrOnly, ((ReadOnlySpan<byte>) filePath)[2..]);
    }

    public static SelectApduCommand MasterFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile, SelectionMode.File,
            FilePosition.FirstOrOnly, FileIdentifier.MasterFile.AsByteArray());

    /// <summary>
    ///     Select an Dedicated File that is a child of the current Dedicated File
    /// </summary>
    public static SelectApduCommand NextDedicatedFileFromCurrentDedicatedFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.DedicatedFileChild, FilePosition.Next);

    /// <summary>
    ///     Select an Elementary File that is a child of the current Dedicated File
    /// </summary>
    public static SelectApduCommand NextElementaryFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.ElementaryFileChild, FilePosition.Next);

    /// <summary>
    ///     Select an Elementary File that is a child of the current Dedicated File
    /// </summary>
    public static SelectApduCommand NextElementaryFile(ShortFileId shortFileIdentifier) =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.ElementaryFileChild, FilePosition.FirstOrOnly, new byte[] {shortFileIdentifier});

    /// <summary>
    ///     Select the next Elementary File that is a child of the current Dedicated File
    /// </summary>
    public static SelectApduCommand NextElementaryFileFromCurrentDedicatedFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.ElementaryFileChild, FilePosition.FirstOrOnly, FileIdentifier.CurrentDedicatedFile.AsByteArray());

    /// <summary>
    ///     Select the next Elementary File that is a child of the current Master File
    /// </summary>
    public static SelectApduCommand NextElementaryFileFromCurrentMasterFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.ElementaryFileChild, FilePosition.FirstOrOnly, FileIdentifier.MasterFile.AsByteArray());

    /// <summary>
    ///     Select the next file from the current Dedicated File
    /// </summary>
    public static SelectApduCommand NextFile()
    {
        return new SelectApduCommand(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
                                     SelectionMode.File, FilePosition.FirstOrOnly, new ReadOnlySpan<byte>(new byte[] {0x02}));
    }

    /// <summary>
    ///     Select a Dedicated File that is a parent of the current Dedicated File
    /// </summary>
    public static SelectApduCommand ParentDedicatedFileFromCurrentDedicatedFile() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.DedicatedFileParent, FilePosition.FirstOrOnly);

    /// <summary>
    ///     Selects the File Control Information for the current ICC
    /// </summary>
    /// <returns></returns>
    public static SelectApduCommand SelectFileControlInformation(DedicatedFileName fileName) =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile, SelectionMode.File,
            FilePosition.FirstOrOnly, fileName.AsByteArray());

    /// <summary>
    ///     Selects the PSE (Payment System Environment) in a Contact environment
    /// </summary>
    /// <returns></returns>
    public static SelectApduCommand SelectPaymentSystemEnvironment() =>
        new(new Class(SecureMessaging.NotAuthenticated, LogicalChannel.BasicChannel), Instruction.SelectFile,
            SelectionMode.DedicatedFileName, FilePosition.FirstOrOnly, DedicatedFileName.PaymentSystemEnvironment.AsByteArray());

    /// <summary>
    ///     Selects the PPSE (Proximity Payment System Environment) in a Contactless environment
    /// </summary>
    /// <returns></returns>
    public static SelectApduCommand SelectProximityPaymentSystemEnvironment() =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.SelectFile, SelectionMode.DedicatedFileName,
            FilePosition.FirstOrOnly, DedicatedFileName.ProximityPaymentSystemEnvironment.AsByteArray());

    #endregion
}