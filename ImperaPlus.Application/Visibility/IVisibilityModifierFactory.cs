using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImperaPlus.Application.Visibility
{
    public interface IVisibilityModifierFactory
    {
        IVisibilityModifier Construct(Domain.Enums.VisibilityModifierType visibilityModifierType);
    }
}
