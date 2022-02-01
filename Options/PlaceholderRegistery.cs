using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Options
{
    internal class PlaceholderRegistery
    {
        public const string Namespace = "{<Namespace>}";
        public const string Type = "{<Type>}";
        public const string Assembly = "{<Assembly>}";
        public const string BaseType = "{<BaseType>}";
        public const string FullName = "{<FullName>}";

        private Dictionary<string, Placeholder> PlaceholderDictionary;
        public PlaceholderRegistery()
        {
            PlaceholderDictionary = new Dictionary<string, Placeholder>();
            PlaceholderDictionary.Add(Namespace, new NamespacePlaceholder());
            PlaceholderDictionary.Add(Type, new TypePlaceholder());
            PlaceholderDictionary.Add(Assembly, new AssemblyPlaceholder());
            PlaceholderDictionary.Add(BaseType, new BaseTypePlaceholder());
            PlaceholderDictionary.Add(FullName, new FullnamePlaceholder());
        }
        public string ResolvePlaceholder<TMessage>(string template, TMessage message) where TMessage : IMessage
        {
            if (string.IsNullOrEmpty(template)) return "";
            string input = template;

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var key in PlaceholderDictionary.Keys)
            {
                if (input.Contains(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    string value = PlaceholderDictionary[key].Resolve<TMessage>();
                    string flaggedPlaceholder = string.Format("[{0}]", Guid.NewGuid().ToString());
                    input = input.Replace(key, flaggedPlaceholder);
                    keyValuePairs.Add(flaggedPlaceholder, value);
                }
            }
            if (message != null)
            {
                Type type = message.GetType();
                var properties = type.GetProperties();

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    if (propName != null)
                    {
                        var propTemplate = "{" + propName.Trim() + "}";
                        if (input.Contains(propTemplate, StringComparison.CurrentCultureIgnoreCase))
                        {
                            var propValue = property.GetValue(message);
                            string flaggedPlaceholder = string.Format("[{0}]", Guid.NewGuid().ToString());
                            input = input.Replace(propTemplate, flaggedPlaceholder);
                            keyValuePairs.Add(flaggedPlaceholder, StringValue(propValue));
                        }
                    }
                }
            }
            foreach (var item in keyValuePairs)
            {
                input = input.Replace(item.Key, item.Value);
            }
            return input;
        }
        public string ResolvePlaceholder<TMessage>(string template) where TMessage : IMessage
        {
            if (string.IsNullOrEmpty(template)) return "";
            string input = template;

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var key in PlaceholderDictionary.Keys)
            {
                if (input.Contains(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    string value = PlaceholderDictionary[key].Resolve<TMessage>();
                    string flaggedPlaceholder = string.Format("[{0}]", Guid.NewGuid().ToString());
                    input = input.Replace(key, flaggedPlaceholder);
                    keyValuePairs.Add(flaggedPlaceholder, value);
                }
            }
            foreach (var item in keyValuePairs)
            {
                input = input.Replace(item.Key, item.Value);
            }
            return input;
        }

        private string StringValue(object? propValue)
        {
            if (propValue == null) return "";
            return (string)propValue;
        }
    }
}