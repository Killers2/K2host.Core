/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

using System.Net.Mail;

namespace K2host.Core.Delegates
{

    public delegate void OServiceMethod(object e);
   
    public delegate object OServiceFunction(object e);

    public delegate void OnAddAttacmentsEvent(MailMessage e);


}
