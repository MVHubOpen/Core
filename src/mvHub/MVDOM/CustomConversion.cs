namespace mvHub
{
    public abstract class MvCustomConversion
    {
        public MvCustomConversion()
        {
        }

        /// <summary>
        /// Conversion Code Name
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Conversion Code Safe Name {Upper Cased}
        /// </summary>
        public virtual string SafeName => Name.ToUpper();

        /// <summary>
        /// The conversion process to convert data string into internal format
        /// </summary>
        public abstract string Conv(string value);
        /// <summary>
        /// The cconversion process to convert data string into external format
        /// </summary>
        public abstract string OConv(string value);

        protected abstract string About();
        public virtual string About(string pad)
        {
            return pad + About();
        }
        public override string ToString()
        { return About(); }

    }

}
