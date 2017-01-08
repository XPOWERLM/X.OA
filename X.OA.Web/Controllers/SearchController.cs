using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using X.OA.IBLL;
using X.OA.Model;
using X.OA.Web.Models;
using static X.OA.Common.Helper.LuceneHelper;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Helper.JsonHelper;
using System.IO;

namespace X.OA.Web.Controllers
{
    public class SearchController : Controller
    {
        #region Create required objects
        IBookBLL bBLL = container.Resolve<IBookBLL>();
        ISearchDetailBLL searchBLL = container.Resolve<ISearchDetailBLL>();
        IKeywordRankBLL keywordBLL = container.Resolve<IKeywordRankBLL>();
        #endregion
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            string indexPath = Request.MapPath("/Lucene");

            // Write
            using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool exists = IndexReader.IndexExists(directory);
                if (exists)
                    if (IndexWriter.IsLocked(directory))
                        IndexWriter.Unlock(directory);

                using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !exists, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    IEnumerable<Book> books = bBLL.Retrieve(b => true);

                    foreach (Book book in books)
                    {
                        // Notice : One document store one item.
                        Document document = new Document();
                        document.Add(new Field(nameof(book.Id), book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field(nameof(book.Title), book.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field(nameof(book.ContentDescription), book.ContentDescription, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        writer.AddDocument(document);
                    }
                }
            }
            return Content("OK");
        }

        public ActionResult Search(string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord.Trim()))
                return JsonNT(new { content = string.Empty, keyWords = string.Empty });
            IEnumerable<string> keyWords = SplitWord(keyWord);

            // Search statistics
            searchBLL.Create(new SearchDetail
            {
                Keyword = keyWord,
                SearchTime = DateTime.Now
            });
            searchBLL.SaveChanges();

            return JsonNT(new { content = LuceneSearch(keyWords), keyWords = keyWords });
        }

        public ActionResult SearchTip(string term)
        {
            return JsonNT(keywordBLL.SearchTip(term));
        }

        #region Auxiliary routines
        /// <summary>
        /// Search using lucene
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public IEnumerable<SearchViewModels> LuceneSearch(IEnumerable<string> keyWords)
        {
            Book book = new Book();
            string indexPath = Request.MapPath("/Lucene");

            using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory()))
            {
                using (IndexReader reader = IndexReader.Open(directory, true))
                {
                    using (IndexSearcher searcher = new IndexSearcher(reader))
                    {
                        // Query condition
                        PhraseQuery query = new PhraseQuery { Slop = 10 };
                        foreach (string keyword in keyWords)
                            query.Add(new Term(nameof(book.ContentDescription), keyword));

                        // Search
                        TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
                        searcher.Search(query, null, collector);
                        ScoreDoc[] docs = collector.TopDocs(0, collector.TotalHits).ScoreDocs;

                        // Process
                        string id = nameof(book.Id);
                        string title = nameof(book.Title);
                        string contentDescription = nameof(book.ContentDescription);
                        for (int i = 0; i < docs.Length; i++)
                        {
                            int docId = docs[i].Doc;
                            Document doc = searcher.Doc(docId);
                            yield return new SearchViewModels
                            {
                                Id = doc.Get(id).ToInt32(),
                                Title = doc.Get(title),
                                ContentDescription = doc.Get(contentDescription)
                            };
                        }
                    }
                }
            }
        }

        #endregion

    }
}