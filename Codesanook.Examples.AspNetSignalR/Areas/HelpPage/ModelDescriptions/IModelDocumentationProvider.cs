using System;
using System.Reflection;

namespace Codesanook.Examples.AspNetSignalR.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}