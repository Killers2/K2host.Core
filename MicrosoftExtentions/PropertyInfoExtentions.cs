/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-03-26                        | 
'| Use: General                                         |
' \====================================================/
*/

namespace System.Reflection
{
   
    public static class PropertyInfoExtentions
    {

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <typeparam name="T">The property value of the specified object.</typeparam>
        /// <param name="e"></param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns></returns>
        public static T GetValue<T>(this PropertyInfo e, object obj) => (T)e.GetValue(obj);


    }

}
