using System;

namespace FireflySoft.DbUtility.ModelQuery
{
	public class PageInfo
	{
		private int _PageSize;
		private int _CurrentPage;
		private int _RecordCount;
		private int _PageCount;
		public int PageCount
		{
			get
			{
				return this._PageCount;
			}
			set
			{
				this._PageCount = value;
			}
		}
		public int PageSize
		{
			get
			{
				if (this._PageSize >= 1)
				{
					return this._PageSize;
				}
				return 10;
			}
			set
			{
				this._PageSize = value;
			}
		}
		public int CurrentPage
		{
			get
			{
				if (this._CurrentPage >= 1)
				{
					return this._CurrentPage;
				}
				return 1;
			}
			set
			{
				this._CurrentPage = value;
			}
		}
		public int RecordCount
		{
			get
			{
				if (this._RecordCount >= 0)
				{
					return this._RecordCount;
				}
				return 0;
			}
			set
			{
				this._RecordCount = value;
				this._PageCount = (int)Math.Ceiling((double)this._RecordCount / (double)this._PageSize);
			}
		}
	}
}
