using System;

interface IAssociation
{
    string Value
    {
        get;
    }
    int AncestorIndex
    {
        get;
    }
}

namespace AssocsConsole
{
    public class Association: IAssociation
    {
        public Association(string value, int ancestorIndex)
        {
            Value = value;
            AncestorIndex = ancestorIndex;
        }

        public string Value { get; }
        public int AncestorIndex { get; }
    }
}
