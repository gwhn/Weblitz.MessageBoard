using System;
using System.Web;

namespace Weblitz.MessageBoard.Infrastructure.StructureMap
{
    public class ObjectRegistryModule : IHttpModule
    {
        private static bool _isRegistered;
        private static readonly object SyncRoot = new object();

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        public void Dispose()
        {
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            if (_isRegistered) return;
            
            lock (SyncRoot)
            {
                if (_isRegistered) return;

                DependencyRegistration.Register();

                _isRegistered = true;
            }
        }
    }
}