using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X.OA.Common.Helper
{
    public static class LuceneHelper
    {
        #region Create required objects
        private static Queue queue = new Queue();
        #endregion

        /// <summary>
        /// Split chinese word
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitWord(string keyWord)
        {
            using (Analyzer analyzer = new PanGuAnalyzer())
            {
                using (TokenStream tokenStream = analyzer.TokenStream(string.Empty, new StringReader(keyWord)))
                {
                    ITermAttribute termAttribute = tokenStream.AddAttribute<ITermAttribute>();
                    while (tokenStream.IncrementToken())
                        yield return termAttribute.Term;
                }
            }
        }

        /// <summary>
        /// Enqueue index
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="indexPath"></param>
        public static void EnQueueIndex(IEnumerable<object> entitys)
        {
            queue.Enqueue(entitys);
        }

        /// <summary>
        /// Start polling thread
        /// </summary>
        public static void StartPolling(string indexPath)
        {
            // Timer is better ？ 
            new Thread(() =>
            {
                while (true)
                {
                    if (queue.Count > 0)
                        ProcessIndex((IEnumerable<object>)queue.Dequeue(), indexPath);
                    else
                        Thread.Sleep(5000);
                }
            })
            { IsBackground = true }.Start();
        }

        /// <summary>
        /// Process Index, using reflect，add or delete,
        /// </summary>
        /// <param name="entitys">Each entity have two required property: Id, isDelete</param>
        /// <param name="indexPath">The index path</param>
        public static void ProcessIndex(IEnumerable<object> entitys, string indexPath)
        {
            // Write
            using (FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool exists = IndexReader.IndexExists(directory);
                if (exists)
                    if (IndexWriter.IsLocked(directory))
                        IndexWriter.Unlock(directory);

                using (IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !exists, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (object entity in entitys)
                    {
                        Type type = entity.GetType();
                        PropertyInfo[] props = type.GetProperties();

                        // Notice : One document store one instance.
                        Document document = new Document();

                        foreach (PropertyInfo prop in props)
                        {
                            if (!prop.CanRead) continue;

                            // If the isDelete property is true, delete document
                            if (prop.Name.Equals("isDelete", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if ((bool)prop.GetValue(entity))
                                    writer.DeleteDocuments(new Term(prop.Name, prop.GetValue(entity).ToString()));
                                continue;
                            }

                            // Id do not analyze
                            if (prop.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                                document.Add(new Field(prop.Name, prop.GetValue(entity).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            
                            document.Add(new Field(prop.Name, prop.GetValue(entity).ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                        }
                        writer.AddDocument(document);
                    }
                }
            }
        }

    }
}
