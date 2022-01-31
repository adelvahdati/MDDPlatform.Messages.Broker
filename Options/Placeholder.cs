using MDDPlatform.Messages.Broker.Exctensions;
using MDDPlatform.Messages.Core;

namespace MDDPlatform.Messages.Broker.Options
{
    public interface IPlaceholder
    {
        public abstract string Template { get; }
        public string Resolve<TMessage>() where TMessage : IMessage;
        public string Resolve<TMessage>(TMessage message) where TMessage : IMessage;
    }
    public abstract class Placeholder : IPlaceholder
    {

        public abstract string Template { get; }
        protected abstract string ResolvePlaceHolder(Type type);

        public string Resolve<TMessage>() where TMessage : IMessage
        {
            Type type = typeof(TMessage);
            return ResolvePlaceHolder(type);
        }
        public string Resolve<TMessage>(TMessage message) where TMessage : IMessage
        {
            Type type;
            if (message == null)
                type = typeof(TMessage);
            else
                type = message.GetType();

            return ResolvePlaceHolder(type);
        }
    }
    internal class PlainTextPlaceHolder : Placeholder
    {
        private string input;
        public override string Template => input;

        public PlainTextPlaceHolder(string input)
        {
            this.input = input;
        }


        protected override string ResolvePlaceHolder(Type type)
        {
            return input;
        }
    }

    internal class FullnamePlaceholder : Placeholder
    {
        public override string Template => PlaceholderRegistery.FullName;
        protected override string ResolvePlaceHolder(Type type)
        {
            if (type == null) return PlaceholderRegistery.FullName;
            if (type.FullName == null) return PlaceholderRegistery.FullName;

            return type.FullName;
        }

    }

    internal class BaseTypePlaceholder : Placeholder
    {
        public override string Template => PlaceholderRegistery.BaseType;

        protected override string ResolvePlaceHolder(Type type)
        {
            if (type == null) return PlaceholderRegistery.BaseType;
            if (type.BaseType == null) return PlaceholderRegistery.BaseType;

            return type.BaseType.Name;
        }
    }

    internal class AssemblyPlaceholder : Placeholder
    {
        public override string Template => PlaceholderRegistery.Assembly;

        protected override string ResolvePlaceHolder(Type type)
        {
            if (type == null) return PlaceholderRegistery.Assembly;
            if (type.Assembly == null) return PlaceholderRegistery.Assembly;

            var assemly = type.Assembly.GetName();
            if (assemly == null) return PlaceholderRegistery.Assembly;
            if (assemly.Name == null) return PlaceholderRegistery.Assembly;
            return assemly.Name;

        }
    }

    internal class TypePlaceholder : Placeholder
    {
        public override string Template => PlaceholderRegistery.Type;

        protected override string ResolvePlaceHolder(Type type)
        {
            if (type == null) return PlaceholderRegistery.Type;
            if (type.Name == null) return PlaceholderRegistery.Type;

            return type.Name;

        }
    }

    internal class NamespacePlaceholder : Placeholder
    {
        public override string Template => PlaceholderRegistery.Namespace;

        protected override string ResolvePlaceHolder(Type type)
        {
            if (type == null) return PlaceholderRegistery.Namespace;
            if (type.Namespace == null) return PlaceholderRegistery.Namespace;

            return type.Namespace;
        }
    }
    internal class PropertyPlaceholder : IPlaceholder
    {
        private string wildcard = "*";
        private readonly string input;

        public string Template => input;
        public PropertyPlaceholder(string input)
        {
            this.input = input;
            Console.WriteLine("---> Property Placeholder Returned for "+input);
        }

        public string Resolve<TMessage>(TMessage message) where TMessage : IMessage
        {
            var insideText = input.ExtractPlaceholderVariable();
            if (insideText == null) return wildcard;

            if (message == null) return wildcard;
            Type type = message.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var propName = property.Name;
                if (propName != null)
                {
                    Console.WriteLine($"----> Property : {propName}" );
                    if (propName.Equals(insideText, StringComparison.CurrentCultureIgnoreCase))
                    {                        
                        var propValue = property.GetValue(message);

                        Console.WriteLine($"---> Value : {propValue}" );
                        if (propValue == null) return wildcard;
                        return ((string)propValue);
                    }
                }
            }
            return wildcard;
        }

        public string Resolve<TMessage>() where TMessage : IMessage
        {
            return input;
        }
    }
    internal class CompositePlaceholder : IPlaceholder
    {
        private string _input;
        private List<IPlaceholder> _placeholders;
        public string Template => _input;

        public CompositePlaceholder(string input)
        {
            Console.WriteLine("---> Composite Placeholder Returned for "+input);
            _input = input;
            _placeholders = new List<IPlaceholder>();

            PlaceholderRegistery registery = new PlaceholderRegistery();

            List<string> textBloks = input.GetPlaceHolders();

            if (textBloks.Count == 0)
                _placeholders.Add(new PlainTextPlaceHolder(input));
            else
            {
                foreach (var textBlok in textBloks)
                {
                    var placeholder = registery.GetPlaceholder(textBlok);
                    _placeholders.Add(placeholder);
                }

            }
        }
        public string Resolve<TMessage>() where TMessage : IMessage
        {
            var input = Template;
            if(input== null) return string.Empty;
            if(input.Length == 0) return input;
            foreach (var item in _placeholders)
            {
                var value = item.Resolve<TMessage>();
                input = input.Replace(item.Template, value);
            }
            return input;
        }

        public string Resolve<TMessage>(TMessage message) where TMessage : IMessage
        {
            var input = Template;
            if(input== null) return string.Empty;
            if(input.Length==0) return input;
            foreach (var item in _placeholders)
            {
                var value = item.Resolve<TMessage>(message);
                input = input.Replace(item.Template, value);
            }
            Console.WriteLine("---> Composite Placehoder resolved to : " + input);
            return input;
        }
    }
}