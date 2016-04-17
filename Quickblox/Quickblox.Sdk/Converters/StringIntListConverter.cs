using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Converters
{
    public class StringIntListConverter : JsonConverter
    {
        #region JsonConverter Members

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var intList = value as IList<int>;
            if (intList != null)
                serializer.Serialize(writer, ConvertToString(intList));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var intListString = (string)serializer.Deserialize(reader, typeof(string));

            return ConvertToIntList(intListString);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion

        public IList<int> ConvertToIntList(string intListString)
        {
            var intList = new List<int>();
            if (string.IsNullOrEmpty(intListString)) return intList;

            var idsStrings = intListString.Split(',');
            foreach (string idsString in idsStrings)
            {
                int id;
                if (int.TryParse(idsString, out id))
                    intList.Add(id);
            }

            return intList;
        }

        public string ConvertToString(IList<int> intList)
        {
            if (!intList.Any()) return null;

            string intListString = intList.Aggregate("", (current, intValue) => current + intValue.ToString() + ",");
            return intListString.Trim(',');
        }
    }
}
