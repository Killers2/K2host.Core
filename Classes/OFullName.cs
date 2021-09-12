/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;
using System.Globalization;

using K2host.Core.Enums;

namespace K2host.Core.Classes
{

    public class OFullName : IDisposable
    {

        #region Properties

        public string Title { get; private set; }

        public string Firstname { get; private set; }

        public string Surname { get; private set; }

        #endregion

        #region Constuctor

        public OFullName(string fullName)
        {

            List<string> FullName = new();

            FullName.AddRange(
                fullName.Split(new char[] { ' ' })
            );

            Title = ParseTitle(FullName[0]);

            if (Title == string.Empty)
                Firstname = FullName[0];
            else
                Firstname = FullName[1];

            Surname = FullName[^1];

            if ((Firstname == Surname) && (Title != string.Empty))
                Firstname = string.Empty;

            if ((Firstname == Surname) && (Title == string.Empty))
                Surname = string.Empty;

            TextInfo Ci = new CultureInfo("en-US", false).TextInfo;

            Title       = Ci.ToTitleCase(Title);
            Firstname   = Ci.ToTitleCase(Firstname);
            Surname     = Ci.ToTitleCase(Surname);

            FullName.Clear();

        }

        #endregion

        #region Methods

        private static string ParseTitle(string item)
        {

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("mr"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("master"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("mrs"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("miss"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("ms"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("dr"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("sir"))
                return item;

            if (item.ToLower().Trim().Replace(" ", string.Empty).Contains("prof"))
                return item;

            return string.Empty;
        }
        
        public OGenderStatus GetGender()
        {

            if (Title.ToLower().Contains("mr") || Title.ToLower().Contains("master"))
                return OGenderStatus.Male;
            else
                return OGenderStatus.Female;

        }

        public static OGenderStatus GetGender(string title)
        {

            if (title.ToLower().Contains("mr") || title.ToLower().Contains("master"))
                return OGenderStatus.Male;
            else
                return OGenderStatus.Female;

        }
       
        #endregion

        #region Shared

        public static OFullName Parse(string fullName)
        {
            return new OFullName(fullName);
        }

        #endregion
      
        #region Destructor

        bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {


                }
            }
            _isDisposed = true;
        }

        #endregion

    }

}
