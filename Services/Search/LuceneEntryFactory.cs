namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using Lucene.Net.Index;
    using Lucene.Net.Search;
    using Lucene.Net.Documents;

    internal static class LuceneEntryFactory
    {
        public static void AddUpdateDocument(ISearchable searchable, IndexWriter writer)
        {
            if (searchable is IBeachSearchable)
            {
                var beachSearchable = (IBeachSearchable)searchable;

                AddUpdateBeachDoc(beachSearchable, writer);
            }
            else if (searchable is ICountrySearchable)
            {
                var countrySearchable = (ICountrySearchable)searchable;

                AddUpdateCountryDoc(countrySearchable, writer);
            }
            //else if (searchable is IRegionSearchable)
            //{
            //    var regionSearchable = (IRegionSearchable)searchable;

            //    AddUpdateRegionDoc(regionSearchable, writer);
            //}
            //else if (searchable is IAreaSearchable)
            //{
            //    var areaSearchable = (IAreaSearchable)searchable;

            //    AddUpdateAreaDoc(areaSearchable, writer);
            //}
            else if (searchable is IWaterBodySearchable)
            {
                var waterBodySearchable = (IWaterBodySearchable)searchable;

                AddUpdateWaterBodyDoc(waterBodySearchable, writer);
            }            
        }

        private static void AddUpdateBeachDoc(IBeachSearchable searchable, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);
            var descriptionField = new Field("Description", searchable.Description ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED);
            var addressField = new Field("Address", searchable.Address ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED);
            var coordinatesField = new Field("Coordinates", searchable.Coordinates ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED);

            nameField.Boost = 3.0f;
            descriptionField.Boost = 0.7f;
            addressField.Boost = 1.8f;

            newDoc.Add(idField);
            newDoc.Add(nameField);
            newDoc.Add(descriptionField);
            newDoc.Add(addressField);
            newDoc.Add(coordinatesField);

            writer.AddDocument(newDoc);
        }

        private static void AddUpdateCountryDoc(ICountrySearchable searchable, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);

            writer.AddDocument(newDoc);
        }

        //private static void AddUpdateRegionDoc(IRegionSearchable searchable, IndexWriter writer)
        //{
        //    var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

        //    writer.DeleteDocuments(oldDoc);

        //    var newDoc = new Document();
        //    var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
        //    var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);

        //    nameField.Boost = 3.0f;

        //    newDoc.Add(idField);
        //    newDoc.Add(nameField);

        //    writer.AddDocument(newDoc);
        //}

        //private static void AddUpdateAreaDoc(IAreaSearchable searchable, IndexWriter writer)
        //{
        //    var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

        //    writer.DeleteDocuments(oldDoc);

        //    var newDoc = new Document();
        //    var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
        //    var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);

        //    nameField.Boost = 3.0f;

        //    newDoc.Add(idField);
        //    newDoc.Add(nameField);

        //    writer.AddDocument(newDoc);
        //}

        private static void AddUpdateWaterBodyDoc(IWaterBodySearchable searchable, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);

            writer.AddDocument(newDoc);
        }
    }
}