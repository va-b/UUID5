using System;
using System.Security.Cryptography;
using System.Text;

namespace UUID5
{
    /// <summary>
    /// Name based UUID generator according to RFC4122.
    /// This is capable of generating Version-5 (with SHA-1 hashing) variant of RFC4122 UUID.
    /// </summary>
    public sealed class NameBasedGenerator : IDisposable
    {
        private static readonly Guid[] NameSpaceGuids = {
            Guid.Empty,
            Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), // DNS
            Guid.Parse("6ba7b811-9dad-11d1-80b4-00c04fd430c8"), // URL
            Guid.Parse("6ba7b812-9dad-11d1-80b4-00c04fd430c8"), // IOD
            Guid.Parse("6ba7b814-9dad-11d1-80b4-00c04fd430c8") // X500
        };

        private HashAlgorithm _hashAlgorithm;
        
        public NameBasedGenerator()
        {
            _hashAlgorithm = SHA1.Create();
        }
        
        public Guid GenerateGuid(string name) => GenerateGuid(UUIDNameSpace.None, name);
        public Guid GenerateGuid(UUIDNameSpace nameSpace, string name) => GenerateGuid(NameSpaceGuids[(int)nameSpace], name);
        
        public Guid GenerateGuid(Guid customNamespaceGuid, string name)
        {
            var nsBytes = Guid.Empty == customNamespaceGuid ? new byte[0] : customNamespaceGuid.ToByteArray();
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var data = new byte[nsBytes.Length + nameBytes.Length];
            if(nsBytes.Length > 0)
            {
                Array.Copy(nsBytes, data, nsBytes.Length);
            }
            Array.Copy(nameBytes, 0, data, nsBytes.Length, nameBytes.Length);

            var result = _hashAlgorithm
                .ComputeHash(data)
                .TrimTo16Bytes()
                .AddMarkers();

            return new Guid(result);
        }

        public void Dispose()
        {
            if (_hashAlgorithm != null)
            {
                _hashAlgorithm.Dispose();
                _hashAlgorithm = null;
            }
        }

        ~NameBasedGenerator()
        {
            _hashAlgorithm?.Dispose();
        }
    }
}
