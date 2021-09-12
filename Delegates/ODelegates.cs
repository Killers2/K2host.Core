/*
' /====================================================\
'| Developed Tony N. Hyde (k2host.co.uk).               |
'| Founder of ConsoleX                                  |
'| Application:                                         |
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
