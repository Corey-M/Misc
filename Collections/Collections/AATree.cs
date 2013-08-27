/*
 * AATree https://github.com/Corey-M/Misc
 * 
 * Copyright (c) 2013 Corey Murtagh
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */

/*
 * Attributions:
 * 
 * Code for this class was adapted from the following sources:
 * 
 * 1.	Author:		Aleksey Demakov
 *		Title:		Balanced Search Trees Made Simple (in C#)
 *		Source:		Aleksey Demakov's Web Corner
 *		URL:		http://demakov.com/snippets/aatree.html
 *		Licence:	Unknown
 *		(Archived by WebCite® at http://www.webcitation.org/6JAzpyrdW)
 *		
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CoreyM.Collections
{
	public class AATree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		[DebuggerDisplay("{key}\t-> {value}")]
		public class Node
		{
			// node internal data
			internal int level;
			public Node Parent { get; internal set; }
			internal Node left;
			internal Node right;

			// user data
			internal TKey key;
			internal TValue value;

			// constuctor for the sentinel node
			internal Node()
			{
				this.level = 0;
				this.Parent = null;
				this.left = this;
				this.right = this;
			}

			// constuctor for regular nodes (that all start life as leaf nodes)
			internal Node(Node parent, TKey key, TValue value, Node sentinel)
			{
				this.level = 1;
				this.Parent = parent;
				this.left = sentinel;
				this.right = sentinel;
				this.key = key;
				this.value = value;
			}

			internal KeyValuePair<TKey, TValue> KeyValuePair
			{
				get { return new KeyValuePair<TKey, TValue>(key, value); }
			}

			internal Node LeftMost
			{
				get
				{
					if (level < 1)
						return null;
					Node curr = this;
					while (curr.left.level > 0)
						curr = curr.left;
					return curr;
				}
			}

			internal Node RightMost
			{
				get
				{
					if (level < 1)
						return null;
					Node curr = this;
					while (curr.right.level > 0)
						curr = curr.right;
					return curr;
				}
			}

			internal Node next
			{
				get
				{
					if (right.level < 1)
					{
						Node node = this;
						while (true)
						{
							if (node.Parent == null)
								return null;
							if (node == node.Parent.left)
								return node.Parent;
							node = node.Parent;
						}
					}
					else
						return right.LeftMost;
				}
			}

			internal Node Previous
			{
				get
				{
					if (left.level < 1)
					{
						Node node = this;
						while (true)
						{
							if (node.Parent == null)
								return null;
							if (node == node.Parent.right)
								return node.Parent;
							node = node.Parent;
						}
					}
					else
						return left.RightMost;
				}
			}

			#region Contract
			[ContractInvariantMethod]
			private void ObjectInvariant()
			{
				Contract.Invariant(left.level < 1 || left.Parent == this);
				Contract.Invariant(left.level < level);
				Contract.Invariant(right.level < 1 || right.Parent == this);
			}
			#endregion
		}

		private IComparer<TKey> KeyComparer;
		private IComparer<TValue> ValueComparer;

		Node root;
		Node sentinel;
		Node deleted;

		private AATree(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
		{
			KeyComparer = keyComparer;
			ValueComparer = valueComparer;

			root = sentinel = new Node();
			deleted = null;
		}

		public AATree()
			: this(Comparer<TKey>.Default, null)
		{ }

		public AATree(IComparer<TValue> valComparer)
			: this(null, valComparer)
		{ }

		public AATree(IComparer<TKey> keyComparer)
			: this(keyComparer, null)
		{ }

		internal int Compare(Node l, Node r)
		{
			if (ValueComparer != null)
				return ValueComparer.Compare(l.value, r.value);

			return (KeyComparer ?? Comparer<TKey>.Default).Compare(l.key, r.key);
		}

		internal int Compare(TKey l, Node r)
		{
			return (KeyComparer ?? Comparer<TKey>.Default).Compare(l, r.key);
		}

		internal int Compare(TKey lk, TValue lv, Node r)
		{
			if (ValueComparer != null)
				return ValueComparer.Compare(lv, r.value);
			return (KeyComparer ?? Comparer<TKey>.Default).Compare(lk, r.key);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (root.level < 1)
				yield break;

			Node curr = root.LeftMost;
			do
			{
				yield return curr.KeyValuePair;
				curr = curr.next;

			} while (curr != null);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
		}

		public IEnumerable<TKey> Keys
		{
			get
			{
				return this.Select(kv => kv.Key);
			}
		}

		public IEnumerable<TValue> Values
		{
			get
			{
				return this.Select(kv => kv.Value);
			}
		}

		/// <summary>Conditional rotate?</summary>
		/// <param name="node"></param>
		private void Skew(ref Node node)
		{
			if (node.level == node.left.level)
			{
				// [a L> b R> c] => [b R> a L> c]
				Node a = node;
				Node b = node.left;
				Node c = b.right;

				Node p = a.Parent;

				// swap a and b
				b.Parent = p;
				b.right = a;

				a.Parent = b;
				a.left = c;

				if (c.level > 0)
					c.Parent = a;

				node = b;
			}
		}

		private void Split(ref Node node)
		{
			if (node.right.right.level == node.level)
			{
				// [a R> b L> c] => [b L> a R> c]
				Node a = node;
				Node b = node.right;
				Node c = b.left;
				Node p = a.Parent;

				// swap a and b
				b.Parent = p;
				b.left = a;

				a.Parent = b;
				a.right = c;

				if (c.level > 0)
					c.Parent = a;

				b.level++;
				node = b;
			}
		}

		private bool Insert(ref Node node, TKey key, TValue value, Node parent = null, int depth = 1)
		{
			Contract.Requires(depth < 100);

			if (node == sentinel)
			{
				node = new Node(parent, key, value, sentinel);
				return true;
			}

			//int compare = key.CompareTo(node.key);
			int compare = Compare(key, value, node);
			if (compare < 0)
			{
				if (!Insert(ref node.left, key, value, node, depth + 1))
				{
					return false;
				}
			}
			else if (compare > 0)
			{
				if (!Insert(ref node.right, key, value, node, depth + 1))
				{
					return false;
				}
			}
			else
			{
				return false;
			}

			Skew(ref node);
			Split(ref node);

			return true;
		}

		private bool Delete(ref Node node, TKey key)
		{
			Contract.Requires(node != null);
			
			if (node == sentinel)
				return (deleted != null);

			int compare = Compare(key, node);
			if (compare < 0)
			{
				if (!Delete(ref node.left, key))
				{
					return false;
				}
			}
			else
			{
				if (compare == 0)
				{
					deleted = node;
				}
				if (!Delete(ref node.right, key))
				{
					return false;
				}
			}

			if (deleted != null)
			{
				deleted.key = node.key;
				deleted.value = node.value;
				deleted = null;
				node = node.right;
			}
			else if (node.left.level < node.level - 1
					|| node.right.level < node.level - 1)
			{
				--node.level;
				if (node.right.level > node.level)
				{
					node.right.level = node.level;
				}
				Skew(ref node);
				Skew(ref node.right);
				Skew(ref node.right.right);
				Split(ref node);
				Split(ref node.right);
			}

			return true;
		}

		private Node Search(Node node, TKey key)
		{
			if (node == sentinel)
			{
				return null;
			}

			//int compare = key.CompareTo(node.key);
			int compare = Compare(key, node);
			if (compare < 0)
			{
				return Search(node.left, key);
			}
			else if (compare > 0)
			{
				return Search(node.right, key);
			}
			else
			{
				return node;
			}
		}

		public bool Add(TKey key, TValue value)
		{
			return Insert(ref root, key, value);
		}

		public bool Remove(TKey key)
		{
			return Delete(ref root, key);
		}

		public TValue this[TKey key]
		{
			get
			{
				Node node = Search(root, key);
				return node == null ? default(TValue) : node.value;
			}
			set
			{
				Node node = Search(root, key);
				if (node == null)
				{
					Add(key, value);
				}
				else
				{
					node.value = value;
				}
			}
		}

		public TValue First
		{
			get
			{
				Node curr = root;
				if (curr.level < 1)
					return default(TValue);

				while (curr.left.level > 0)
					curr = curr.left;

				return curr.value;
			}
		}
	}
}
