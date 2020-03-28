using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MintTransactionsDownloader
{
    class Class1
    {
        public void login()
        {
            //Run selenium
            ChromeDriver cd = new ChromeDriver(@"chromedriver_win32");
            cd.Url =
                HttpUtility.UrlEncode(
                    @"https://accounts.intuit.com/index.html" 
                    + "?"
                    + "offering_id" + "=" + "Intuit.ifs.mint"
                    + "&"
                    + "namespace_id" + "=" + "50000026"
                    + "&"
                    //+ "redirect_url" + "=" + "https%3A%2F%2Fmint.intuit.com%2Foverview.event%3Futm_medium%3Ddirect%26cta%3Dnav_login_dropdown%26ivid%3D94378575-ca6a-4af6-88a2-fce191c0a46e%26adobe_mc%3DMCMID%253D05894163352002821093324041646152498466%257CMCAID%253D2EC55E9085031488-40001198E0000414%257CMCORGID%253D969430F0543F253D0A4C98C6%252540AdobeOrg%257CTS%253D1572063926%26ivid%3D94378575-ca6a-4af6-88a2-fce191c0a46e";
                    + "redirect_url" + "=" + "https://mint.intuit.com/overview.event?utm_medium=direct"
                    + "&"
                    + "cta" + "=" + "nav_login_dropdown"
                    + "&"
                    + "ivid" + "=" + "94378575-ca6a-4af6-88a2-fce191c0a46e"
                    + "&"
                    //+ "adobe_mc" + "=" + "MCMID%3D05894163352002821093324041646152498466%7CMCAID%3D2EC55E9085031488-40001198E0000414%7CMCORGID%3D969430F0543F253D0A4C98C6%2540AdobeOrg%7CTS%3D1572063926"
                    + "adobe_mc" + "=" 
                        + "MCMID" + "=" + "05894163352002821093324041646152498466"
                        + "|" 
                        + "MCAID" + "=" + "2EC55E9085031488-40001198E0000414"
                        + "|"
                        + "MCORGID" + "=" + "969430F0543F253D0A4C98C6@AdobeOrg"
                        + "|"
                        + "TS" + "=" + "1572063926"
                    + "&"
                    + "ivid" + "=" + "94378575-ca6a-4af6-88a2-fce191c0a46e");

            cd.Navigate();
            IWebElement e = cd.FindElementById("ius-userid");
            e.SendKeys("EMAILHERE");
            e = cd.FindElementById("ius-password");
            e.SendKeys("PASSWORDHERE");
            e = cd.FindElementByXPath(@"//*[@id=""ius-sign-in-submit-btn""]");
            e.Click();

            //Get the cookies
            CookieContainer cc = new CookieContainer();
            foreach (OpenQA.Selenium.Cookie c in cd.Manage().Cookies.AllCookies)
            {
                string name = c.Name;
                string value = c.Value;
                cc.Add(new System.Net.Cookie(name, value, c.Path, c.Domain));
            }

            //Fire off the request
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create("https://fif.com/components/com_fif/tools/capacity/values/");
            hwr.CookieContainer = cc;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            StreamWriter swr = new StreamWriter(hwr.GetRequestStream());
            swr.Write("feeds=35");
            swr.Close();

            WebResponse wr = hwr.GetResponse();
            string s = new System.IO.StreamReader(wr.GetResponseStream()).ReadToEnd();
        }
    }
}
