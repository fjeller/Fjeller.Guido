using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Editor;
using System.Diagnostics;

namespace Fjeller.Guido;

/// <summary>
/// Command that inserts a new lowercase GUID, encased in curly braces, at the current caret position in the editor.
/// </summary>
[VisualStudioContribution]
internal class InsertLowercaseGuidWithBracesCommand : Command
{
	private readonly TraceSource logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="InsertLowercaseGuidWithBracesCommand"/> class.
	/// </summary>
	/// <param name="traceSource">Trace source instance to utilize.</param>
	public InsertLowercaseGuidWithBracesCommand( TraceSource traceSource )
	{
		this.logger = Requires.NotNull( traceSource, nameof( traceSource ) );
	}

	/// <inheritdoc />
#pragma warning disable CEE0027
	public override CommandConfiguration CommandConfiguration => new( "%InsertLowercaseGuidWithBracesCommand.DisplayName%" );
#pragma warning restore CEE0027

	/// <inheritdoc />
	public override async Task ExecuteCommandAsync( IClientContext context, CancellationToken cancellationToken )
	{
		using ITextViewSnapshot? textView = await this.Extensibility.Editor().GetActiveTextViewAsync( context, cancellationToken );

		if ( textView is null )
		{
			return;
		}

		string newGuid = Guid.NewGuid().ToString( "B" ).ToLower();

		await this.Extensibility.Editor().EditAsync(
			batch =>
			{
				textView.Document.AsEditable( batch ).Replace( textView.Selection.Extent, newGuid );
			},
			cancellationToken );
	}
}
