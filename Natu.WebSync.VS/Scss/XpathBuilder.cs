using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace RoslynSpike.Scss {
    public class XPathBuilder {
        public const string DESCENDANT_AXIS = "descendant::";
        public const string ANCESTOR_AXIS = "ancestor::";
        public const string ANCESTORORSELF_AXIS = "ancestor-or-self::";
        public const string ATTRIBUTE_AXIS = "attribute::";
        public const string CHILD_AXIS = "child::";
        public const string DESCENDANTORSELF_AXIS = "descendant-or-self::";
        public const string FOLLOWING_AXIS = "following::";
        public const string FOLLOWINGSIBLING_AXIS = "following-sibling::";
        public const string NAMESPACE_AXIS = "namespace::";
        public const string PARENT_AXIS = "parent::";
        public const string PRECEDING_AXIS = "preceding::";
        public const string PRECEDINGSIBLING_AXIS = "preceding-sibling::";
        public const string SELF_AXIS = "self::";
        private const string XPATH_ROOT = "//";

        public static string Concat(string root, string relative, params object[] args) {
            relative = string.Format(relative, args);
            if (relative.StartsWith(XPATH_ROOT))
                relative = relative.Substring(2, relative.Length - 2);
            if (string.IsNullOrWhiteSpace(relative)) {
                if (string.IsNullOrWhiteSpace(root))
                    throw new Exception("Invalid xpath: root and relative parts are empty");
                return root;
            }
            if (string.IsNullOrWhiteSpace(root))
                return XPATH_ROOT + relative;
            var rootXpaths = root.Split('|');
            if (rootXpaths.Length == 1) {
                string axis = HasAxis(relative) ? string.Empty : DESCENDANT_AXIS;
                return string.Format("{0}/{1}{2}", root, axis, relative);
            }
            string s = rootXpaths.Aggregate(string.Empty,
                                            (current, rootXpath) => current + (Concat(rootXpath.Trim(), relative) + "|"));
            return s.Substring(0, s.Length - 1);
        }

        private static bool HasAxis(string xpath) {
            var axises = new List<string> {
                                            ANCESTOR_AXIS,
                                            ANCESTORORSELF_AXIS,
                                            ATTRIBUTE_AXIS,
                                            CHILD_AXIS,
                                            DESCENDANT_AXIS,
                                            DESCENDANTORSELF_AXIS,
                                            FOLLOWING_AXIS,
                                            FOLLOWINGSIBLING_AXIS,
                                            NAMESPACE_AXIS,
                                            PARENT_AXIS,
                                            PRECEDING_AXIS,
                                            PRECEDINGSIBLING_AXIS,
                                            SELF_AXIS
                                        };
            return axises.Any(xpath.StartsWith);
        }

        public static bool IsXPath(string value) {
            try {
                XPathExpression.Compile(value);
            }
            catch (Exception) {

                return false;
            }
            return true;
        }
    }

    [TestFixture]
    public class XpathBuilderTest {

        //*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']

        [TestCase("div", true)]
        [TestCase("//div", true)]
        [TestCase("//div[@id='myId']", true)]
        [TestCase("//div[text()='mytext']", true)]
        [TestCase("//div[text()='mytext' and @class='myclass']", true)]
        [TestCase("//div[@id='myId']/descendant::span", true)]
        [TestCase("//div[@id='myId1']|//div[@id='myId2']", true)]
        [TestCase("#myId", false)]
        [TestCase(".myclass", false)]
        public void IsXpath(string xpath, bool isXpath) {
            Assert.AreEqual(isXpath, XPathBuilder.IsXPath(xpath));
        }


        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void RootIsEmpty(string root)
        {
            string relative = "div";
            Assert.AreEqual("//div", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void RelativeIsEmpty() {
            string root = "//*[@id='aaa1']";
            string relative = "";
            Assert.AreEqual("//*[@id='aaa1']", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void MakeRelative() {
            string root = "//*[@id='aaa1']";
            string relative = "//*[@id='bbb']";
            Assert.AreEqual("//*[@id='aaa1']/descendant::*[@id='bbb']",
                            XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void MultipleRootXpath() {
            string root = "//*[@id='aaa1'] | //*[@id='aaa2']";
            string relative = "*[@id='bbb']";
            Assert.AreEqual("//*[@id='aaa1']/descendant::*[@id='bbb']|//*[@id='aaa2']/descendant::*[@id='bbb']",
                            XPathBuilder.Concat(root, relative, "myid"));
        }

        [Test]
        public void InsertArgsToRelative() {
            string root = "//div";
            string relative = "*[@id='{0}']";
            Assert.AreEqual("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));

            root = "//div[@id='{0}']";
            relative = "*[@id='{0}']";
            Assert.AreEqual("//div[@id='{0}']/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative, "myid"));
        }

        [Test]
        public void ConcatAsDescendant() {
            string root = "//div";
            string relative = "*[@id='myid']";
            Assert.AreEqual("//div/descendant::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }

        [Test]
        public void LeaveAxis() {
            string root = "//div";
            string relative = "self::*[@id='myid']";
            Assert.AreEqual("//div/self::*[@id='myid']", XPathBuilder.Concat(root, relative));
        }
    }
}