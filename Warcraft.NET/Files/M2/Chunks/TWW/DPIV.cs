using Warcraft.NET.Files.Interfaces;

namespace Warcraft.NET.Files.M2.Chunks.TWW
{
    public class DPIV : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "DPIV";

        /// <summary>
        /// Gets or sets the DPIV data
        /// </summary>
        public byte[] Data = new byte[32];

        /// <summary>
        /// Initializes a new instance of <see cref="DPIV"/>
        /// </summary>
        public DPIV() { }

        /// <summary> 
        /// Initializes a new instance of <see cref="DPIV"/>
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public DPIV(byte[] inData) => LoadBinaryData(inData);

        /// <inheritdoc />
        public string GetSignature() { return Signature; }

        /// <inheritdoc />
        public uint GetSize() { return (uint)Serialize().Length; }

        /// <inheritdoc />
        public void LoadBinaryData(byte[] inData)
        {
            Data = inData;
        }

        /// <inheritdoc />
        public byte[] Serialize(long offset = 0)
        {
            return Data;
        }
    }
}