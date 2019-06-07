using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using React;
using React.Web;
using React.Web.Mvc;
using System;
using System.IO;
using System.Web;
using IHtmlHelper = System.Web.Mvc.HtmlHelper;

namespace ReactServerSideRendering.Helpers
{
    /// <summary>
    /// HTML Helpers for utilising React from an ASP.NET MVC application.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        private static IReactEnvironment reactEnvironment;

        /// <summary>
        /// Gets the React environment
        /// </summary>
        private static IReactEnvironment Environment
        {
            get
            {
                if (reactEnvironment != null) return reactEnvironment;

                ReactSiteConfiguration.Configuration
                    .SetLoadBabel(false)
                    .AddScriptWithoutTransform("~/scripts/server.bundle.js");

                JsEngineSwitcher.Current.DefaultEngineName = V8JsEngine.EngineName;
                JsEngineSwitcher.Current.EngineFactories.AddV8();

                // One instance shared for the whole app
                var switcher = JsEngineSwitcher.Current;//Resolve by return an instance
                var config = ReactSiteConfiguration.Configuration;// Resolve by return an instance
                var cache = new AspNetCache(HttpRuntime.Cache);//AsPerRequestSingleton();//safe to singleton

                var fileCacheHash = new FileCacheHash();//AsPerRequestSingleton()//
                var fileSystem = new AspNetFileSystem(); //AsPerRequestSingleton(); //safe to singleton

                var factor = new JavaScriptEngineFactory(switcher, config, cache, fileSystem); //AsSingleton();
                var reactIdGenerator = new ReactIdGenerator();//AsSingleton()

                reactEnvironment = new ReactEnvironment(factor, config, cache, fileSystem, fileCacheHash, reactIdGenerator);//AsPerRequestSingleton()
                return reactEnvironment;
            }
        }

        /// <summary>
        /// Renders the specified React component
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialise the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialisation code. Defaults to <c>false</c></param>
        /// <param name="serverOnly">Skip rendering React specific data-attributes, container and client-side initialisation during server side rendering. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <param name="exceptionHandler">A custom exception handler that will be called if a component throws during a render. Args: (Exception ex, string componentName, string containerId)</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString React<T>(
            this IHtmlHelper htmlHelper,
            string componentName,
            T props,
            string htmlTag = null,
            string containerId = null,
            bool clientOnly = false,
            bool serverOnly = false,
            string containerClass = null,
            Action<Exception, string, string> exceptionHandler = null
        )
        {
            return new ActionHtmlString(writer =>
            {
                try
                {
                    var reactComponent = Environment.CreateComponent(componentName, props, containerId, clientOnly, serverOnly);
                    if (!string.IsNullOrEmpty(htmlTag))
                    {
                        reactComponent.ContainerTag = htmlTag;
                    }

                    if (!string.IsNullOrEmpty(containerClass))
                    {
                        reactComponent.ContainerClass = containerClass;
                    }

                    reactComponent.RenderHtml(writer, clientOnly, serverOnly, exceptionHandler);
                }
                finally
                {
                    //Environment.ReturnEngineToPool();
                }
            });
        }

        /// <summary>
        /// Renders the specified React component, along with its client-side initialisation code.
        /// Normally you would use the <see cref="React{T}"/> method, but <see cref="ReactWithInit{T}"/>
        /// is useful when rendering self-contained partial views.
        /// </summary>
        /// <typeparam name="T">Type of the props</typeparam>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="componentName">Name of the component</param>
        /// <param name="props">Props to initialise the component with</param>
        /// <param name="htmlTag">HTML tag to wrap the component in. Defaults to &lt;div&gt;</param>
        /// <param name="containerId">ID to use for the container HTML tag. Defaults to an auto-generated ID</param>
        /// <param name="clientOnly">Skip rendering server-side and only output client-side initialisation code. Defaults to <c>false</c></param>
        /// <param name="containerClass">HTML class(es) to set on the container tag</param>
        /// <param name="exceptionHandler">A custom exception handler that will be called if a component throws during a render. Args: (Exception ex, string componentName, string containerId)</param>
        /// <returns>The component's HTML</returns>
        public static IHtmlString ReactWithInit<T>(
            this IHtmlHelper htmlHelper,
            string componentName,
            T props,
            string htmlTag = null,
            string containerId = null,
            bool clientOnly = false,
            string containerClass = null,
            Action<Exception, string, string> exceptionHandler = null
        )
        {
            return new ActionHtmlString(writer =>
            {
                try
                {
                    var reactComponent = Environment.CreateComponent(componentName, props, containerId, clientOnly);
                    if (!string.IsNullOrEmpty(htmlTag))
                    {
                        reactComponent.ContainerTag = htmlTag;
                    }

                    if (!string.IsNullOrEmpty(containerClass))
                    {
                        reactComponent.ContainerClass = containerClass;
                    }

                    reactComponent.RenderHtml(writer, clientOnly, exceptionHandler: exceptionHandler);
                    writer.WriteLine();
                    WriteScriptTag(writer, bodyWriter => reactComponent.RenderJavaScript(bodyWriter));
                }
                finally
                {
                    //    Environment.ReturnEngineToPool();
                }
            });
        }

        /// <summary>
        /// Renders the JavaScript required to initialise all components client-side. This will
        /// attach event handlers to the server-rendered HTML.
        /// </summary>
        /// <returns>JavaScript for all components</returns>
        public static IHtmlString ReactInitJavaScript(this IHtmlHelper htmlHelper, bool clientOnly = false)
        {
            return new ActionHtmlString(writer =>
            {
                try
                {
                    WriteScriptTag(writer, bodyWriter => Environment.GetInitJavaScript(bodyWriter, clientOnly));
                }
                finally
                {
                    //Environment.ReturnEngineToPool();
                }
            });
        }

        private static void WriteScriptTag(TextWriter writer, Action<TextWriter> bodyWriter)
        {
            writer.Write("<script");
            if (Environment.Configuration.ScriptNonceProvider != null)
            {
                writer.Write(" nonce=\"");
                writer.Write(Environment.Configuration.ScriptNonceProvider());
                writer.Write("\"");
            }

            writer.Write(">");

            bodyWriter(writer);

            writer.Write("</script>");
        }
    }
}
