using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumCSharpFramework.utilities
{
    public class BaseClass
    {

        //To access this driver from child , we will be using public GetDriver() which will return driver
       // private IWebDriver driver;
     
       private ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        ExtentReports extent;
        ExtentTest test;

        /*Steps for reporting 
         1. Create one reporter file 
         2. Attach the file with extentreports obj where extentreports object will log everything
         3. Add system info to test
         4. Under setup pass test case name to extent
         5. Under tear down, attach the test status to the extent and screenshot
        6. Flush extent object
         
         */


        [OneTimeSetUp]
        public void onesetupforexecution()
        {
            string workingdir = Environment.CurrentDirectory;
            string projectDir = Directory.GetParent(workingdir).Parent.Parent.FullName;
            DateTime time = DateTime.Now;
            string fileName = "//Results//"+ time.ToString("h_mm_ss") ;
            string folderpath = projectDir+ fileName + "//index.html";
            var htmlreport = new ExtentHtmlReporter(folderpath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlreport);
            extent.AddSystemInfo("Host Name:","Local Host");
            extent.AddSystemInfo("Environment:", "Test");
            extent.AddSystemInfo("Username:", "Riya");
        }



        [SetUp]
        public void StartBrowser()
        {

            /*chromedriver.exe file is needed as selenium cannot directly talk to browser drivers , 
            all the commands with be passed to chromedriver.exe,
            WebDriverManager will do all the work here*/

            /*WebDriverManager -- Namespace
             DriverManager -- Class
            SetUpDriver--- Method*/

            /* Use configuration file to handle all dynamic values and configuration manager package needs to be imported 
            ToString read and pass the values to the base class
            
            <Target Name="CopyCustomContent" AfterTargets="AfterBuild">
		<Copy SourceFiles="App.config" DestinationFiles="$(OutDir)\testhost.dll.config" />
	</Target>

            Afterbuilding App.config file has to be copied in the respective folder. so above Target tag has to be added
             
             
             */

            test= extent.CreateTest(TestContext.CurrentContext.Test.Name); //passing current test case name to extenttest object

            var browserName = TestContext.Parameters["browserNamePassed"]; // browserName will be taken from commnad prompt first.
            /*User can pass the parameter value like this 
              dotnet test SeleniumCSharpFramework.csproj --filter TestCategory=Smoke --TestParameters.Parameter(name=\"browserNamePassed\",value="\Firefox\")   
            */
            if (browserName == null)
            {
                browserName = ConfigurationManager.AppSettings["browser"];  // if nothing is passed from Command prompt, from App settings value will be taken

            }
        
            var url = ConfigurationManager.AppSettings["url"];
            InitBrowser(browserName);
            driver.Value.Url = url;
            driver.Value.Manage().Window.Maximize();
            TestContext.Progress.WriteLine("Set up is executed successfully");

        }

        public void InitBrowser(string browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    {
                        new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                        driver.Value = new FirefoxDriver();
                        break;
                    }

                case "Chrome":
                    {
                       // new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                        driver.Value = new ChromeDriver();
                        break;
                    }
            }

        }

        public IWebDriver GetDriver()
        {
            return driver.Value;
        }

        /*If we want to create  object of same class in different different classes, then we must create the object of the class in base class , so that child class can inherit this*/
        public static JSONReader getJSONOObject()

        {
            return new JSONReader();
        }

        [TearDown]
        public void AfterTest()
        {

            string  status = TestContext.CurrentContext.Result.Outcome.Status.ToString();
            var stackTrace= TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            string fileName =  "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            switch (status)
            {
                case "Passed":
                    {
                        test.Pass("Test case is passed");
                        break;
                    }

                case "Failed":
                    {

                        test.Fail("Test case is failed", captureScreenShot(driver.Value, fileName));
                        test.Log(Status.Fail,"test failed with stacktrace:"+stackTrace);
                        break;
                    }
            }
            extent.Flush();
            driver.Value.Quit();
            TestContext.Progress.WriteLine("Tear down is successful");
        }

        public MediaEntityModelProvider captureScreenShot(IWebDriver driver, String screenShotName)

        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot().AsBase64EncodedString;

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();




        }




    }
}
