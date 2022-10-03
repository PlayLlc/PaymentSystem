using System.Collections.Immutable;

using Play.Core;

namespace Play.Merchants.Onboarding.Domain.Enums
{
    public record StateAbbreviations : EnumObjectString
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<string, StateAbbreviations> _ValueObjectMap;
        public static readonly StateAbbreviations Empty;
        public static readonly StateAbbreviations Alabama;
        public static readonly StateAbbreviations Alaska;
        public static readonly StateAbbreviations Arizona;
        public static readonly StateAbbreviations Arkansas;
        public static readonly StateAbbreviations California;
        public static readonly StateAbbreviations Colorado;
        public static readonly StateAbbreviations Connecticut;
        public static readonly StateAbbreviations Delaware;
        public static readonly StateAbbreviations DistrictOfColumbia;
        public static readonly StateAbbreviations Florida;
        public static readonly StateAbbreviations Georgia;
        public static readonly StateAbbreviations Hawaii;
        public static readonly StateAbbreviations Idaho;
        public static readonly StateAbbreviations Illinois;
        public static readonly StateAbbreviations Indiana;
        public static readonly StateAbbreviations Iowa;
        public static readonly StateAbbreviations Kansas;
        public static readonly StateAbbreviations Kentucky;
        public static readonly StateAbbreviations Louisiana;
        public static readonly StateAbbreviations Maine;
        public static readonly StateAbbreviations Maryland;
        public static readonly StateAbbreviations Massachusetts;
        public static readonly StateAbbreviations Michigan;
        public static readonly StateAbbreviations Minnesota;
        public static readonly StateAbbreviations Mississippi;
        public static readonly StateAbbreviations Missouri;
        public static readonly StateAbbreviations Montana;
        public static readonly StateAbbreviations Nebraska;
        public static readonly StateAbbreviations Nevada;
        public static readonly StateAbbreviations NewHampshire;
        public static readonly StateAbbreviations NewJersey;
        public static readonly StateAbbreviations NewMexico;
        public static readonly StateAbbreviations NewYork;
        public static readonly StateAbbreviations NorthCarolina;
        public static readonly StateAbbreviations NorthDakota;
        public static readonly StateAbbreviations Ohio;
        public static readonly StateAbbreviations Oklahoma;
        public static readonly StateAbbreviations Oregon;
        public static readonly StateAbbreviations Pennsylvania;
        public static readonly StateAbbreviations RhodeIsland;
        public static readonly StateAbbreviations SouthCarolina;
        public static readonly StateAbbreviations SouthDakota;
        public static readonly StateAbbreviations Tennessee;
        public static readonly StateAbbreviations Texas;
        public static readonly StateAbbreviations Utah;
        public static readonly StateAbbreviations Vermont;
        public static readonly StateAbbreviations Virginia;
        public static readonly StateAbbreviations Washington;
        public static readonly StateAbbreviations WestVirginia;
        public static readonly StateAbbreviations Wisconsin;
        public static readonly StateAbbreviations Wyoming;

        #endregion

        #region Constructor

        private StateAbbreviations(string value) : base(value)
        { }

