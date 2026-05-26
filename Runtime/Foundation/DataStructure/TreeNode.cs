using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XiheFramework.Runtime.Utility.DataStructure {
    public class TreeNode<T> {
        private readonly List<TreeNode<T>> m_Children = new();

        public TreeNode(T value) {
            Value = value;
        }

        public TreeNode<T> this[int i] => m_Children[i];

        public TreeNode<T> Parent { get; private set; }

        public T Value { get; set; }

        public ReadOnlyCollection<TreeNode<T>> Children => m_Children.AsReadOnly();

        public TreeNode<T> AddChild(T value) {
            var node = new TreeNode<T>(value) { Parent = this };
            m_Children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values) {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node) {
            return m_Children.Remove(node);
        }

        public void Traverse(Action<T> action) {
            action(Value);
            foreach (var child in m_Children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten() {
            return new[] { Value }.Concat(m_Children.SelectMany(x => x.Flatten()));
        }
    }
}