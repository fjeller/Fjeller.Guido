using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace Fjeller.Guido;

/// <summary>
/// Command that inserts a new GUID at the current caret position in the editor.
/// </summary>
[VisualStudioContribution]
internal class AddMenuCommand
{
	/// <summary>
	/// Places this command in a group inside the code editor context menu (right-click menu).
	/// </summary>
	[VisualStudioContribution]
	public static CommandGroupConfiguration EditorContextMenuGroup => new(
		GroupPlacement.VsctParent( new Guid( "{d309f791-903f-11d0-9efc-00a0c911004f}" ), id: 0x040D, priority: 0x0100 ) )
	{
		Children = [GroupChild.Menu( GuidoMenu )]
	};

	/// <summary>
	/// Submenu named "Guido" that appears inside the editor context menu.
	/// </summary>
	[VisualStudioContribution]
#pragma warning disable CEE0027
	public static MenuConfiguration GuidoMenu => new( "Guido" )
#pragma warning restore CEE0027
	{
		Children = [
			MenuChild.Menu( InsertGuidMenu ),
			MenuChild.Menu( InsertGuidV7Menu )
		]
	};

	/// <summary>
	/// Submenu named "Insert Guid" containing the standard GUID insertion commands.
	/// </summary>
	[VisualStudioContribution]
#pragma warning disable CEE0027
	public static MenuConfiguration InsertGuidMenu => new( "%InsertGuidMenu.DisplayName%" )
#pragma warning restore CEE0027
	{
		Children = [
			MenuChild.Command<InsertGuidCommand>(),
			MenuChild.Command<InsertLowercaseGuidCommand>(),
			MenuChild.Command<InsertGuidWithBracesCommand>(),
			MenuChild.Command<InsertLowercaseGuidWithBracesCommand>()
		]
	};

	/// <summary>
	/// Submenu named "Insert V7 Guid" containing the version 7 GUID insertion commands.
	/// </summary>
	[VisualStudioContribution]
#pragma warning disable CEE0027
	public static MenuConfiguration InsertGuidV7Menu => new( "%InsertGuidV7Menu.DisplayName%" )
#pragma warning restore CEE0027
	{
		Children = [
			MenuChild.Command<InsertGuidV7Command>(),
			MenuChild.Command<InsertLowercaseGuidV7Command>(),
			MenuChild.Command<InsertGuidV7WithBracesCommand>(),
			MenuChild.Command<InsertLowercaseGuidV7WithBracesCommand>()
		]
	};
}
