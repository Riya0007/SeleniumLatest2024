using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumCSharpFramework.test
{


    /*With out Page Object Model*/
    /*This class will handle selecting radio buttons*/
    public class SelectClass
    {

        IWebDriver driver;
       [SetUp]
        public void Setup()
        {
            /*chromedriver.exe file is needed as selenium cannot directly talk to browser drivers , 
            all the commands with be passed to chromedriver.exe,
            WebDriverManager will do all the work here*/

            /*WebDriverManager -- Namespace
             DriverManager -- Class
            SetUpDriver--- Method*/

         // new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
          driver = new ChromeDriver();
          driver.Manage().Window.Maximize();
          TestContext.Progress.WriteLine("Set up step is executed succesfully");
          driver.Url = "https://rahulshettyacademy.com/dropdownsPractise/";

            //https://rahulshettyacademy.com/loginpagePractise/
        }

        [Test]
        public void Dropdown()
        {
           
           String title = driver.Title;
           TestContext.Progress.WriteLine("In Test 1");
           TestContext.Progress.WriteLine("Title is :"+ title);

            By depatureLocator = By.XPath("//input[@id='ctl00_mainContent_ddl_originStation1_CTXT']");
            By depatureLocator1 = By.XPath("//select[@name='ctl00$mainContent$ddl_originStation1']");
            By arrivalLocator = By.XPath("//input[@id='ctl00_mainContent_ddl_destinationStation1_CTXT']");
            By country = By.XPath("//*[contains(text(),'Country')]");

            WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(country));


            IWebElement departureCity = driver.FindElement(depatureLocator);
            IWebElement departureCity1 = driver.FindElement(depatureLocator1);
            IWebElement arrivalCity = driver.FindElement(arrivalLocator);

            WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(departureCity));


            departureCity.Click();
            departureCity.SendKeys("Goa");
            arrivalCity.Click();
            arrivalCity.SendKeys("BOM");


            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            TestContext.Progress.WriteLine("New URL is:"+driver.Url);

            By radioButtons = By.XPath("//*[@class='radiotextsty']");
            By inputUser = By.XPath("//input[@name='username']");


            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait2.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(inputUser));

            /*Radio Button handling strategy -- if we need to search all the buttons and click*/

            IList<IWebElement> radioButtonList = driver.FindElements(radioButtons);
            foreach (IWebElement radioButton in radioButtonList)
            {
                TestContext.Progress.WriteLine("Radio button value is" + radioButton.Text);
                if (radioButton.Text.Equals("User"))
                {
                    radioButton.Click();
                    TestContext.Progress.WriteLine("Radio button is clicked");
                    Boolean result = radioButton.Selected;
                    /*Boolen assertion
                    Assert.That(result,Is.True); */
                    break;
                }
            }






        }

        [TearDown]
        public void CloseBrowser() 
        {
            driver.Quit();
        }
    }
}