using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.IBLL;
using X.OA.Model;

namespace X.OA.BLL
{
    public partial class KeywordRankBLL : BaseBLL<KeywordRank>, IKeywordRankBLL
    {
        /// <summary>
        /// Truncate table: KeywordRank
        /// </summary>
        /// <returns></returns>
        public bool Truncate()
        {
            return dbSession.ExecuteSql($"truncate table KeywordRank") > 0;
        }

        /// <summary>
        /// Truncate and Statistic
        /// </summary>
        /// <returns></returns>
        public bool Statistic()
        {
            Truncate();
            // select Keyword , count(Keyword) as 'Count' from SearchDetail where  datediff(DAY,SearchTime,getdate()) <=6 group by Keyword
            string sqlStatement = "insert into KeywordRank select Keyword , count(Keyword) from SearchDetail where datediff(DAY,SearchTime,getdate()) <=6 group by Keyword";
            return dbSession.ExecuteSql(sqlStatement) > 0;
        }

        public IEnumerable<string> SearchTip(string term)
        {
            return Retrieve(r => r.Keyword.Contains(term)).Select(r=>r.Keyword);
        }
    }
}
