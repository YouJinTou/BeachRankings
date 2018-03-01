namespace BeachRankings.App.CustomAttributes
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class BeachValidAttribute : ValidationAttribute
    {
        private IBeachRankingsData data;
        private Country country;

        public BeachValidAttribute()
        {
            this.data = DependencyResolver.Current.GetService<IBeachRankingsData>();
        }

        private void InitializeCachedValues(IAddEditBeachModel model)
        {
            this.country = this.data.Countries.All()
                .Include(c => c.PrimaryDivisions)
                .Include(c => c.SecondaryDivisions)
                .Include(c => c.TertiaryDivisions)
                .Include(c => c.QuaternaryDivisions)
                .FirstOrDefault(c => c.Id == model.CountryId);
        }

        public override bool IsValid(object value)
        {
            var model = (IAddEditBeachModel)value;

            this.InitializeCachedValues(model);

            if (!this.DivisionsValid(model))
            {
                this.ErrorMessage = "Some of the regions selected do not match.";

                return false;
            }

            if (!this.EnglishAlphabetUsed(model))
            {
                this.ErrorMessage = "Use only letters in the English alphabet.";

                return false;
            }

            if (this.PrimaryIdMissing(model))
            {
                this.ErrorMessage = "The Region field is required.";

                return false;
            }

            if (this.SecondaryIdMissing(model))
            {
                this.ErrorMessage = "The Area field is required.";

                return false;
            }

            if (this.TertiaryIdMissing(model))
            {
                this.ErrorMessage = "The Sub-area field is required.";

                return false;
            }

            if (this.QuaternaryIdMissing(model))
            {
                this.ErrorMessage = "The Locality field is required.";

                return false;
            }

            if (!this.BeachNameUnique(model))
            {
                this.ErrorMessage = "A beach with this name already exists.";

                return false;
            }

            return true;
        }

        private bool EnglishAlphabetUsed(IAddEditBeachModel model)
        {
            var englishAlphabetUsed = Regex.IsMatch(model.Name, @"^[A-Za-z0-9\s-]+$");

            return englishAlphabetUsed;
        }

        private bool PrimaryIdMissing(IAddEditBeachModel model)
        {
            var primaryDivisionExists = (this.country?.PrimaryDivisions.Count > 0);
            var primaryIdMissing = (primaryDivisionExists && model.PrimaryDivisionId == null);

            return primaryIdMissing;
        }

        private bool SecondaryIdMissing(IAddEditBeachModel model)
        {
            var primaryDivision = this.data.PrimaryDivisions.All()
                .Include(pd => pd.SecondaryDivisions)
                .FirstOrDefault(pd => pd.Id == model.PrimaryDivisionId);
            var secondaryDivisionsExist = (primaryDivision.SecondaryDivisions.Count > 0);
            var secondaryIdMissing = (secondaryDivisionsExist && model.SecondaryDivisionId == null);

            return secondaryIdMissing;
        }

        private bool TertiaryIdMissing(IAddEditBeachModel model)
        {
            var secondaryDivision = this.data.SecondaryDivisions.All()
                .Include(sd => sd.TertiaryDivisions)
                .FirstOrDefault(sd => sd.Id == model.SecondaryDivisionId);
            var tertiaryDivisionsExist = (secondaryDivision?.TertiaryDivisions.Count > 0);
            var tertiaryIdMissing = (tertiaryDivisionsExist && model.TertiaryDivisionId == null);

            return tertiaryIdMissing;
        }

        private bool QuaternaryIdMissing(IAddEditBeachModel model)
        {
            var tertiaryDivision = this.data.TertiaryDivisions.All()
                .Include(td => td.QuaternaryDivisions)
                .FirstOrDefault(td => td.Id == model.TertiaryDivisionId);
            var quaternaryDivisionsExist = (tertiaryDivision?.QuaternaryDivisions.Count > 0);
            var quaternaryIdMissing = (quaternaryDivisionsExist && model.QuaternaryDivisionId == null);

            return quaternaryIdMissing;
        }

        private bool DivisionsValid(IAddEditBeachModel model)
        {
            var primaryDivision = this.country?.PrimaryDivisions
                .FirstOrDefault(pd => pd.Id == model.PrimaryDivisionId);
            var secondaryDivision = primaryDivision?.SecondaryDivisions
                .FirstOrDefault(sd => sd.Id == model.SecondaryDivisionId);
            var tertiaryDivision = secondaryDivision?.TertiaryDivisions
                .FirstOrDefault(td => td.Id == model.TertiaryDivisionId);
            var quaternaryDivision = tertiaryDivision?.QuaternaryDivisions
                .FirstOrDefault(qd => qd.Id == model.QuaternaryDivisionId);
            var primaryDivisionValid = model.PrimaryDivisionId == null ? 
                (primaryDivision == null) : 
                (primaryDivision != null);
            var secondaryDivisionValid = model.SecondaryDivisionId == null ?
                (secondaryDivision == null) :
                (secondaryDivision != null);
            var tertiaryDivisionValid = model.TertiaryDivisionId == null ?
                (tertiaryDivision == null) :
                (tertiaryDivision != null);
            var quaternaryDivisionValid = model.QuaternaryDivisionId == null ?
                (quaternaryDivision == null) :
                (quaternaryDivision != null);

            return 
                primaryDivisionValid && 
                secondaryDivisionValid && 
                tertiaryDivisionValid && 
                quaternaryDivisionValid;
        }

        private bool BeachNameUnique(IAddEditBeachModel model)
        {
            var isAdd = (model.Id == 0);
            var currentBeach = isAdd ? null : this.data.Beaches.Find(model.Id);
            var namesMatch = model.Name.ToLower() == currentBeach?.Name.ToLower();
            var sameBeachNamesCount = this.data.Beaches.All().Where(b => 
                b.CountryId == model.CountryId &&
                b.PrimaryDivisionId == model.PrimaryDivisionId &&
                b.SecondaryDivisionId == model.SecondaryDivisionId &&
                b.TertiaryDivisionId == model.TertiaryDivisionId &&
                b.QuaternaryDivisionId == model.QuaternaryDivisionId &&
                b.Name.ToLower() == model.Name.ToLower())
                .ToList()
                .Count;
            var beachNameUnique =
                (isAdd && sameBeachNamesCount == 0) ||
                (!isAdd && namesMatch && sameBeachNamesCount == 1) ||
                (!isAdd && !namesMatch && sameBeachNamesCount == 0);

            return beachNameUnique;
        }
    }
}