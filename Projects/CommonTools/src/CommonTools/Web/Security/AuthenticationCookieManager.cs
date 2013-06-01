using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web.Security;
using System.Web;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using CommonTools.Xml;

namespace CommonTools.Web.Security
{
    /// <summary>
    /// This manager class offers helper methods for manipulating the .NET forms authentication cookie. They offer an easy
    /// interface for dynamically assigning authentication timeouts and embedding custom objects within the cookie.
    /// </summary>
    public static class AuthenticationCookieManager
    {
        #region private members
        internal enum CookieGenerationMode { RefreshExisting, CreateNew, UpdateExisting }
        #endregion

        #region private static methods
        /// <summary>
        /// Generates the authentication cookie.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        /// <param name="isPersistent">The is persistent.</param>
        /// <param name="cookieGenerationMode">The cookie generation mode.</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        /// <returns></returns>
        internal static HttpCookie GenerateAuthenticationCookie<T>(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds
            , T userObject, bool? isPersistent, CookieGenerationMode cookieGenerationMode, string domain) where T : IXmlSerializable
        {
            HttpCookie cookie = null;

            switch (cookieGenerationMode)
            {
                case CookieGenerationMode.RefreshExisting:
                case CookieGenerationMode.UpdateExisting:
                    cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    break;
                case CookieGenerationMode.CreateNew:
                    cookie = FormsAuthentication.GetAuthCookie(username, isPersistent ?? false);
                    break;
            }

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (!isPersistent.HasValue)
                isPersistent = ticket.IsPersistent;

            DateTime expires = isPersistent.Value ? DateTime.Now.AddSeconds(persistentAuthenticationTimeoutInSeconds)
                                                  : DateTime.Now.AddSeconds(authenticationTimeoutInSeconds);

            string userData = string.Empty;

            switch (cookieGenerationMode)
            {
                case CookieGenerationMode.RefreshExisting: userData = ticket.UserData; break;

                case CookieGenerationMode.CreateNew:
                case CookieGenerationMode.UpdateExisting: userData = XmlSerializationHelper<T>.ConvertToXml(userObject); break;
            }

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                2 // only microsoft knows what the version flag means. 2 is default, 1 works as well...
                , ticket.Name
                , ticket.IssueDate
                , expires
                , isPersistent.Value
                , userData);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);

            if (isPersistent.Value)
                cookie.Expires = expires;

            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;

