using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

#nullable disable

namespace Simplify.Web.App.TagHelpers
{
    [HtmlTargetElement("icon", ParentTag = null)]
    public class IconTagHelper : TagHelper
    {
        public enum Type
        {
            [Description("f-fill")]
            Fill,
            [Description("f-outline")]
            Outline
        }

        [Required]
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; } = string.Empty;

        [HtmlAttributeName("as")]
        public Type As { get; set; } = Type.Fill;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "svg";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", $"f-24 {As.GetDescription()} {Class?.Trim()}");
            output.Content.SetHtmlContent($@"<use xlink:href='/assets/feather-sprite.svg#{Name.ToLower()}'/>");
        }
    }

    [HtmlTargetElement("f-icon", ParentTag = null)]
    public class FIconTagHelper : TagHelper
    {
        [Required]
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [HtmlAttributeName("color")]
        public string Color { get; set; }

        [HtmlAttributeName("fill")]
        public string Fill { get; set; } = "black";

        [HtmlAttributeName("stroke")]
        public string Stroke { get; set; } = "none";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "svg";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", string.IsNullOrWhiteSpace(Class) ? "feather" : $"feather {Class}");
            output.Attributes.Add("fill", Fill);
            output.Attributes.Add("stroke", Stroke);
            if(!string.IsNullOrWhiteSpace(Color))
            {
                output.Attributes.SetAttribute("style", $"color:{Color}");
            }


            output.Content.SetHtmlContent($@"<use xlink:href='/assets/feather-sprite.svg#{Name.ToLower()}'/>");
        }
    }

    [HtmlTargetElement("icon-css", ParentTag = null)]
    public class IonIconTagHelper : TagHelper
    {
        [Required]
        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; } = "icon";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "svg";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", Class);


            output.Content.SetHtmlContent($@"<use xlink:href='/assets/cssgg.svg#{Name.ToLower()}'/>");
        }
    }

    [HtmlTargetElement("a", Attributes = "active-url")]
    public class ActiveTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpService;

        public ActiveTagHelper(IHttpContextAccessor httpService) => _httpService = httpService;
        public string ActiveUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!string.IsNullOrWhiteSpace(ActiveUrl) && _httpService.HttpContext.Request.Path == ActiveUrl)
            {
                output.Attributes.SetAttribute("class", $"active {output.Attributes["class"]?.Value}");
            }
        }
    }

    [HtmlTargetElement("banner")]
    public class BannerTagHelper : TagHelper
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string BackgroundColor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) => output.Content.SetHtmlContent(
            $@"<div class='jumbotron jumbotron-fluid jumbotron-{BackgroundColor}'>
                    <div class='container'>
                        <h1 class='display-4'>{Title}</h1>
                        <p class='lead'>{SubTitle}</p>
                    </div>
                </div>");
    }

    [HtmlTargetElement("email")]
    public class EmailTagHelper : TagHelper
    {
        private const string Domain = "bhanu.dev";

        public string Class { get; set; }

        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent();
            output.Attributes.SetAttribute("href", $"mailto:{target.ToLower()}@{Domain}");
            if(!string.IsNullOrWhiteSpace(Class))
            {
                output.Attributes.SetAttribute("class", Class.Trim());
            }
            output.Content.SetContent(target);
        }
    }

    [HtmlTargetElement("gravatar")]
    public class GravatarTagHelper : TagHelper
    {
        [HtmlAttributeName("email")]
        public string Email { get; set; }

        [HtmlAttributeName("alt")]
        public string Alt { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [HtmlAttributeName("size")]
        public int Size { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!string.IsNullOrWhiteSpace(Email))
            {
                output.TagName = "img";
                if(!string.IsNullOrWhiteSpace(Class))
                {
                    output.Attributes.Add("class", Class);
                }

                if(!string.IsNullOrWhiteSpace(Alt))
                {
                    output.Attributes.Add("alt", Alt);
                }

                output.Attributes.Add("src", GetAvatarUrl(SimplifyWebHelper.CreateMD5(Email), Size));
                output.TagMode = TagMode.SelfClosing;
            }
        }

        private static string GetAvatarUrl(string hash, int size)
        {
            var sizeArg = size > 0 ? $"?s={size}" : "";

            return $"https://www.gravatar.com/avatar/{hash}{sizeArg}";
        }
    }

    [HtmlTargetElement("picker")]
    public class PickerTagHelper : TagHelper
    {
        public string Url { get; set; }

        [Required]
        public string Id { get; set; }

        public string SelectedItemsTitle { get; set; }

        public string SearchInputPlaceholder { get; set; }

        public string SearchResultTitle { get; set; }

        public string SuggestedItemsTitle { get; set; }

        public string NoItemSelectedTitle { get; set; }

        public List<string> SelectedItems { get; set; }

        public string SelectedItem { get; set; }

        public string ShowAllItemsTitle { get; set; }

        public int MinSearchText { get; set; }

        public bool MultipleSelect { get; set; }

        public bool AllowItemAlreadySelectedNotification { get; set; } = true;

        public string ItemAlreadySelectedTitle { get; set; }

        public bool AllowSuggestedItems { get; set; } = true;

        public int TopSuggestedItems { get; set; } = 5;

        public bool Required { get; set; }

        public string RequiredMessage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddWrapper(output);
            AddComponent(output);
            AddHiddenField(output);
        }

        private void AddWrapper(TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "hidden picker");
            output.Attributes.Add(new TagHelperAttribute("data-bind", new HtmlString("css: { hidden: false }")));
        }

        private void AddComponent(TagHelperOutput output)
        {
            var selectedItems = GetSelectedItems();

            var component = new
            {
                name = "picker",
                @params = new
                {
                    search = "",
                    hiddenId = Id,
                    url = Url,
                    selectedItemsTitle = SelectedItemsTitle,
                    allowSuggestedItems = AllowSuggestedItems,
                    searchResultTitle = SearchResultTitle,
                    suggestedItemsTitle = SuggestedItemsTitle,
                    noItemSelectedTitle = NoItemSelectedTitle,
                    searchInputPlaceholder = SearchInputPlaceholder,
                    showAllItemsTitle = ShowAllItemsTitle,
                    selectedItems,
                    minSearchText = MinSearchText,
                    topSuggestedItems = TopSuggestedItems,
                    multipleSelect = MultipleSelect,
                    allowItemAlreadySelectedNotification = AllowItemAlreadySelectedNotification,
                    itemAlreadySelectedTitle = ItemAlreadySelectedTitle
                }
            };

            var rawPickerHtml = new HtmlString($"<div data-bind='component: {JsonSerializer.Serialize(component)}'></div>");
            // var rawPickerHtml = new HtmlString($"<div data-bind='component: {JsonConvert.SerializeObject(component)}'></div>"); TODO: Review

            output.Content.AppendHtml(rawPickerHtml);
        }

        /// <summary>
        ///     Get Selected Items
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedItems() => MultipleSelect ? GetSelectedItemsWithRemovedQuotes() : GetSelectedItemWithRemovedQuotes();

        /// <summary>
        ///     Get Selected Items - with removed quotes
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedItemsWithRemovedQuotes()
        {
            for(var i = 0; i < SelectedItems?.Count; i++)
            {
                SelectedItems[i] = SelectedItems[i].Replace("'", "").Replace("\"", "");
            }

            return SelectedItems;
        }

        /// <summary>
        ///     Get Selected Item - with removed quotes, the picker component expect the collection of items, therefore it is used
        ///     the list for single value as well
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectedItemWithRemovedQuotes()
        {
            SelectedItem = SelectedItem.Replace("'", "").Replace("\"", "");

            return string.IsNullOrWhiteSpace(SelectedItem) ? new List<string>() : new List<string> { SelectedItem };
        }

        private void AddHiddenField(TagHelperOutput output)
        {
            var hiddenField = new TagBuilder("input");
            hiddenField.Attributes.Add("type", "hidden");
            hiddenField.Attributes.Add("id", Id);
            hiddenField.Attributes.Add("name", Id);
            hiddenField.Attributes.Add("value", string.Empty);

            if(Required)
            {
                hiddenField.Attributes.Add("required", string.Empty);
                hiddenField.Attributes.Add("data-val", "true");
                hiddenField.Attributes.Add("data-val-required", RequiredMessage ?? $"The {Id} field is required.");
                hiddenField.Attributes.Add("aria-required", "true");
            }

            output.Content.AppendHtml(hiddenField);
        }
    }

    [HtmlTargetElement("script-p", Attributes = "inline")]
    public class ScriptNonceTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ScriptNonceTagHelper(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        [HtmlAttributeName("inline")]
        public bool Inline { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!Inline)
            {
                return;
            }

            output.Attributes.SetAttribute("nonce", _contextAccessor.HttpContext.Items["ScriptNonce"].ToString() ?? string.Empty);
        }
    }

    [HtmlTargetElement("style-p", Attributes = "inline")]
    public class StyleNonceTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public StyleNonceTagHelper(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        [HtmlAttributeName("inline")]
        public bool Inline { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!Inline)
            {
                return;
            }

            output.Attributes.SetAttribute("nonce", _contextAccessor.HttpContext.Items["ScriptNonce"].ToString() ?? string.Empty);
        }
    }

    [HtmlTargetElement("toggle-button")]
    public class SwitchTagHelper : TagHelper
    {
        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var divSlider = new TagBuilder("div");
            divSlider.AddCssClass("slider round");

            output.TagName = "label";
            output.Attributes.Add("class", "switch");
            output.Content.AppendHtml(childContent);
            output.Content.AppendHtml(divSlider);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
