using Cloud4Feed.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.BackgroundTask.Startup
{
    public class MigrationRunner : IStartupTask
    {
        private readonly ECommerceContext eCommerceContext;

        public MigrationRunner(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(CancellationToken cancellationToken = default)
        {

            await eCommerceContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
