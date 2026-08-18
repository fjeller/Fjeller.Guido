using System.Reflection;

namespace Fjeller.Guido;

/// <summary>
/// Provides GUID creation helpers for APIs not available in .NET 8.
/// </summary>
internal static class GuidHelper
{
	/// <summary>
	/// Cached reflection handle to <c>Guid.CreateVersion7()</c> (.NET 9+), or <see langword="null"/>
	/// if the runtime hosting this extension does not expose it.
	/// </summary>
	private static readonly Func<Guid>? _nativeCreateVersion7 = ResolveNativeCreateVersion7();

	/// <summary>
	/// Creates a version 7 GUID (RFC 9562).
	/// Uses <c>Guid.CreateVersion7()</c> when running on a .NET 9+ runtime; otherwise falls back
	/// to a manual implementation using a Unix millisecond timestamp and random data.
	/// </summary>
	internal static Guid CreateVersion7()
	{
		if ( _nativeCreateVersion7 is not null )
		{
			return _nativeCreateVersion7();
		}

		long unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		Span<byte> rand = stackalloc byte[10];
		Random.Shared.NextBytes( rand );

		// Bits 0–47: Unix timestamp in milliseconds
		int   data1 = (int)(unixMs >> 16);
		short data2 = (short)(unixMs & 0xFFFF);

		// Bits 48–63: version nibble (7) + 12 random bits (rand_a)
		short data3 = (short)(0x7000 | (((rand[0] << 8) | rand[1]) & 0x0FFF));

		// Bits 64–127: variant (0b10) in the high 2 bits + 62 random bits (rand_b)
		byte b0 = (byte)(0x80 | (rand[2] & 0x3F));

		return new Guid( data1, data2, data3, b0, rand[3], rand[4], rand[5], rand[6], rand[7], rand[8], rand[9] );
	}

	/// <summary>
	/// Attempts to resolve <c>Guid.CreateVersion7()</c> via reflection, since it is not part of the
	/// .NET 8 API surface but may be present when the extension runs on a newer host runtime.
	/// </summary>
	/// <returns>
	/// A delegate invoking the native method, or <see langword="null"/> if it is not available.
	/// </returns>
	private static Func<Guid>? ResolveNativeCreateVersion7()
	{
		MethodInfo? method = typeof( Guid ).GetMethod(
			"CreateVersion7",
			BindingFlags.Public | BindingFlags.Static,
			Type.EmptyTypes );

		return method?.CreateDelegate<Func<Guid>>();
	}
}
