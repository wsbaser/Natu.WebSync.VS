/**
* Created by VolkovA on 27.02.14.
*/ // Методы для получения данных со страницы

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using automateit.SCSS;
using automateit.utils.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;
using selenium.core.Auxiliary;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
    public class BrowserGet : DriverFacade {
        public BrowserGet(Browser browser)
            : base(browser) {
        }

        /// <summary>
        /// Получить исходный код страницы
        /// </summary>
        public string PageSource {
            get { return Browser.Driver.PageSource; }
        }

        // Получить содержимое элемента
        public string TextS(string scssSelector) {
            return TextS(ScssBuilder.CreateBy(scssSelector));
        }

        public string TextS(By by) {
            return RepeatAfterStale(
                () => {
                    IWebElement element = Browser.Find.ElementFastS(by);
                    if (element == null)
                        return null;
                    return element.Text;
                });
        }

        // Получить содержимое элемента
        public String Text(string scssSelector, bool displayed = false) {
            return Text(ScssBuilder.CreateBy(scssSelector), null);
        }

        public String Text(By by, By frameBy, bool displayed = false) {
            return Text(Driver, by, frameBy, displayed);
        }

        /// <summary>
        /// Получить содержимое элемента
        /// </summary>
        public String Text(ISearchContext context, By by, By frameBy, bool displayed = false) {
            if (displayed && !Browser.Is.Visible(context, by,frameBy))
                return null;
            return RepeatAfterStale(() => Browser.Find.Element(context, @by,frameBy, displayed).Text);
        }

        /// <summary>
        /// Найти элементы по указанному селектору и получить их тексты
        /// </summary>
        public List<string> Texts(string scssSelector, By frameBy=null, bool displayed = false) {
            return Texts(ScssBuilder.CreateBy(scssSelector), frameBy, displayed);
        }

        public List<string> Texts(By by, By frameBy = null, bool displayed = false)
        {
            return Texts(Driver, by, frameBy, displayed);
        }

        public List<string> Texts(ISearchContext context, string scssSelector, By frameBy = null, bool displayed = false)
        {
            return Texts(context, ScssBuilder.CreateBy(scssSelector), frameBy, displayed);
        }

        public List<string> Texts(ISearchContext context, By by, By frameBy = null, bool displayed = false) {
            return RepeatAfterStale(
                () => {
                    var elements = Browser.Find.Elements(context, by, frameBy);
                    if (displayed)
                        elements = elements.Where(e => e.Displayed).ToList();
                    return elements.Select(e => e.Text).ToList();
                });
        }

        public List<string> Ids(string scssSelector, bool displayed = false)
        {
            return Ids(Driver, ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public List<string> Ids(By by, bool displayed = false)
        {
            return Ids(Driver, by, displayed);
        }

        public List<string> Ids(ISearchContext context, By by, bool displayed = false)
        {
            return RepeatAfterStale(
                () =>
                {
                    var elements = Browser.Find.Elements(context, by);
                    if (displayed)
                        elements = elements.Where(e => e.Displayed).ToList();
                    return elements.Select(e => e.Text).ToList();
                });
        }

        /// <summary>
        /// Получить атрибут src тега img
        /// </summary>
        public string ImgFileName(string scssSelector) {
            return ImgFileName(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgFileName(By by) {
            return ImgSrc(by).Split('/').Last();
        }

        /// <summary>
        /// Получить атрибут src тега img
        /// </summary>
        public string ImgSrc(string scssSelector) {
            return ImgSrc(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgSrc(By by) {
            return Attr(by, "src");
        }

        /// <summary>
        /// Получить список атрибутов src тегов img
        /// </summary>
        public List<string> ImgSrcs(string scssSelector) {
            return ImgSrcs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> ImgSrcs(By by) {
            return Attrs(by, "src");
        }

        /// <summary>
        /// Получить аттрибут элемента
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        /// <param name="displayed">искать только видимые элементы</param>
        public string Attr(string scssSelector, string name, bool displayed = true)
        {
            return Attr(ScssBuilder.CreateBy(scssSelector), null, name, displayed);
        }

        public string Attr(By by, string name, bool displayed = true)
        {
            return Attr(by, null, name, displayed);
        }

        public string Attr(By by, By frameBy, string name, bool displayed = true) {
            return RepeatAfterStale(
                () => Attr(Browser.Find.Element(@by, frameBy, displayed), name));
        }

        public T Attr<T>(string scssSelector, string name, bool displayed = true) {
            return Attr<T>(ScssBuilder.CreateBy(scssSelector), name, displayed);
        }

        public T Attr<T>(By by, string name, bool displayed = true) {
            return Cast<T>(Attr(by, name, displayed));
        }

        public string Attr(IWebElement element, string name)
        {
            return element.GetAttribute(name);
        }

        /// <summary>
        /// Получить аттрибуты элементов
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        public List<string> Attrs(string scssSelector, string name) {
            return Attrs(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<string> Attrs(By by, string name) {
            return RepeatAfterStale(
                () => Browser.Find.Elements(@by).Select(e => Attr(e, name)).ToList());
        }

        /// <summary>
        /// Получить атрибут href тега a
        /// </summary>
        public string Href(string scssSelector) {
            return Href(ScssBuilder.CreateBy(scssSelector));
        }

        public string Href(By by) {
            return Attr(by, "href");
        }

        /// <summary>
        /// Получить список атрибутов href тегов a
        /// </summary>
        public List<string> Hrefs(string scssSelector) {
            return Hrefs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> Hrefs(By by) {
            return Attrs(by, "href");
        }


        /// <summary>
        /// Получить список атрибутов указанного типа
        /// </summary>
        public List<T> Attrs<T>(string scssSelector, string name) {
            return Attrs<T>(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<T> Attrs<T>(By by, string name) {
            return RepeatAfterStale(
                () => Browser.Find.Elements(by).Select(e => Attr(e, name)).Select(Cast<T>).ToList());
        }

        private T Cast<T>(string value) {
            var type = typeof (T);
            if (type == typeof (Int16) || type == typeof (Int32) || type == typeof (Int64))
                return (T) Convert.ChangeType(value.FindInt(), typeof (T));
            if (type == typeof (UInt16) || type == typeof (UInt32) || type == typeof (UInt64))
                return (T) Convert.ChangeType(value.FindUInt(), typeof (T));
            if (type == typeof (decimal) || type == typeof (float))
                return (T) Convert.ChangeType(value.FindNumber(), typeof (T));
            return (T) Convert.ChangeType(value, typeof (T));
        }

        /// <summary>
        /// Получить содержимое поля ввода
        /// </summary>
        public string InputValue(string scssSelector, bool displayed = true)
        {
            return InputValue(ScssBuilder.CreateBy(scssSelector), null, displayed);
        }

        public string InputValue(By by,By frameBy, bool displayed = true) {
            return Attr(by, frameBy, "value",displayed);
        }

        public string InputValue(IWebElement element, bool displayed = true) {
            return Attr(element, "value");
        }

        /// <summary>
        /// Получить целое значение из элемента найденного по селектору
        /// </summary>
        public int Int(string scssSelector, bool displayed = false) {
            return Int(ScssBuilder.CreateBy(scssSelector), null, displayed);
        }

        public int Int(By by, By frameBy, bool displayed = false) {
            return Int(Driver, by, frameBy, displayed);
        }

        public int Int(ISearchContext context, By by, By frameBy, bool displayed=false) {
            return Text(context, by, frameBy, displayed).AsInt();
        }

        /// <summary>
        /// Получить целые значения из элементов, найденных по селектору
        /// </summary>
        public List<int> Ints(string scssSelector) {
            return Ints(ScssBuilder.CreateBy(scssSelector));
        }

        public List<int> Ints(By by) {
            return Texts(by).Select(s => s.AsInt()).ToList();
        }

        /// <summary>
        /// Получить css параметр тега
        /// </summary>
        public T CssValue<T>(string scssSelector, ECssProperty property) {
            return CssValue<T>(ScssBuilder.CreateBy(scssSelector), property);
        }

        public T CssValue<T>(By by, ECssProperty property) {
            return RepeatAfterStale(() => CssValue<T>(Browser.Find.Element(by), property));
        }

        public T CssValue<T>(IWebElement element, ECssProperty property) {
            string value = element.GetCssValue(property.StringValue());
            return Cast<T>(value);
        }

        /// <summary>
        /// Получить количество элементов, найденных по указанному селектору
        /// </summary>
        public int Count(string scssSelector) {
            return Count(ScssBuilder.CreateBy(scssSelector));
        }

        public int Count(By by) {
            return RepeatAfterStale(() => Browser.Find.Elements(by).Count);
        }

        /// <summary>
        /// Сделать скриншот указанной области экрана
        /// </summary>
        public Bitmap Screenshot() {
            Browser.Js.ScrollToTop();
            // Get the Total Size of the Document
            int totalWidth = (int)Browser.Js.Excecute<long>("return document.documentElement.scrollWidth");
            int totalHeight = (int)Browser.Js.Excecute<long>("return document.documentElement.scrollHeight");
            
            // Get the Size of the Viewport
            int viewportWidth = (int) Browser.Js.Excecute<long>("return document.documentElement.clientWidth");
            int viewportHeight = (int) Browser.Js.Excecute<long>("return document.documentElement.clientHeight");

            // Split the Screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the Total Height is reached
            for (int i = 0; i < totalHeight; i += viewportHeight) {
                int newHeight = viewportHeight;
                // Fix if the Height of the Element is too big
                if (i + viewportHeight > totalHeight) {
                    newHeight = totalHeight - i;
                }
                // Loop until the Total Width is reached
                for (int ii = 0; ii < totalWidth; ii += viewportWidth) {
                    int newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (ii + viewportWidth > totalWidth) {
                        newWidth = totalWidth - ii;
                    }

                    // Create and add the Rectangle
                    Rectangle currRect = new Rectangle(ii, i, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }

            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            Rectangle previous = Rectangle.Empty;
            foreach (var rectangle in rectangles) {
                // Calculate the Scrolling (if needed)
                if (previous != Rectangle.Empty) {
                    int xDiff = rectangle.Right - previous.Right;
                    int yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    Browser.Js.Excecute("window.scrollBy({0}, {1})", xDiff, yDiff);
                    System.Threading.Thread.Sleep(200);
                }

                // Take Screenshot
                var screenshot = ((ITakesScreenshot) Driver).GetScreenshot();

                // Build an Image out of the Screenshot
                Image screenshotImage;
                using (MemoryStream memStream = new MemoryStream(screenshot.AsByteArray)) {
                    screenshotImage = Image.FromStream(memStream);
                }

                // Calculate the Source Rectangle
                Rectangle sourceRectangle = new Rectangle(viewportWidth - rectangle.Width,
                                                          viewportHeight - rectangle.Height, rectangle.Width,
                                                          rectangle.Height);

                // Copy the Image
                using (Graphics g = Graphics.FromImage(stitchedImage)) {
                    g.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }

                // Set the Previous Rectangle
                previous = rectangle;
            }
            // The full Screenshot is now in the Variable "stitchedImage"
            return stitchedImage;
        }

        /// <summary>
        /// Получить прямоугольник, заданный Css свойствами элемента left, top, height, width
        /// </summary>
        public Rectangle Bounds(string scssSelector) {
            return Bounds(ScssBuilder.CreateBy(scssSelector));
        }

        public Rectangle Bounds(By by) {
            return RepeatAfterStale(() => Bounds(Browser.Find.Element(by)));
        }

        /// <summary>
        /// Получить границы элемента
        /// </summary>
        public Rectangle Bounds(IWebElement element) {
            return new Rectangle(CssValue<int>(element, ECssProperty.left),
                                 CssValue<int>(element, ECssProperty.top),
                                 CssValue<int>(element, ECssProperty.width),
                                 CssValue<int>(element, ECssProperty.height));
        }

        /// <summary>
        /// Получить координаты элемента
        /// </summary>
        public Point Point(IWebElement element) {
            return new Point(CssValue<int>(element, ECssProperty.left),
                             CssValue<int>(element, ECssProperty.top));
        }

        /// <summary>
        /// Получить значение указанного типа из найденного по селектору элементу
        /// </summary>
        public T Value<T>(string scssSelector) {
            return Value<T>(ScssBuilder.CreateBy(scssSelector));
        }

        public T Value<T>(By by) {
            return RepeatAfterStale(() => Value<T>(Browser.Find.Element(by)));
        }

        public T Value<T>(IWebElement element) {
            return Cast<T>(element.Text);
        }

        /// <summary>
        /// Получить список текстов по селектору и объединить их в строку 
        /// </summary>
        public string TextsAsString(string scssSelector, string delimiter = " ") {
            return TextsAsString(ScssBuilder.CreateBy(scssSelector), delimiter);
        }

        public string TextsAsString(By by, string delimiter = " ") {
            return Texts(by).AsString(delimiter);
        }

        /// <summary>
        /// Получить атрибут href и получить из него абсолютный Url(без домена)
        /// </summary>
        public string AbsoluteHref(By by) {
            return Href(by).CutBaseUrl();
        }

        /// <summary>
        /// Returns FileSystemInfo of the last file downloaded in Browser
        /// </summary>
        public FileInfo LastDownloadedFile(bool waitUntilDownloaded = true) {
            var downloadDirectory = new DirectoryInfo(Browser.Settings.DownloadDirectory);
            var lastDownloadedFile = downloadDirectory.GetFiles().OrderByDescending(f => f.CreationTime).First();
            if (waitUntilDownloaded) {
                long curLength = 0;
                const int NO_CHANGE_INTERVAL = 1000;
                Thread.Sleep(NO_CHANGE_INTERVAL);
                Browser.Wait.Until(() => {
                    if (curLength != 0 && lastDownloadedFile.Length == curLength) {
                        return true;
                    }
                    curLength = lastDownloadedFile.Length;
                    return false;
                }, BrowserTimeouts.FILE_DOWNLOAD, NO_CHANGE_INTERVAL);
            }
            return lastDownloadedFile;
        }

        public string ChecksumOfLastDownloadedFile(bool waitUntilDownloaded=true) {
            var lastDownloadedFile = LastDownloadedFile(waitUntilDownloaded);
            string checksum;
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(lastDownloadedFile.FullName)) {
                    var computeHash = md5.ComputeHash(stream);
                    checksum = Convert.ToBase64String(computeHash);
                }
            }
            return checksum;
        }
    }
}