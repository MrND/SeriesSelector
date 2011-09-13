using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using SeriesSelector.Services;

namespace SeriesSelector.Frame
{
    public class BootStrapper
    {
        private static CompositionContainer _container;

        public static void Bootstrap()
        {
            var dirCat = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory, "Ser*");
            
            _container = new CompositionContainer(dirCat);
            var startups = ResolveAll<IStartup>();
            foreach (var s in startups)
                s.Start();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.GetExportedValue<T>();
        }

        public static T Resolve<T>(string contractName) where T : class
        {
            return _container.GetExportedValue<T>(contractName);
        }

        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return _container.GetExportedValues<T>();
        }

        public static IEnumerable<T> ResolveAll<T>(string contractName) where T : class
        {
            return _container.GetExportedValues<T>(contractName);
        }

        public static void SatisfyImports(object liveObject)
        {
            if (liveObject == null)
                throw new ArgumentNullException("liveObject");

            var objAsPart = AttributedModelServices.CreatePart(liveObject);
            _container.SatisfyImportsOnce(objAsPart);
        }
    }
}