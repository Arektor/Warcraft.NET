using Warcraft.NET.Files.ADT.Flags;
using Warcraft.NET.Files.Interfaces;
using System.IO;
using Warcraft.NET.Attribute;
using System;

namespace Warcraft.NET.Files.ADT.Chunks
{
    /// <summary>
    /// MHDR Chunk - Contains offsets in the file for specific chunks. WoW only takes this for parsing the ADT file.
    /// </summary>
    [AutoDocChunk(AutoDocChunkVersionHelper.VersionAll)]
    public class MHDR : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MHDR";

        /// <summary>
        /// Gets or sets flags for this ADT.
        /// </summary>
        public MHDRFlags Flags { get; set; } = (MHDRFlags)0;

        /// <summary>
        /// Gets or sets offset into the file where the MCIN Chunk can be found.
        /// </summary>
        public uint MCINOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MTEX Chunk can be found.
        /// </summary>
        public uint MTEXOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MMDX Chunk can be found.
        /// </summary>
        public uint MMDXOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MMID Chunk can be found.
        /// </summary>
        public uint MMIDOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MWMO Chunk can be found.
        /// </summary>
        public uint MWMOOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MWID Chunk can be found.
        /// </summary>
        public uint MWIDOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MMDF Chunk can be found.
        /// </summary>
        public uint MDDFOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MODF Chunk can be found.
        /// </summary>
        public uint MODFOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MFBO Chunk can be found. This is only set if the Flags contains MDHR_MFBO.
        /// </summary>
        public uint MFBOOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MH2O Chunk can be found.
        /// </summary>
        public uint MH2OOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets offset into the file where the MTXF Chunk can be found.
        /// </summary>
        public uint MTXFOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets mamp value (Cata+)
        /// </summary>
        public byte MAMPValue { get; set; } = 0;

        /// <summary>
        /// Gets or sets padding 1
        /// </summary>
        public byte Padding1 { get; set; } = 0;

        /// <summary>
        /// Gets or sets padding 2
        /// </summary>
        public byte Padding2 { get; set; } = 0;

        /// <summary>
        /// Gets or sets padding 3
        /// </summary>
        public byte Padding3 { get; set; } = 0;

        /// <summary>
        /// Gets or sets unused 1
        /// </summary>
        public uint Unused1 { get; set; } = 0;

        /// <summary>
        /// Gets or sets unused 2
        /// </summary>
        public uint Unused2 { get; set; } = 0;

        /// <summary>
        /// Gets or sets unused 3
        /// </summary>
        public uint Unused3 { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MHDR"/> class.
        /// </summary>
        public MHDR()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MHDR"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public MHDR(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            using (var br = new BinaryReader(ms))
            {
                Flags = (MHDRFlags)br.ReadInt32();

                MCINOffset = br.ReadUInt32();
                MTEXOffset = br.ReadUInt32();

                MMDXOffset = br.ReadUInt32();
                MMIDOffset = br.ReadUInt32();

                MWMOOffset = br.ReadUInt32();
                MWIDOffset = br.ReadUInt32();

                MDDFOffset = br.ReadUInt32();
                MODFOffset = br.ReadUInt32();

                MFBOOffset = br.ReadUInt32();
                MH2OOffset = br.ReadUInt32();
                MTXFOffset = br.ReadUInt32();

                MAMPValue = br.ReadByte();

                Padding1 = br.ReadByte();
                Padding2 = br.ReadByte();
                Padding3 = br.ReadByte();

                Unused1 = br.ReadUInt32();
                Unused2 = br.ReadUInt32();
                Unused3 = br.ReadUInt32();
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public uint GetSize()
        {
            return (uint)Serialize().Length;
        }

        /// <inheritdoc/>
        public byte[] Serialize(long offset = 0)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((uint)Flags);

                bw.Write(MCINOffset);
                bw.Write(MTEXOffset);

                bw.Write(MMDXOffset);
                bw.Write(MMIDOffset);

                bw.Write(MWMOOffset);
                bw.Write(MWIDOffset);

                bw.Write(MDDFOffset);
                bw.Write(MODFOffset);

                bw.Write(MFBOOffset);
                bw.Write(MH2OOffset);
                bw.Write(MTXFOffset);

                bw.Write(MAMPValue);

                bw.Write(Padding1);
                bw.Write(Padding2);
                bw.Write(Padding3);

                bw.Write(Unused1);
                bw.Write(Unused2);
                bw.Write(Unused3);

                return ms.ToArray();
            }
        }
    }
}
