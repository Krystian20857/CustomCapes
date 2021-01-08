using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace CustomCapes.Validator {

    public class FileValidator : ValidationRule {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            if (value == null)
                return new ValidationResult(false, "File cannot be empty.");
            return new ValidationResult(File.Exists(value.ToString()), "Cannot find file.");
        }

        #endregion
        
    }

}