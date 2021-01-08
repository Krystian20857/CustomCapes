using System;

namespace CustomCapes.Internal.Config {

    public class User {

        #region Fields

        #endregion

        #region Properties

        public Guid UUID { get; set; }
        public string Username { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public override bool Equals(object obj) {
            if (!(obj is User user))
                return false;
            return UUID.Equals(user.UUID) && Username.Equals(user.Username);
        }

        public override int GetHashCode() {
            return UUID.GetHashCode() ^ Username.GetHashCode();
        }

        #endregion

    }

}