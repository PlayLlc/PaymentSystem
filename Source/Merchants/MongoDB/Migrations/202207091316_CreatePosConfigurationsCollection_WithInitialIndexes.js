//Create collection
db.createCollection("PosConfigurations");

//Create indexes:  basically 2 indexes besides pk will be needed. One for terminalId and one for {companyId, merchantId, storeId, terminalId}

db.getCollection("PosConfigurations").createIndex({CompanyId: 1, MerchantId: 1, StoreId: 1, TerminalId: 1}, {unique: true});
db.getCollection("PosConfigurations").createIndex({TerminalId: 1}, {unique: true});