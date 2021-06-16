using System;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Persistence
{
    internal sealed class SwitchGameRepositoryFactory : ISwitchGameRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SwitchGameRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISwitchGameRepository Create()
        {
            return _serviceProvider.GetRequiredService<ISwitchGameRepository>();
        }
    }
}