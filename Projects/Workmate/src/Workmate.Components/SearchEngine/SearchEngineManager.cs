using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Workmate.Components.SearchEngine
{
  public class SearchEngineManager
  {
    private string _Location;

    public void AddDocument(string text)
    {
      //state the file location of the index
      DirectoryInfo directoryInfo = new DirectoryInfo(_Location);
      Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.Open(directoryInfo);

      //create an analyzer to process the text
      Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

      //create the index writer with the directory and analyzer defined.
      using (Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(dir, analyzer
        , true, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED))
      {
        //create a document, add in a single field
        Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

        Lucene.Net.Documents.Field fldContent = new Lucene.Net.Documents.Field("content",
          text,
          Lucene.Net.Documents.Field.Store.YES,
          Lucene.Net.Documents.Field.Index.ANALYZED,
          Lucene.Net.Documents.Field.TermVector.YES);

        doc.Add(fldContent);

        //write the document to the index
        indexWriter.AddDocument(doc);

        //optimize and close the writer
        indexWriter.Optimize();
        indexWriter.Close();
      }
    }

    public void Query(string searchText)
    {
      //state the file location of the index
      DirectoryInfo directoryInfo = new DirectoryInfo(_Location);
      Lucene.Net.Store.Directory dir = Lucene.Net.Store.FSDirectory.Open(directoryInfo);

      //create an index searcher that will perform the search
      Lucene.Net.Search.IndexSearcher searcher = new Lucene.Net.Search.IndexSearcher(dir, true);

      //build a query object
      Lucene.Net.Index.Term searchTerm = new Lucene.Net.Index.Term("content", searchText);
      Lucene.Net.Search.Query query = new Lucene.Net.Search.TermQuery(searchTerm);

      //execute the query
      Lucene.Net.Search.TopDocs hits = searcher.Search(query, null, 100);
      
      //iterate over the results.
      for (int i = 0; i < hits.TotalHits; i++)
      {
        Lucene.Net.Search.ScoreDoc doc = hits.ScoreDocs[i];
      }
    }

    public SearchEngineManager(string location)
    {
      _Location = location;

      DirectoryInfo directoryInfo = new DirectoryInfo(location);
      if (!directoryInfo.Exists)
        directoryInfo.Create();
    }
  }
}
