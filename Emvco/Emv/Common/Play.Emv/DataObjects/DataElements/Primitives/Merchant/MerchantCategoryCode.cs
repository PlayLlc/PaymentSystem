using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Classifies the type of business being done by the merchant, represented according to ISO 8583:1993 for Card
///     Acceptor Business Code
/// </summary>
public record MerchantCategoryCode : DataElement<ushort>, IEqualityComparer<MerchantCategoryCode>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F15;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public MerchantCategoryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() =>
        throw new NotImplementedException("This is an internal configuration object so serialization is not needed");

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MerchantCategoryCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MerchantCategoryCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        return new MerchantCategoryCode(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(MerchantCategoryCode? x, MerchantCategoryCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MerchantCategoryCode obj) => obj.GetHashCode();

    #endregion

    #region Codes

    public static readonly MerchantCategoryCode AdvertisingServices = new(7311);
    public static readonly MerchantCategoryCode AirConditioningAndRefrigerationRepairShops = new(7623);
    public static readonly MerchantCategoryCode AmusementParksCarnivalsCircusesFortuneTellers = new(7996);
    public static readonly MerchantCategoryCode AntiqueReproductions = new(5937);
    public static readonly MerchantCategoryCode AntiqueShopsSalesRepairsAndRestorationServices = new(5832);
    public static readonly MerchantCategoryCode ArtDealersAndGalleries = new(5971);
    public static readonly MerchantCategoryCode ArtistsSupplyAndCraftShops = new(5970);
    public static readonly MerchantCategoryCode AutomatedFuelDispensers = new(5542);
    public static readonly MerchantCategoryCode AutomobileAndTruckDealersUsedOnly = new(5521);
    public static readonly MerchantCategoryCode AutomobileParkingLotsAndGarages = new(7523);
    public static readonly MerchantCategoryCode AutomobileSupplyStores = new(5531);
    public static readonly MerchantCategoryCode AutomotiveBodyRepairShops = new(7531);
    public static readonly MerchantCategoryCode AutomotivePartsAccessoriesStores = new(5533);
    public static readonly MerchantCategoryCode AutomotiveServiceShops = new(7538);
    public static readonly MerchantCategoryCode AutomotiveTireStores = new(5532);
    public static readonly MerchantCategoryCode Bakeries = new(5462);
    public static readonly MerchantCategoryCode BandsOrchestrasAndMiscellaneousEntertainersNotElsewhereClassified = new(7929);
    public static readonly MerchantCategoryCode BarberAndBeautyShops = new(7230);
    public static readonly MerchantCategoryCode BettingIncludingLotteryTicketsCasinoGamingChipsOffTrackBettingAndWagers = new(7995);
    public static readonly MerchantCategoryCode BicycleShopsSalesAndService = new(5940);
    public static readonly MerchantCategoryCode BilliardAndPoolEstablishments = new(7932);
    public static readonly MerchantCategoryCode BlueprintingAndPhotocopyingServices = new(7332);
    public static readonly MerchantCategoryCode BoatDealers = new(5551);
    public static readonly MerchantCategoryCode BooksPeriodicalsAndNewspapers = new(5192);
    public static readonly MerchantCategoryCode BookStores = new(5942);
    public static readonly MerchantCategoryCode BowlingAlleys = new(7933);
    public static readonly MerchantCategoryCode BusinessServicesNotElsewhereClassified = new(7399);
    public static readonly MerchantCategoryCode BuyingshoppingServicesClubs = new(7278);
    public static readonly MerchantCategoryCode CableAndOtherPayTelevisionPreviouslyCableServices = new(4899);
    public static readonly MerchantCategoryCode CameraAndPhotographicSupplyStores = new(5946);
    public static readonly MerchantCategoryCode CandyStores = new(5441);
    public static readonly MerchantCategoryCode CarAndTruckDealersNewAndUsedSalesServiceRepairsPartsAndLeasing = new(5511);
    public static readonly MerchantCategoryCode CardShopsGiftNoveltyAndSouvenirShops = new(5947);
    public static readonly MerchantCategoryCode CarpetAndUpholsteryCleaning = new(7217);
    public static readonly MerchantCategoryCode CarRentalCompaniesNotListedBelow = new(7512);
    public static readonly MerchantCategoryCode CarWashes = new(7542);
    public static readonly MerchantCategoryCode Caterers = new(5811);
    public static readonly MerchantCategoryCode ChemicalsAndAlliedProductsNotElsewhereClassified = new(5169);
    public static readonly MerchantCategoryCode ChildrensAndInfantsWearStores = new(5641);
    public static readonly MerchantCategoryCode CigarStoresAndStands = new(5993);
    public static readonly MerchantCategoryCode CleaningAndMaintenanceJanitorialServices = new(7349);
    public static readonly MerchantCategoryCode ClothingRentalCostumesFormalWearUniforms = new(7296);
    public static readonly MerchantCategoryCode CommercialEquipmentNotElsewhereClassified = new(5046);
    public static readonly MerchantCategoryCode CommercialFootwear = new(5139);
    public static readonly MerchantCategoryCode CommercialPhotographyArtAndGraphics = new(7333);
    public static readonly MerchantCategoryCode CommercialSportsAthleticFieldsProfessionalSportClubsAndSportPromoters = new(7941);
    public static readonly MerchantCategoryCode ComputerMaintenanceAndRepairServicesNotElsewhereClassified = new(7379);
    public static readonly MerchantCategoryCode ComputerProgrammingIntegratedSystemsDesignAndDataProcessingServices = new(7372);
    public static readonly MerchantCategoryCode ComputersComputerPeripheralEquipmentSoftware = new(5045);
    public static readonly MerchantCategoryCode ComputerSoftwareStores = new(5734);
    public static readonly MerchantCategoryCode ConfectioneryStores = new(5441);
    public static readonly MerchantCategoryCode ConstructionMaterialsNotElsewhereClassified = new(5039);
    public static readonly MerchantCategoryCode ConsumerCreditReportingAgencies = new(7321);
    public static readonly MerchantCategoryCode CosmeticStores = new(5977);
    public static readonly MerchantCategoryCode CounselingServiceDebtMarriagePersonal = new(7277);
    public static readonly MerchantCategoryCode DairyProductsStores = new(5451);
    public static readonly MerchantCategoryCode DanceHallsStudiosAndSchools = new(7911);
    public static readonly MerchantCategoryCode DatingAndEscortServices = new(7273);
    public static readonly MerchantCategoryCode DepartmentStores = new(5311);

    public static readonly MerchantCategoryCode
        DirectMarketingCatalogAndCatalogAndRetailMerchantDirectMarketingOutboundTelemarketingMerchant = new(5965);

    public static readonly MerchantCategoryCode DirectMarketingCatalogMerchant = new(5964);
    public static readonly MerchantCategoryCode DirectMarketingContinuitysubscriptionMerchant = new(5968);
    public static readonly MerchantCategoryCode DirectMarketingInboundTeleservicesMerchant = new(5967);
    public static readonly MerchantCategoryCode DirectMarketingInsuranceService = new(5960);
    public static readonly MerchantCategoryCode DirectMarketingNotElsewhereClassified = new(5969);
    public static readonly MerchantCategoryCode DirectMarketingTravelRelatedArrangementsServices = new(5962);
    public static readonly MerchantCategoryCode DiscountStores = new(5310);
    public static readonly MerchantCategoryCode DisinfectingServices = new(7342);
    public static readonly MerchantCategoryCode DoorToDoorSales = new(5963);
    public static readonly MerchantCategoryCode DraperyWindowCoveringAndUpholsteryStores = new(5714);

    public static readonly MerchantCategoryCode DrinkingPlacesAlcoholicBeveragesBarsTavernsCocktailLoungesNightclubsAndDiscotheques =
        new(5813);

    public static readonly MerchantCategoryCode DrugsDrugProprietorsAndDruggistsSundries = new(5122);
    public static readonly MerchantCategoryCode DrugStoresAndPharmacies = new(5912);
    public static readonly MerchantCategoryCode DryCleaners = new(7216);
    public static readonly MerchantCategoryCode DurableGoodsNotElsewhereClassified = new(5099);
    public static readonly MerchantCategoryCode DutyFreeStore = new(5309);
    public static readonly MerchantCategoryCode EatingPlacesAndRestaurants = new(5812);
    public static readonly MerchantCategoryCode ElectricalAndSmallApplianceRepairShops = new(7629);
    public static readonly MerchantCategoryCode ElectricalPartsAndEquipment = new(5065);
    public static readonly MerchantCategoryCode ElectricGasSanitaryAndWaterUtilities = new(4900);
    public static readonly MerchantCategoryCode ElectricRazorStoresSalesAndService = new(5997);
    public static readonly MerchantCategoryCode ElectronicSales = new(5732);
    public static readonly MerchantCategoryCode EmploymentAgenciesTemporaryHelpServices = new(7361);
    public static readonly MerchantCategoryCode EquipmentRentalAndLeasingServicesToolRentalFurnitureRentalAndApplianceRental = new(7394);
    public static readonly MerchantCategoryCode ExterminatingAndDisinfectingServices = new(7342);
    public static readonly MerchantCategoryCode FamilyClothingStores = new(5651);
    public static readonly MerchantCategoryCode FastFoodRestaurants = new(5814);

    // TODO: These should probably be stored in the gateway and validated there along with the OnlineAndOfflineCapable
    // TODO: configuration. Leaving this here for now to be able to do shit. good enough theory
    public static readonly MerchantCategoryCode FaxServices = new(4814);
    public static readonly MerchantCategoryCode FinancialInstitutionsAutomatedCashDisbursements = new(6011);
    public static readonly MerchantCategoryCode FinancialInstitutionsManualCashDisbursements = new(6010);
    public static readonly MerchantCategoryCode FinancialInstitutionsMerchandiseAndServices = new(6012);
    public static readonly MerchantCategoryCode FireplaceFireplaceScreensAndAccessoriesStores = new(5718);
    public static readonly MerchantCategoryCode FloorCoveringStores = new(5713);
    public static readonly MerchantCategoryCode Florists = new(5992);
    public static readonly MerchantCategoryCode FloristsSuppliesNurseryStockAndFlowers = new(5193);
    public static readonly MerchantCategoryCode FreezerAndLockerMeatProvisioners = new(5422);
    public static readonly MerchantCategoryCode FuelFuelOilWoodCoalLiquefiedPetroleum = new(5983);
    public static readonly MerchantCategoryCode FuneralServiceAndCrematories = new(7261);
    public static readonly MerchantCategoryCode FurnitureFurnitureRepairAndFurnitureRefinishing = new(7641);
    public static readonly MerchantCategoryCode FurnitureHomeFurnishingsAndEquipmentStoresExceptAppliances = new(5712);
    public static readonly MerchantCategoryCode FurriersAndFurShops = new(5681);
    public static readonly MerchantCategoryCode GlassStores = new(5231);
    public static readonly MerchantCategoryCode GlassWareCrystalStores = new(5950);
    public static readonly MerchantCategoryCode GolfCoursesPublic = new(7992);
    public static readonly MerchantCategoryCode GroceryStores = new(5411);
    public static readonly MerchantCategoryCode HardwareEquipmentAndSupplies = new(5072);
    public static readonly MerchantCategoryCode HardwareStores = new(5251);
    public static readonly MerchantCategoryCode HealthAndBeautyShops = new(7298);
    public static readonly MerchantCategoryCode HearingAidsSalesServiceAndSupplyStores = new(5975);
    public static readonly MerchantCategoryCode HobbyToyAndGameShops = new(5945);
    public static readonly MerchantCategoryCode HomeSupplyWarehouseStores = new(5200);
    public static readonly MerchantCategoryCode HouseholdApplianceStores = new(5722);
    public static readonly MerchantCategoryCode IndustrialSuppliesNotElsewhereClassified = new(5085);
    public static readonly MerchantCategoryCode InformationRetrievalServices = new(7375);
    public static readonly MerchantCategoryCode InsuranceNotElsewhereClassifiedNoLongerValidForFirstPresentmentWork = new(6399);
    public static readonly MerchantCategoryCode InsurancePremiumsNoLongerValidForFirstPresentmentWork = new(6381);
    public static readonly MerchantCategoryCode InsuranceSalesUnderwritingAndPremiums = new(6300);
    public static readonly MerchantCategoryCode LaundryCleaningAndGarmentServices = new(7210);
    public static readonly MerchantCategoryCode LaundryFamilyAndCommercial = new(7211);
    public static readonly MerchantCategoryCode LeatherFoodsStores = new(5948);
    public static readonly MerchantCategoryCode LodgingHotelsMotelsResortsCentralReservationServicesNotElsewhereClassified = new(7011);
    public static readonly MerchantCategoryCode LumberAndBuildingMaterialsStores = new(5211);

    public static readonly MerchantCategoryCode
        MailOrderHousesIncludingCatalogOrderStoresBookrecordClubsNoLongerPermittedForSOriginalPresentments = new(5961);

    public static readonly MerchantCategoryCode ManagementConsultingAndPublicRelationsServices = new(7392);
    public static readonly MerchantCategoryCode MassageParlors = new(7297);
    public static readonly MerchantCategoryCode MeatProvisionersFreezerAndLocker = new(5422);
    public static readonly MerchantCategoryCode MedicalDentalOphthalmicHospitalEquipmentAndSupplies = new(5047);
    public static readonly MerchantCategoryCode MensAndBoysClothingAndAccessoriesStores = new(5611);
    public static readonly MerchantCategoryCode MensAndWomensClothingStores = new(5691);
    public static readonly MerchantCategoryCode MensWomensAndChildrensUniformsAndCommercialClothing = new(5137);
    public static readonly MerchantCategoryCode MetalServiceCentersAndOffices = new(5051);
    public static readonly MerchantCategoryCode MiscellaneousAndSpecialtyRetailStores = new(5999);
    public static readonly MerchantCategoryCode MiscellaneousApparelAndAccessoryShops = new(5699);
    public static readonly MerchantCategoryCode MiscellaneousHomeFurnishingSpecialtyStores = new(5719);
    public static readonly MerchantCategoryCode MiscellaneousPersonalServicesNotElsewhereClassifies = new(7299);
    public static readonly MerchantCategoryCode MiscFoodStoresConvenienceStoresAndSpecialtyMarkets = new(5499);
    public static readonly MerchantCategoryCode MiscGeneralMerchandise = new(5399);
    public static readonly MerchantCategoryCode MobileHomeDealers = new(5271);
    public static readonly MerchantCategoryCode MoneyOrdersWireTransfer = new(4829);
    public static readonly MerchantCategoryCode MotionPicturesAndVideoTapeProductionAndDistribution = new(7829);
    public static readonly MerchantCategoryCode MotionPictureTheaters = new(7832);
    public static readonly MerchantCategoryCode MotorcycleDealers = new(5571);
    public static readonly MerchantCategoryCode MotorHomeAndRecreationalVehicleRentals = new(7519);
    public static readonly MerchantCategoryCode MotorHomeDealers = new(5592);
    public static readonly MerchantCategoryCode MotorVehicleSuppliesAndNewParts = new(5013);
    public static readonly MerchantCategoryCode MusicStoresMusicalInstrumentsPianoSheetMusic = new(5733);
    public static readonly MerchantCategoryCode NewsDealersAndNewsstands = new(5994);
    public static readonly MerchantCategoryCode NondurableGoodsNotElsewhereClassified = new(5199);

    public static readonly MerchantCategoryCode NonFinancialInstitutionsForeignCurrencyMoneyOrdersNotWireTransferAndTravelersCheques =
        new(6051);

    public static readonly MerchantCategoryCode NurseriesLawnAndGardenSupplyStore = new(5261);
    public static readonly MerchantCategoryCode NutStores = new(5441);
    public static readonly MerchantCategoryCode OfficeAndCommercialFurniture = new(5021);
    public static readonly MerchantCategoryCode OfficePhotographicPhotocopyAndMicrofilmEquipment = new(5044);
    public static readonly MerchantCategoryCode OrthopedicGoodsProstheticDevices = new(5976);
    public static readonly MerchantCategoryCode PackageStoresBeerWineAndLiquor = new(5921);
    public static readonly MerchantCategoryCode PaintAndWallpaperStores = new(5231);
    public static readonly MerchantCategoryCode PaintShopsAutomotive = new(7535);
    public static readonly MerchantCategoryCode PaintsVarnishesAndSupplies = new(5198);
    public static readonly MerchantCategoryCode PawnShopsAndSalvageYards = new(5933);
    public static readonly MerchantCategoryCode PetroleumAndPetroleumProducts = new(5172);
    public static readonly MerchantCategoryCode PetShopsPetFoodsAndSuppliesStores = new(5995);
    public static readonly MerchantCategoryCode PhotofinishingLaboratoriesPhotoDeveloping = new(7395);
    public static readonly MerchantCategoryCode PhotographicStudios = new(7221);
    public static readonly MerchantCategoryCode PieceGoodsNotionsAndOtherDryGoods = new(5131);
    public static readonly MerchantCategoryCode PlumbingAndHeatingEquipmentAndSupplies = new(5074);
    public static readonly MerchantCategoryCode PreciousStonesAndMetalsWatchesAndJewelry = new(5094);
    public static readonly MerchantCategoryCode ProtectiveAndSecurityServicesIncludingArmoredCarsAndGuardDogs = new(7393);
    public static readonly MerchantCategoryCode QuickCopyReproductionAndBlueprintingServices = new(7338);
    public static readonly MerchantCategoryCode RadioRepairShops = new(7622);
    public static readonly MerchantCategoryCode RecordShops = new(5735);
    public static readonly MerchantCategoryCode RecreationalAndUtilityTrailersCampDealers = new(5561);
    public static readonly MerchantCategoryCode ReligiousGoodsStores = new(5973);
    public static readonly MerchantCategoryCode RepairShopsAndRelatedServicesMiscellaneous = new(7699);
    public static readonly MerchantCategoryCode SecurityBrokersdealers = new(6211);
    public static readonly MerchantCategoryCode ServiceStationsWithOrWithoutAncillaryServices = new(5541);
    public static readonly MerchantCategoryCode SewingNeedleFabricAndPriceGoodsStores = new(5949);
    public static readonly MerchantCategoryCode ShoeStores = new(5661);
    public static readonly MerchantCategoryCode ShopRepairShopsAndShoeShineParlorsAndHatCleaningShops = new(7251);
    public static readonly MerchantCategoryCode SnowmobileDealers = new(5598);
    public static readonly MerchantCategoryCode SportingAndRecreationalCamps = new(7032);
    public static readonly MerchantCategoryCode SportingGoodsStores = new(5941);
    public static readonly MerchantCategoryCode SportsApparelRidingApparelStores = new(5655);
    public static readonly MerchantCategoryCode StampAndCoinStoresPhilatelicAndNumismaticSupplies = new(5972);
    public static readonly MerchantCategoryCode StationeryOfficeSuppliesPrintingAndWritingPaper = new(5111);
    public static readonly MerchantCategoryCode StationeryStoresOfficeAndSchoolSupplyStores = new(5943);
    public static readonly MerchantCategoryCode StenographicAndSecretarialSupportServices = new(7339);
    public static readonly MerchantCategoryCode Supermarkets = new(5411);
    public static readonly MerchantCategoryCode SwimmingPoolsSalesServiceAndSupplies = new(5996);
    public static readonly MerchantCategoryCode TailorsSeamstressMendingAndAlterations = new(5697);
    public static readonly MerchantCategoryCode TaxPreparationService = new(7276);

    public static readonly MerchantCategoryCode
        TelecommunicationServiceIncludingLocalAndLongDistanceCallsCreditCardCallsCallsThroughUseOfMagneticstripReadingTelephonesAndFaxServices =
            new(4814);

    public static readonly MerchantCategoryCode TelegraphServices = new(4821);
    public static readonly MerchantCategoryCode TentAndAwningShops = new(5998);
    public static readonly MerchantCategoryCode TheatricalProducersExceptMotionPicturesTicketAgencies = new(7922);
    public static readonly MerchantCategoryCode Timeshares = new(7012);
    public static readonly MerchantCategoryCode TireReTreadingAndRepairShops = new(7534);
    public static readonly MerchantCategoryCode TouristAttractionsAndExhibits = new(7991);
    public static readonly MerchantCategoryCode TowingServices = new(7549);
    public static readonly MerchantCategoryCode TrailerParksAndCampGrounds = new(7033);
    public static readonly MerchantCategoryCode TruckAndUtilityTrailerRentals = new(7513);
    public static readonly MerchantCategoryCode TypewriterStoresSalesRentalService = new(5978);
    public static readonly MerchantCategoryCode UsedMerchandiseAndSecondhandStores = new(5931);
    public static readonly MerchantCategoryCode VarietyStores = new(5331);
    public static readonly MerchantCategoryCode VideoAmusementGameSupplies = new(7993);
    public static readonly MerchantCategoryCode VideoGameArcadesestablishments = new(7994);
    public static readonly MerchantCategoryCode VideoTapeRentalStores = new(7841);
    public static readonly MerchantCategoryCode VisaPhone = new(4815);
    public static readonly MerchantCategoryCode WallpaperStores = new(5231);
    public static readonly MerchantCategoryCode WatchClockAndJewelryRepair = new(7631);
    public static readonly MerchantCategoryCode WatchClockJewelryAndSilverwareStores = new(5944);
    public static readonly MerchantCategoryCode WeldingRepair = new(7692);
    public static readonly MerchantCategoryCode WholesaleClubs = new(5300);
    public static readonly MerchantCategoryCode WigAndToupeeStores = new(5698);
    public static readonly MerchantCategoryCode WomensAccessoryAndSpecialtyShops = new(5631);
    public static readonly MerchantCategoryCode WomensReadyToWearStores = new(5621);
    public static readonly MerchantCategoryCode WreckingAndSalvageYards = new(5935);

    #endregion

    //public TerminalCategoryCode GetTerminalCategoryCode() => new TerminalCategoryCode(_Value % 10);
}