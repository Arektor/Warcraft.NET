using System.Collections.Generic;
using System.IO;
using Warcraft.NET.Attribute;
using Warcraft.NET.Extensions;
using Warcraft.NET.Files.Interfaces;
using Warcraft.NET.Files.Structures;

namespace Warcraft.NET.Files.M2.Chunks.TWW
{
    [AutoDocChunk(AutoDocChunkVersionHelper.VersionAfterTWW, AutoDocChunkVersionHelper.VersionBeforeMD)]
    public class PCOL : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "PCOL";
        
        /// <summary>
        /// Gets or sets the vertex positions
        /// </summary>
        public List<C3Vector> VertexPositions { get; set; }

        /// <summary>
        /// Gets or sets the face normals
        /// </summary>
        public List<C3Vector> FaceNormals { get; set; }
        
        /// <summary>
        /// Gets or sets the indices
        /// </summary>
        public List<ushort> Indices { get; set; }
        
        /// <summary>
        /// Gets or sets the flags (unknown meaning)
        /// </summary>
        public List<ushort> Flags { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PCOL"/>
        /// </summary>
        public PCOL() { }

        /// <summary> 
        /// Initializes a new instance of <see cref="PCOL"/>
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public PCOL(byte[] inData) => LoadBinaryData(inData);

        /// <inheritdoc />
        public string GetSignature() { return Signature; }

        /// <inheritdoc />
        public uint GetSize() { return (uint)Serialize().Length; }

        /// <inheritdoc />
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            using (var br = new BinaryReader(ms))
            {
                var vertexPositionCount = br.ReadUInt32();
                var vertexPositionOffset = br.ReadUInt32();
                var faceNormalCount = br.ReadUInt32();
                var faceNormalOffset = br.ReadUInt32();
                var indexCount = br.ReadUInt32();
                var indexOffset = br.ReadUInt32();
                var flagCount = br.ReadUInt32();
                var flagOffset = br.ReadUInt32();

                VertexPositions = br.ReadStructList<C3Vector>(vertexPositionCount, vertexPositionOffset);
                FaceNormals = br.ReadStructList<C3Vector>(faceNormalCount, faceNormalOffset);
                Indices = br.ReadStructList<ushort>(indexCount, indexOffset);
                Flags = br.ReadStructList<ushort>(flagCount, flagOffset);
            }
        }

        /// <inheritdoc />
        public byte[] Serialize(long offset = 0)
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((uint)VertexPositions.Count);
                bw.Write((uint)(32)); // vertex positions offset
                
                bw.Write((uint)FaceNormals.Count);
                bw.Write((uint)(32 + VertexPositions.Count * sizeof(float) * 3)); // face normals offset
                
                bw.Write((uint)Indices.Count);
                bw.Write((uint)(32 + VertexPositions.Count * sizeof(float) * 3 + FaceNormals.Count * sizeof(float) * 3)); // indices offset
                
                bw.Write((uint)Flags.Count);
                bw.Write((uint)(32 + VertexPositions.Count * sizeof(float) * 3 + FaceNormals.Count * sizeof(float) * 3 + Indices.Count * sizeof(ushort))); // flags offset
                
                foreach (var vertex in VertexPositions)
                    bw.WriteStruct(vertex);
                
                foreach (var normal in FaceNormals)
                    bw.WriteStruct(normal);
                
                foreach (var index in Indices)
                    bw.Write(index);
                
                foreach (var flag in Flags)
                    bw.Write(flag);
                
                return ms.ToArray();
            }
        }
    }
}