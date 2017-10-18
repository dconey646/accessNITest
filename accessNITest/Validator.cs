using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace accessNITest
{
    public class Validator
    {
        public DateTime? CheckDOB(string inputtedDate)
        {
            try
            {
                return DateTime.Parse(inputtedDate);
            }
            catch
            {
                return null;
            }
        }

        public bool CheckNINumber(string inputtedNINumber)
        {
            string niNumberRegExp = "^([a-zA-Z]){2}( )?([0-9]){2}( )?([0-9]){2}( )?([0-9]){2}( )?([a-zA-Z]){1}?$";
            Regex r = new Regex(niNumberRegExp, RegexOptions.IgnoreCase);
            Match m = r.Match(inputtedNINumber);
            if(m.Success)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public bool CheckAddress(string postCode, int houseNumber)
        {
            //Make call to public api for address.
            //Send string back if possible
            //If 404, return false;
            return true;
        }

        public bool CheckPhoneNumber()
        {
            string phoneNumRegExp = "^(+44s ? 7d{ 3}|(? 07d{ 3})?)s ?d{ 3}s ?d{ 3}$";

            return true;
        }

        public bool CheckDriverLicenseNumber()
        {
            // 8 or 16
            return true;
        }

        public bool CheckPassportNumber()
        {
            //9 digits
            return true;
        }
    }
}