using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using System.Threading;

namespace ServiceStack.WebHost.Endpoints.Utils
{
    public static class FilterAttributeCache
    {
		private static Dictionary<Type, Func<IHasRequestFilter[]>> requestFilterAttributes
            = new Dictionary<Type, Func<IHasRequestFilter[]>>();

		private static Dictionary<Type, Func<IHasResponseFilter[]>> responseFilterAttributes
            = new Dictionary<Type, Func<IHasResponseFilter[]>>();

        public static IHasRequestFilter[] GetRequestFilterAttributes(Type requestDtoType)
        {
            IHasRequestFilter[] attrs;

            var attributes = new List<IHasRequestFilter>(
                (IHasRequestFilter[])requestDtoType.GetCustomAttributes(typeof(IHasRequestFilter), true));

            var serviceType = EndpointHost.ServiceManager.ServiceController.RequestServiceTypeMap[requestDtoType];
            attributes.AddRange(
                (IHasRequestFilter[])serviceType.GetCustomAttributes(typeof(IHasRequestFilter), true));

            attributes.Sort((x, y) => x.Priority - y.Priority);
            attrs = attributes.ToArray();

            return attrs;
        }

        public static IHasResponseFilter[] GetResponseFilterAttributes(Type responseDtoType)
        {
			IHasResponseFilter[] attrs;

			var attributes = new List<IHasResponseFilter>(
	            (IHasResponseFilter[])responseDtoType.GetCustomAttributes(typeof(IHasResponseFilter), true));

        	Type serviceType;
			EndpointHost.ServiceManager.ServiceController.ResponseServiceTypeMap.TryGetValue(responseDtoType, out serviceType);
			if (serviceType != null)
			{
				attributes.AddRange(
					(IHasResponseFilter[])serviceType.GetCustomAttributes(typeof(IHasResponseFilter), true));
			}

			attributes.Sort((x, y) => x.Priority - y.Priority);
			attrs = attributes.ToArray();

			return attrs;
        }
    }
}
