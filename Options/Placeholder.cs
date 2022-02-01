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
}