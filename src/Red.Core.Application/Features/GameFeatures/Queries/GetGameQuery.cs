using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Queries
{
    public class GetGameQuery : IQuery<SwitchGame?>
    {
        public string FsId { get; }

        public GetGameQuery(string fsId)
        {
            FsId = fsId;
        }
    }
}
