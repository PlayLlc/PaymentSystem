﻿using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface
    ISelectDirectoryDefinitionFileInformation : ITransceiveData<SelectDirectoryDefinitionFileCommand, SelectDirectoryDefinitionFileResponse>
{ }