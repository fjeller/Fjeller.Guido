
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace Fjeller.Guido;
/// <summary>
/// Extension entrypoint for the VisualStudio.Extensibility extension.
/// </summary>
[VisualStudioContribution]
internal class ExtensionEntrypoint : Extension
{
	/// <inheritdoc/>
	public override ExtensionConfiguration ExtensionConfiguration => new()
	{
		Metadata = new(
				id: "Fjeller.Guido.bab7ea95-46ff-41e7-9121-acff60c47f56",
				version: this.ExtensionAssemblyVersion,
				publisherName: "fjeller",
				displayName: "Fjeller.Guido",
				description: "A small extension that allows you to add guids to the sourcecode without any other tooling" )
		{
			License = "LICENSE.txt",
			MoreInfo = "https://github.com/fjeller/Fjeller.Guido",
			Icon = "icon.png",
			Tags = [ "Guid", "GUID", "UUID", "Editor", "Productivity", "Code Generation", "Snippet" ],
		}
	};

	/// <inheritdoc />
	protected override void InitializeServices( IServiceCollection serviceCollection )
	{
		base.InitializeServices( serviceCollection );

		// You can configure dependency injection here by adding services to the serviceCollection.
	}
}
