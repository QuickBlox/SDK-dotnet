using System.Collections.Generic;
using System.Text;

namespace Quickblox.Sdk.GeneralDataModel.Request
{
    public class FilterAggregator : Filter
    {
        public FilterAggregator()
        {
            this.Filters = new List<Filter>();
        }

        public IList<Filter> Filters { get; private set; }

        public string BuildFilters()
        {
            var stringBuilder = new StringBuilder();

            foreach (var filter in this.Filters)
            {
                stringBuilder.Append("&");
                stringBuilder.Append(filter.BuildFilter());
            }

            return stringBuilder.ToString();
        }

        public override string BuildFilter()
        {
            return this.BuildFilters();
        }
    }
}
