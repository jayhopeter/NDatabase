using System.Text;
using NDatabase.Btree;

namespace Test.NDatabase.Tool
{
    /// <summary>
    ///   an utility to display a btree
    /// </summary>
    internal sealed class BTreeDisplay
    {
        private StringBuilder[] _lines;
        private StringBuilder _result;

        public StringBuilder Build(IBTreeNode node, int height, bool withIds)
        {
            _lines = new StringBuilder[height];
            for (var i = 0; i < height; i++)
                _lines[i] = new StringBuilder();

            BuildDisplay(node, 0, "0", withIds);
            BuildRepresentation();

            return _result;
        }

        private void BuildRepresentation()
        {
            var maxLineSize = _lines[_lines.Length - 1].Length;
            _result = new StringBuilder();

            for (var i = 0; i < _lines.Length; i++)
                _result.Append(Format(_lines[i], i, maxLineSize)).Append("\n");
        }

        private static StringBuilder Format(StringBuilder line, int height, int maxLineSize)
        {
            var diff = maxLineSize - line.Length;
            var lineResult = new StringBuilder();
            lineResult.Append("h=").Append(height + 1).Append(":");
            lineResult.Append(Fill(diff / 2, ' '));
            lineResult.Append(line);
            lineResult.Append(Fill(diff / 2, ' '));
            return lineResult;
        }

        private static StringBuilder Fill(int size, char c)
        {
            var buffer = new StringBuilder();

            for (var i = 0; i < size; i++)
                buffer.Append(c);

            return buffer;
        }

        private void BuildDisplay(IBTreeNode node, int currentHeight, object parentId, bool withIds)
        {
            if (currentHeight > _lines.Length - 1)
                return;

            // get string buffer of this line
            var line = _lines[currentHeight];
            if (withIds)
                line.Append(node.GetId()).Append(":[");
            else
                line.Append("[");

            for (var i = 0; i < node.GetNbKeys(); i++)
            {
                if (i > 0)
                    line.Append(" , ");

                var keyAndValue = node.GetKeyAndValueAt(i);
                line.Append(keyAndValue.GetKey());
            }

            if (withIds)
                line.Append("]:").Append(node.GetParentId()).Append("/").Append(parentId).Append("    ");
            else
                line.Append("]  ");

            for (var i = 0; i < node.GetNbChildren(); i++)
            {
                var child = node.GetChildAt(i, false);

                if (child != null)
                    BuildDisplay(child, currentHeight + 1, node.GetId(), withIds);
                else
                    _lines[currentHeight + 1].Append(string.Concat("[Child {0} null!] ", (i + 1).ToString()));
            }
        }
    }
}
