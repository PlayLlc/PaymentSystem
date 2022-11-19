using System.Collections.Immutable;

using Play.Core;

namespace Play.Domain.Common.Enums;

public record States : EnumObjectString
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<string, States> _ValueObjectMap;
    public static readonly States Empty;
    public static readonly States Alabama;
    public static readonly States Alaska;
    public static readonly States Arizona;
    public static readonly States Arkansas;
    public static readonly States California;
    public static readonly States Colorado;
    public static readonly States Connecticut;
    public static readonly States Delaware;
    public static readonly States DistrictOfColumbia;
    public static readonly States Florida;
    public static readonly States Georgia;
    public static readonly States Hawaii;
    public static readonly States Idaho;
    public static readonly States Illinois;
    public static readonly States Indiana;
    public static readonly States Iowa;
    public static readonly States Kansas;
    public static readonly States Kentucky;
    public static readonly States Louisiana;
    public static readonly States Maine;
    public static readonly States Maryland;
    public static readonly States Massachusetts;
    public static readonly States Michigan;
    public static readonly States Minnesota;
    public static readonly States Mississippi;
    public static readonly States Missouri;
    public static readonly States Montana;
    public static readonly States Nebraska;
    public static readonly States Nevada;
    public static readonly States NewHampshire;
    public static readonly States NewJersey;
    public static readonly States NewMexico;
    public static readonly States NewYork;
    public static readonly States NorthCarolina;
    public static readonly States NorthDakota;
    public static readonly States Ohio;
    public static readonly States Oklahoma;
    public static readonly States Oregon;
    public static readonly States Pennsylvania;
    public static readonly States RhodeIsland;
    public static readonly States SouthCarolina;
    public static readonly States SouthDakota;
    public static readonly States Tennessee;
    public static readonly States Texas;
    public static readonly States Utah;
    public static readonly States Vermont;
    public static readonly States Virginia;
    public static readonly States Washington;
    public static readonly States WestVirginia;
    public static readonly States Wisconsin;
    public static readonly States Wyoming;

    #endregion

    #region Constructor

    private States(string value) : base(value)
    { }

    static States()
    {
        Empty = new States("");
        Alabama = new States(nameof(Alabama));
        Alaska = new States(nameof(Alaska));
        Arizona = new States(nameof(Arizona));
        Arkansas = new States(nameof(Arkansas));
        California = new States(nameof(California));
        Colorado = new States(nameof(Colorado));
        Connecticut = new States(nameof(Connecticut));
        Delaware = new States(nameof(Delaware));
        DistrictOfColumbia = new States(nameof(DistrictOfColumbia));
        Florida = new States(nameof(Florida));
        Georgia = new States(nameof(Georgia));
        Hawaii = new States(nameof(Hawaii));
        Idaho = new States(nameof(Idaho));
        Illinois = new States(nameof(Illinois));
        Indiana = new States(nameof(Indiana));
        Iowa = new States(nameof(Iowa));
        Kansas = new States(nameof(Kansas));
        Kentucky = new States(nameof(Kentucky));
        Louisiana = new States(nameof(Louisiana));
        Maine = new States(nameof(Maine));
        Maryland = new States(nameof(Maryland));
        Massachusetts = new States(nameof(Massachusetts));
        Michigan = new States(nameof(Michigan));
        Minnesota = new States(nameof(Minnesota));
        Mississippi = new States(nameof(Mississippi));
        Missouri = new States(nameof(Missouri));
        Montana = new States(nameof(Montana));
        Nebraska = new States(nameof(Nebraska));
        Nevada = new States(nameof(Nevada));
        NewHampshire = new States(nameof(NewHampshire));
        NewJersey = new States(nameof(NewJersey));
        NewMexico = new States(nameof(NewMexico));
        NewYork = new States(nameof(NewYork));
        NorthCarolina = new States(nameof(NorthCarolina));
        NorthDakota = new States(nameof(NorthDakota));
        Ohio = new States(nameof(Ohio));
        Oklahoma = new States(nameof(Oklahoma));
        Oregon = new States(nameof(Oregon));
        Pennsylvania = new States(nameof(Pennsylvania));
        RhodeIsland = new States(nameof(RhodeIsland));
        SouthCarolina = new States(nameof(SouthCarolina));
        SouthDakota = new States(nameof(SouthDakota));
        Tennessee = new States(nameof(Tennessee));
        Texas = new States(nameof(Texas));
        Utah = new States(nameof(Utah));
        Vermont = new States(nameof(Vermont));
        Virginia = new States(nameof(Virginia));
        Washington = new States(nameof(Washington));
        WestVirginia = new States(nameof(WestVirginia));
        Wisconsin = new States(nameof(Wisconsin));
        Wyoming = new States(nameof(Wyoming));
        _ValueObjectMap = new Dictionary<string, States>
        {
            {Alabama, Alabama},
            {Alaska, Alaska},
            {Arizona, Arizona},
            {Arkansas, Arkansas},
            {California, California},
            {Colorado, Colorado},
            {Connecticut, Connecticut},
            {Delaware, Delaware},
            {DistrictOfColumbia, DistrictOfColumbia},
            {Florida, Florida},
            {Georgia, Georgia},
            {Hawaii, Hawaii},
            {Idaho, Idaho},
            {Illinois, Illinois},
            {Indiana, Indiana},
            {Iowa, Iowa},
            {Kansas, Kansas},
            {Kentucky, Kentucky},
            {Louisiana, Louisiana},
            {Maine, Maine},
            {Maryland, Maryland},
            {Massachusetts, Massachusetts},
            {Michigan, Michigan},
            {Minnesota, Minnesota},
            {Mississippi, Mississippi},
            {Missouri, Missouri},
            {Montana, Montana},
            {Nebraska, Nebraska},
            {Nevada, Nevada},
            {NewHampshire, NewHampshire},
            {NewJersey, NewJersey},
            {NewMexico, NewMexico},
            {NewYork, NewYork},
            {NorthCarolina, NorthCarolina},
            {NorthDakota, NorthDakota},
            {Ohio, Ohio},
            {Oklahoma, Oklahoma},
            {Oregon, Oregon},
            {Pennsylvania, Pennsylvania},
            {RhodeIsland, RhodeIsland},
            {SouthCarolina, SouthCarolina},
            {SouthDakota, SouthDakota},
            {Tennessee, Tennessee},
            {Texas, Texas},
            {Utah, Utah},
            {Vermont, Vermont},
            {Virginia, Virginia},
            {Washington, Washington},
            {WestVirginia, WestVirginia},
            {Wisconsin, Wisconsin},
            {Wyoming, Wyoming}
        }.ToImmutableSortedDictionary();
    }

    #endregion

    #region Instance Members

    public override States[] GetAll()
    {
        return _ValueObjectMap.Values.ToArray();
    }

    public States Get(string value)
    {
        return _ValueObjectMap[value];
    }

    public override bool TryGet(string value, out EnumObjectString? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out States? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion
}