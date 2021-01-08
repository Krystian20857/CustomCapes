using System.Globalization;
using System.Windows.Controls;

namespace CustomCapes.Validator {

    public class UserValidator : ValidationRule {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            if (value == null)
                return new ValidationResult(false, "User cannot be empty.");
            var valueString = value.ToString();
            var userManager = Bootstrapper.Instance.UserManager;
            return new ValidationResult(!userManager.Exists(valueString), "User already exists.");
        }

        #endregion
        
    }

}