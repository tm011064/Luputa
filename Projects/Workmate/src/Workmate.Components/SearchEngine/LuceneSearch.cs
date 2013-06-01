﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace MvcLuceneSampleApp.Search
{
  public static class LuceneSearch
  {
    // properties
    public static string _luceneDir = @"C:\temp\LuceneIndexes";
    private static FSDirectory _directoryTemp;
    private static FSDirectory _directory
    {
      get
      {
        if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
        if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
        var lockFilePath = Path.Combine(_luceneDir, "write.lock");
        if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
        return _directoryTemp;
      }
    }


    // search methods
    public static IEnumerable<CMSContent> GetAllIndexRecords()
    {
      // validate search index
      if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<CMSContent>();

      // set up lucene searcher
      var searcher = new IndexSearcher(_directory, false);
      var reader = IndexReader.Open(_directory, false);
      var docs = new List<Document>();
      var term = reader.TermDocs();
      while (term.Next()) docs.Add(searcher.Doc(term.Doc()));
      reader.Close();
      reader.Dispose();
      searcher.Close();
      searcher.Dispose();
      return _mapLuceneToDataList(docs);
    }
    public static IEnumerable<CMSContent> Search(string input, string fieldName = "")
    {
      if (string.IsNullOrEmpty(input)) return new List<CMSContent>();

      var terms = input.Trim().Replace("-", " ").Split(' ')
        .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
      input = string.Join(" ", terms);

      return _search(input, fieldName);
    }
    public static IEnumerable<CMSContent> SearchDefault(string input, string fieldName = "")
    {
      return string.IsNullOrEmpty(input) ? new List<CMSContent>() : _search(input, fieldName);
    }


    // main search method
    private static IEnumerable<CMSContent> _search(string searchQuery, string searchField = "")
    {
      // validation
      if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<CMSContent>();

      // set up lucene searcher
      using (var searcher = new IndexSearcher(_directory, false))
      {
        var hits_limit = 1000;
        var analyzer = new StandardAnalyzer(Version.LUCENE_29);

        // search by single field
        if (!string.IsNullOrEmpty(searchField))
        {
          var parser = new QueryParser(Version.LUCENE_29, searchField, analyzer);
          var query = parseQuery(searchQuery, parser);
          var hits = searcher.Search(query, hits_limit).ScoreDocs;
          var results = _mapLuceneToDataList(hits, searcher);
          analyzer.Close();
          searcher.Close();
          searcher.Dispose();
          return results;
        }
        // search by multiple fields (ordered by RELEVANCE)
        else
        {
          var parser = new MultiFieldQueryParser
            (Version.LUCENE_29, new[] { "Id", "Header", "Body" }, analyzer);
          var query = parseQuery(searchQuery, parser);
          var hits = searcher.Search(query, null, hits_limit, Sort.INDEXORDER).ScoreDocs;
          var results = _mapLuceneToDataList(hits, searcher);
          analyzer.Close();
          searcher.Close();
          searcher.Dispose();
          return results;
        }
      }
    }
    private static Query parseQuery(string searchQuery, QueryParser parser)
    {
      Query query;
      try
      {
        query = parser.Parse(searchQuery.Trim());
      }
      catch (ParseException)
      {
        query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
      }
      return query;
    }


    // map Lucene search index to data
    private static IEnumerable<CMSContent> _mapLuceneToDataList(IEnumerable<Document> hits)
    {
      return hits.Select(_mapLuceneDocumentToData).ToList();
    }
    private static IEnumerable<CMSContent> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
    {
      return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
    }
    private static CMSContent _mapLuceneDocumentToData(Document doc)
    {
      return new CMSContent
      {
        Id = Convert.ToInt32(doc.Get("Id")),
        Header = doc.Get("Header"),
        Body = doc.Get("Body")
      };
    }


    // add/update/clear search index data 
    public static void AddUpdateLuceneIndex(CMSContent cmsContent)
    {
      AddUpdateLuceneIndex(new List<CMSContent> { cmsContent });
    }
    public static void AddUpdateLuceneIndex(IEnumerable<CMSContent> cmsContents)
    {
      // init lucene
      var analyzer = new StandardAnalyzer(Version.LUCENE_29);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
      {
        // add data to lucene search index (replaces older entries if any)
        foreach (var cmsContent in cmsContents) 
          _addToLuceneIndex(cmsContent, writer);

        // close handles
        analyzer.Close();
        writer.Close();
        writer.Dispose();
      }
    }
    public static void ClearLuceneIndexRecord(int record_id)
    {
      // init lucene
      var analyzer = new StandardAnalyzer(Version.LUCENE_29);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
      {
        // remove older index entry
        var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
        writer.DeleteDocuments(searchQuery);

        // close handles
        analyzer.Close();
        writer.Close();
        writer.Dispose();
      }
    }
    public static bool ClearLuceneIndex()
    {
      try
      {
        var analyzer = new StandardAnalyzer(Version.LUCENE_29);
        using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
        {
          // remove older index entries
          writer.DeleteAll();

          // close handles
          analyzer.Close();
          writer.Close();
          writer.Dispose();
        }
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }
    public static void Optimize()
    {
      var analyzer = new StandardAnalyzer(Version.LUCENE_29);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
      {
        analyzer.Close();
        writer.Optimize();
        writer.Close();
        writer.Dispose();
      }
    }
    private static void _addToLuceneIndex(CMSContent cmsContent, IndexWriter writer)
    {
      // remove older index entry
      var searchQuery = new TermQuery(new Term("Id", cmsContent.Id.ToString()));
      writer.DeleteDocuments(searchQuery);

      // add new index entry
      var doc = new Document();

      // add lucene fields mapped to db fields
      doc.Add(new Field("Id", cmsContent.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("Header", cmsContent.Header, Field.Store.YES, Field.Index.ANALYZED));
      doc.Add(new Field("Body", cmsContent.Body, Field.Store.YES, Field.Index.ANALYZED));

      // add entry to index
      writer.AddDocument(doc);
    }

  }
}