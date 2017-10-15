namespace ImperaPlus.Application.Visibility
{
    public interface IVisibilityModifierFactory
    {
        IVisibilityModifier Construct(Domain.Enums.VisibilityModifierType visibilityModifierType);
    }
}
