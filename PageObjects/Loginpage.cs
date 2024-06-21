using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCSharpFramework.PageObjects
{
    public class Loginpage
    {
        private IWebDriver driver;
        public Loginpage(IWebDriver driver) 
        {

            /*We need to register all the webelements and objects with driver and initElement method will register all objects with this driver*/ 
            this.driver = driver;
            PageFactory.InitElements(driver, this);

        
        }

        /*To define the locator we will follow the page object factory, to access pageobject factory we need 
         to install page object factory package (page objects. page object core)*/

        /*all the locators are private so that no one can change their value from outside , therefore through methods (objectoftheclass.methodName) we will
         return the value-- accessing objects through methods --this is encapsulation*/

        [FindsBy(How = How.XPath, Using = "//input[@id='username']")]
        private IWebElement username;

        [FindsBy(How = How.XPath, Using = "//input[@id='password']")]
        private IWebElement password;

        [FindsBy(How=How.XPath,Using = "//select[@class='form-control']")]
        private IWebElement role;

        [FindsBy(How = How.XPath, Using = "//input[@id='signInBtn']")]
        private IWebElement signin;

        [FindsBy(How = How.Id, Using = "usertype")]
         private  IList<IWebElement> usertype;

        [FindsBy(How = How.Id, Using = "okayBtn")]
        private IWebElement okButton;

        //By usertype = By.Id("usertype");

        public IWebElement getUsername()
        {
            return username;
        }

        //returning product details page so that in the flow we don't need to create productdetails object
        public ProductDetails ValidLogin(String usernamevalue, String passwordvalue,String userrole)
        {
            username.SendKeys(usernamevalue);
            password.SendKeys(passwordvalue);

            foreach (IWebElement type in usertype)
            {
                if (type.GetAttribute("value").Equals("user"))
                {
                    type.Click();
                    break;
                   
                }


            }


            WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait1.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(okButton));
            okButton.Click();

            SelectElement select = new SelectElement(role);
            select.SelectByText(userrole);

            signin.Click();

           return new ProductDetails(driver);
        }



    }
}
