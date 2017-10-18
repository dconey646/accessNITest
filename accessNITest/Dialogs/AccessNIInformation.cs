using System;

namespace accessNITest.Dialogs
{
    [Serializable]
    internal class AccessNIInformation
    {
        public string Title { get; set; }

        public string UsuallyKnownByName { get; set; }

        public string PlaceOfBirth { get; set; }

        public string Country { get; set; }

        public string Nationality { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NINumber { get; set; }

        public long ContactNumber { get; set; }

        public string Address { get; set; }

        public int DriverLicenseNumber { get; set; }

        public int PassportNumber { get; set; }

        public string PreviousAddress { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}