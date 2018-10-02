using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[ExecuteInEditMode]
[System.Serializable]
public class NodeGroup : MonoBehaviour {
    [SerializeField]
    public List<Node> nodeList;//TODO Como usar la lista para manejar los removeat y los addat
    public Node nodePrefab;
    public int _count;
    public Node _first;
    public Node _last;

    public void AddFirst( Node value) {
        if ( _count >= 0 ) {
            _first = value;
            _last = _first;
        } else {
            var oldfirst = _first;
            value.previous = _last;
            value.next = oldfirst;
            _first = value;
            oldfirst.previous = _first;
            _last.next = _first;
        }
        nodeList.Add(value);
        value.id = _count + 1;
        _count++;
    }
    public void AddLast( Node value ) {
        if ( _count < 1 ) {
            AddFirst(value);
            return;
        } else if ( _count == 1 ) {
            value.previous = _first;
            value.next = _first;
            _last = value;
            _first.next = _last;
            _first.previous = _last;
            nodeList.Add(value);
        } else {
            var oldLast = _last;
            value.previous = oldLast;
            value.next = _first;
            _last = value;
            oldLast.next = _last;
            _first.previous = _last;
            nodeList.Add(value);
        }
            value.id = _count + 1;
        _count++;
    }
    public void Clear() {
        _count = 0;
        _first = null;
        _last = null;
        foreach ( Node n in nodeList ) {
            if (n)
            DestroyImmediate(n.gameObject);
        }
        nodeList = new List<Node>();
        Debug.Log("NodosLimpiados");
    }
    public Node NewNode() {
        Node n = Instantiate(nodePrefab);
        AddLast(n);
        return n;
    }
    public bool Contains(Node node) {
        return nodeList.Contains(node);
    }
    public void InsertAt( int index, Node value ) {
        if ( index > _count ) throw new System.IndexOutOfRangeException("El índice dado excede al maximo de elementos contenidos en la Lista.");

        if ( index == 0 ) {
            AddFirst(value);
            return;
        }
        if ( index == _count - 1 ) {
            AddLast(value);
            return;
        }

        var current = _first;
        for ( int i = 0; i <= index; i++ ) {
            if ( i == index ) {     //TODO Chechear si esto funciona
                var oldprevious = current.previous;
                value.previous = current.previous;
                value.next = current;
                oldprevious.next = value;
                current.previous = value;
            }
            current = current.next;
        }
        _count++;
        nodeList.Add(value);
    }
    public void RemoveAt( int index ) {
        if ( index > _count || index < 0 ) throw new System.IndexOutOfRangeException("El índice dado no es válido, o es mayor a la cantidad de elementos disponibles");

        if ( index == 0 ) {
            RemoveFirst();
            return;
        }
        if ( index == _count - 1 ) {
            RemoveLast();
            return;
        }

        var current = _first;
        for ( int i = 0; i <= index; i++ ) {
            if ( i == index ) {
                current.previous.next = current.next;
                nodeList.Remove(current);
            }
            current = current.next;
        }

    }
    public void RemoveFirst() {
        _first = _first.next;
        _first.previous = _last;
        nodeList.Remove(_first);
        _count--;
    }
    public void RemoveLast() {
        if ( _count == 1 ) {
            Clear();
            return;
        }
        _last = _last.previous;
        _last.next = _first;
        nodeList.Remove(_last);
    }
}
