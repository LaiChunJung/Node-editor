using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem
{
    [System.Serializable]
    public class BuildNodeObject : ScriptableObject {
        [SerializeField] private List<BuildNode> nodes = new List<BuildNode>();
        [SerializeField] private int start_index;
        [SerializeField] private int current_index;

        //something something no constructos for instances
        public void Init(List<BuildNode> nodes, int start_index, int current_index) {
            this.nodes = nodes;
            this.start_index = start_index;
            this.current_index = current_index;
        }

        public BuildNodeObject Get() {
            return (BuildNodeObject)MemberwiseClone();
        }

        public BuildNode Next(string trigger) {
            current_index = nodes[current_index].next_node(trigger);
            if (current_index >= 0) {
                return nodes[current_index];
            } else {
                return null;
            }
        }

        public BuildNode GetCurrent() {
            return nodes[current_index];
        }

        public void Reset() {
            current_index = start_index;
        }
    }

    [System.Serializable]
    public class BuildNode {
        [SerializeField] private string mName;
        [SerializeField] private List<string> mTriggers;
        public List<int> next_index; //TODO figure out what your own code does so we can make this private k?

        public List<string> Triggers { get { return mTriggers; } }

        public int next_node(string iTrigger) {
            if (!iTrigger.Contains(iTrigger)) {
                Debug.LogWarning("Trigger does not exist in this node!");
            }
            return next_index[mTriggers.IndexOf(iTrigger)];
        }

        public BuildNode(string iName, List<string> iTriggers) {
            this.mName = iName;
            this.mTriggers = iTriggers;
            next_index = new List<int>();
            for (int i = 0; i < iTriggers.Count; i++) {
                next_index.Add(-1);
            }
        }
    }
}