namespace Quickblox.Sdk.GeneralDataModel.Filters
{
    /// <summary>
    /// lt (Less Than operator), 
    /// lte (Less Than or Equal to operator), 
    /// gt (Greater Than operator), 
    /// gte (Greater Than or Equal to operator), 
    /// ne (Not Equal to operator),
    /// in (Contained IN array operator), 
    /// nin (Not contained IN array), 
    /// all (ALL contained IN array),
    /// or (OR operator),
    /// ctn (Contains substring operator)
    /// </summary>
    public enum SearchOperators
    {
        Lt,
        Lte,
        Gt,
        Gte,
        Ne,
        In,
        Nin,
        All,
        Or,
        Ctn
    }
}
