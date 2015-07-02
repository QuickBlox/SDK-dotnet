namespace Quickblox.Sdk.Modules.CustomObjectModule.Models
{
    public enum CustomObjectOperator
    {
        //Integer, Float  score_value[lt]=1000	Less Than operator
        Lt,
        ////Integer, Float  score_value[lte]=850	Less Than or Equal to operator
        Lte,
        //Integer, Float bonus_count[gt]= 2.45    Greater Than operator
        Gt,
        //Integer, Float bonus_count[gte]= 56.443 Greater Than or Equal to operator
        Gte,
        //Integer, Float, String, Boolean game_mode_name[ne]= ctf  Not Equal to operator
        Ne,
        //Integer, Float, String game_mode_name[in]= deathmatch, rage Contained IN array operator
        In,
        //Integer, Float, String game_mode_name[nin]= survivor, crazy_nightmare Not contained IN array
        Nin,
        //Array game_modes[all]= survivor, crazy ALL contained IN array
        All,
        //Integer, Float, String  1.name[or]= sam, igor
        //2.name[or]= sam & lastname[or] = johnson	1.Will return records with name sam or igor 
        //2.Will return records with name sam or lastname johnson
        Or,
        //String username[ctn]= son   Will return all records where username field contains son substring
        Ctn
    }
}
