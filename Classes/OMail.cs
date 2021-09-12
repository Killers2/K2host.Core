/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2018-10-30                        | 
'| Use: General                                         |
' \====================================================/
*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

using K2host.Core.Delegates;

namespace K2host.Core.Classes
{

    public class OMail : IDisposable
    {

        public class Setup : IDisposable
        {

            public string From              = string.Empty;
            public string To                = string.Empty;
            public string Bcc               = string.Empty;
            public string Cc                = string.Empty;
            public List<string> Attachments = null;
            public string Subject           = string.Empty;
            public string Message           = string.Empty;
            public bool IsHtml              = false;
            public MailPriority Priority    = MailPriority.Normal;
            public bool EnableSSL           = false;
            public int PortNumber           = 25;

            public Setup()
            {
                Attachments = new List<string>();
            }

            #region Deconstuctor

            private bool IsDisposed = false;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!IsDisposed)
                    if (disposing)
                    {




                    }
                IsDisposed = true;
            }

            #endregion

        }

        public OnAddAttacmentsEvent OnAddMoreAttacments;

        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public OMail()
        {

        }

        public OMail(string server, string username, string password)
        {
            Server      = server;
            Username    = username;
            Password    = password;
        }     

        public bool Send(Setup e)
        {

            bool result = false;

            try
            {

                MailMessage m = new() { From = new MailAddress(e.From) };

                if (e.To.Contains(",") || e.To.Contains(";"))
                    e.To.Split(new char[] { ',', ';' }).ForEach(addr => { m.To.Add(addr); });
                else
                    m.To.Add(e.To);

                if (e.Bcc.Length != 0)
                    m.Bcc.Add(e.Bcc);

                if (e.Cc.Length != 0)
                    m.CC.Add(e.Cc);

                if (e.Attachments.Count > 0)
                    e.Attachments.ForEach(file => { m.Attachments.Add(new Attachment(file)); });

                OnAddMoreAttacments?.Invoke(m);

                m.Subject           = e.Subject;
                m.Body              = e.Message;
                m.IsBodyHtml        = e.IsHtml;
                m.Priority          = e.Priority;
                SmtpClient s        = new(Server);
                NetworkCredential c = new(Username, Password);
                s.EnableSsl         = e.EnableSSL;
                s.Port              = e.PortNumber;

                if (e.EnableSSL && e.PortNumber == 25)
                    s.Port = 465;

                s.DeliveryMethod        = SmtpDeliveryMethod.Network;
                s.UseDefaultCredentials = false;
                s.Credentials           = c;

                s.Send(m);

                result = true;

            }
            catch 
            {
                result = false;
            }

            return result;
        }

        public bool Send(Setup e, out Exception error) 
        {

            error = null;
            bool result = false;

            try
            {

                MailMessage m = new() { From = new MailAddress(e.From) };

                if (e.To.Contains(",") || e.To.Contains(";"))
                    e.To.Split(new char[] { ',', ';' }).ForEach(addr => { m.To.Add(addr); });
                else
                    m.To.Add(e.To);

                if (e.Bcc.Length != 0)
                    m.Bcc.Add(e.Bcc);

                if (e.Cc.Length != 0)
                    m.CC.Add(e.Cc);

                if (e.Attachments.Count > 0)
                    e.Attachments.ForEach(file => { m.Attachments.Add(new Attachment(file)); });

                OnAddMoreAttacments?.Invoke(m);

                m.Subject           = e.Subject;
                m.Body              = e.Message;
                m.IsBodyHtml        = e.IsHtml;
                m.Priority          = e.Priority;
                SmtpClient s        = new(Server);
                NetworkCredential c = new(Username, Password);
                s.EnableSsl         = e.EnableSSL;
                s.Port              = e.PortNumber;

                if (e.EnableSSL && e.PortNumber == 25)
                    s.Port = 465;

                s.DeliveryMethod        = SmtpDeliveryMethod.Network;
                s.UseDefaultCredentials = false;
                s.Credentials           = c;

                s.Send(m);

                result = true;

            }
            catch(Exception ex)
            {
                error = ex;
                result = false;
            }

            return result;

        }

        #region Deconstuctor

        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
                if (disposing)
                {




                }
            IsDisposed = true;
        }

        #endregion

    }


}
