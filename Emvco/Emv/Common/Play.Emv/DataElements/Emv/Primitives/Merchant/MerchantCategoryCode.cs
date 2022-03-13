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

    public static readonly MerchantCategoryCode AdvertisingServices = new MerchantCategoryCode(7311);
    public static readonly MerchantCategoryCode AirConditioningAndRefrigerationRepairShops = new MerchantCategoryCode(7623);
    public static readonly MerchantCategoryCode AmusementParksCarnivalsCircusesFortuneTellers = new MerchantCategoryCode(7996);
    public static readonly MerchantCategoryCode AntiqueReproductions = new MerchantCategoryCode(5937);
    public static readonly MerchantCategoryCode AntiqueShopsSalesRepairsAndRestorationServices = new MerchantCategoryCode(5832);
    public static readonly MerchantCategoryCode ArtDealersAndGalleries = new MerchantCategoryCode(5971);
    public static readonly MerchantCategoryCode ArtistsSupplyAndCraftShops = new MerchantCategoryCode(5970);
    public static readonly MerchantCategoryCode AutomatedFuelDispensers = new MerchantCategoryCode(5542);
    public static readonly MerchantCategoryCode AutomobileAndTruckDealersUsedOnly = new MerchantCategoryCode(5521);
    public static readonly MerchantCategoryCode AutomobileParkingLotsAndGarages = new MerchantCategoryCode(7523);
    public static readonly MerchantCategoryCode AutomobileSupplyStores = new MerchantCategoryCode(5531);
    public static readonly MerchantCategoryCode AutomotiveBodyRepairShops = new MerchantCategoryCode(7531);
    public static readonly MerchantCategoryCode AutomotivePartsAccessoriesStores = new MerchantCategoryCode(5533);
    public static readonly MerchantCategoryCode AutomotiveServiceShops = new MerchantCategoryCode(7538);
    public static readonly MerchantCategoryCode AutomotiveTireStores = new MerchantCategoryCode(5532);
    public static readonly MerchantCategoryCode Bakeries = new MerchantCategoryCode(5462);
    public static readonly MerchantCategoryCode BandsOrchestrasAndMiscellaneousEntertainersNotElsewhereClassified =
        new MerchantCategoryCode(7929);
    public static readonly MerchantCategoryCode BarberAndBeautyShops = new MerchantCategoryCode(7230);
    public static readonly MerchantCategoryCode BettingIncludingLotteryTicketsCasinoGamingChipsOffTrackBettingAndWagers =
        new MerchantCategoryCode(7995);
    public static readonly MerchantCategoryCode BicycleShopsSalesAndService = new MerchantCategoryCode(5940);
    public static readonly MerchantCategoryCode BilliardAndPoolEstablishments = new MerchantCategoryCode(7932);
    public static readonly MerchantCategoryCode BlueprintingAndPhotocopyingServices = new MerchantCategoryCode(7332);
    public static readonly MerchantCategoryCode BoatDealers = new MerchantCategoryCode(5551);
    public static readonly MerchantCategoryCode BooksPeriodicalsAndNewspapers = new MerchantCategoryCode(5192);
    public static readonly MerchantCategoryCode BookStores = new MerchantCategoryCode(5942);
    public static readonly MerchantCategoryCode BowlingAlleys = new MerchantCategoryCode(7933);
    public static readonly MerchantCategoryCode BusinessServicesNotElsewhereClassified = new MerchantCategoryCode(7399);
    public static readonly MerchantCategoryCode BuyingshoppingServicesClubs = new MerchantCategoryCode(7278);
    public static readonly MerchantCategoryCode CableAndOtherPayTelevisionPreviouslyCableServices = new MerchantCategoryCode(4899);
    public static readonly MerchantCategoryCode CameraAndPhotographicSupplyStores = new MerchantCategoryCode(5946);
    public static readonly MerchantCategoryCode CandyStores = new MerchantCategoryCode(5441);
    public static readonly MerchantCategoryCode CarAndTruckDealersNewAndUsedSalesServiceRepairsPartsAndLeasing =
        new MerchantCategoryCode(5511);
    public static readonly MerchantCategoryCode CardShopsGiftNoveltyAndSouvenirShops = new MerchantCategoryCode(5947);
    public static readonly MerchantCategoryCode CarpetAndUpholsteryCleaning = new MerchantCategoryCode(7217);
    public static readonly MerchantCategoryCode CarRentalCompaniesNotListedBelow = new MerchantCategoryCode(7512);
    public static readonly MerchantCategoryCode CarWashes = new MerchantCategoryCode(7542);
    public static readonly MerchantCategoryCode Caterers = new MerchantCategoryCode(5811);
    public static readonly MerchantCategoryCode ChemicalsAndAlliedProductsNotElsewhereClassified = new MerchantCategoryCode(5169);
    public static readonly MerchantCategoryCode ChildrensAndInfantsWearStores = new MerchantCategoryCode(5641);
    public static readonly MerchantCategoryCode CigarStoresAndStands = new MerchantCategoryCode(5993);
    public static readonly MerchantCategoryCode CleaningAndMaintenanceJanitorialServices = new MerchantCategoryCode(7349);
    public static readonly MerchantCategoryCode ClothingRentalCostumesFormalWearUniforms = new MerchantCategoryCode(7296);
    public static readonly MerchantCategoryCode CommercialEquipmentNotElsewhereClassified = new MerchantCategoryCode(5046);
    public static readonly MerchantCategoryCode CommercialFootwear = new MerchantCategoryCode(5139);
    public static readonly MerchantCategoryCode CommercialPhotographyArtAndGraphics = new MerchantCategoryCode(7333);
    public static readonly MerchantCategoryCode CommercialSportsAthleticFieldsProfessionalSportClubsAndSportPromoters =
        new MerchantCategoryCode(7941);
    public static readonly MerchantCategoryCode ComputerMaintenanceAndRepairServicesNotElsewhereClassified = new MerchantCategoryCode(7379);
    public static readonly MerchantCategoryCode ComputerProgrammingIntegratedSystemsDesignAndDataProcessingServices =
        new MerchantCategoryCode(7372);
    public static readonly MerchantCategoryCode ComputersComputerPeripheralEquipmentSoftware = new MerchantCategoryCode(5045);
    public static readonly MerchantCategoryCode ComputerSoftwareStores = new MerchantCategoryCode(5734);
    public static readonly MerchantCategoryCode ConfectioneryStores = new MerchantCategoryCode(5441);
    public static readonly MerchantCategoryCode ConstructionMaterialsNotElsewhereClassified = new MerchantCategoryCode(5039);
    public static readonly MerchantCategoryCode ConsumerCreditReportingAgencies = new MerchantCategoryCode(7321);
    public static readonly MerchantCategoryCode CosmeticStores = new MerchantCategoryCode(5977);
    public static readonly MerchantCategoryCode CounselingServiceDebtMarriagePersonal = new MerchantCategoryCode(7277);
    public static readonly MerchantCategoryCode DairyProductsStores = new MerchantCategoryCode(5451);
    public static readonly MerchantCategoryCode DanceHallsStudiosAndSchools = new MerchantCategoryCode(7911);
    public static readonly MerchantCategoryCode DatingAndEscortServices = new MerchantCategoryCode(7273);
    public static readonly MerchantCategoryCode DepartmentStores = new MerchantCategoryCode(5311);

    public static readonly MerchantCategoryCode
        DirectMarketingCatalogAndCatalogAndRetailMerchantDirectMarketingOutboundTelemarketingMerchant = new MerchantCategoryCode(5965);

    public static readonly MerchantCategoryCode DirectMarketingCatalogMerchant = new MerchantCategoryCode(5964);
    public static readonly MerchantCategoryCode DirectMarketingContinuitysubscriptionMerchant = new MerchantCategoryCode(5968);
    public static readonly MerchantCategoryCode DirectMarketingInboundTeleservicesMerchant = new MerchantCategoryCode(5967);
    public static readonly MerchantCategoryCode DirectMarketingInsuranceService = new MerchantCategoryCode(5960);
    public static readonly MerchantCategoryCode DirectMarketingNotElsewhereClassified = new MerchantCategoryCode(5969);
    public static readonly MerchantCategoryCode DirectMarketingTravelRelatedArrangementsServices = new MerchantCategoryCode(5962);
    public static readonly MerchantCategoryCode DiscountStores = new MerchantCategoryCode(5310);
    public static readonly MerchantCategoryCode DisinfectingServices = new MerchantCategoryCode(7342);
    public static readonly MerchantCategoryCode DoorToDoorSales = new MerchantCategoryCode(5963);
    public static readonly MerchantCategoryCode DraperyWindowCoveringAndUpholsteryStores = new MerchantCategoryCode(5714);

    public static readonly MerchantCategoryCode DrinkingPlacesAlcoholicBeveragesBarsTavernsCocktailLoungesNightclubsAndDiscotheques =
        new MerchantCategoryCode(5813);

    public static readonly MerchantCategoryCode DrugsDrugProprietorsAndDruggistsSundries = new MerchantCategoryCode(5122);
    public static readonly MerchantCategoryCode DrugStoresAndPharmacies = new MerchantCategoryCode(5912);
    public static readonly MerchantCategoryCode DryCleaners = new MerchantCategoryCode(7216);
    public static readonly MerchantCategoryCode DurableGoodsNotElsewhereClassified = new MerchantCategoryCode(5099);
    public static readonly MerchantCategoryCode DutyFreeStore = new MerchantCategoryCode(5309);
    public static readonly MerchantCategoryCode EatingPlacesAndRestaurants = new MerchantCategoryCode(5812);
    public static readonly MerchantCategoryCode ElectricalAndSmallApplianceRepairShops = new MerchantCategoryCode(7629);
    public static readonly MerchantCategoryCode ElectricalPartsAndEquipment = new MerchantCategoryCode(5065);
    public static readonly MerchantCategoryCode ElectricGasSanitaryAndWaterUtilities = new MerchantCategoryCode(4900);
    public static readonly MerchantCategoryCode ElectricRazorStoresSalesAndService = new MerchantCategoryCode(5997);
    public static readonly MerchantCategoryCode ElectronicSales = new MerchantCategoryCode(5732);
    public static readonly MerchantCategoryCode EmploymentAgenciesTemporaryHelpServices = new MerchantCategoryCode(7361);
    public static readonly MerchantCategoryCode EquipmentRentalAndLeasingServicesToolRentalFurnitureRentalAndApplianceRental =
        new MerchantCategoryCode(7394);
    public static readonly MerchantCategoryCode ExterminatingAndDisinfectingServices = new MerchantCategoryCode(7342);
    public static readonly MerchantCategoryCode FamilyClothingStores = new MerchantCategoryCode(5651);
    public static readonly MerchantCategoryCode FastFoodRestaurants = new MerchantCategoryCode(5814);

    // TODO: These should probably be stored in the gateway and validated there along with the OnlineAndOfflineCapable
    // TODO: configuration. Leaving this here for now to be able to do shit. good enough theory
    public static readonly MerchantCategoryCode FaxServices = new MerchantCategoryCode(4814);
    public static readonly MerchantCategoryCode FinancialInstitutionsAutomatedCashDisbursements = new MerchantCategoryCode(6011);
    public static readonly MerchantCategoryCode FinancialInstitutionsManualCashDisbursements = new MerchantCategoryCode(6010);
    public static readonly MerchantCategoryCode FinancialInstitutionsMerchandiseAndServices = new MerchantCategoryCode(6012);
    public static readonly MerchantCategoryCode FireplaceFireplaceScreensAndAccessoriesStores = new MerchantCategoryCode(5718);
    public static readonly MerchantCategoryCode FloorCoveringStores = new MerchantCategoryCode(5713);
    public static readonly MerchantCategoryCode Florists = new MerchantCategoryCode(5992);
    public static readonly MerchantCategoryCode FloristsSuppliesNurseryStockAndFlowers = new MerchantCategoryCode(5193);
    public static readonly MerchantCategoryCode FreezerAndLockerMeatProvisioners = new MerchantCategoryCode(5422);
    public static readonly MerchantCategoryCode FuelFuelOilWoodCoalLiquefiedPetroleum = new MerchantCategoryCode(5983);
    public static readonly MerchantCategoryCode FuneralServiceAndCrematories = new MerchantCategoryCode(7261);
    public static readonly MerchantCategoryCode FurnitureFurnitureRepairAndFurnitureRefinishing = new MerchantCategoryCode(7641);
    public static readonly MerchantCategoryCode FurnitureHomeFurnishingsAndEquipmentStoresExceptAppliances = new MerchantCategoryCode(5712);
    public static readonly MerchantCategoryCode FurriersAndFurShops = new MerchantCategoryCode(5681);
    public static readonly MerchantCategoryCode GlassStores = new MerchantCategoryCode(5231);
    public static readonly MerchantCategoryCode GlassWareCrystalStores = new MerchantCategoryCode(5950);
    public static readonly MerchantCategoryCode GolfCoursesPublic = new MerchantCategoryCode(7992);
    public static readonly MerchantCategoryCode GroceryStores = new MerchantCategoryCode(5411);
    public static readonly MerchantCategoryCode HardwareEquipmentAndSupplies = new MerchantCategoryCode(5072);
    public static readonly MerchantCategoryCode HardwareStores = new MerchantCategoryCode(5251);
    public static readonly MerchantCategoryCode HealthAndBeautyShops = new MerchantCategoryCode(7298);
    public static readonly MerchantCategoryCode HearingAidsSalesServiceAndSupplyStores = new MerchantCategoryCode(5975);
    public static readonly MerchantCategoryCode HobbyToyAndGameShops = new MerchantCategoryCode(5945);
    public static readonly MerchantCategoryCode HomeSupplyWarehouseStores = new MerchantCategoryCode(5200);
    public static readonly MerchantCategoryCode HouseholdApplianceStores = new MerchantCategoryCode(5722);
    public static readonly MerchantCategoryCode IndustrialSuppliesNotElsewhereClassified = new MerchantCategoryCode(5085);
    public static readonly MerchantCategoryCode InformationRetrievalServices = new MerchantCategoryCode(7375);
    public static readonly MerchantCategoryCode InsuranceNotElsewhereClassifiedNoLongerValidForFirstPresentmentWork =
        new MerchantCategoryCode(6399);
    public static readonly MerchantCategoryCode InsurancePremiumsNoLongerValidForFirstPresentmentWork = new MerchantCategoryCode(6381);
    public static readonly MerchantCategoryCode InsuranceSalesUnderwritingAndPremiums = new MerchantCategoryCode(6300);
    public static readonly MerchantCategoryCode LaundryCleaningAndGarmentServices = new MerchantCategoryCode(7210);
    public static readonly MerchantCategoryCode LaundryFamilyAndCommercial = new MerchantCategoryCode(7211);
    public static readonly MerchantCategoryCode LeatherFoodsStores = new MerchantCategoryCode(5948);
    public static readonly MerchantCategoryCode LodgingHotelsMotelsResortsCentralReservationServicesNotElsewhereClassified =
        new MerchantCategoryCode(7011);
    public static readonly MerchantCategoryCode LumberAndBuildingMaterialsStores = new MerchantCategoryCode(5211);

    public static readonly MerchantCategoryCode
        MailOrderHousesIncludingCatalogOrderStoresBookrecordClubsNoLongerPermittedForSOriginalPresentments = new MerchantCategoryCode(5961);

    public static readonly MerchantCategoryCode ManagementConsultingAndPublicRelationsServices = new MerchantCategoryCode(7392);
    public static readonly MerchantCategoryCode MassageParlors = new MerchantCategoryCode(7297);
    public static readonly MerchantCategoryCode MeatProvisionersFreezerAndLocker = new MerchantCategoryCode(5422);
    public static readonly MerchantCategoryCode MedicalDentalOphthalmicHospitalEquipmentAndSupplies = new MerchantCategoryCode(5047);
    public static readonly MerchantCategoryCode MensAndBoysClothingAndAccessoriesStores = new MerchantCategoryCode(5611);
    public static readonly MerchantCategoryCode MensAndWomensClothingStores = new MerchantCategoryCode(5691);
    public static readonly MerchantCategoryCode MensWomensAndChildrensUniformsAndCommercialClothing = new MerchantCategoryCode(5137);
    public static readonly MerchantCategoryCode MetalServiceCentersAndOffices = new MerchantCategoryCode(5051);
    public static readonly MerchantCategoryCode MiscellaneousAndSpecialtyRetailStores = new MerchantCategoryCode(5999);
    public static readonly MerchantCategoryCode MiscellaneousApparelAndAccessoryShops = new MerchantCategoryCode(5699);
    public static readonly MerchantCategoryCode MiscellaneousHomeFurnishingSpecialtyStores = new MerchantCategoryCode(5719);
    public static readonly MerchantCategoryCode MiscellaneousPersonalServicesNotElsewhereClassifies = new MerchantCategoryCode(7299);
    public static readonly MerchantCategoryCode MiscFoodStoresConvenienceStoresAndSpecialtyMarkets = new MerchantCategoryCode(5499);
    public static readonly MerchantCategoryCode MiscGeneralMerchandise = new MerchantCategoryCode(5399);
    public static readonly MerchantCategoryCode MobileHomeDealers = new MerchantCategoryCode(5271);
    public static readonly MerchantCategoryCode MoneyOrdersWireTransfer = new MerchantCategoryCode(4829);
    public static readonly MerchantCategoryCode MotionPicturesAndVideoTapeProductionAndDistribution = new MerchantCategoryCode(7829);
    public static readonly MerchantCategoryCode MotionPictureTheaters = new MerchantCategoryCode(7832);
    public static readonly MerchantCategoryCode MotorcycleDealers = new MerchantCategoryCode(5571);
    public static readonly MerchantCategoryCode MotorHomeAndRecreationalVehicleRentals = new MerchantCategoryCode(7519);
    public static readonly MerchantCategoryCode MotorHomeDealers = new MerchantCategoryCode(5592);
    public static readonly MerchantCategoryCode MotorVehicleSuppliesAndNewParts = new MerchantCategoryCode(5013);
    public static readonly MerchantCategoryCode MusicStoresMusicalInstrumentsPianoSheetMusic = new MerchantCategoryCode(5733);
    public static readonly MerchantCategoryCode NewsDealersAndNewsstands = new MerchantCategoryCode(5994);
    public static readonly MerchantCategoryCode NondurableGoodsNotElsewhereClassified = new MerchantCategoryCode(5199);

    public static readonly MerchantCategoryCode NonFinancialInstitutionsForeignCurrencyMoneyOrdersNotWireTransferAndTravelersCheques =
        new MerchantCategoryCode(6051);

    public static readonly MerchantCategoryCode NurseriesLawnAndGardenSupplyStore = new MerchantCategoryCode(5261);
    public static readonly MerchantCategoryCode NutStores = new MerchantCategoryCode(5441);
    public static readonly MerchantCategoryCode OfficeAndCommercialFurniture = new MerchantCategoryCode(5021);
    public static readonly MerchantCategoryCode OfficePhotographicPhotocopyAndMicrofilmEquipment = new MerchantCategoryCode(5044);
    public static readonly MerchantCategoryCode OrthopedicGoodsProstheticDevices = new MerchantCategoryCode(5976);
    public static readonly MerchantCategoryCode PackageStoresBeerWineAndLiquor = new MerchantCategoryCode(5921);
    public static readonly MerchantCategoryCode PaintAndWallpaperStores = new MerchantCategoryCode(5231);
    public static readonly MerchantCategoryCode PaintShopsAutomotive = new MerchantCategoryCode(7535);
    public static readonly MerchantCategoryCode PaintsVarnishesAndSupplies = new MerchantCategoryCode(5198);
    public static readonly MerchantCategoryCode PawnShopsAndSalvageYards = new MerchantCategoryCode(5933);
    public static readonly MerchantCategoryCode PetroleumAndPetroleumProducts = new MerchantCategoryCode(5172);
    public static readonly MerchantCategoryCode PetShopsPetFoodsAndSuppliesStores = new MerchantCategoryCode(5995);
    public static readonly MerchantCategoryCode PhotofinishingLaboratoriesPhotoDeveloping = new MerchantCategoryCode(7395);
    public static readonly MerchantCategoryCode PhotographicStudios = new MerchantCategoryCode(7221);
    public static readonly MerchantCategoryCode PieceGoodsNotionsAndOtherDryGoods = new MerchantCategoryCode(5131);
    public static readonly MerchantCategoryCode PlumbingAndHeatingEquipmentAndSupplies = new MerchantCategoryCode(5074);
    public static readonly MerchantCategoryCode PreciousStonesAndMetalsWatchesAndJewelry = new MerchantCategoryCode(5094);
    public static readonly MerchantCategoryCode ProtectiveAndSecurityServicesIncludingArmoredCarsAndGuardDogs =
        new MerchantCategoryCode(7393);
    public static readonly MerchantCategoryCode QuickCopyReproductionAndBlueprintingServices = new MerchantCategoryCode(7338);
    public static readonly MerchantCategoryCode RadioRepairShops = new MerchantCategoryCode(7622);
    public static readonly MerchantCategoryCode RecordShops = new MerchantCategoryCode(5735);
    public static readonly MerchantCategoryCode RecreationalAndUtilityTrailersCampDealers = new MerchantCategoryCode(5561);
    public static readonly MerchantCategoryCode ReligiousGoodsStores = new MerchantCategoryCode(5973);
    public static readonly MerchantCategoryCode RepairShopsAndRelatedServicesMiscellaneous = new MerchantCategoryCode(7699);
    public static readonly MerchantCategoryCode SecurityBrokersdealers = new MerchantCategoryCode(6211);
    public static readonly MerchantCategoryCode ServiceStationsWithOrWithoutAncillaryServices = new MerchantCategoryCode(5541);
    public static readonly MerchantCategoryCode SewingNeedleFabricAndPriceGoodsStores = new MerchantCategoryCode(5949);
    public static readonly MerchantCategoryCode ShoeStores = new MerchantCategoryCode(5661);
    public static readonly MerchantCategoryCode ShopRepairShopsAndShoeShineParlorsAndHatCleaningShops = new MerchantCategoryCode(7251);
    public static readonly MerchantCategoryCode SnowmobileDealers = new MerchantCategoryCode(5598);
    public static readonly MerchantCategoryCode SportingAndRecreationalCamps = new MerchantCategoryCode(7032);
    public static readonly MerchantCategoryCode SportingGoodsStores = new MerchantCategoryCode(5941);
    public static readonly MerchantCategoryCode SportsApparelRidingApparelStores = new MerchantCategoryCode(5655);
    public static readonly MerchantCategoryCode StampAndCoinStoresPhilatelicAndNumismaticSupplies = new MerchantCategoryCode(5972);
    public static readonly MerchantCategoryCode StationeryOfficeSuppliesPrintingAndWritingPaper = new MerchantCategoryCode(5111);
    public static readonly MerchantCategoryCode StationeryStoresOfficeAndSchoolSupplyStores = new MerchantCategoryCode(5943);
    public static readonly MerchantCategoryCode StenographicAndSecretarialSupportServices = new MerchantCategoryCode(7339);
    public static readonly MerchantCategoryCode Supermarkets = new MerchantCategoryCode(5411);
    public static readonly MerchantCategoryCode SwimmingPoolsSalesServiceAndSupplies = new MerchantCategoryCode(5996);
    public static readonly MerchantCategoryCode TailorsSeamstressMendingAndAlterations = new MerchantCategoryCode(5697);
    public static readonly MerchantCategoryCode TaxPreparationService = new MerchantCategoryCode(7276);

    public static readonly MerchantCategoryCode
        TelecommunicationServiceIncludingLocalAndLongDistanceCallsCreditCardCallsCallsThroughUseOfMagneticstripReadingTelephonesAndFaxServices =
            new MerchantCategoryCode(4814);

    public static readonly MerchantCategoryCode TelegraphServices = new MerchantCategoryCode(4821);
    public static readonly MerchantCategoryCode TentAndAwningShops = new MerchantCategoryCode(5998);
    public static readonly MerchantCategoryCode TheatricalProducersExceptMotionPicturesTicketAgencies = new MerchantCategoryCode(7922);
    public static readonly MerchantCategoryCode Timeshares = new MerchantCategoryCode(7012);
    public static readonly MerchantCategoryCode TireReTreadingAndRepairShops = new MerchantCategoryCode(7534);
    public static readonly MerchantCategoryCode TouristAttractionsAndExhibits = new MerchantCategoryCode(7991);
    public static readonly MerchantCategoryCode TowingServices = new MerchantCategoryCode(7549);
    public static readonly MerchantCategoryCode TrailerParksAndCampGrounds = new MerchantCategoryCode(7033);
    public static readonly MerchantCategoryCode TruckAndUtilityTrailerRentals = new MerchantCategoryCode(7513);
    public static readonly MerchantCategoryCode TypewriterStoresSalesRentalService = new MerchantCategoryCode(5978);
    public static readonly MerchantCategoryCode UsedMerchandiseAndSecondhandStores = new MerchantCategoryCode(5931);
    public static readonly MerchantCategoryCode VarietyStores = new MerchantCategoryCode(5331);
    public static readonly MerchantCategoryCode VideoAmusementGameSupplies = new MerchantCategoryCode(7993);
    public static readonly MerchantCategoryCode VideoGameArcadesestablishments = new MerchantCategoryCode(7994);
    public static readonly MerchantCategoryCode VideoTapeRentalStores = new MerchantCategoryCode(7841);
    public static readonly MerchantCategoryCode VisaPhone = new MerchantCategoryCode(4815);
    public static readonly MerchantCategoryCode WallpaperStores = new MerchantCategoryCode(5231);
    public static readonly MerchantCategoryCode WatchClockAndJewelryRepair = new MerchantCategoryCode(7631);
    public static readonly MerchantCategoryCode WatchClockJewelryAndSilverwareStores = new MerchantCategoryCode(5944);
    public static readonly MerchantCategoryCode WeldingRepair = new MerchantCategoryCode(7692);
    public static readonly MerchantCategoryCode WholesaleClubs = new MerchantCategoryCode(5300);
    public static readonly MerchantCategoryCode WigAndToupeeStores = new MerchantCategoryCode(5698);
    public static readonly MerchantCategoryCode WomensAccessoryAndSpecialtyShops = new MerchantCategoryCode(5631);
    public static readonly MerchantCategoryCode WomensReadyToWearStores = new MerchantCategoryCode(5621);
    public static readonly MerchantCategoryCode WreckingAndSalvageYards = new MerchantCategoryCode(5935);

    #endregion

    //public TerminalCategoryCode GetTerminalCategoryCode() => new TerminalCategoryCode(_Value % 10);
}