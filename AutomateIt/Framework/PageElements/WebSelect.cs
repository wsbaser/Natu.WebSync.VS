using System;
using System.Collections.Generic;
using System.Linq;
using automateit.SCSS;
using automateit.utils.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;

namespace automateit.Framework.PageElements {
    public class WebSelect : DropListBase {
        public WebSelect(IPage parent, string rootScss) : base(parent, rootScss) {
        }

        public override string GetItemScss(string name) {
            return InnerScss($">option['{name}']");
        }

        public override string ItemNameScss => InnerScss("option");

        private SelectElement GetSelectElement() {
            return new SelectElement(Find.Element(RootSelector, FrameBy));
        }

        protected override bool IsOptionSelected(string name) {
            var selectElement = GetSelectElement();
            return selectElement.SelectedOption.Text == name;
        }
    }

    public class WebSelect<T> : WebSelect {
        public WebSelect(IPage parent, string rootScss) : base(parent, rootScss) {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Invalid generic type. T should be an enum.");
        }

        public new List<T> GetItems() {
            return Get.Texts(ItemNameScss).Select(t => (T) Enum.Parse(typeof(T), t)).ToList();
        }

        public virtual bool SelectOption(T option) {
            var optionEnum = option as Enum;
            return SelectOption(optionEnum.StringValue());
        }
    }
}