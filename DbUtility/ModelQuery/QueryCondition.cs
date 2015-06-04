using System;

namespace FireflySoft.DbUtility.ModelQuery
{
    public class QueryCondition
    {
        public LinkType LinkType
        {
            get;
            set;
        }

        public object Property
        {
            get;
            set;
        }

        public string ConditionName
        {
            get;
            set;
        }

        public CompareType CompareType
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public QueryCondition[] SubQuery
        {
            get;
            set;
        }

        public new string ToString()
        {
            if (this.Property == null && (this.SubQuery == null || this.SubQuery.Length <= 0))
            {
                return string.Empty;
            }

            string text = string.Empty;
            if (this.SubQuery != null && this.SubQuery.Length > 0)
            {
                QueryCondition[] subQuery = this.SubQuery;
                for (int i = 0; i < subQuery.Length; i++)
                {
                    QueryCondition queryCondition = subQuery[i];
                    text = text + "," + queryCondition.ToString();
                }
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.Substring(1);
                }
            }

            return "{" + string.Format("LinkType:{0},Property:{1},ConditionName:{2},CompareType:{3},Value:{4},SubQuery:[{5}]", new object[]
			{
				this.LinkType.ToString(),
				this.Property ?? string.Empty,
                this.ConditionName ?? string.Empty,
				this.CompareType.ToString(),
				this.Value ?? string.Empty,
				text
			}) + "}";
        }
    }
}
