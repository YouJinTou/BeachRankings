namespace BeachRankings.Services.Search
{
    using BeachRankings.Models.Interfaces;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;

    internal class LuceneEntryFactory
    {
        public void AddUpdateDocument(ISearchable searchable, IndexWriter writer)
        {
            if (searchable == null)
            {
                return;
            }

            if (searchable is IBeachSearchable)
            {
                var beachSearchable = (IBeachSearchable)searchable;

                this.AddUpdateBeachDoc(beachSearchable, writer);
            }
            else if (searchable is IPlaceSearchable)
            {
                var placeSearchable = (IPlaceSearchable)searchable;

                this.AddUpdatePlaceDoc(placeSearchable, writer);
            }
        }

        private void AddUpdateBeachDoc(IBeachSearchable searchable, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);
            var descriptionField = new Field("Description", searchable.Description ?? string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var addressField = new Field("Address", searchable.Address ?? string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var coordinatesField = new Field("Coordinates", searchable.Coordinates ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);
            newDoc.Add(descriptionField);
            newDoc.Add(addressField);
            newDoc.Add(coordinatesField);

            writer.AddDocument(newDoc);
        }

        private void AddUpdatePlaceDoc(IPlaceSearchable searchable, IndexWriter writer)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            writer.DeleteDocuments(oldDoc);

            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", searchable.Name, Field.Store.YES, Field.Index.ANALYZED);
            var beachCountField = new NumericField("BeachCount", Field.Store.YES, false).SetIntValue(searchable.Beaches.Count);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);
            newDoc.Add(beachCountField);

            writer.AddDocument(newDoc);
        }
    }
}