        static StateAbbreviations()
        {
            Alabama = new StateAbbreviations("AL");
            Alaska = new StateAbbreviations("AK");
            Arizona = new StateAbbreviations("AZ");
            Arkansas = new StateAbbreviations("AR");
            California = new StateAbbreviations("CA");
            Colorado = new StateAbbreviations("CO");
            Connecticut = new StateAbbreviations("CT");
            Delaware = new StateAbbreviations("DE");
            DistrictOfColumbia = new StateAbbreviations("DC");
            Florida = new StateAbbreviations("FL");
            Georgia = new StateAbbreviations("GA");
            Hawaii = new StateAbbreviations("HI");
            Idaho = new StateAbbreviations("ID");
            Illinois = new StateAbbreviations("IL");
            Indiana = new StateAbbreviations("IN");
            Iowa = new StateAbbreviations("IA");
            Kansas = new StateAbbreviations("KS");
            Kentucky = new StateAbbreviations("KY");
            Louisiana = new StateAbbreviations("LA");
            Maine = new StateAbbreviations("ME");
            Maryland = new StateAbbreviations("MD");
            Massachusetts = new StateAbbreviations("MA");
            Michigan = new StateAbbreviations("MI");
            Minnesota = new StateAbbreviations("MN");
            Mississippi = new StateAbbreviations("MS");
            Missouri = new StateAbbreviations("MO");
            Montana = new StateAbbreviations("MT");
            Nebraska = new StateAbbreviations("NE");
            Nevada = new StateAbbreviations("NV");
            NewHampshire = new StateAbbreviations("NH");
            NewJersey = new StateAbbreviations("NJ");
            NewMexico = new StateAbbreviations("NM");
            NewYork = new StateAbbreviations("NY");
            NorthCarolina = new StateAbbreviations("NC");
            NorthDakota = new StateAbbreviations("ND");
            Ohio = new StateAbbreviations("OH");
            Oklahoma = new StateAbbreviations("OK");
            Oregon = new StateAbbreviations("OR");
            Pennsylvania = new StateAbbreviations("PA");
            RhodeIsland = new StateAbbreviations("RI");
            SouthCarolina = new StateAbbreviations("SC");
            SouthDakota = new StateAbbreviations("SD");
            Tennessee = new StateAbbreviations("TN");
            Texas = new StateAbbreviations("TX");
            Utah = new StateAbbreviations("UT");
            Vermont = new StateAbbreviations("VT");
            Virginia = new StateAbbreviations("VA");
            Washington = new StateAbbreviations("WA");
            WestVirginia = new StateAbbreviations("WV");
            Wisconsin = new StateAbbreviations("WI");
            Wyoming = new StateAbbreviations("WY");
            _ValueObjectMap = new Dictionary<string, StateAbbreviations>
            {
                {Alabama, Alabama}, {Alaska, Alaska}, {Arizona, Arizona}, {Arkansas, Arkansas}, {California, California}, {Colorado, Colorado},
                {Connecticut, Connecticut}, {Delaware, Delaware}, {DistrictOfColumbia, DistrictOfColumbia}, {Florida, Florida}, {Georgia, Georgia},
                {Hawaii, Hawaii}, {Idaho, Idaho}, {Illinois, Illinois}, {Indiana, Indiana}, {Iowa, Iowa}, {Kansas, Kansas}, {Kentucky, Kentucky},
                {Louisiana, Louisiana}, {Maine, Maine}, {Maryland, Maryland}, {Massachusetts, Massachusetts}, {Michigan, Michigan}, {Minnesota, Minnesota},
                {Mississippi, Mississippi}, {Missouri, Missouri}, {Montana, Montana}, {Nebraska, Nebraska}, {Nevada, Nevada}, {NewHampshire, NewHampshire},
                {NewJersey, NewJersey}, {NewMexico, NewMexico}, {NewYork, NewYork}, {NorthCarolina, NorthCarolina}, {NorthDakota, NorthDakota},
                {Ohio, Ohio}, {Oklahoma, Oklahoma}, {Oregon, Oregon}, {Pennsylvania, Pennsylvania}, {RhodeIsland, RhodeIsland},
                {SouthCarolina, SouthCarolina}, {SouthDakota, SouthDakota}, {Tennessee, Tennessee}, {Texas, Texas}, {Utah, Utah}, {Vermont, Vermont},
                {Virginia, Virginia}, {Washington, Washington}, {WestVirginia, WestVirginia}, {Wisconsin, Wisconsin}, {Wyoming, Wyoming}
            }.ToImmutableSortedDictionary();
        }

        #endregion

        #region Instance Members

        public override StateAbbreviations[] GetAll()
        {
            return _ValueObjectMap.Values.ToArray();
        }

        public override bool TryGet(string value, out EnumObjectString? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out StateAbbreviations? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        #endregion
    }
}