            return cookie;
        }
        /// <summary>
        /// Generates the authentication cookie.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="isPersistent">The is persistent.</param>
        /// <param name="refreshExisting">if set to <c>true</c> [refresh existing].</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        /// <returns></returns>
        internal static HttpCookie GenerateAuthenticationCookie(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds
            , bool? isPersistent, bool refreshExisting, string domain)
        {
            HttpCookie cookie = null;

            if (refreshExisting)
            {
                cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            }
            else
            {
                cookie = FormsAuthentication.GetAuthCookie(username, isPersistent ?? false);
            }

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (!isPersistent.HasValue)
                isPersistent = ticket.IsPersistent;

            DateTime expires = isPersistent.Value ? DateTime.Now.AddSeconds(persistentAuthenticationTimeoutInSeconds)
                                                  : DateTime.Now.AddSeconds(authenticationTimeoutInSeconds);

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                2 // only microsoft knows what the version flag means. 2 is default, 1 works as well...
                , ticket.Name
                , ticket.IssueDate
                , expires
                , isPersistent.Value
                , ticket.UserData);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);

            if (isPersistent.Value)
                cookie.Expires = expires;

            if (!string.IsNullOrEmpty(domain))
                cookie.Domain = domain;

            return cookie;
        }
        #endregion

        #region public static methods

        #region refresh cookies
        /// <summary>
        /// Refreshes the authentication cookie.
        /// </summary>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        public static void RefreshAuthenticationCookie(int authenticationTimeoutInSeconds)
        {
            RefreshAuthenticationCookie(authenticationTimeoutInSeconds, authenticationTimeoutInSeconds);
        }
        /// <summary>
        /// Refreshes the authentication cookie.
        /// </summary>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        public static void RefreshAuthenticationCookie(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, string domain)
        {
            IPrincipal user = HttpContext.Current.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                try
                {
                    HttpContext.Current.Response.Cookies.Add(
                        GenerateAuthenticationCookie(
                            null
                            , authenticationTimeoutInSeconds
                            , persistentAuthenticationTimeoutInSeconds
                            , null
                            , true
                            , domain));
                }
                catch (Exception)
                {
                    // there was an error, so log the user out
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                    cookie.Expires = DateTime.MinValue;

                    if (!string.IsNullOrEmpty(domain))
                        cookie.Domain = domain;

                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        /// <summary>
        /// Refreshes the current authentication cookie. The userObject values will not be set, if you want to set the userObject values, use <![CDATA[UpdateAuthenticationCookie<T>]]> instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        public static void RefreshAuthenticationCookie<T>(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, T userObject, string domain)
            where T : IXmlSerializable
        {
            IPrincipal user = HttpContext.Current.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                try
                {
                    HttpContext.Current.Response.Cookies.Add(GenerateAuthenticationCookie<T>(null, authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds
                        , userObject, null, CookieGenerationMode.RefreshExisting, domain));
                }
                catch (Exception)
                {
                    // there was an error, so log the user out
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                    cookie.Expires = DateTime.MinValue;

                    if (!string.IsNullOrEmpty(domain))
                        cookie.Domain = domain;

                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Refreshes the authentication cookie.
        /// </summary>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        public static void RefreshAuthenticationCookie(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds)
        {
            RefreshAuthenticationCookie(authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds, null);
        }
        /// <summary>
        /// Refreshes the current authentication cookie. The userObject values will not be set, if you want to set the userObject values, use <![CDATA[UpdateAuthenticationCookie<T>]]> instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        public static void RefreshAuthenticationCookie<T>(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, T userObject)
            where T : IXmlSerializable
        {
            RefreshAuthenticationCookie<T>(authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds, userObject, null);
        }
        #endregion
        
        #region update cookies
        /// <summary>
        /// Refreshes and updates the current authentication cookie with the new userObject values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        public static void UpdateAuthenticationCookie<T>(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, T userObject, string domain)
            where T : IXmlSerializable
        {
            IPrincipal user = HttpContext.Current.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                try
                {
                    HttpContext.Current.Response.Cookies.Add(GenerateAuthenticationCookie<T>(null, authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds
                        , userObject, null, CookieGenerationMode.UpdateExisting, domain));
                }
                catch (Exception)
                {
                    // there was an error, so log the user out
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                    cookie.Expires = DateTime.MinValue;

                    if (!string.IsNullOrEmpty(domain))
                        cookie.Domain = domain;

                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        /// <summary>
        /// Refreshes and updates the current authentication cookie with the new userObject values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        public static void UpdateAuthenticationCookie<T>(int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, T userObject)
            where T : IXmlSerializable
        {
            UpdateAuthenticationCookie<T>(authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds, userObject, null);
        }
        #endregion

        #region write cookies
        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        public static void WriteAuthenticationCookie(string username, int authenticationTimeoutInSeconds)
        {
            WriteAuthenticationCookie(username, authenticationTimeoutInSeconds, authenticationTimeoutInSeconds);
        }
        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        public static void WriteAuthenticationCookie(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds)
        {
            WriteAuthenticationCookie(username, authenticationTimeoutInSeconds, authenticationTimeoutInSeconds, null);
        }
        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        public static void WriteAuthenticationCookie(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds
            , bool? isPersistent, string domain)
        {
            HttpContext.Current.Response.Cookies.Add(
                GenerateAuthenticationCookie(
                    username
                    , authenticationTimeoutInSeconds
                    , persistentAuthenticationTimeoutInSeconds
                    , isPersistent
                    , false
                    , domain));
        }
        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        /// <param name="isPersistent">The is persistent.</param>
        /// <param name="domain">The domain to share the cookie in. Set this to null if you don't want to share the cookie in a domain.</param>
        public static void WriteAuthenticationCookie<T>(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds
            , T userObject, bool? isPersistent, string domain) where T : IXmlSerializable
        {
            HttpContext.Current.Response.Cookies.Add(GenerateAuthenticationCookie(username, authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds
                , userObject, isPersistent, CookieGenerationMode.CreateNew, domain));
        }


        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="isPersistent">if set to <c>true</c> [is persistent].</param>
        public static void WriteAuthenticationCookie(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds, bool? isPersistent)
        {
            WriteAuthenticationCookie(username, authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds, isPersistent, string.Empty);
        }
        /// <summary>
        /// Writes the authentication cookie.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="username">The username.</param>
        /// <param name="authenticationTimeoutInSeconds">The authentication timeout in seconds.</param>
        /// <param name="persistentAuthenticationTimeoutInSeconds">The persistent authentication timeout in seconds.</param>
        /// <param name="userObject">The user object.</param>
        /// <param name="isPersistent">The is persistent.</param>
        public static void WriteAuthenticationCookie<T>(string username, int authenticationTimeoutInSeconds, int persistentAuthenticationTimeoutInSeconds
            , T userObject, bool? isPersistent) where T : IXmlSerializable
        {
            WriteAuthenticationCookie<T>(username, authenticationTimeoutInSeconds, persistentAuthenticationTimeoutInSeconds, userObject, isPersistent, string.Empty);
        }
        #endregion

        #region read cookie
        /// <summary>
        /// Gets the embedded data from authentication cookie.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEmbeddedDataFromAuthenticationCookie<T>()
        {
            HttpContext currentContext = HttpContext.Current;
            IPrincipal user = currentContext.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                return XmlSerializationHelper<T>.ConvertFromXml(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData);
            }

            return default(T);
        }
        #endregion

        #region signout
        /// <summary>
        /// Signouts this instance.
        /// </summary>
        public static void Signout()
        {
            HttpContext context = HttpContext.Current;
            if (context.User.Identity.IsAuthenticated)
            {
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(context.User.Identity.Name, false);
                cookie.Expires = DateTime.MinValue;
                context.Response.Cookies.Remove(cookie.Name);
                context.Response.Cookies.Add(cookie);
            }
            FormsAuthentication.SignOut();
        }
        /// <summary>
        /// Signouts the and redirect to login.
        /// </summary>
        public static void SignoutAndRedirectToLogin()
        {
            Signout();
            FormsAuthentication.RedirectToLoginPage();
        }
        #endregion

        #endregion
    }
}
