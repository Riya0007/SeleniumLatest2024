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
    public class ProductDetails
    {

        IWebDriver driver;
        public ProductDetails(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver,this);


        }

        [FindsBy(How = How.PartialLinkText, Using = "Checkout")]
        [CacheLookup]
        private IWebElement checkoutButton;

        [FindsBy(How = How.XPath, Using = "//tbody//*[text()='Total']//following::strong[text()='₹. 0']")]
        private IWebElement zeroTotal;

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,\"-cart\")]")]
        private IWebElement continueShopping;

        [FindsBy(How = How.TagName, Using = "app-card")]
        private IList<IWebElement> products;

        By cardTitle = By.CssSelector(".card-title a");
        By cardFooter = By.CssSelector(".card-footer button");



        public void WaitForPageToDisplay()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("shop"));
        }


        public void addProduct()
        {

            WaitForPageToDisplay();
           string title = driver.Url;
            Assert.That(title, Is.EqualTo("https://rahulshettyacademy.com/angularpractice/shop"));



            string[] phoneName = { "Nokia Edge" , "iphone X" };

          
            foreach (IWebElement product in products)
            {
                
                string mobileName = product.FindElement(cardTitle).Text;
                TestContext.Progress.WriteLine(mobileName);

                if (phoneName.Contains(mobileName))
                {
                    product.FindElement(cardFooter).Click();
                }
            }

            checkoutButton.Click();

        }



    }
}
