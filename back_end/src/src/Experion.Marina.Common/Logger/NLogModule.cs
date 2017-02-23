using Autofac;
using Autofac.Core;
using NLog;
using System.Linq;

namespace Experion.Marina.Common
{
    public class NLogModule : Module
    {
        /// <summary>
        /// Attaches to component registration.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="registration">The registration.</param>
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        /// <summary>
        /// Called when [component preparing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PreparingEventArgs"/> instance containing the event data.</param>
        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var typePreparing = e.Component.Activator.LimitType;

            // By default, the name supplied to the logging instance is the name of the type in which it is being injected into.
            string loggerName = typePreparing.FullName;

            // If there is a class-level logger attribute,
            // then promote its supplied name value instead as the logger name to use.
            var loggerAttribute = (LoggerAttribute)typePreparing.GetCustomAttributes(typeof(LoggerAttribute), true).FirstOrDefault();
            if (loggerAttribute != null)
            {
                loggerName = loggerAttribute.Name;
            }

            e.Parameters = e.Parameters.Union(new Parameter[]
            {
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof (ILogger),
                    (p, i) =>
                    {
                        // If the parameter being injected has its own logger attribute, then promote its name value instead as the logger name to use.
                        loggerAttribute = (LoggerAttribute)
                        p.GetCustomAttributes(typeof(LoggerAttribute),true).FirstOrDefault();
                        if (loggerAttribute != null)
                        {
                            loggerName = loggerAttribute.Name;
                        }

                        // Return a new Logger instance for injection, parameterized with the most appropriate name which we have determined above.
                        return new MarinaLogger(typePreparing);
                    }),

                // Always make an unnamed instance of Logger available for use in delegate-based registration e.g.: Register((c,p) => new Foo(p.TypedAs<Logger>())
                new TypedParameter(typeof(ILogger), LogManager.GetLogger(loggerName))
            });
        }
    }
}
