namespace BeachRankings.Services.Search
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.Services.Search.Enums;
    using BeachRankings.Services.Formatters;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Search;

    internal class LuceneEntryFactory
    {
        private ModelType modelType;
        private IndexWriter writer;

        public LuceneEntryFactory(ModelType modelType, IndexWriter writer)
        {
            this.modelType = modelType;
            this.writer = writer;
        }

        public void AddUpdateDocument(ISearchable searchable)
        {
            if (searchable == null)
            {
                return;
            }

            switch (this.modelType)
            {
                case ModelType.Beach:
                    var beach = (Beach)searchable;

                    this.AddUpdateBeachDoc(beach);

                    break;
                default:
                    var placeSearchable = (IPlaceSearchable)searchable;

                    this.AddUpdatePlaceDoc(placeSearchable);

                    break;
            }
        }

        private void AddUpdateBeachDoc(IBeachSearchable searchable)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            this.writer.DeleteDocuments(oldDoc);

            var formatter = new NameFormatter();
            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", formatter.GetFormattedBeachName(searchable.Name), Field.Store.YES, Field.Index.ANALYZED);
            var originalNameField = new Field("OriginalName", formatter.GetOutboundName(searchable.Name), Field.Store.YES, Field.Index.NOT_ANALYZED);
            var descriptionField = new Field("Description", searchable.Description ?? string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var addressField = new Field("Address", searchable.Address ?? string.Empty, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var coordinatesField = new Field("Coordinates", searchable.Coordinates ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);
            newDoc.Add(originalNameField);
            newDoc.Add(descriptionField);
            newDoc.Add(addressField);
            newDoc.Add(coordinatesField);

            this.writer.AddDocument(newDoc);
        }

        private void AddUpdatePlaceDoc(IPlaceSearchable searchable)
        {
            var oldDoc = new TermQuery(new Term("Id", searchable.Id.ToString()));

            this.writer.DeleteDocuments(oldDoc);

            var formatter = new NameFormatter();
            var newDoc = new Document();
            var idField = new Field("Id", searchable.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            var nameField = new Field("Name", formatter.GetFormattedPlaceName(searchable.Name), Field.Store.YES, Field.Index.ANALYZED);
            var originalNameField = new Field("OriginalName", formatter.GetOutboundName(searchable.Name), Field.Store.YES, Field.Index.NOT_ANALYZED);
            var beachCountField = new NumericField("BeachCount", Field.Store.YES, false).SetIntValue(searchable.Beaches.Count);

            nameField.Boost = 3.0f;

            newDoc.Add(idField);
            newDoc.Add(nameField);
            newDoc.Add(originalNameField);
            newDoc.Add(beachCountField);

            this.AddPlaceDependentFields(searchable, newDoc);

            this.writer.AddDocument(newDoc);
        }

        private void AddPlaceDependentFields(IPlaceSearchable searchable, Document doc)
        {
            switch (this.modelType)
            {
                case ModelType.Continent:
                case ModelType.WaterBody:
                    return;
                case ModelType.Country:
                    var country = (Country)searchable;

                    this.AddUpdateCountryDoc(doc, country);

                    break;
                case ModelType.PrimaryDivision:
                    var primaryDivision = (PrimaryDivision)searchable;

                    this.AddUpdatePrimaryDivisionDoc(doc, primaryDivision);

                    break;
                case ModelType.SecondaryDivision:
                    var secondaryDivision = (SecondaryDivision)searchable;

                    this.AddUpdateSecondaryDivisionDoc(doc, secondaryDivision);

                    break;
                case ModelType.TertiaryDivision:
                    var tertiaryDivision = (TertiaryDivision)searchable;

                    this.AddUpdateTertiaryDivisionDoc(doc, tertiaryDivision);

                    break;
                case ModelType.QuaternaryDivision:
                    var quaternaryDivision = (QuaternaryDivision)searchable;

                    this.AddUpdateQuaternaryDivisionDoc(doc, quaternaryDivision);

                    break;                
            }
        }

        private void AddUpdateCountryDoc(Document doc, Country country)
        {
            if (country.Continent == null)
            {
                return;
            }

            var continentField = new Field("Continent", country.Continent.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);

            doc.Add(continentField);
        }

        private void AddUpdatePrimaryDivisionDoc(Document doc, PrimaryDivision primaryDivision)
        {
            var countryField = new Field("Country", primaryDivision.Country.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);

            doc.Add(countryField);
        }

        private void AddUpdateSecondaryDivisionDoc(Document doc, SecondaryDivision secondaryDivision)
        {
            var countryField = new Field("Country", secondaryDivision.Country.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var primaryField = new Field("PrimaryDivision", secondaryDivision.PrimaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);

            doc.Add(countryField);
            doc.Add(primaryField);
        }

        private void AddUpdateTertiaryDivisionDoc(Document doc, TertiaryDivision tertiaryDivision)
        {
            var countryField = new Field("Country", tertiaryDivision.Country.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var primaryField = new Field("PrimaryDivision", tertiaryDivision.PrimaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var secondaryField = new Field("SecondaryDivision", tertiaryDivision.SecondaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);

            doc.Add(countryField);
            doc.Add(primaryField);
            doc.Add(secondaryField);
        }

        private void AddUpdateQuaternaryDivisionDoc(Document doc, QuaternaryDivision quaternaryDivision)
        {
            var countryField = new Field("Country", quaternaryDivision.Country.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var primaryField = new Field("PrimaryDivision", quaternaryDivision.PrimaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var secondaryField = new Field("SecondaryDivision", quaternaryDivision.SecondaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);
            var tertiaryField = new Field("TertiaryDivision", quaternaryDivision.TertiaryDivision.Name, Field.Store.YES, Field.Index.NOT_ANALYZED);

            doc.Add(countryField);
            doc.Add(primaryField);
            doc.Add(secondaryField);
            doc.Add(tertiaryField);
        }
    }
}