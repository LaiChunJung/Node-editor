using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem
{
    [System.Serializable]
    public class BuildNodeObject : ScriptableObject {
        [SerializeField] private List<BuildNode> Nodes = new List<BuildNode>();
        [SerializeField] private int InitNode;
        [SerializeField] private int CurrentIndex;

        //something something no constructos for instances
        public void Init(List<BuildNode> nodes, int start_index, int current_index) {
            this.Nodes = nodes;
            this.InitNode = start_index;
            this.CurrentIndex = current_index;
        }

        public BuildNodeObject Get() {
            return (BuildNodeObject)MemberwiseClone();
        }

        public BuildNode Next(string iTrigger) {
            CurrentIndex = Nodes[CurrentIndex].NextNode(iTrigger);
            if (CurrentIndex >= 0) {
                return Nodes[CurrentIndex];
            } else {
                return null;
            }
        }

        public BuildNode GetCurrent() {
            return Nodes[CurrentIndex];
        }

        public void Reset() {
            CurrentIndex = InitNode;
        }
    }

    [System.Serializable]
    public class BuildNode {
        [SerializeField] private string mName;
        [SerializeField] private List<string> mTriggers;
        public List<int> NextIndex;

		public string Name { get { return mName; } }
		public List<string> Triggers { get { return mTriggers; } }

        public int NextNode(string iTrigger) {
            if (!iTrigger.Contains(iTrigger)) {
                Debug.LogWarning("Trigger does not exist in this node!");
            }
            return NextIndex[mTriggers.IndexOf(iTrigger)];
        }

        public BuildNode(string iName, List<string> iTriggers) {
            this.mName = iName;
            this.mTriggers = iTriggers;
			NextIndex = new List<int>();
            for (int i = 0; i < iTriggers.Count; i++) {
				NextIndex.Add(-1);
            }
        }
    }
}