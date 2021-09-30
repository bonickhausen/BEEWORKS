public interface IInteractable
{
	bool CanUse(Pawn pawn);

	void Use(Pawn pawn);

	string GetInteractionMessage();
}

public interface IMouseLockable
{
	bool ShouldShowMouse();
}