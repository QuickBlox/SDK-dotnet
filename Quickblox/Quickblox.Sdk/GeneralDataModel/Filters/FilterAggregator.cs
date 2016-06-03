using System.Collections.Generic;
using System.Text;

namespace Quickblox.Sdk.GeneralDataModel.Filters
{
    public class FilterAggregator : Filters.Filter
    {
        public FilterAggregator()
        {
            this.Filters = new List<Filters.Filter>();
        }

        public IList<Filters.Filter> Filters { get; private set; }

        internal string BuildFilters()
        {
            var stringBuilder = new StringBuilder();

            foreach (var filter in this.Filters)
            {
                stringBuilder.Append(filter.BuildFilter());
                stringBuilder.Append("&");
            }

            return stringBuilder.ToString();
        }

        internal override string BuildFilter()
        {
            return this.BuildFilters();
        }
    }
}
