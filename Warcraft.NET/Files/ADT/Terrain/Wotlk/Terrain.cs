using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Warcraft.NET.Attribute;
using Warcraft.NET.Files.ADT.Chunks;
using Warcraft.NET.Files.ADT.Chunks.Wotlk;
using Warcraft.NET.Files.ADT.Entries.Wotlk;

namespace Warcraft.NET.Files.ADT.Terrain.Wotlk
{
    [AutoDocFile("adt")]
    public class Terrain : TerrainBase
    {
        /// <summary>
        /// Gets or sets the contains a list of all textures referenced by this ADT.
        /// </summary>
        [ChunkOrder(3)]
        public MCIN MapChunkOffsets { get; set; }

        /// <summary>
        /// Gets or sets the contains a list of all textures referenced by this ADT.
        /// </summary>
        [ChunkOrder(4)]
        public MTEX Textures { get; set; }

        /// <summary>
        /// Gets or sets the contains M2 model indexes for the list in ADTModels (MMDX chunk).
        /// </summary>
        [ChunkOrder(5)]
        public MMDX Models { get; set; }

        /// <summary>
        /// Gets or sets the contains M2 model indexes for the list in ADTModels (MMDX chunk).
        /// </summary>
        [ChunkOrder(6)]
        public MMID ModelIndices { get; set; }

        /// <summary>
        /// Gets or sets the contains a list of all WMOs referenced by this ADT.
        /// </summary>
        [ChunkOrder(7)]
        public MWMO WorldModelObjects { get; set; }

        /// <summary>
        /// Gets or sets the contains WMO indexes for the list in ADTWMOs (MWMO chunk).
        /// </summary>
        [ChunkOrder(8)]
        public MWID WorldModelObjectIndices { get; set; }

        /// <summary>
        /// Gets or sets the contains position information for all M2 models in this ADT.
        /// </summary>
        [ChunkOrder(8)]
        public MDDF ModelPlacementInfo { get; set; }

        /// <summary>
        /// Gets or sets the contains position information for all WMO models in this ADT.
        /// </summary>
        [ChunkOrder(9)]
        public MODF WorldModelObjectPlacementInfo { get; set; }

        /// <summary>
        /// Gets or sets the water informations in this ADT.
        /// </summary>
        [ChunkOrder(10), ChunkOptional]
        public MH2O Water { get; set; }

        /// <summary>
        /// Gets or sets the contains an array of offsets where MCNKs are in the file.
        /// </summary>
        [ChunkOrder(11), ChunkArray(256)]
        public MCNK[] Chunks { get; set; }

        /// <summary>
        /// Gets or Sets a array of flags for entries in MTEX. Always same number of entries as MTEX.
        /// </summary>
        [ChunkOrder(101), ChunkOptional]
        public MTXF TextureFlags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wotlk.Terrain"/> class.
        /// </summary>
        public Terrain() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wotlk.Terrain"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public Terrain(byte[] inData) : base(inData)
        {
        }

        /// <summary>
        /// Recalculate the offsets and sizes of the various chunks.
        /// </summary>
        public void ComputeOffsets()
        {
            uint offsMHDRdata = 20; // = size of MVER chunk (12) + size of MHDR header (8)
            Dictionary<string, uint> chunkSizes = new()
            {
                ["MHDR"] = Header.GetSize(),
                ["MCIN"] = MapChunkOffsets.GetSize(),
                ["MTEX"] = Textures != null ? Textures.GetSize() : 0,
                ["MMDX"] = Models != null ? Models.GetSize() : 0,
                ["MMID"] = ModelIndices != null ? ModelIndices.GetSize() : 0,
                ["MWMO"] = WorldModelObjects != null ? WorldModelObjects.GetSize() : 0,
                ["MWID"] = WorldModelObjectIndices != null ? WorldModelObjectIndices.GetSize() : 0,
                ["MDDF"] = ModelPlacementInfo != null ? ModelPlacementInfo.GetSize() : 0,
                ["MODF"] = WorldModelObjectPlacementInfo != null ? WorldModelObjectPlacementInfo.GetSize() : 0,
                ["MH2O"] = Water != null ? Water.GetSize() : 0,
                ["MFBO"] = BoundingBox != null ? BoundingBox.GetSize() : 0,
                ["MTXF"] = TextureFlags != null ? TextureFlags.GetSize() : 0
            };

            uint offset = chunkSizes["MHDR"];
            Header.MCINOffset = offset; offset += chunkSizes["MCIN"] + 8; // +8 here refers to a chunk's header size. Since #GetSize only returns the
            Header.MTEXOffset = offset; offset += chunkSizes["MTEX"] + 8; // size of the chunk's data block, we need to add 8 more to account for the size of
            Header.MMDXOffset = offset; offset += chunkSizes["MMDX"] + 8; // the header.
            Header.MMIDOffset = offset; offset += chunkSizes["MMID"] + 8;
            Header.MWMOOffset = offset; offset += chunkSizes["MWMO"] + 8;
            Header.MWIDOffset = offset; offset += chunkSizes["MWID"] + 8;
            Header.MDDFOffset = offset; offset += chunkSizes["MDDF"] + 8;
            Header.MODFOffset = offset; offset += chunkSizes["MODF"] + 8;

            if (Water != null)
            {
                Header.MH2OOffset = offset; offset += chunkSizes["MH2O"] + 8;
            }
            else
                Header.MH2OOffset = 0;

            for (int i = 0; i < 256; i++)
            {
                var entryMCIN = MapChunkOffsets.Entries[i];
                entryMCIN.Adress = offset + offsMHDRdata; // MCINEntry addresses are absolute, so we need to include the MHDR data block offset
                entryMCIN.Size = Chunks[i].GetSize() + 8;
                offset += entryMCIN.Size;
            }

            if (BoundingBox != null)
            {
                Header.MFBOOffset = offset; offset += chunkSizes["MFBO"] + 8;
            }
            else
                Header.MFBOOffset = 0;

            if (TextureFlags != null)
            {
                Header.MTXFOffset = offset; offset += chunkSizes["MTXF"] + 8;
            }
            else
                Header.MTXFOffset = 0;
        }
    }
}
