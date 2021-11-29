using System;
using System.Collections.Generic;

interface IWordWorker
{
    void AddAssoc(string assoc);
    bool IsComplete();
    string GetResult();
    IList<string> GetAssociationSource();
    string MainWord { get; }
}


namespace AssocsConsole
{
    public class WordWorker : IWordWorker
    {
        private readonly int _targetDepth;
        private int _maxSize;
        private List<IAssociation> _associations;

        public WordWorker(string mainWord, int targetDepth = 4)
        {
            MainWord = mainWord;
            _targetDepth = targetDepth;
            double size = 0;
            for (int i = targetDepth; i >= 0; i--)
            {
                size += Math.Pow(2, i);
            }
            _maxSize = Convert.ToInt32(size);
            _associations = new List<IAssociation>();
        }
        public string MainWord { get; }

        private int _getStartIndexForDepth(int depth)
        {
            int start = 0;
            int odometer = 0;
            while (odometer < depth)
            {

                start += Convert.ToInt32(Math.Pow(2, _targetDepth - odometer));
                odometer++;
            }
            return start;
        }


        private int _getCurrentDepth()
        {
            int currentPosition = _associations.Count;
            int depth = 0;
            double sum = Math.Pow(2, _targetDepth);
            while (currentPosition > sum)
            {
                depth++;
                sum += Math.Pow(2, _targetDepth - depth);
            }
            return depth;
        }

        private int _getNextAncestor()
        {
            int currentDepth = _getCurrentDepth();
            int positionAtLevel = _associations.Count - _getStartIndexForDepth(currentDepth);
            int ancestorLevelStart = _getStartIndexForDepth(currentDepth + 1);

            return ancestorLevelStart + (positionAtLevel / 2);
        }

        public void AddAssoc(string assoc)
        {

            int child = _getNextAncestor();
            _associations.Add(new Association(assoc, child));
        }

        public string GetResult()
        {
            return _associations.FindLast(assoc => assoc != null).Value;
        }

        public bool IsComplete()
        {
            return _associations.Count >= _maxSize;
        }

        public IList<string> GetAssociationSource()
        {
            if (_associations.Count < Math.Pow(2, _targetDepth))
            {
                var result = new List<string>();
                result.Add(MainWord);
                return result;
            }
            return _associations.FindAll(assoc => assoc.AncestorIndex == _associations.Count).ConvertAll(assoc => assoc.Value);
        }
    }
}
