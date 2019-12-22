namespace BR.Seed
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Seed
    {

        private SeedContinent[] continentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Continent")]
        public SeedContinent[] Continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinent
    {

        private SeedContinentCountry[] countryField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Country")]
        public SeedContinentCountry[] Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinentCountry
    {

        private SeedContinentCountryL1[] l1Field;

        private string nameField;

        private string continentField;

        private string waterBodyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("L1")]
        public SeedContinentCountryL1[] L1
        {
            get
            {
                return this.l1Field;
            }
            set
            {
                this.l1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string waterBody
        {
            get
            {
                return this.waterBodyField;
            }
            set
            {
                this.waterBodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinentCountryL1
    {

        private SeedContinentCountryL1L2[] l2Field;

        private string nameField;

        private string continentField;

        private string countryField;

        private string waterBodyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("L2")]
        public SeedContinentCountryL1L2[] L2
        {
            get
            {
                return this.l2Field;
            }
            set
            {
                this.l2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string waterBody
        {
            get
            {
                return this.waterBodyField;
            }
            set
            {
                this.waterBodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinentCountryL1L2
    {

        private SeedContinentCountryL1L2L3[] l3Field;

        private string nameField;

        private string continentField;

        private string countryField;

        private string l1Field;

        private string waterBodyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("L3")]
        public SeedContinentCountryL1L2L3[] L3
        {
            get
            {
                return this.l3Field;
            }
            set
            {
                this.l3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L1
        {
            get
            {
                return this.l1Field;
            }
            set
            {
                this.l1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string waterBody
        {
            get
            {
                return this.waterBodyField;
            }
            set
            {
                this.waterBodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinentCountryL1L2L3
    {

        private SeedContinentCountryL1L2L3L4[] l4Field;

        private string nameField;

        private string continentField;

        private string countryField;

        private string l1Field;

        private string l2Field;

        private string waterBodyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("L4")]
        public SeedContinentCountryL1L2L3L4[] L4
        {
            get
            {
                return this.l4Field;
            }
            set
            {
                this.l4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L1
        {
            get
            {
                return this.l1Field;
            }
            set
            {
                this.l1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L2
        {
            get
            {
                return this.l2Field;
            }
            set
            {
                this.l2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string waterBody
        {
            get
            {
                return this.waterBodyField;
            }
            set
            {
                this.waterBodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SeedContinentCountryL1L2L3L4
    {

        private string nameField;

        private string continentField;

        private string countryField;

        private string l1Field;

        private string l2Field;

        private string l3Field;

        private string waterBodyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string continent
        {
            get
            {
                return this.continentField;
            }
            set
            {
                this.continentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L1
        {
            get
            {
                return this.l1Field;
            }
            set
            {
                this.l1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L2
        {
            get
            {
                return this.l2Field;
            }
            set
            {
                this.l2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string L3
        {
            get
            {
                return this.l3Field;
            }
            set
            {
                this.l3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string waterBody
        {
            get
            {
                return this.waterBodyField;
            }
            set
            {
                this.waterBodyField = value;
            }
        }
    }
}
