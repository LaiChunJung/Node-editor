using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NodeSystem
{
    public class EditorSaveNodeData : ScriptableObject {
        public List<NodeData> NodeDatas;
        public List<int> NodeCPIndex;
        public List<int> ConnectionIndexIn;
        public List<int> ConnectionIndexOut;
        public int NumberOfCP;
        public Vector2 offset;

        public void init(List<NodeData> iNodeDatas, List<int> iNodeCPIndex, List<int> ConnectionIndexIn, List<int> ConnectionIndexOut, int NumberOfCP, Vector2 offset) {
            this.NodeDatas = iNodeDatas;
            this.NodeCPIndex = iNodeCPIndex;
            this.ConnectionIndexIn = ConnectionIndexIn;
            this.ConnectionIndexOut = ConnectionIndexOut;
            this.NumberOfCP = NumberOfCP;
            this.offset = offset;
        }
    }
}