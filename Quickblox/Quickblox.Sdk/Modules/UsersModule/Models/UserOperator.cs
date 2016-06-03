namespace Quickblox.Sdk.GeneralDataModel.Filter
{
    public enum UserOperator
    {
        /// <summary>
        /// Sample: will return users withs IDs greater than 3
        /// </summary>
        Gt,
        /// <summary>
        /// Sample: will return users withs IDs less than 34
        /// </summary>
        Lt,
        /// <summary>
        /// Sample: will return users withs IDs greater than or equal to 445
        /// </summary>
        Ge,
        /// <summary>
        /// Sample: Will return users withs IDs less than or equal 2241
        /// </summary>
        Le,
        /// <summary>
        /// Sample: Will return a user withs ID equal to 3
        /// </summary>
        Eq,
        /// <summary>
        /// Sample: Will return users withs IDs not equal to 3
        /// </summary>
        Ne,
        /// <summary>
        /// Sample: Will return users withs IDs between 3 and 2241
        /// </summary>
        Between,
        /// <summary>
        /// Sample: Will return users withs IDs 3,45,2241
        /// </summary>
        In
    }
}
