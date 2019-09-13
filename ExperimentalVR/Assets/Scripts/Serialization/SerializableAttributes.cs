using System;

namespace Serialization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializableName : Attribute
    {
        private string _baseName;

        public SerializableName(string baseName)
        {
            _baseName = baseName;
        }

        public string BaseName
        {
            get => _baseName;
            set => _baseName = value;
        }
    }
}