using MDDPlatform.Messages.Broker.Exctensions;

namespace MDDPlatform.Messages.Broker.Options
{
    internal class  PlaceholderRegistery
    {
        public const string Namespace = "{<Namespace>}";
        public const string Type = "{<Type>}";
        public const string Assembly = "{<Assembly>}";
        public const string BaseType = "{<BaseType>}";
        public const string FullName = "{<FullName>}";

        private Dictionary<string,Placeholder> PlaceholderDictionary;
        public PlaceholderRegistery()
        {
            PlaceholderDictionary = new Dictionary<string, Placeholder>();
            PlaceholderDictionary.Add(Namespace,new NamespacePlaceholder());
            PlaceholderDictionary.Add(Type,new TypePlaceholder());
            PlaceholderDictionary.Add(Assembly,new AssemblyPlaceholder());
            PlaceholderDictionary.Add(BaseType,new BaseTypePlaceholder());
            PlaceholderDictionary.Add(FullName,new FullnamePlaceholder());            
        }

        public IPlaceholder GetPlaceholder(string template){
            if(PlaceholderDictionary.ContainsKey(template))
                return PlaceholderDictionary[template];
            
            if(template.IsValidPlaceholder())
                return new PropertyPlaceholder(template);

            return new CompositePlaceholder(template);
        }        
    }
}