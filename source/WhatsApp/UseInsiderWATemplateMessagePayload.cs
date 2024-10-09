using System.Collections.Generic;
using System.Linq;

namespace Webhook.WhatsApp
{
    internal class UseInsiderWATemplateMessagePayload
    {
        public List<TemplateMessage> Messages { get; set; } = new();

        public static UseInsiderWATemplateMessagePayload FactoryFrom(string phone, string templateName, List<string> variables)
        {
            UseInsiderWATemplateMessagePayload payload = new();

            payload.Messages.Add(TemplateMessage.FactoryFrom(phone, templateName, variables));

            return payload;
        }
    } 

    internal class TemplateMessage
    {
        public string PhoneNumber { get; set; }
        public Message Message { get; set; } = new();

        internal static TemplateMessage FactoryFrom(string phone, string templateName, List<string> variables)
        {
            TemplateMessage  factored = new()
            {
                //Number should be in the international format
                PhoneNumber = phone.Replace(" ", "")
                                   .Replace("(", "")
                                   .Replace(")", "")
                                   .Replace("-", "")
            };

            factored.Message.Template.Name = templateName;

            Component component = new()
            {
                Parameters = variables.Select(v => new Parameter() { Text = v }).ToList()
            };

            factored.Message.Template.Components.Add(component);

            return factored;
        }
    }

    public class Message
    {
        public string Type { get; } = "template";
        public Template Template { get; set; } = new();
    }

    public class Template
    {
        public string Name { get; set; }
        public Language Language { get; set; } = new();
        public List<Component> Components { get; set; } = new();
    }

    public class Language
    {
        public string Code { get; } = "pt_BR";
    }

    public class Component
    {
        public string Type { get; } = "body";
        public List<Parameter> Parameters { get; set; } = new();
    }

    public class Parameter
    {
        public string Type { get; } = "text";
        public string Text { get; set; }
    }
}