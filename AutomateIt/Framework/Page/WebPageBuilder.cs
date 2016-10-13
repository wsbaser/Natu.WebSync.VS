using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using selenium.core.Framework.PageElements;

namespace selenium.core.Framework.Page {
    public static class WebPageBuilder {

        public static void InitPage(IPage page) {
            InitComponents(page, page);
        }

        /// <summary>
        /// ������� ������ ��������� � ��� ������� �������� ���������������� ��� ��������� ����������
        /// (���������� ��������� WebComponent)
        /// </summary>
        public static List<T> CreateItems<T>(IContainer container, IEnumerable<string> ids) {
            return ids.Select(id => CreateComponent<T>(container.ParentPage, container, id)).Cast<T>().ToList();
        }

        public static IComponent CreateComponent<T>(IContainer container, params object[] additionalArgs) {
            var component = CreateComponent(container.ParentPage, container, typeof (T),
                                            new WebComponentAttribute(additionalArgs));
            InitComponents(container.ParentPage, component);
            return component;
        }

        public static IComponent CreateComponent<T>(IPage page, params object[] additionalArgs) {
            var component = CreateComponent(page, page, typeof (T), new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

        /// <summary>
        /// ������� ��������� � ���������������� ��� ��������� ����������
        /// (���������� ��������� WebComponent)
        /// </summary>
        public static IComponent CreateComponent<T>(IPage page, object componentContainer, params object[] additionalArgs) {
            var component = CreateComponent(page, componentContainer, typeof (T),
                                            new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

        public static IComponent CreateComponent(IPage page, Type type, params object[] additionalArgs) {
            var component = CreateComponent(page, page, type, new WebComponentAttribute(additionalArgs));
            InitComponents(page, component);
            return component;
        }

	    public static IComponent CreateComponent(IPage page, object componentContainer, Type type,
		    IComponentAttribute attribute)
	    {
		    List<object> args = typeof(ItemBase).IsAssignableFrom(type)
			    ? new List<object> {componentContainer} // �������
			    : new List<object> {page};
		    var container = componentContainer as IContainer;
		    if (attribute.Args != null)
		    {
			    if (container != null)
			    {
				    // ������������� ������������� ���� � ����������
				    for (int i = 0; i < attribute.Args.Length; i++)
					    attribute.Args[i] = CreateInnerSelector(container, attribute.Args[i]);
			    }
			    args.AddRange(attribute.Args);
		    }
		    IComponent component = (IComponent) Activator.CreateInstance(type, args.ToArray());
		    component.ComponentName = attribute.ComponentName;
		    component.FrameScss = attribute.FrameScss ?? container?.FrameScss;
		    return component;
	    }

	    private static object CreateInnerSelector(IContainer container, object argument) {
            var argumentString = argument as string;
            if (argumentString != null && argumentString.StartsWith("root:"))
                return container.InnerScss(argumentString.Replace("root:", string.Empty));
            return argument;
        }

        /// <summary>
        /// ���������������� ����������
        /// </summary>
        /// <remarks>
        /// ����� Reflection ����� � ���������������� ��� ���� ������� ����������� ��������� IComponent
        /// </remarks>
        public static void InitComponents(IPage page, object containerObject) {
            if (page == null)
                throw new ArgumentNullException("page", "page cannot be null");
            if (containerObject == null)
                containerObject = page;
            var container = containerObject as IContainer;
            Type type = containerObject.GetType();
            Dictionary<MemberInfo, IComponentAttribute> components = GetComponents(type);
            foreach (MemberInfo memberInfo in components.Keys) {
                IComponentAttribute attribute = components[memberInfo];
                IComponent instance;
                if (memberInfo is FieldInfo) {
                    var fieldInfo = memberInfo as FieldInfo;
                    instance = (IComponent) fieldInfo.GetValue(containerObject);
                    if (instance == null) {
                        instance = CreateComponent(page, containerObject, fieldInfo.FieldType, attribute);
                        fieldInfo.SetValue(containerObject, instance);
                    }
                    else {
                        instance.FrameScss = instance.FrameScss ?? container?.FrameScss;
                    }
                }
                else if (memberInfo is PropertyInfo) {
                    var propertyInfo = memberInfo as PropertyInfo;
                    instance = (IComponent) propertyInfo.GetValue(containerObject);
                    if (instance == null) {
                        instance = CreateComponent(page, containerObject, propertyInfo.PropertyType, attribute);
                        propertyInfo.SetValue(containerObject, instance);
                    }
                    else {
                        instance.FrameScss = instance.FrameScss ?? container?.FrameScss;
                    }
                }
                else
                    throw new NotSupportedException("Unknown member type");
                page.RegisterComponent(instance);
                InitComponents(page, instance);
            }
        }

        /// <summary>
        /// �������� ������ �����-����������� ����(������� ����-���������� ������������ �����)
        /// </summary>
        private static Dictionary<MemberInfo, IComponentAttribute> GetComponents(Type type) {
            var components = new Dictionary<MemberInfo, IComponentAttribute>();
            // �������� ������ �����
            List<MemberInfo> members =
                type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Cast<MemberInfo>().ToList();
            // �������� ������ �������
            members.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            Type attributeType = typeof(IComponentAttribute);
            foreach (var field in members) {
                object[] attributes = field.GetCustomAttributes(attributeType, true);
                if (attributes.Length > 0) {
                    if (!IsComponent(field)) {
                        throw new Exception("IComponentAttribute attribute can be applied only to IComponent field");
                    }
                    components.Add(field, attributes[0] as IComponentAttribute);
                }
            }
            return components;
        }

        private static bool IsComponent(MemberInfo memberInfo) {
            Type componentType = typeof(IComponent);
            if (memberInfo is FieldInfo) {
                return componentType.IsAssignableFrom(((FieldInfo) memberInfo).FieldType);
            }
            if (memberInfo is PropertyInfo) {
                return componentType.IsAssignableFrom(((PropertyInfo) memberInfo).PropertyType);
            }
            return false;
        }
    }

    [TestFixture]
    public class WebPageBuilderTest {
        [Test]
        public void DoNotAddRootWithouPrefix() {
            var page = new Page();
            var container = new Container(page, "//*[@id='rootelementid']");
            WebPageBuilder.InitComponents(page, container);
            Assert.AreEqual("//div[text()" +
                            "='mytext']",
                            container.Component2.Xpath, "������������� xpath �� �������������� � ����������");
        }

        [Test]
        public void ReplacePrefixWithRootSelector() {
            var page = new Page();
            var container = new Container(page, "//*[@id='rootelementid']");
            WebPageBuilder.InitComponents(page, container);
            Assert.AreEqual("//*[@id='rootelementid']/descendant::div[text()='mytext']",
                            container.Component1.Xpath, "������������� xpath �� �������������� � ����������");
        }

        private class Container : ContainerBase {
            [WebComponent("root:div[text()='mytext']")]
            public Component Component1;
            [WebComponent("//div[text()='mytext']")]
            public Component Component2;


            public Container(IPage parent, string rootScss)
                : base(parent, rootScss) {
            }
        }

        private class Component:ComponentBase {
            public readonly string Xpath;

            public Component(IPage page, string xpath) : base(page) {
                Xpath = xpath;
            }

            public override bool IsVisible() {
                throw new NotImplementedException();
            }
        }

        private class Page : PageBase {
        }
    }
}