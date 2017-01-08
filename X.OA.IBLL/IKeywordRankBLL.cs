using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.Model;

namespace X.OA.IBLL
{
    public partial interface IKeywordRankBLL : IBaseBll<KeywordRank>
    {
        bool Truncate();

        bool Statistic();

        IEnumerable<string> SearchTip(string keyword);
    }
}
