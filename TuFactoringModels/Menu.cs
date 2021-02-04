using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TuFactoringModels
{
    public class Menu
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid ID { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ParentID { get; set; }
        [JsonProperty("item_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ItemType { get; set; }
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string IconClass { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("selected_style", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedStyle { get; set; }
        [JsonProperty("selected_class", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedClass { get; set; }
        [JsonProperty("on_click", NullValueHandling = NullValueHandling.Ignore)]
        public string OnClick { get; set; }
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public long Order { get; set; }
    }

    public class MenuViewModel
    {
        public Guid ID { get; set; }
        public string Participant { get; set; }
        public string ItemType { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public string Url { get; set; }
        public string SelectedStyle { get; set; }
        public string SelectedClass { get; set; }
        public string OnClick { get; set; }
        public IList<MenuViewModel> Children { get; set; }
    }
}
