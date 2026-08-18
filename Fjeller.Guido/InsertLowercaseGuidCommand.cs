using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Fjeller.Guido;

[VisualStudioContribution]
internal class InsertLowercaseGuidCommand : Command
{
	private readonly TraceSource logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="InsertGuidV7Command"/> class.
	/// </summary>
	/// <param name="traceSource">Trace source instance to utilize.</param>
	public InsertLowercaseGuidCommand( TraceSource traceSource )
	{
		this.logger = Requires.NotNull( traceSource, nameof( traceSource ) );
	}

	/// <inheritdoc />
#pragma warning disable CEE0027
	public override CommandConfiguration CommandConfiguration => new( "%InsertLowercaseGuidCommand.DisplayName%" );
#pragma warning restore CEE0027

	/// <inheritdoc />
	public override async Task ExecuteCommandAsync( IClientContext context, CancellationToken cancellationToken )
	{
		using ITextViewSnapshot? textView = await this.Extensibility.Editor().GetActiveTextViewAsync( context, cancellationToken );

		if ( textView is null )
		{
			return;
		}

		string newGuid = Guid.NewGuid().ToString().ToLower();

		await this.Extensibility.Editor().EditAsync(
			batch =>
			{
				textView.Document.AsEditable( batch ).Replace( textView.Selection.Extent, newGuid );
			},
			cancellationToken );
	}
}
