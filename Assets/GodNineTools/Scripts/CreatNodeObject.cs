using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem
{
    [System.Serializable]
    public class CreatNodeObject : ScriptableObject {
        [SerializeField] private List<CreatNode> Nodes = new List<CreatNode>();
        [SerializeField] private int InitNode;
        [SerializeField] private int CurrentIndex;

        //something something no constructos for instances
        public void Init(List<CreatNode> nodes, int start_index, int current_index) {
            this.Nodes = nodes;
            this.InitNode = start_index;
            this.CurrentIndex = current_index;
        }

        public CreatNodeObject Get() {
            return (CreatNodeObject)MemberwiseClone();
        }

        public CreatNode Next(string iTrigger) {
            CurrentIndex = Nodes[CurrentIndex].NextNode(iTrigger);
            if (CurrentIndex >= 0) {
                return Nodes[CurrentIndex];
            } else {
                return null;
            }
        }

        public CreatNode GetCurrent() {
            return Nodes[CurrentIndex];
        }

        public void Reset() {
            CurrentIndex = InitNode;
        }
    }

    [System.Serializable]
    public class CreatNode {
        [SerializeField] private string mName;
        [SerializeField] private List<string> mTriggers;
		[SerializeField] private List<Vector3> mTargetPositions;
		public List<int> NextIndex;

		public string Name { get { return mName; } }
		public List<string> Triggers { get { return mTriggers; } }
		public List<Vector3> TargetPositions { get { return mTargetPositions; } }

		public int NextNode(string iTrigger) {
            if (!iTrigger.Contains(iTrigger)) {
                Debug.LogWarning("Trigger does not exist in this node!");
            }
            return NextIndex[mTriggers.IndexOf(iTrigger)];
        }

        public CreatNode(string iName, List<string> iTriggers, List<Vector3> iTargetPositions) {
            this.mName = iName;
            this.mTriggers = iTriggers;
			this.mTargetPositions = iTargetPositions;
			NextIndex = new List<int>();
            for (int i = 0; i < iTriggers.Count; i++) {
				NextIndex.Add(-1);
            }
        }
    }
}