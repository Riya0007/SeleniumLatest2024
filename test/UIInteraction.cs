using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumCSharpFramework.test
{

    /*With out Page Object Model*/
    /*This class will handle javascript alerts,mouse hover using action class,windows and frames Handling*/
    public class UIInteraction
    {
        IWebDriver driver;
        [SetUp]
        public void setup()
        {
            
      //  new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait= TimeSpan.FromSeconds(5);
        driver.Url = "https://rahulshettyacademy.com/AutomationPractice/";
        TestContext.Progress.WriteLine("Set up step is executed succesfully");
           

        }

        [Test]
        public void testInteraction() 
        {
            By entername = By.CssSelector("input[name='enter-name']");
            By alertBtn = By.CssSelector("input[id='alertbtn']");
            driver.FindElement(entername).SendKeys("Checking alert");
            driver.FindElement(alertBtn).Click();
            TestContext.Progress.WriteLine (driver.SwitchTo().Alert().Text);
            driver.SwitchTo().Alert().Accept();

        }


        [Test]
        public void ActionInteration()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/droppable/");
            IWebElement source = driver.FindElement(By.XPath("//div[text()='Drag me']"));
            IWebElement destination = driver.FindElement(By.XPath("//p[text()='Drop here']"));

            WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Drag me']")));
          
            Actions a = new Actions(driver);
            a.DragAndDrop(source, destination).Perform();
        }

        [Test]
        public void Frames()
        {

            


            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/AutomationPractice/");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h1[text()='Practice Page']")));

            /*How to scroll*/
            IWebElement iFrameElement = driver.FindElement(By.XPath("//iframe[@id='courses-iframe']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", iFrameElement);


            // Frame Name, id or index

            driver.SwitchTo().Frame("courses-iframe");
            driver.FindElement(By.XPath("(//a[text()='Learning paths'])[1]")).Click();
            driver.SwitchTo().DefaultContent();
        
        }

        [Test]
        public void WindowHandling()
        {
            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//a[@class='blinkingText']")));

            string currentWindow =driver.CurrentWindowHandle;
            driver.FindElement(By.XPath("//a[@class='blinkingText']")).Click();
            
            // Store all the opened window into the list 
            // Print each and every items of the list
            List<string> lstWindow = driver.WindowHandles.ToList();
            foreach (var handle in lstWindow)
            {
                Console.WriteLine(handle);
            }
            
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            String email = driver.FindElement(By.XPath("//a[contains(@href,'mailto')]")).Text;
            driver.SwitchTo().Window(currentWindow);
            By username = By.XPath("//input[@id='username']");
            driver.FindElement(username).SendKeys("rahulshettyacademy");

        }


        [TearDown]
        public void tearDown() { 

            driver.Quit();
        }


    }
}
