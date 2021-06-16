using System.Globalization;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface ISwitchGameMerger
    {
        SwitchGame MergeLibrary(SwitchGame targetEntity, SwitchGame sourceEntity);
        SwitchGame MergePrice(CultureInfo culture, SwitchGame targetEntity, SwitchGamePrice price);
    }
}
